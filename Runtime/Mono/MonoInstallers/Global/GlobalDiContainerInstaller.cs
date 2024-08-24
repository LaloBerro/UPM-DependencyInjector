using DependencyInjector.Core;
using ServiceLocatorPattern;

namespace DependencyInjector.Installers
{
    public class GlobalDiContainerInstaller : MonoInstaller
    {
        public override void Install(IDIContainer diContainer)
        {
            if (ServiceLocatorInstance.Instance.IsContained<IDIContainer>())
                ServiceLocatorInstance.Instance.Remove<IDIContainer>();
            
            ServiceLocatorInstance.Instance.Add(new DIContainer());
        }
    }
}