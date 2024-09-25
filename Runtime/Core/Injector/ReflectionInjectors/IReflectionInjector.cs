using System;

namespace DependencyInjector.Core
{
    public interface IReflectionInjector
    {
        Action<string> OnErrorThrown { get; set;}
        void Inject(IDIContainer[] diContainers, object objectToSetInjections);
    }
}