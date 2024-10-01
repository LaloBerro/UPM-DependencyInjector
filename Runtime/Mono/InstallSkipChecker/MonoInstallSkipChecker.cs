using UnityEngine;

namespace DependencyInjector.Installers
{
    public abstract class MonoInstallSkipChecker : MonoBehaviour, IInstallSkipChecker
    {
        public abstract bool HasToSkip();
    }
}