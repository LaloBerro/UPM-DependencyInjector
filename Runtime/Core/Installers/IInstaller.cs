namespace DependencyInjector.Core
{
    public interface IInstaller
    {
        bool HasToSkipInstallation();
        public void Install(IDIContainer diContainer);
    }
}