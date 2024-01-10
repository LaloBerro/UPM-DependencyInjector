using DependencyInjector.Core;

namespace DependencyInjector.Installers
{
    public abstract class MultipleMonoInstaller<TServiceType> : MonoInstaller<TServiceType>
    {
        protected override void InstallServiceInContainer(IDIContainer diContainer, TServiceType serviceInstance)
        {
            diContainer.RegisterAsMultiple<TServiceType>(serviceInstance);
        }
    }
}