using System;

namespace DependencyInjector.Examples
{
    public interface ICounterView
    {
        Action OnAddNumberClick { get; set;}
        void ShowCount(string count);
    }
}