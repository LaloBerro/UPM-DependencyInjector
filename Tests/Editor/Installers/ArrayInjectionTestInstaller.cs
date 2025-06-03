using DependencyInjector.Core;
using DependencyInjector.Tests.BaseClasses;

namespace DependencyInjector.EditorTests
{
    public class ArrayInjectionTestInstaller : IInstaller
    {
        [Inject] private IInjectThis[] _injectThisArray;

        public bool HasToForceUseGlobalInstaller { get; }

        public bool HasToSkipInstallation()
        {
            return false;
        }

        public void Install(IDIContainer diContainer)
        {
            diContainer.RegisterAsMultiple(new ArrayInjectionTest(_injectThisArray));
        }
        
#if UNITY_EDITOR
        private bool _isInstalled;
        
        public bool IsInstalled => _isInstalled;
        
        public void SetAsInstalled()
        {
            _isInstalled = true;
        }
#endif
    }
}