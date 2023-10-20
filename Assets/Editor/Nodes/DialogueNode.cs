using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public enum DialogueNodeType
    {
        SINGLE,
        MULTIPLE
    }

    public class DialogueNode : Node
    {
        public string dialogueTitle { get; set; }
        public string dialogueText { get; set; }
        public List<string> replyList { get; set; }

        public virtual void Init(Vector2 position)
        {
            dialogueTitle = "Dialogue Title";
            dialogueText = "Dialogue Text";
            replyList = new List<string>();

            SetPosition(new Rect(position, Vector2.zero));

            extensionContainer.style.backgroundColor = new StyleColor(new Color(0.1f, 0.1f, 0.1f));
            extensionContainer.style.paddingTop = 8;
            extensionContainer.style.paddingLeft = 8;
            extensionContainer.style.paddingRight = 8;
            extensionContainer.style.paddingBottom = 8;
            titleContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
            inputContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f));
            outputContainer.style.backgroundColor = new StyleColor(new Color(0.15f, 0.15f, 0.15f));
        }

        public virtual void Draw()
        {
            // Title
            TextField titleTextField = new TextField();
            titleTextField.value = dialogueTitle;
            titleTextField.AddToClassList("de-node__text-field");
            titleTextField.AddToClassList("de-node__title-text-field");
            titleTextField.AddToClassList("de-node__text-field__hidden");
            titleContainer.Insert(0, titleTextField);

            // Input Port
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Previous Dialogue";
            inputContainer.Add(inputPort);

            // Dialogue Text
            TextField dialogueTextField = new TextField();
            dialogueTextField.value = dialogueText;
            dialogueTextField.AddToClassList("de-node__text-field");
            dialogueTextField.AddToClassList("de-node__dialogue-text-field");
            dialogueTextField.AddToClassList("de-node__text-field__hidden");
            extensionContainer.Add(dialogueTextField);
        }
    }
}
