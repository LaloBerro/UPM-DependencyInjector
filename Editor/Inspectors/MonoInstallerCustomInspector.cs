using System;
using System.Collections.Generic;
using System.Reflection;
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

            base.OnInspectorGUI();
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
                GUILayout.Label(typeInfo.Name);
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