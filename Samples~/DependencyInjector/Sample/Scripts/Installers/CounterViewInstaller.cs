using DependencyInjector.Installers;
using UnityEngine;

namespace DependencyInjector.Examples.Installers
{
    public class CounterViewInstaller : SingleMonoInstaller<ICounterView>
    {
        [Header("References")] 
        [SerializeField] private CounterView _counterView; 

        protected override ICounterView GetData()
        {
            return _counterView;
        }
    }
}