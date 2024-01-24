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

            if (GUILayout.Button("Use Children"))
                FillInstallerUsingChild();
        }

        private void FillInstallerUsingChild()
        {
            MonoInjector monoInjector = (MonoInjector)target;
            MonoInstaller[] monoInstallers = monoInjector.GetComponentsInChildren<MonoInstaller>();

            monoInjector.SetInstallers(monoInstallers);
        }
    }
}