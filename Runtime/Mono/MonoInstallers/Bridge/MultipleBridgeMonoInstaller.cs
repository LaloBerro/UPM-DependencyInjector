using DependencyInjector.Core;

namespace DependencyInjector.Installers
{
    public class MultipleBridgeMonoInstaller<TServiceType> : BridgeMonoInstaller<TServiceType>
    {
        protected override void InstallServiceInContainer(IDIContainer diContainer, TServiceType serviceInstance)
        {
            diContainer.RegisterAsMultiple<TServiceType>(serviceInstance);
        }
    }
}