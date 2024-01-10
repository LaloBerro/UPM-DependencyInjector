using DependencyInjector.Core;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.EditorTests
{
    public class MultipleInjectionTestInstaller : IInstaller
    {
        [Inject]
        private IInjectThis _injectThis;
        
        public void Install(IDIContainer diContainer)
        {
            diContainer.RegisterAsMultiple(new InjectionTest(_injectThis));
        }
    }
}