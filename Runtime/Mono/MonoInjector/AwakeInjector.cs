using UnityEngine;

namespace DependencyInjector.Installers
{
    public class AwakeInjector : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private BaseMonoInjector _monoInjector;
        
        public void SetBaseMonoInjector(BaseMonoInjector baseMonoInjector)
        {
            _monoInjector = baseMonoInjector;
        }
        
        private void Awake()
        {
            _monoInjector.InjectAll();
        }
    }
}