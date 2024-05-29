using UnityEngine;

namespace DependencyInjector.Installers
{
    public class AwakeInjector : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private BaseMonoInjector _monoInjector;
        
        private void Awake()
        {
            _monoInjector.InjectAll();
        }
    }
}