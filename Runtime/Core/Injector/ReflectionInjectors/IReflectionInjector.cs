namespace DependencyInjector.Core
{
    public interface IReflectionInjector
    {
        void Inject(IDIContainer[] diContainers, object objectToSetInjections);
    }
}