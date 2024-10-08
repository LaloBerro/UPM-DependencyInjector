using DependencyInjector.Core;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.EditorTests
{
    public class SingleInjectThisInstaller : IInstaller
    {
        public bool HasToSkipInstallation()
        {
            return false;
        }
        
        public void Install(IDIContainer diContainer)
        {
            diContainer.RegisterAsSingle<IInjectThis>(new InjectThis());
        }
    }
}