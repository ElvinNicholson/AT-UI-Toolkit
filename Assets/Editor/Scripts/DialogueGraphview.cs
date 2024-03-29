using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using System;

namespace DialogueEditor
{
    public class DialogueGraphview : GraphView
    {
        public DialogueGraphview()
        {
            AddStyleSheet();
            AddBackground();
            AddControls();

            OnElementsDeleted();

            graphViewChanged += OnGraphViewChanged;
        }

        private void AddStyleSheet()
        {
            StyleSheet graphViewStyleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueGraphview.uss");
            StyleSheet nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueNode.uss");

            styleSheets.Add(graphViewStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }

        private void AddBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddControls()
        {
            // Middle click drag
            this.AddManipulator(new ContentDragger());

            // Scroll wheel zoom
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // Right click window
            ContextualMenuManipulator singleContextMenu = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Dialogue Single Node", 
                actionEvent => AddElement(AddNode(DialogueNodeType.SINGLE, 
                viewTransform.matrix.inverse.MultiplyPoint(actionEvent.eventInfo.localMousePosition)))));
            this.AddManipulator(singleContextMenu);

            ContextualMenuManipulator multipleContextMenu = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Dialogue Reply Node", 
                actionEvent => AddElement(AddNode(DialogueNodeType.REPLY, 
                viewTransform.matrix.inverse.MultiplyPoint(actionEvent.eventInfo.localMousePosition)))));
            this.AddManipulator(multipleContextMenu);

            ContextualMenuManipulator startContextMenu = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Dialogue Start Node",
                actionEvent => AddElement(AddNode(DialogueNodeType.START,
                viewTransform.matrix.inverse.MultiplyPoint(actionEvent.eventInfo.localMousePosition)))));
            this.AddManipulator(startContextMenu);

            ContextualMenuManipulator endContextMenu = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Dialogue End Node",
                actionEvent => AddElement(AddNode(DialogueNodeType.END,
                viewTransform.matrix.inverse.MultiplyPoint(actionEvent.eventInfo.localMousePosition)))));
            this.AddManipulator(endContextMenu);

            // Dragging nodes
            this.AddManipulator(new SelectionDragger());

            // Rect select
            this.AddManipulator(new RectangleSelector());
        }

        private DialogueNode AddNode(DialogueNodeType type, Vector2 position)
        {
            DialogueNode newNode;

            switch(type)
            {
                case DialogueNodeType.SINGLE:
                    newNode = new DialogueNodeSingle();
                    break;

                case DialogueNodeType.REPLY:
                    newNode = new DialogueNodeMultiple();
                    break;

                case DialogueNodeType.START:
                    if (!StartNodeExist())
                    {
                        newNode = new DialogueNodeStart();
                    }
                    else
                    {
                        Debug.LogError("Start Node already exists");
                        return null;
                    }
                    break;

                case DialogueNodeType.END:
                    newNode = new DialogueNodeEnd();
                    break;

                default:
                    return null;
            }

            newNode.Init(this, position);
            newNode.SetColorRed();
            newNode.Draw();

            return newNode;
        }

        public void Save(string filename)
        {
            List<DialogueNode> nodes = this.Query<DialogueNode>().ToList();

            foreach (DialogueNode node in nodes)
            {
                if (!node.IsAllPortConnected())
                {
                    Debug.LogError("Cannot save when there is an unconnected node.");
                    return;
                }
            }

            MainDialogueAsset mainAssetInstance = ScriptableObject.CreateInstance<MainDialogueAsset>();
            AssetDatabase.CreateAsset(mainAssetInstance, $"Assets/Dialogue Assets/{filename}.asset");

            string firstNodeID = "";

            // Save DialogueNodeAssets to dialogueNodeAssets List
            foreach (DialogueNode node in nodes)
            {
                switch(node.dialogueType)
                {
                    case DialogueNodeType.SINGLE:
                    case DialogueNodeType.REPLY:
                    case DialogueNodeType.END:
                        mainAssetInstance.dialogueNodeAssets.Add(node.Save());
                        break;

                    case DialogueNodeType.START:
                        DialogueNodeStart startNode = node as DialogueNodeStart;
                        firstNodeID = startNode.GetFirstNodeID();
                        mainAssetInstance.dialogueNodeAssets.Add(node.Save());
                        break;
                }
            }

            // Set reference to nextNode
            foreach (DialogueNodeAsset nodeAsset in mainAssetInstance.dialogueNodeAssets)
            {
                // Write nodeAsset to mainAssetInstance
                AssetDatabase.AddObjectToAsset(nodeAsset, mainAssetInstance);

                if (firstNodeID == nodeAsset.nodeID)
                {
                    mainAssetInstance.firstNode = nodeAsset;
                }

                switch (nodeAsset.type)
                {
                    case DialogueNodeType.SINGLE:
                        SingleNodeAsset singleNodeAsset = nodeAsset as SingleNodeAsset;
                        foreach (DialogueNodeAsset nextNodeAsset in mainAssetInstance.dialogueNodeAssets)
                        {
                            if (singleNodeAsset.nextNodeID == nextNodeAsset.nodeID)
                            {
                                singleNodeAsset.nextNode = nextNodeAsset;
                                break;
                            }
                        }
                        break;

                    case DialogueNodeType.REPLY:
                        ReplyNodeAsset replyNodeAsset = nodeAsset as ReplyNodeAsset;
                        foreach (ReplyData replyData in replyNodeAsset.replies)
                        {
                            foreach (DialogueNodeAsset nextNodeAsset in mainAssetInstance.dialogueNodeAssets)
                            {
                                if (replyData.nextNodeID == nextNodeAsset.nodeID)
                                {
                                    replyData.nextNode = nextNodeAsset;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }

            // Save asset
            AssetDatabase.SaveAssets();
        }

        public void Load(MainDialogueAsset mainDialogueAsset)
        {
            // Clear current graphView
            ClearNodesAndEdges();

            foreach (DialogueNodeAsset asset in mainDialogueAsset.dialogueNodeAssets)
            {
                AddNodeFromAsset(asset);
            }

            // Create port connections
            List<DialogueNode> nodes = this.Query<DialogueNode>().ToList();
            foreach (DialogueNode node in nodes)
            {
                switch (node.dialogueType)
                {
                    case DialogueNodeType.SINGLE:
                        DialogueNodeSingle singleNode = node as DialogueNodeSingle;

                        foreach (DialogueNode nextNode in nodes)
                        {
                            if (singleNode.nextNodeID == nextNode.nodeID)
                            {
                                ConnectPorts(singleNode.outputPort, nextNode.inputPort);
                                break;
                            }
                        }
                        break;

                    case DialogueNodeType.REPLY:
                        DialogueNodeMultiple replyNode = node as DialogueNodeMultiple;

                        for (int i = 0; i < replyNode.nextNodeIDs.Count; i++)
                        {
                            foreach (DialogueNode nextNode in nodes)
                            {
                                if (replyNode.nextNodeIDs[i] == nextNode.nodeID)
                                {
                                    ConnectPorts(replyNode.outputPortList[i], nextNode.inputPort);
                                    break;
                                }
                            }
                        }
                        break;

                    case DialogueNodeType.START:
                        DialogueNodeStart startNode = node as DialogueNodeStart;

                        foreach (DialogueNode nextNode in nodes)
                        {
                            if (startNode.nextNodeID == nextNode.nodeID)
                            {
                                ConnectPorts(startNode.outputPort, nextNode.inputPort);
                                break;
                            }
                        }
                        break;
                }
            }
        }

        private void ConnectPorts(Port fromPort, Port toPort)
        {
            Edge edge = fromPort.ConnectTo(toPort);
            edge.output = fromPort;
            edge.input = toPort;

            fromPort.Connect(edge);
            toPort.Connect(edge);

            AddElement(edge);
        }

        private void AddNodeFromAsset(DialogueNodeAsset asset)
        {
            DialogueNode newNode;

            switch (asset.type)
            {
                case DialogueNodeType.SINGLE:
                    newNode = new DialogueNodeSingle();
                    break;

                case DialogueNodeType.REPLY:
                    newNode = new DialogueNodeMultiple();
                    break;

                case DialogueNodeType.START:
                    newNode = new DialogueNodeStart();
                    break;

                case DialogueNodeType.END:
                    newNode = new DialogueNodeEnd();
                    break;

                default:
                    return;
            }

            newNode.InitFromAsset(asset, this);

            AddElement(newNode);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port || startPort.node == port.node || startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                List<DialogueNode> nodesToDelete = new List<DialogueNode>();

                foreach (GraphElement element in selection)
                {
                    if (element is DialogueNode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }
                }

                foreach (DialogueNode node in nodesToDelete)
                {
                    node.DisconnectAllPort();
                    RemoveElement(node);
                }
            };
        }

        private void ClearNodesAndEdges()
        {
            List<GraphElement> elementList = graphElements.ToList();
            foreach (GraphElement element in elementList)
            {
                if (element is Node || element is Edge)
                {
                    RemoveElement(element);
                }
            }
        }

        private bool StartNodeExist()
        {
            List<DialogueNode> nodes = this.Query<DialogueNode>().ToList();
            foreach (DialogueNode node in nodes)
            {
                if (node.dialogueType == DialogueNodeType.START)
                {
                    return true;
                }
            }
            return false;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                foreach (Edge edge in change.edgesToCreate)
                {
                    edge.input.parent.GetFirstOfType<DialogueNode>().OnPortConnect(PortType.INPUT);
                    edge.output.parent.GetFirstOfType<DialogueNode>().OnPortConnect(PortType.OUTPUT);
                }
            }

            if (change.elementsToRemove != null)
            {
                foreach (GraphElement element in change.elementsToRemove)
                {
                    if (element.GetType() == typeof(Edge))
                    {
                        Edge edge = element as Edge;
                        edge.input.parent.GetFirstOfType<DialogueNode>().OnPortDisconnect(PortType.INPUT);
                        edge.output.parent.GetFirstOfType<DialogueNode>().OnPortDisconnect(PortType.OUTPUT);
                    }
                }
            }

            return change;
        }
    }
}
