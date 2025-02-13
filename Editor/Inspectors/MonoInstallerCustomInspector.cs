using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DependencyInjector.Core;
using DependencyInjector.Installers;
using UnityEditor;
using UnityEngine;

namespace DependencyInjectorEditor
{
    [CustomEditor(typeof(MonoInstaller), true)]
    public class MonoInstallerCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            List<FieldInfo> fields = GetFields(target);
            if(fields != null && fields.Count > 0)
                DrawInjectFields(fields);
            
            DrawInstallerInformation();

            base.OnInspectorGUI();
        }
        
        public Type GetGenericMonoInstaller(Type givenType, Type genericType)
        {
            while (givenType != null && givenType != typeof(object))
            {
                Type currentType = givenType.IsGenericType ? givenType.GetGenericTypeDefinition() : givenType;

                if (currentType == genericType)
                    return givenType;

                givenType = givenType.BaseType; 
            }
            
            return null;
        }

        private void DrawInstallerInformation()
        {
            GUILayout.BeginVertical("Box");
            
            DrawInstallerType();
            DrawInstallerClassType();
            
            GUILayout.EndVertical();
        }

        private void DrawInstallerType()
        {
            string typeName = "Not Type Based Installer";
            Type type = target.GetType();
            Type singleMonoInstallerType = GetGenericMonoInstaller(type, typeof(SingleMonoInstaller<>));
            if (singleMonoInstallerType != null)
            {
                typeName = "Single Installer:";
            }
            else
            {
                Type multipleMonoInstallerType = GetGenericMonoInstaller(type, typeof(MultipleMonoInstaller<>));
                if (multipleMonoInstallerType != null)
                {
                    typeName = "Multiple Installer:";
                }
            }
            
            GUIStyle boldStyle = new GUIStyle(EditorStyles.boldLabel);
            EditorGUILayout.LabelField(typeName, boldStyle);
        }

        private void DrawInstallerClassType()
        {
            Type type = target.GetType();

            Type genericMonoInstallerType = GetGenericMonoInstaller(type, typeof(MonoInstaller<>));

            if (genericMonoInstallerType == null) 
                return;
            
            Type[] genericArguments = genericMonoInstallerType.GetGenericArguments();

            if (genericArguments.Length <= 0) 
                return;
            Type monoInstallerType = genericArguments[0];

            string className = GetGenericTypeName(monoInstallerType);
                    
            GUIStyle boldStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = Color.yellow},
                fontSize = 12
            };

            EditorGUILayout.LabelField(className, boldStyle);
        }

        private string GetGenericTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }
            
            string genericTypeName = type.Name.Split('`')[0];

            Type[] genericArguments = type.GetGenericArguments();
            
            StringBuilder fullName = new StringBuilder();
            fullName.Append(genericTypeName);
            fullName.Append("<");

            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (i > 0)
                {
                    fullName.Append(", ");
                }
                fullName.Append(GetGenericTypeName(genericArguments[i]));
            }

            fullName.Append(">");

            return fullName.ToString();
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

        private void DrawInjectFields(List<FieldInfo> fields)
        {
            List<Type> dependencyTypes = new List<Type>();
            
            foreach (var fieldInfo in fields)
            {
                object attribute = fieldInfo.GetCustomAttribute<InjectAttribute>();

                if (ReferenceEquals(attribute, null))
                    continue;
                
                Type type = fieldInfo.FieldType;
                
                dependencyTypes.Add(type);
            }

            if (dependencyTypes.Count <= 0)
                return;
            
            GUILayout.BeginVertical("Box");
            
            DrawTitle("Dependencies");

            foreach (var typeInfo in dependencyTypes)
            {
                string typeName = typeInfo.Name;
                string displayTypeName = typeName;
                
                Type[] genericTypesArguments = typeInfo.GetGenericArguments();
                
                if (genericTypesArguments != null && genericTypesArguments.Length > 0)
                {
                    displayTypeName = GetGenericTypeName(typeInfo);
                }
                
                GUIStyle boldStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    normal = { textColor = new Color(0.9f, 0.5f, 0.2f)},
                    fontSize = 13
                        
                };

                EditorGUILayout.LabelField(displayTypeName, boldStyle);
            }

            GUILayout.EndVertical();
        }
        
        private void DrawTitle(string title)
        {
            GUILayout.Space(10);
            
            GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
            boldStyle.fontSize = 13;
            boldStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label(title, boldStyle);

            DrawDivider();
        }

        private void DrawDivider()
        {
            GUIStyle lineStyle = new GUIStyle();
            lineStyle.normal.background = EditorGUIUtility.whiteTexture;
            lineStyle.margin = new RectOffset(0, 0, 4, 4);
            lineStyle.fixedHeight = 1;
            GUILayout.Box(GUIContent.none, lineStyle, GUILayout.ExpandWidth(true), GUILayout.Height(1));
        }
    }
}