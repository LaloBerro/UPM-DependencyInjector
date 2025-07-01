using System.Collections.Generic;
using DependencyInjector.Installers;
using UnityEditor;
using UnityEngine;

namespace DependencyInjectorEditor
{
    [CustomEditor(typeof(InjectorsInitializer))]
    public class InjectorsInitializerCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawProperties();
        }
        
        private bool DrawButton(string title, string iconName, int fontSize, int height)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = fontSize;
            buttonStyle.fontStyle = FontStyle.Bold;
            Texture icon = EditorGUIUtility.IconContent(iconName).image;
            GUIContent buttonContent = new GUIContent(title, icon);
            
            if (GUILayout.Button(buttonContent, buttonStyle, GUILayout.Height(height)))
            {
                return true;
            }

            return false;
        }

        private void DrawProperties()
        {
            serializedObject.Update();
            DrawInstallers();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawInstallers()
        {
            string title = "Injectors";
            DrawTitle(title);
            
            if (DrawButton("  Load Injectors", "d_ol_plus", 12, 20))
                FillInjectorsUsingChildren();
            
            
            EditorGUI.indentLevel++;
            SerializedProperty serializedProperty = serializedObject.FindProperty("_monoInjectors");
            EditorGUILayout.PropertyField(serializedProperty);
        }

        private void DrawTitle(string title)
        {
            GUILayout.Space(10);
            
            GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
            boldStyle.fontSize = 15;
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

        private void FillInjectorsUsingChildren()
        {
            InjectorsInitializer injectorsInitializer = (InjectorsInitializer)target;
            
            Undo.RecordObject(injectorsInitializer, "Injector Initializer");
            
            Transform root = injectorsInitializer.transform;
            List<BaseMonoInjector> baseMonoInjects = new List<BaseMonoInjector>();
            for (int i = 0; i < root.childCount; i++)
            {
                BaseMonoInjector injector = root.GetChild(i).GetComponent<BaseMonoInjector>();
                
                if(injector != null)
                    baseMonoInjects.Add(injector);
            }
            
            injectorsInitializer.SetInjectors(baseMonoInjects.ToArray());
            EditorUtility.SetDirty(injectorsInitializer);
        }
    }
}