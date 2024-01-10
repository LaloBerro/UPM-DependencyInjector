namespace DependencyInjector.Core
{
    public interface IReflectionInjector
    {
        void Inject(IDIContainer diContainer, object objectToSetInjections);
    }
}