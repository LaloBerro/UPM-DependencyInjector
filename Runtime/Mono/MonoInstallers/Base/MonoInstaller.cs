using System;
using DependencyInjector.Core;
using UnityEngine;

namespace DependencyInjector.Installers
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        protected bool _hasToForceUseGlobalInstaller;
        public bool HasToForceUseGlobalInstaller => _hasToForceUseGlobalInstaller;

        public bool HasToSkipInstallation()
        {
            if (!TryGetComponent(out IInstallSkipChecker installSkipChecker)) 
                return false;
            
            bool hasToSkipInstall = installSkipChecker.HasToSkip();
            return hasToSkipInstall;
        }

        public abstract void Install(IDIContainer diContainer);

        public virtual void RemoveFromDiContainer(IDIContainer diContainer)
        {
            
        }
    }

    public abstract class MonoInstaller<TServiceType> : MonoInstaller
    {
        private TServiceType _serviceInstance;
        public TServiceType ServiceInstance => _serviceInstance;

        public override void Install(IDIContainer diContainer)
        {
            _serviceInstance = GetData();

            if (ReferenceEquals(ServiceInstance, null))
                throw new Exception($"Installer error: The MonoInstaller ${gameObject.name} is null");

            InstallServiceInContainer(diContainer, _serviceInstance);
        }

        protected abstract void InstallServiceInContainer(IDIContainer diContainer, TServiceType serviceInstance);

        protected abstract TServiceType GetData();

        public override void RemoveFromDiContainer(IDIContainer diContainer)
        {
            diContainer.RemoveService<TServiceType>();
        }

        private void OnDestroy()
        {
            if (!(ServiceInstance is IDisposable disposable))
                return;

            disposable.Dispose();
        }
    }
}
