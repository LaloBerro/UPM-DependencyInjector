using UnityEditor.Experimental.GraphView;

namespace DependencyInjectorEditor
{
    public class DependencyNode : Node
    {
        public string GUID;
        public string Text;
        public bool EntryPoint = false;
    }
}