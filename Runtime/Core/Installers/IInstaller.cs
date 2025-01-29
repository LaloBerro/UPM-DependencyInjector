namespace DependencyInjector.Core
{
    public interface IInstaller
    {
        bool HasToForceUseGlobalInstaller { get; }
        bool HasToSkipInstallation();
        public void Install(IDIContainer diContainer);
    }
}