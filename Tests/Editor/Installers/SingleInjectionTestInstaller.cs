using DependencyInjector.Core;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.EditorTests
{
    public class SingleInjectionTestInstaller : IInstaller
    {
        [Inject]
        private IInjectThis _injectThis;
        
        public void Install(IDIContainer diContainer)
        {
            diContainer.RegisterAsSingle(new InjectionTest(_injectThis));
        }
    }
}