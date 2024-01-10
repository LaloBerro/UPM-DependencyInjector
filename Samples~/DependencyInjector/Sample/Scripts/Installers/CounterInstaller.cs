using DependencyInjector.Core;
using DependencyInjector.Installers;

namespace DependencyInjector.Examples.Installers
{
    public class CounterInstaller : SingleMonoInstaller<Counter>
    {
        [Inject] private ICounterPresenter counterPresenter;
        
        protected override Counter GetData()
        {
            return new Counter(counterPresenter);
        }
    }
}