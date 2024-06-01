using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DependencyInjectorEditor
{
    public class DependencyNode : Node
    {
        public string GUID;
        public string Text;
        public bool EntryPoint = false;

        public DependencyNode(string uiFile) : base(uiFile)
        {
        }
    }
}