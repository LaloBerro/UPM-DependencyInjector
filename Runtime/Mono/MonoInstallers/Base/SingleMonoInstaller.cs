using DependencyInjector.Core;

namespace DependencyInjector.Installers
{
    public abstract class SingleMonoInstaller<TServiceType> : MonoInstaller<TServiceType>
    {
        protected override void InstallServiceInContainer(IDIContainer diContainer, TServiceType serviceInstance)
        {
            diContainer.RegisterAsSingle<TServiceType>(serviceInstance);
        }
    }
}