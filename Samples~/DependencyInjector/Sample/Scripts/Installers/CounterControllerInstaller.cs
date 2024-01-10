using DependencyInjector.Core;
using DependencyInjector.Installers;

namespace DependencyInjector.Examples.Installers
{
    public class CounterControllerInstaller : SingleMonoInstaller<CounterController>
    {
        [Inject] private ICounterView counterView;
        [Inject] private Counter counter;

        protected override CounterController GetData()
        {
            return new CounterController(counterView, counter);
        }
    }
}