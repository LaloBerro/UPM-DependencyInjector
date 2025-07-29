using System;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
using System.Diagnostics;
#endif

namespace DependencyInjector.Installers
{
    public class InjectorsInitializer : BaseMonoInjector
    {
        [Header("References")] 
        [SerializeField] private BaseMonoInjector[] _monoInjectors;
        
        [Header("Debug")]
        [SerializeField] private bool _isInitialized;

#if UNITY_EDITOR
        private string _initializerClass;
#endif

        public bool IsInitialized => _isInitialized;

        public void SetInjectors(BaseMonoInjector[] baseMonoInjectors)
        {
            _monoInjectors = baseMonoInjectors;
        }
        
        public override void InjectAll()
        {
            if (IsInitialized)
            {
                
#if UNITY_EDITOR
                Debug.LogError("Injector already initialized. Press here to select it", this);
                
                StackTrace stackTraceNew = new StackTrace();
                MethodBase methodBase = stackTraceNew.GetFrame(1)?.GetMethod();
                string initializerClass = methodBase.DeclaringType.Name;
                throw new Exception("Injector already initialized: " + gameObject.name + " | This class was installed by: " + _initializerClass + 
                " ,and now you are trying to initialize it by: " + initializerClass);
#else
                Debug.LogError("Injector already initialized: " + gameObject.name);
#endif
            }
            
#if UNITY_EDITOR
            StackTrace stackTrace = new StackTrace();
            MethodBase caller = stackTrace.GetFrame(1)?.GetMethod();
            _initializerClass = caller.DeclaringType.Name;
#endif
            
            if (_monoInjectors.Length <= 0)
            {
                throw new Exception("Injector is empty: " + gameObject.name);
            }
            
            bool hasToSkipInstall = HasToSkipInstallation();
            if(hasToSkipInstall) 
                return;
            
            _isInitialized = true;
            
            foreach (var baseMonoInjector in _monoInjectors)
            {
                baseMonoInjector.InjectAll();
            }
        }

        public override void Dispose()
        {
            if (!IsInitialized)
                return;
            
            _isInitialized = false;
            
            foreach (var baseMonoInjector in _monoInjectors)
            {
                baseMonoInjector.Dispose();
            }
        }
    }
}