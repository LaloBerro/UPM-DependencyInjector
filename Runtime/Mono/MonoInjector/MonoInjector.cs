using System;
using DependencyInjector.Core;
using UnityEngine;

namespace DependencyInjector.Installers
{
    public class MonoInjector : BaseMonoInjector
    {
        [Header("References")] 
        [SerializeField] private MonoInstaller[] _monoInstallers;
        [SerializeField] private BaseMonoInjector[] _monoInjectors; 

        public void SetInstallers(MonoInstaller[] monoInstallers)
        {
            _monoInstallers = monoInstallers;
        }
        
        public override void InjectAll()
        {
            _diContainer = new DIContainer();
            
            IDIContainer[] diContainers = new IDIContainer[_monoInjectors.Length];
            for (var index = 0; index < _monoInjectors.Length; index++)
            {
                diContainers[index] = _monoInjectors[index].DiContainer;
            }

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(_monoInstallers, _diContainer, reflectionInjectors, diContainers);
            
            injector.InjectAll();
        }
    }
}