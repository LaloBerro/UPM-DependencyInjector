using DependencyInjector.Core;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.EditorTests
{
    public class SingleInjectionTestInstaller : IInstaller
    {
        [Inject]
        private IInjectThis _injectThis;
        
        public bool HasToSkipInstallation()
        {
            return false;
        }
        
        public void Install(IDIContainer diContainer)
        {
            diContainer.RegisterAsSingle(new InjectionTest(_injectThis));
        }
    }
}