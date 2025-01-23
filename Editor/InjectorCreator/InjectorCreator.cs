using DependencyInjector.Installers;
using UnityEditor;
using UnityEngine;

namespace DependencyInjectorEditor
{
    public class InjectorCreator : Editor
    {
        private static void CreateNewGameObject<TComponent>() where TComponent : MonoBehaviour
        {
            GameObject newGameObject = new GameObject(typeof(TComponent).Name);
            TComponent component = newGameObject.AddComponent<TComponent>();
            
            if(Selection.activeGameObject != null)
                component.transform.SetParent(Selection.activeGameObject.transform);
            
            Selection.activeGameObject = component.gameObject;
        }
        
        [MenuItem("GameObject/DependencyInjection/MonoInjector", false, 1)]
        private static void CreateMonoInjector()
        {
            CreateNewGameObject<MonoInjector>();
        }
        
        [MenuItem("GameObject/DependencyInjection/InjectorsInitializer", false, 1)]
        private static void CreateInjectorInitializer()
        {
            CreateNewGameObject<InjectorsInitializer>();
        }
    }
}