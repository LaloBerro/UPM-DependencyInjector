using System;
using DependencyInjector.Core;
using UnityEngine;

namespace DependencyInjector.Installers
{
    public class MonoInjector : BaseMonoInjector
    {
        [Header("References")] 
        [SerializeField] private MonoInstaller[] _monoInstallers;

        public void SetInstallers(MonoInstaller[] monoInstallers)
        {
            _monoInstallers = monoInstallers;
        }
        
        public override void InjectAll()
        {
            IDIContainer diContainer = new DIContainer();

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(_monoInstallers, diContainer, reflectionInjectors);
            
            injector.InjectAll();
        }
    }
}