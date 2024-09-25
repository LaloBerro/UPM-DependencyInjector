using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DependencyInjector.Core
{
    public class FieldsReflectionInjector : IReflectionInjector
    {
        private IDIContainer[] _diContainers;

        public Action<string> OnErrorThrown { get; set; }

        public void Inject(IDIContainer[] diContainers, object objectToSetInjections)
        {
            _diContainers = diContainers;
            List<FieldInfo> fields = GetFields(objectToSetInjections);

            SetFields(objectToSetInjections, fields);
        }
        
        private List<FieldInfo> GetFields(object objectToSetInjections)
        {
            List<FieldInfo> fields = new List<FieldInfo>();
            Type currentType = objectToSetInjections.GetType();
            while (true)
            {
                FieldInfo[] fieldInfos = currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                fields.AddRange(fieldInfos);

                currentType = currentType.BaseType;

                if (ReferenceEquals(currentType, null) || currentType == typeof(MonoBehaviour))
                    break;
            }

            return fields;
        }
        
        private void SetFields(object objectToSetInjections, List<FieldInfo> fields)
        {
            foreach (var fieldInfo in fields)
            {
                object attribute = fieldInfo.GetCustomAttribute<InjectAttribute>();

                if (ReferenceEquals(attribute, null))
                    continue;

                SetFieldValue(objectToSetInjections, fieldInfo);
            }
        }

        private void SetFieldValue(object objectToSetInjections, FieldInfo fieldInfo)
        {
            Type type = fieldInfo.FieldType;

            if (type.IsArray)
            {
                SetArrayField(objectToSetInjections, fieldInfo, type);
                return;
            }

            object value = GetFieldByElementType(type);
            fieldInfo.SetValue(objectToSetInjections, value);
        }

        private void SetArrayField(object objectToSetInjections, FieldInfo fieldInfo, Type type)
        {
            Type elementType = type.GetElementType();
            object[] fieldValue = GetFieldsByElementType(elementType);

            Array destinationArray = Array.CreateInstance(elementType, fieldValue.Length);
            Array.Copy(fieldValue, destinationArray, fieldValue.Length);

            fieldInfo.SetValue(objectToSetInjections, destinationArray);
        }

        private object[] GetFieldsByElementType(Type elementType)
        {
            foreach (var diContainer in _diContainers)
            {
                if (diContainer.IsTypeContained(elementType))
                    return diContainer.GetArrayByType(elementType);
            }
            
            string error = "FieldsReflectionInjector Error: GetCachedArrayByType can't return because it doesn't exist: " + elementType;
            OnErrorThrown?.Invoke(error);
            
            throw new Exception(error);
        }
        
        private object GetFieldByElementType(Type elementType)
        {
            foreach (var diContainer in _diContainers)
            {
                if (diContainer.IsTypeContained(elementType))
                    return diContainer.GetObjectByType(elementType);
            }

            string error = "FieldsReflectionInjector Error: GetCachedArrayByType can't return because it doesn't exist: " + elementType;
            OnErrorThrown?.Invoke(error);
            
            throw new Exception(error);
        }
    }
}