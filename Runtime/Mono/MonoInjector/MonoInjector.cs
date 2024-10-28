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

        public void SetInstallers(MonoInstaller[] monoInstallers)
        {
            _monoInstallers = monoInstallers;
        }
        
        public override void InjectAll()
        {
            InitializeDiContainer();
            
            List<IDIContainer> diContainers = new List<IDIContainer>();

            if (_monoInjectors != null)
            {
                for (var index = 0; index < _monoInjectors.Length; index++)
                {
                    diContainers.Add(_monoInjectors[index].DiContainer);
                }
            }
            
            if (_hasToUseGlobalDiContainer)
                diContainers.Add(ServiceLocatorInstance.Instance.Get<IDIContainer>());

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            foreach (var reflectionInjector in reflectionInjectors)
            {
                reflectionInjector.OnErrorThrown += ThrowError;
            }
            
            Injector injector = new Injector(_monoInstallers, _diContainer, reflectionInjectors, diContainers.ToArray());
            
            injector.InjectAll();
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
    }
}