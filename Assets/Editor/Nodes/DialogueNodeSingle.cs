using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace DialogueEditor
{
    public class DialogueNodeSingle : DialogueNode
    {
        private Port outputPort;

        public override void Init(GraphView graphViewRef, Vector2 position)
        {
            dialogueType = DialogueNodeType.SINGLE;
            base.Init(graphViewRef, position);
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
            asset.title = dialogueTitle;
            asset.text = dialogueText;

            // Get next DialogueNode
            var edges = outputPort.connections;
            foreach (Edge edge in edges)
            {
                Port nextNodeInputPort = edge.input;
                DialogueNode nextNode = nextNodeInputPort.parent.GetFirstOfType<DialogueNode>();
                if (nextNode.dialogueType != DialogueNodeType.END)
                {
                    asset.nextNode = nextNode.Save();
                }
            }

            return asset;
        }
    }
}