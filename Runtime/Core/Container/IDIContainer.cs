using System;

namespace DependencyInjector.Core
{
    public interface IDIContainer : IDisposable
    {
        void RegisterAsSingle<TServiceType>(TServiceType serviceInstance);
        void RegisterAsMultiple<TServiceType>(TServiceType serviceInstance);
        TServiceType Get<TServiceType>();
        object GetObjectByType(Type type);
        object[] GetArrayByType(Type type);
        bool IsTypeContained(Type elementType);
        void RemoveService<TServiceType>();
    }
}