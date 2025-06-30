using DependencyInjector.Installers;
using UnityEditor;

namespace DependencyInjectorEditor.DependencyInjectorEditor
{
    [CustomEditor(typeof(AwakeInjector))]
    public class AwakeInjectorCustomInspector : Editor
    {
        public void OnEnable()
        {
            AwakeInjector awakeInjector = (AwakeInjector)target;
            InjectorsInitializer injectorsInitializer = awakeInjector.GetComponent<InjectorsInitializer>();

            if (!injectorsInitializer) 
                return;
            
            awakeInjector.SetBaseMonoInjector(injectorsInitializer);
            EditorUtility.SetDirty(awakeInjector);
        }
    }
}