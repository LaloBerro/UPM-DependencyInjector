namespace DependencyInjector.Core
{
    public interface IInstaller
    {
        bool HasToForceUseGlobalInstaller { get; }
        bool HasToSkipInstallation();
        void Install(IDIContainer diContainer);
        
        #if UNITY_EDITOR
        bool IsInstalled { get; }
        void SetAsInstalled();
        #endif
    }
}