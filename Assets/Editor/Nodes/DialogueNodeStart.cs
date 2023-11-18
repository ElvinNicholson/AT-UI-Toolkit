using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueNodeStart : DialogueNode
    {
        private Port outputPort;

        public override void Init(GraphView graphViewRef, Vector2 position)
        {
            dialogueType = DialogueNodeType.START;
            dialogueTitle = "Start Dialogue";
            graphView = graphViewRef;
            SetPosition(new Rect(position, Vector2.zero));

            titleContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
            outputContainer.style.backgroundColor = new StyleColor(new Color(0.15f, 0.15f, 0.15f));
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

        public override DialogueNodeAsset Save()
        {
            // Get next DialogueNode
            var edges = outputPort.connections;
            DialogueNodeAsset asset = null;

            foreach (Edge edge in edges)
            {
                Port nextNodeInputPort = edge.input;
                DialogueNode nextNode = nextNodeInputPort.parent.GetFirstOfType<DialogueNode>();
                asset = nextNode.Save();
            }

            return asset;
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
    }
}