using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace DependencyInjectorEditor
{
    public class NodeDependencies
    {
        private string _typeName;
        private HashSet<string> _dependencyNames;
        private Node _node;
        
        public string TypeName => _typeName;
        public HashSet<string> DependencyNames => _dependencyNames;
        public Node NodeData => _node;
        
        public NodeDependencies(string typeName, HashSet<string> dependencyNames, Node node)
        {
            _typeName = typeName;
            _dependencyNames = dependencyNames;
            _node = node;
        }
    }
}