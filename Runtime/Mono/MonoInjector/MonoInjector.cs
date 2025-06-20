﻿using System;
using System.Collections.Generic;
using DependencyInjector.Core;
using ServiceLocatorPattern;
using UnityEngine;

namespace DependencyInjector.Installers
{
    public class MonoInjector : BaseMonoInjector
    {
        [SerializeField] private MonoInstaller[] _monoInstallers;
        [SerializeField] private BaseMonoInjector[] _monoInjectors;
        [SerializeField] private bool _hasInstallInGlobalDiContainer;
        [SerializeField] private bool _hasToUseGlobalDiContainer;

        public MonoInstaller[] MonoInstallers => _monoInstallers;
        
#if UNITY_EDITOR
        private bool _isInstalled;
        
        public bool IsInstalled => _isInstalled;
#endif

        public void SetInstallers(MonoInstaller[] monoInstallers)
        {
            _monoInstallers = monoInstallers;
        }
        
        public override void InjectAll()
        {
            bool hasToSkipInstall = HasToSkipInstallation();
            if(hasToSkipInstall) 
                return;
            
            InitializeDiContainer();
            
            List<IDIContainer> diContainers = new List<IDIContainer>();

            if (_monoInjectors != null)
            {
                for (var index = 0; index < _monoInjectors.Length; index++)
                {
                    if(_monoInjectors[index] == null)
                        throw new Exception("Null Injector" + gameObject.name);
                    
                    IDIContainer diContainer = _monoInjectors[index].DiContainer;
                    if(null == diContainer)
                        throw new Exception("Null DiContainer probably is not installed yet: " + _monoInjectors[index].gameObject.name + "\n It's needed here: " + gameObject.name);
                    
                    diContainers.Add(diContainer);
                }
            }

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            foreach (var reflectionInjector in reflectionInjectors)
            {
                reflectionInjector.OnErrorThrown += ThrowError;
            }

            foreach (MonoInstaller monoInstaller in MonoInstallers)
            {
                if (null == monoInstaller)
                    Debug.LogError("Null Installer: " + gameObject.name, gameObject);
                
                if(monoInstaller.HasToForceUseGlobalInstaller)
                    _hasToUseGlobalDiContainer = true;
            }
            
            if (_hasToUseGlobalDiContainer)
                diContainers.Add(ServiceLocatorInstance.Instance.Get<IDIContainer>());
            
            Injector injector = new Injector(MonoInstallers, _diContainer, reflectionInjectors, diContainers.ToArray());

#if UNITY_EDITOR
            for (int i = 0; i < diContainers.Count; i++)
            {
                if(null == diContainers[i])
                    throw new Exception("Null DiContainer probably is not installed yet: " + gameObject.name);
            }
#endif
            
            injector.InjectAll();

#if UNITY_EDITOR
            _isInstalled = true;
#endif
        }

        private void ThrowError(string error)
        {
            Debug.LogError("Inject Error in: " + gameObject.name + "  |  " + error, gameObject);
        }

        private void InitializeDiContainer()
        {
            if (!_hasInstallInGlobalDiContainer)
                _diContainer = new DIContainer();
            else
                _diContainer = ServiceLocatorInstance.Instance.Get<IDIContainer>();
        }
        
        public override void Dispose()
        {
            _diContainer.Dispose();
        }
        
        private void OnDestroy()
        {
            if (!_hasInstallInGlobalDiContainer) 
                return;
            
            foreach (var monoInstaller in MonoInstallers)
            {
                monoInstaller.RemoveFromDiContainer(_diContainer);
            }
        }
    }
}