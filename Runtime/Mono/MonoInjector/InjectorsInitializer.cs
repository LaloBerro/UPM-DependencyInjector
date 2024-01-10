using UnityEngine;

namespace DependencyInjector.Installers
{
    public class InjectorsInitializer : BaseMonoInjector
    {
        [Header("References")] 
        [SerializeField] private BaseMonoInjector[] _monoInjectors;
        
        public void SetInjectors(BaseMonoInjector[] baseMonoInjectors)
        {
            _monoInjectors = baseMonoInjectors;
        }
        
        public override void InjectAll()
        {
            foreach (var baseMonoInjector in _monoInjectors)
            {
                baseMonoInjector.InjectAll();
            }
        }
    }
}