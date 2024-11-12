using System;
using UnityEngine;

namespace DependencyInjector.Installers
{
    public class InjectorsInitializer : BaseMonoInjector
    {
        [Header("References")] 
        [SerializeField] private BaseMonoInjector[] _monoInjectors;
        
        [Header("Debug")]
        [SerializeField] private bool _isInitialized;
        
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

        public override void Dispose()
        {
            if (!_isInitialized)
                return;
            
            _isInitialized = false;
            
            foreach (var baseMonoInjector in _monoInjectors)
            {
                baseMonoInjector.Dispose();
            }
        }
    }
}