using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueNodeStart : DialogueNode
    {
        public Port outputPort;
        public string nextNodeID;

        public override void Init(GraphView graphViewRef, Vector2 position)
        {
            base.Init(graphViewRef, position);
            dialogueTitle = "Start Dialogue";
            dialogueType = DialogueNodeType.START;
        }

        public override void InitFromAsset(DialogueNodeAsset asset, GraphView graphViewRef)
        {
            SingleNodeAsset singleNodeAsset = asset as SingleNodeAsset;
            dialogueTitle = "Start Dialogue";
            dialogueType = DialogueNodeType.START;
            graphView = graphViewRef;
            nodeID = singleNodeAsset.nodeID;
            nextNodeID = singleNodeAsset.nextNodeID;
            SetPosition(singleNodeAsset.position);

            InitStyles();
            Draw();
        }

        public override void Draw()
        {
            // Title
            TextField titleTextField = new TextField();
            titleTextField.value = dialogueTitle;
            titleTextField.AddToClassList("de-node__text-field");
            titleTextField.AddToClassList("de-node__title-text-field");
            titleTextField.AddToClassList("de-node__text-field__hidden");
            titleTextField.isReadOnly = true;
            titleContainer.Insert(0, titleTextField);

            // Output Port
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = "Next Dialogue";
            outputContainer.Add(outputPort);

            RefreshExpandedState();
        }

        public string GetFirstNodeID()
        {
            // Get next DialogueNode
            var edges = outputPort.connections;
            string firstNodeID = "";

            foreach (Edge edge in edges)
            {
                Port nextNodeInputPort = edge.input;
                DialogueNode nextNode = nextNodeInputPort.parent.GetFirstOfType<DialogueNode>();
                firstNodeID = nextNode.nodeID;
            }

            return firstNodeID;
        }

        public override DialogueNodeAsset Save()
        {
            SingleNodeAsset asset = ScriptableObject.CreateInstance<SingleNodeAsset>();
            asset.title = dialogueTitle;
            asset.type = dialogueType;
            asset.nodeID = nodeID;
            asset.nextNodeID = GetFirstNodeID();
            asset.position = GetPosition();

            return asset;
        }
    }
}