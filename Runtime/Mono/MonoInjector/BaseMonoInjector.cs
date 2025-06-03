using DependencyInjector.Core;
using UnityEngine;

namespace DependencyInjector.Installers
{
    public abstract class BaseMonoInjector : MonoBehaviour
    {
        protected IDIContainer _diContainer;
        public IDIContainer DiContainer => _diContainer;
        
        public abstract void InjectAll();

        public abstract void Dispose();
        
        protected bool HasToSkipInstallation()
        {
            if (!TryGetComponent(out IInstallSkipChecker installSkipChecker)) 
                return false;
            
            bool hasToSkipInstall = installSkipChecker.HasToSkip();
            return hasToSkipInstall;
        }
    }
}