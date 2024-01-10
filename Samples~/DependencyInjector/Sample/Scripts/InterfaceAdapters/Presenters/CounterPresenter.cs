namespace DependencyInjector.Examples
{
    public class CounterPresenter : ICounterPresenter
    {
        private readonly ICounterView _counterView;
        
        public CounterPresenter(ICounterView counterView)
        {
            _counterView = counterView;
        }
        
        public void UpdateCount(int count)
        {
            _counterView.ShowCount(count.ToString());
        }
    }
}