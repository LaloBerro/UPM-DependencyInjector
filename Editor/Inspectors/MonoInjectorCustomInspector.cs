using DependencyInjector.Installers;
using UnityEditor;
using UnityEngine;

namespace DependencyInjectorEditor
{
    [CustomEditor(typeof(MonoInjector))]
    public class MonoInjectorCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (DrawButton("  Open Dependency Graph", "BlendTree Icon", 15, 30))
                DependencyGraphWindow.OpenDependencyGraphWindow();
            
            DrawProperties();
            DrawInstalledBox();
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
            DrawInjectors();
            DrawSettings();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawInstallers()
        {
            string title = "Installers";
            DrawTitle(title);
            
            if (DrawButton("  Load Installers", "d_ol_plus", 12, 20))
                FillInstallersUsingChildren();
            
            EditorGUI.BeginDisabledGroup(true);
            
            EditorGUI.indentLevel++;
            SerializedProperty serializedProperty = serializedObject.FindProperty("_monoInstallers");
            EditorGUILayout.PropertyField(serializedProperty);
            
            EditorGUI.EndDisabledGroup();
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

        private void FillInstallersUsingChildren()
        {
            MonoInjector monoInjector = (MonoInjector)target;
            MonoInstaller[] monoInstallers = monoInjector.GetComponentsInChildren<MonoInstaller>();

            Undo.RecordObject(monoInjector, "LoadInstallers");
            
            monoInjector.SetInstallers(monoInstallers);
            
            EditorUtility.SetDirty(monoInjector);

            UpdateChildrenNames();
        }
        
        private void UpdateChildrenNames()
        {
            MonoInjector monoInjector = (MonoInjector)target;
            MonoInstaller[] monoInstallers = monoInjector.GetComponentsInChildren<MonoInstaller>();

            foreach (var monoInstaller in monoInstallers)
            {
                if (monoInstaller == null) 
                    continue;
                
                string gameObjectName = monoInstaller.GetType().Name;
                gameObjectName = gameObjectName.Replace("Installer", "");
                monoInstaller.gameObject.name = gameObjectName;
                    
                EditorUtility.SetDirty(monoInstaller);
            }
            
            EditorUtility.SetDirty(monoInjector);
        }

        private void DrawInjectors()
        {
            string title = "Injectors";
            DrawTitle(title);
            
            GUILayout.Label("Drag others MonoInjectors to use their DiContainers and look references into it");

            
            SerializedProperty serializedProperty = serializedObject.FindProperty("_monoInjectors");
            EditorGUILayout.PropertyField(serializedProperty);
        }
        
        private void DrawSettings()
        {
            string title = "Settings";
            DrawTitle(title);
            
            EditorGUI.indentLevel--;
            SerializedProperty serializedProperty = serializedObject.FindProperty("_hasInstallInGlobalDiContainer");
            EditorGUILayout.PropertyField(serializedProperty, new GUIContent("Install in Global DIContainer"));
            
            SerializedProperty hasToUseGlobalDiContainerSerializedProperty = serializedObject.FindProperty("_hasToUseGlobalDiContainer");
            EditorGUILayout.PropertyField(hasToUseGlobalDiContainerSerializedProperty, new GUIContent("Use Global DIContainer"));
            
            SerializedProperty _hasToDisposeGlobalDiContainerSerializedProperty = serializedObject.FindProperty("_hasToDisposeGlobalDiContainer");
            EditorGUILayout.PropertyField(_hasToDisposeGlobalDiContainerSerializedProperty, new GUIContent("Dispose Global DI Container"));
        }
        
        private void DrawInstalledBox()
        {
            if (!Application.isPlaying) 
                return;
            
            GUILayout.Space(10);
            
            MonoInjector monoInjector = target as MonoInjector;
            bool isInstalled = monoInjector.IsInstalled;
            
            Color color = isInstalled ? Color.green : Color.red;
            string installedText = isInstalled ? "installed" : "not Installed"; 
            
            GUI.color = color;
            EditorGUILayout.HelpBox("It's " + installedText, MessageType.Info);
            GUI.color = Color.white;
        }
    }
}