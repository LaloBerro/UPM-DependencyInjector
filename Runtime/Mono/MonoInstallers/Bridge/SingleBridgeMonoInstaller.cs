using DependencyInjector.Core;

namespace DependencyInjector.Installers
{
    public class SingleBridgeMonoInstaller<TServiceType> : BridgeMonoInstaller<TServiceType>
    {
        protected override void InstallServiceInContainer(IDIContainer diContainer, TServiceType serviceInstance)
        {
            diContainer.RegisterAsSingle<TServiceType>(serviceInstance);
        }
    }
}