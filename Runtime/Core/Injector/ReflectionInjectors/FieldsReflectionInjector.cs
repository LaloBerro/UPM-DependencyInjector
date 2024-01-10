using System;
using System.Reflection;

namespace DependencyInjector.Core
{
    public class FieldsReflectionInjector : IReflectionInjector
    {
        public void Inject(IDIContainer diContainer, object objectToSetInjections)
        {
            FieldInfo[] fields = objectToSetInjections.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

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