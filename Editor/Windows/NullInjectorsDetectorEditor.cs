using System.Collections.Generic;
using DependencyInjector.Installers;
using UnityEditor;
using UnityEngine;

namespace DependencyInjectorEditor
{
    public class NullInjectorsDetectorEditor : EditorWindow
    {
        private List<GameObject> _nullMonoInjectorsGameObjects;
        
        [MenuItem("Tools/Null Injectors Detector")]
        public static void ShowWindow()
        {
            GetWindow(typeof(EditorWindow));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Search")) 
                SearchNullInjectors();

            if (_nullMonoInjectorsGameObjects is { Count: >= 0 })
                DrawNullMonoInjectors();
        }

        private void SearchNullInjectors()
        {
            _nullMonoInjectorsGameObjects = new List<GameObject>(); 
                
            MonoInjector[] monoInjectors = FindObjectsByType<MonoInjector>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);

            foreach (var monoInjector in monoInjectors)
            {
                MonoInstaller[] monoInstallers = monoInjector.MonoInstallers;

                foreach (var monoInstaller in monoInstallers)
                {
                    if (monoInstaller != null) 
                        continue;
                    
                    _nullMonoInjectorsGameObjects.Add(monoInjector.gameObject);
                    break;
                }
            }
        }
        
        private void DrawNullMonoInjectors()
        {
            foreach (var gameObject in _nullMonoInjectorsGameObjects)
            {
                if (GUILayout.Button(gameObject.name))
                {
                    Selection.activeGameObject = gameObject;
                }
            }
        }
    }
}