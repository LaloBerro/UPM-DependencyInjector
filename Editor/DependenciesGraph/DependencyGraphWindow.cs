using System;
using DependencyInjector.Installers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DependencyInjectorEditor
{
    public class DependencyGraphWindow : EditorWindow
    {
        private DependenciesGraphView _dependenciesGraphView;
        private MonoInjector _monoInjector;
        
        [MenuItem("Tools/DependenciesGraphView")]
        public static void OpenDependencyGraphWindow()
        {
            DependencyGraphWindow window = GetWindow<DependencyGraphWindow>();
            window.titleContent = new GUIContent("DependencyGraph");
        }

        private void OnGUI()
        {
            if(!ReferenceEquals(_monoInjector, null))
                return;
            
            GUI.color = Color.red;
            GUILayout.Label("Select an Injector in the hierarchy");
        }

        private void OnEnable()
        {
            OpenGraph();
        }

        private void OnSelectionChange()
        {
            OpenGraph();
        }

        private void OpenGraph()
        {
            GameObject selection = Selection.activeGameObject;

            _monoInjector = selection?.GetComponent<MonoInjector>();
            if (ReferenceEquals(_monoInjector, null))
                return;

            if (ReferenceEquals(_dependenciesGraphView, null))
                InitDependencyGraph();

            _dependenciesGraphView.SetMonoInjector(_monoInjector);
        }

        private void InitDependencyGraph()
        {
            _dependenciesGraphView = new DependenciesGraphView();
            _dependenciesGraphView.name = "Dependencies Graph";
            _dependenciesGraphView.StretchToParentSize();

            rootVisualElement.Add(_dependenciesGraphView);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_dependenciesGraphView);
        }
    }
}