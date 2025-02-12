using UnityEngine;

namespace DependencyInjector.Installers
{
    public class StartInjector : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private BaseMonoInjector _monoInjector;
        
        private void Start()
        {
            _monoInjector.InjectAll();
        }
    }
}