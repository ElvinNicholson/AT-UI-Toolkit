using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueNodeEnd : DialogueNode
    {
        public override void Init(GraphView graphViewRef, Vector2 position)
        {
            dialogueType = DialogueNodeType.END;
            dialogueTitle = "End Dialogue";
            graphView = graphViewRef;
            SetPosition(new Rect(position, Vector2.zero));

            titleContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
            inputContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
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
    }
}
