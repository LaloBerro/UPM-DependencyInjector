using DependencyInjector.Core;
using ServiceLocatorPattern;
using UnityEngine;

namespace DependencyInjector.Installers
{
    public class MonoInjector : BaseMonoInjector
    {
        [SerializeField] private MonoInstaller[] _monoInstallers;
        [SerializeField] private BaseMonoInjector[] _monoInjectors;
        [SerializeField] private bool _hasUseGlobalDiContainer;

        public void SetInstallers(MonoInstaller[] monoInstallers)
        {
            _monoInstallers = monoInstallers;
        }
        
        public override void InjectAll()
        {
            InitializeDiContainer();
            
            IDIContainer[] diContainers = new IDIContainer[_monoInjectors.Length];
            for (var index = 0; index < _monoInjectors.Length; index++)
            {
                diContainers[index] = _monoInjectors[index].DiContainer;
            }

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(_monoInstallers, _diContainer, reflectionInjectors, diContainers);
            
            injector.InjectAll();
        }

        private void InitializeDiContainer()
        {
            if (!_hasUseGlobalDiContainer)
                _diContainer = new DIContainer();
            else
                _diContainer = ServiceLocatorInstance.Instance.Get<IDIContainer>();
        }
    }
}