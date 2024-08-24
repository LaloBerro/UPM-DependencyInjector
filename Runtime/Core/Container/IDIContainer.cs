using System;

namespace DependencyInjector.Core
{
    public interface IDIContainer
    {
        void RegisterAsSingle<TServiceType>(TServiceType serviceInstance);
        void RegisterAsMultiple<TServiceType>(TServiceType serviceInstance);
        TServiceType Get<TServiceType>();
        object GetObjectByType(Type type);
        object[] GetArrayByType(Type type);
        bool IsTypeContained(Type elementType);
    }
}