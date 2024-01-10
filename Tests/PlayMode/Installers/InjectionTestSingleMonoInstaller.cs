using DependencyInjector.Core;
using DependencyInjector.Installers;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.PlayMode
{
    public class InjectionTestSingleMonoInstaller : SingleMonoInstaller<InjectionTest>
    {
        [Inject]
        private IInjectThis _injectThis;
        
        protected override InjectionTest GetData()
        {
            return new InjectionTest(_injectThis);
        }
    }
}