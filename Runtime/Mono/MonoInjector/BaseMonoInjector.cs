using UnityEngine;

namespace DependencyInjector.Installers
{
    public abstract class BaseMonoInjector : MonoBehaviour
    {
        public abstract void InjectAll();
    }
}