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
            base.OnInspectorGUI();

            if (GUILayout.Button("Fill with child"))
            {
                MonoInstaller[] monoInstallers = ((MonoBehaviour)target).GetComponentsInChildren<MonoInstaller>();
                
                ((MonoInjector)target).SetInstallers(monoInstallers);
            }
        }
    }
}