using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace DialogueEditor
{
    public class DialogueNodeSingle : DialogueNode
    {
        public Port outputPort;
        public string nextNodeID;

        public override void Init(GraphView graphViewRef, Vector2 position)
        {
            dialogueType = DialogueNodeType.SINGLE;
            base.Init(graphViewRef, position);
        }

        public override void InitFromAsset(DialogueNodeAsset asset, GraphView graphViewRef)
        {
            SingleNodeAsset singleNodeAsset = asset as SingleNodeAsset;
            dialogueTitle = singleNodeAsset.title;
            dialogueText = singleNodeAsset.text;
            dialogueType = DialogueNodeType.SINGLE;
            graphView = graphViewRef;
            nodeID = singleNodeAsset.nodeID;
            nextNodeID = singleNodeAsset.nextNodeID;
            SetPosition(singleNodeAsset.position);

            InitStyles();
            Draw();
        }

        public override void Draw()
        {
            base.Draw();

            // Output Port
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = "Next Dialogue";
            outputContainer.Add(outputPort);

            RefreshExpandedState();
        }

        public override DialogueNodeAsset Save()
        {
            // Create Asset
            SingleNodeAsset asset = ScriptableObject.CreateInstance<SingleNodeAsset>();
            asset.name = dialogueTitle;
            asset.title = dialogueTitle;
            asset.text = dialogueText;
            asset.type = dialogueType;
            asset.nodeID = nodeID;
            asset.position = GetPosition();

            // Get next DialogueNode
            var edges = outputPort.connections;
            foreach (Edge edge in edges)
            {
                Port nextNodeInputPort = edge.input;
                DialogueNode nextNode = nextNodeInputPort.parent.GetFirstOfType<DialogueNode>();
                asset.nextNodeID = nextNode.nodeID;
            }

            return asset;
        }
    }
}