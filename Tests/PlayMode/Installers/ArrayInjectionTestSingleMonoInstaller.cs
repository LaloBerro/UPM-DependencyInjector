using DependencyInjector.Core;
using DependencyInjector.Installers;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.PlayMode
{
    public class ArrayInjectionTestSingleMonoInstaller : SingleMonoInstaller<ArrayInjectionTest>
    {
        [Inject]
        private IInjectThis[] _injectThis;
        
        protected override ArrayInjectionTest GetData()
        {
            return new ArrayInjectionTest(_injectThis);
        }
    }
}