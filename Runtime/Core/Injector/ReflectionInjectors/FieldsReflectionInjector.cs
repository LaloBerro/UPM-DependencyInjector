using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DependencyInjector.Core
{
    public class FieldsReflectionInjector : IReflectionInjector
    {
        public void Inject(IDIContainer diContainer, object objectToSetInjections)
        {
            List<FieldInfo> fields = new List<FieldInfo>();

            Type currentType = objectToSetInjections.GetType();
            while (true)
            {
                FieldInfo[] fieldInfos = currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                
                fields.AddRange(fieldInfos);

                currentType = currentType.BaseType;
                
                if(ReferenceEquals(currentType, null) || currentType == typeof(MonoBehaviour))
                    break;
            }

            foreach (var fieldInfo in fields)
            {
                object attribute = fieldInfo.GetCustomAttribute<InjectAttribute>();

                if (ReferenceEquals(attribute, null))
                    continue;

                SetFieldValue(diContainer, objectToSetInjections, fieldInfo);
            }
        }

        private void SetFieldValue(IDIContainer diContainer, object objectToSetInjections, FieldInfo fieldInfo)
        {
            Type type = fieldInfo.FieldType;

            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                object[] fieldValue = diContainer.GetArrayByType(elementType);

                Array destinationArray = Array.CreateInstance(elementType, fieldValue.Length);
                Array.Copy(fieldValue, destinationArray, fieldValue.Length);

                fieldInfo.SetValue(objectToSetInjections, destinationArray);

                return;
            }

            object value = diContainer.GetObjectByType(type);
            fieldInfo.SetValue(objectToSetInjections, value);
        }
    }
}