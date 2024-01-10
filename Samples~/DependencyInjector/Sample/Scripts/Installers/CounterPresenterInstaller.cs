using DependencyInjector.Core;
using DependencyInjector.Installers;

namespace DependencyInjector.Examples.Installers
{
    public class CounterPresenterInstaller : SingleMonoInstaller<ICounterPresenter>
    {
        [Inject] private ICounterView counterView;

        protected override ICounterPresenter GetData()
        {
            return new CounterPresenter(counterView);
        }
    }
}