using DependencyInjector.Installers;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.PlayMode
{
    public class InjectThisMultipleMonoInstaller : MultipleMonoInstaller<IInjectThis>
    {
        protected override IInjectThis GetData()
        {
            return new InjectThis();
        }
    }
}