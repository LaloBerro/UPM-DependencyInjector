using DependencyInjector.Core;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.EditorTests
{
    public class MultipleInjectThisInstaller : IInstaller
    {
        public void Install(IDIContainer diContainer)
        {
            diContainer.RegisterAsMultiple<IInjectThis>(new InjectThis());
        }
    }
}