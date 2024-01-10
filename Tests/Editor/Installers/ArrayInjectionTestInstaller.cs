using DependencyInjector.Core;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.EditorTests
{
public class ArrayInjectionTestInstaller : IInstaller
    {
        [Inject]
        private IInjectThis[] _injectThisArray;
        
        public void Install(IDIContainer diContainer)
        {
            diContainer.RegisterAsMultiple(new ArrayInjectionTest(_injectThisArray));
        }
    }
}