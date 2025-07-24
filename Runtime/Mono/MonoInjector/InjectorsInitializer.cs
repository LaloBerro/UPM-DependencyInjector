using System;
using UnityEngine;

namespace DependencyInjector.Installers
{
    public class InjectorsInitializer : BaseMonoInjector
    {
        [Header("References")] 
        [SerializeField] private BaseMonoInjector[] _monoInjectors;
        
        [Header("Debug")]
        [SerializeField] private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        public void SetInjectors(BaseMonoInjector[] baseMonoInjectors)
        {
            _monoInjectors = baseMonoInjectors;
        }
        
        public override void InjectAll()
        {
            if (IsInitialized)
            {
                throw new Exception("Injector already initialized: " + gameObject.name);
            }
            
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