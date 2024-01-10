using System;

namespace DependencyInjector.Examples
{
    public class CounterController : IDisposable
    {
        private readonly ICounterView _counterView;
        private readonly Counter _counter;

        public CounterController(ICounterView counterView, Counter counter)
        {
            _counterView = counterView;
            _counter = counter;
            
            _counterView.OnAddNumberClick += AddNumber;
        }
        
        private void AddNumber()
        {
            _counter.AddNumber();
        }
        
        public void Dispose()
        {
            _counterView.OnAddNumberClick -= AddNumber;
        }
    }
}