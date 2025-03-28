using System;
using System.Collections.Generic;

namespace DependencyInjector.Core
{
    public class DIContainer : IDIContainer
    {
        private readonly Dictionary<Type, object> _singleInstances = new();
        private readonly Dictionary<Type, List<object>> _multipleInstances = new();

        public void RegisterAsSingle<TServiceType>(TServiceType serviceInstance)
        {
            Type type = typeof(TServiceType);

            if (_singleInstances.ContainsKey(type))
                throw new Exception("It is already contained: " + type.FullName);
            
            _singleInstances.Add(type, serviceInstance);
        }

        public void RegisterAsMultiple<TServiceType>(TServiceType serviceInstance)
        {
            Type type = typeof(TServiceType);

            _multipleInstances.TryGetValue(type, out List<object> multipleInstancesList);
            if (ReferenceEquals(null, multipleInstancesList))
            {
                multipleInstancesList = new List<object>();
                _multipleInstances.Add(type, multipleInstancesList);
            }
            
            multipleInstancesList.Add(serviceInstance);
        }

        public TServiceType Get<TServiceType>()
        {
            Type type = typeof(TServiceType);
            
            if (_singleInstances.TryGetValue(type, out var singleServiceInstance))
                 return (TServiceType)singleServiceInstance;

            if (_multipleInstances.TryGetValue(type, out var cachedServiceInstance))
                return (TServiceType)cachedServiceInstance[0];

            throw new Exception("DIContainer Error: Get can't return because it didn't find: " + type);
        }

        public object GetObjectByType(Type type)
        {
            if (_singleInstances.TryGetValue(type, out var singleServiceInstance))
                return singleServiceInstance;

            if (_multipleInstances.TryGetValue(type, out var cachedServiceInstance))
                return cachedServiceInstance[0];

            throw new Exception("DIContainer Error: GetObjectByType can't return because it didn't find: " + type);
        }

        public object[] GetArrayByType(Type type)
        {
            if(_multipleInstances.TryGetValue(type, out var cachedServiceInstanceArray))
                return cachedServiceInstanceArray.ToArray();
            
            throw new Exception("DIContainer Error: GetCachedArrayByType can't return because it didn't find: " + type);
        }

        public bool IsTypeContained(Type type)
        {
            return _singleInstances.ContainsKey(type) || _multipleInstances.ContainsKey(type);
        }

        public void Dispose()
        {
            foreach (var instanceKV in _singleInstances)
            {
                if(instanceKV.Value is IDisposable disposable)
                    disposable.Dispose();
            }
            
            foreach (var instanceKV in _multipleInstances)
            {
                List<object> instances = instanceKV.Value;
                foreach (var instance in instances)
                {
                    if(instance is IDisposable disposable)
                        disposable.Dispose();
                }
            }
            
            _singleInstances.Clear();
            _multipleInstances.Clear();
        }
        
        public void RemoveService<TServiceType>()
        {
            Type type = typeof(TServiceType);
            if (_singleInstances.ContainsKey(type))
                _singleInstances.Remove(type);
            else if(_multipleInstances.ContainsKey(type))
                _multipleInstances.Remove(type);
            else 
                throw new Exception("DIContainer Error: Trying to remove a service that does not exists " + type);
        }
    }
}