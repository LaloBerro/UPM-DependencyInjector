namespace DependencyInjector.Examples
{
    public class Counter
    {
        private readonly ICounterPresenter _counterPresenter;
        
        private int _count;

        public Counter(ICounterPresenter counterPresenter)
        {
            _counterPresenter = counterPresenter;
        }

        public void AddNumber()
        {
            _count++;

            _counterPresenter.UpdateCount(_count);
        }
    }
}

