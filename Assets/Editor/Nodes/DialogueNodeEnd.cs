using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;

namespace DialogueEditor
{
    public class DialogueNodeEnd : DialogueNode
    {
        public override void Init(GraphView graphViewRef, Vector2 position)
        {
            base.Init(graphViewRef, position);
            dialogueTitle = "End Dialogue";
            dialogueType = DialogueNodeType.END;
        }

        public override void InitFromAsset(DialogueNodeAsset asset, GraphView graphViewRef)
        {
            dialogueTitle = "End Dialogue";
            dialogueType = DialogueNodeType.END;
            graphView = graphViewRef;
            nodeID = asset.nodeID;
            SetPosition(asset.position);

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
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Previous Dialogue";
            outputContainer.Add(inputPort);

            RefreshExpandedState();
        }

        public override DialogueNodeAsset Save()
        {
            DialogueNodeAsset asset = ScriptableObject.CreateInstance<DialogueNodeAsset>();
            asset.name = dialogueTitle;
            asset.title = dialogueTitle;
            asset.type = dialogueType;
            asset.nodeID = nodeID;
            asset.position = GetPosition();

            return asset;
        }

        public override void OnPortConnect(PortType portType)
        {
            titleContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
        }

        public override void OnPortDisconnect(PortType portType)
        {
            if (inputPort.connections.Count() > 1)
            {
                return;
            }

            titleContainer.style.backgroundColor = new StyleColor(new Color(0.8f, 0.2f, 0.2f));
        }

        public override bool IsAllPortConnected()
        {
            return inputPort.connected;
        }
    }
}
