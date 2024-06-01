using System;
using System.Collections.Generic;
using System.Reflection;
using DependencyInjector.Core;
using DependencyInjector.Installers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DependencyInjectorEditor
{
    public class DependenciesGraphView : GraphView
    {
        private readonly Vector2 _defaultNodeSize = new(150, 200);
        private List<GraphElement> _visualElements;
        private MonoInjector _monoInjector;
        
        public void SetMonoInjector(MonoInjector monoInjector)
        {
            _monoInjector = monoInjector;
            
            SetupGraph();
            SetupNodes();
            CreateGroup();
            CreateGrid();
        }

        private void SetupGraph()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("DependencyGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
        }

        private void SetupNodes()
        {
            _visualElements = new List<GraphElement>();
            MonoInstaller[] installers = _monoInjector.GetComponentsInChildren<MonoInstaller>();
            Rect position = new Rect(300, 200, 100, 150);
            List<NodeDependencies> _nodeDependencies = new List<NodeDependencies>();

            foreach (var installer in installers)
            {
                MethodInfo getDataMethod = null;
                Type currentType = installer.GetType();
                while (true)
                {
                    MethodInfo[] methodInfos = currentType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance |
                                                                      BindingFlags.DeclaredOnly | BindingFlags.Default |
                                                                      BindingFlags.Public);
                    foreach (var methodInfo in methodInfos)
                    {
                        if (methodInfo.Name.Equals("GetData"))
                        {
                            getDataMethod = methodInfo;
                            break;
                        }
                    }

                    if (!ReferenceEquals(getDataMethod, null))
                        break;

                    currentType = currentType.BaseType;
                    
                    if(ReferenceEquals(currentType, null) || currentType == typeof(MonoBehaviour))
                        break;
                }
                
                string typeName = getDataMethod?.ReturnType.Name;
                string displayTypeName = typeName;
                
                Type[] genericTypesArguments = getDataMethod?.ReturnType.GetGenericArguments();
                
                if (genericTypesArguments != null && genericTypesArguments.Length > 0)
                {
                    displayTypeName = typeName.Remove(typeName.Length - 2);
                    displayTypeName += "<" + genericTypesArguments[0].Name + ">";
                }

                string installerName = installer.gameObject.name;
                installerName = installerName.Replace("Installer", "");

                string nodeName = "<b>" + displayTypeName + "</b>";

                if (!ReferenceEquals(getDataMethod, null) && getDataMethod.ReturnType.IsInterface)
                    nodeName = "<b><color=#dbfc03>" + displayTypeName + "</color></b>";
                else if (!ReferenceEquals(getDataMethod, null) && getDataMethod.ReturnType.IsAbstract && getDataMethod.ReturnType.IsClass)
                    nodeName = "<b><color=#435bd1>" + displayTypeName + "</color></b>";
                
                if(!string.Equals(displayTypeName, installerName))
                    nodeName += "\n" + installerName;
                
                DependencyNode dependencyNodeElement = CreateDependencyNode(nodeName);
                dependencyNodeElement.SetPosition(position);
                position.position += new Vector2(-150 , 100);
                
                AddElement(dependencyNodeElement);

                _visualElements.Add(dependencyNodeElement);

                NodeDependencies nodeDependencies = GetNodeDependencies(installer, typeName, dependencyNodeElement);
                _nodeDependencies.Add(nodeDependencies);
            }

            LinkDependencyNodes(_nodeDependencies);
        }
        
        private DependencyNode CreateDependencyNode(string nodeName)
        {
            VisualTreeAsset uiFile = Resources.Load<VisualTreeAsset>("DependencyNode");
            string path = AssetDatabase.GetAssetPath(uiFile);
            DependencyNode dependencyNode = new DependencyNode(path);
            dependencyNode.title = nodeName;
            dependencyNode.Text = nodeName;
            dependencyNode.GUID = Guid.NewGuid().ToString();

            Port inputPort = GeneratePort(dependencyNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "";
            dependencyNode.inputContainer.Add(inputPort);
            
            Port generatedPort = GeneratePort(dependencyNode, Direction.Output);
            generatedPort.portName = "Needs";
            dependencyNode.outputContainer.Add(generatedPort);
            
            dependencyNode.RefreshExpandedState();
            dependencyNode.RefreshPorts();
            
            dependencyNode.SetPosition(new Rect(Vector2.zero, _defaultNodeSize));

            return dependencyNode;
        }
        
        private Port GeneratePort(DependencyNode dependencyNode, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return dependencyNode.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }
        
        private void LinkDependencyNodes(List<NodeDependencies> _nodeDependencies)
        {
            foreach (var nodeDependency in _nodeDependencies)
            {
                HashSet<string> dependencyNames = nodeDependency.DependencyNames;

                foreach (var dependencyName in dependencyNames)
                {
                    foreach (var nodeDependencyInside in _nodeDependencies)
                    {
                        string typeName = nodeDependencyInside.TypeName;
                        if (!string.Equals(dependencyName, typeName))
                            continue;

                        LinkNodes(nodeDependency.NodeData.outputContainer[0].Q<Port>(), nodeDependencyInside.NodeData.inputContainer[0].Q<Port>());
                    }
                }
            }
        }
        
        private void LinkNodes(Port output, Port input)
        {
            Edge edge = new Edge();
            edge.output = output;
            edge.input = input;
            
            edge.input.Connect(edge);
            edge.output.Connect(edge);
            
            edge.input.styleSheets.Add(Resources.Load<StyleSheet>("Port"));
            edge.output.styleSheets.Add(Resources.Load<StyleSheet>("Port"));
            
            edge.styleSheets.Add(Resources.Load<StyleSheet>("Edge"));
            
            Add(edge);
        }

        private static NodeDependencies GetNodeDependencies(MonoInstaller installer, string typeName, Node node)
        {
            HashSet<string> dependenciesNames = new HashSet<string>();
            
            List<FieldInfo> fields = new List<FieldInfo>();

            Type currentType = installer.GetType();
            while (true)
            {
                FieldInfo[] fieldInfos = currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                
                fields.AddRange(fieldInfos);

                currentType = currentType.BaseType;
                
                if(ReferenceEquals(currentType, null) || currentType == typeof(MonoBehaviour))
                    break;
            }

            foreach (var fieldInfo in fields)
            {
                object attribute = fieldInfo.GetCustomAttribute<InjectAttribute>();
                if (ReferenceEquals(attribute, null))
                    continue;

                dependenciesNames.Add(fieldInfo.FieldType.Name);
            }

            NodeDependencies nodeDependencies = new NodeDependencies(typeName, dependenciesNames, node);
            return nodeDependencies;
        }

        private void CreateGroup()
        {
            Group group = new Group();
            group.title = _monoInjector.gameObject.name;
            group.AddElements(_visualElements);
            AddElement(group);
        }
        
        private void CreateGrid()
        {
            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
        }
    }
}