using UnityEngine;

namespace DependencyInjector.Installers
{
    public abstract class BridgeMonoInstaller<TServiceType> : MonoInstaller<TServiceType>
    {
        [Header("References")] 
        [SerializeField] private MonoInstaller<TServiceType> _monoInstaller;

        public void SetInstaller(MonoInstaller<TServiceType> monoInstaller)
        {
            _monoInstaller = monoInstaller;
        }

        protected override TServiceType GetData()
        {
            return _monoInstaller.ServiceInstance;
        }
    }
}