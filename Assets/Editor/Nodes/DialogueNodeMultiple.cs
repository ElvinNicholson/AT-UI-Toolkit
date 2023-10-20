using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueNodeMultiple : DialogueNode
    {
        public override void Init(Vector2 position)
        {
            base.Init(position);
        }

        public override void Draw()
        {
            base.Draw();

            // Add Choice Button
            Button addChoiceButton = new Button();
            addChoiceButton.text = "Add Dialogue Reply";
            addChoiceButton.clicked += AddNewReply;
            addChoiceButton.AddToClassList("de-node__button");

            extensionContainer.Add(addChoiceButton);

            // Replies
            AddNewReply();

            RefreshExpandedState();
        }

        private void AddChoiceElement(string dialogue)
        {
            // Horizontal Container
            VisualElement textVisualElement = new VisualElement();
            textVisualElement.style.flexDirection = FlexDirection.Row;
            textVisualElement.AddToClassList("de-node__visual-element");

            // Remove Button
            Button removeButton = new Button();
            removeButton.text = "x";
            removeButton.AddToClassList("de-node__button");

            // Dialogue Reply
            TextField replyTextField = new TextField();
            replyTextField.AddToClassList("de-node__text-field");
            replyTextField.AddToClassList("de-node__dialogue-text-field");
            replyTextField.AddToClassList("de-node__reply-text-field");
            replyTextField.AddToClassList("de-node__text-field__hidden");
            replyTextField.value = dialogue;

            // Output Port
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = "";
            outputPort.AddToClassList("de-node__output-port");

            textVisualElement.Add(removeButton);
            textVisualElement.Add(replyTextField);
            textVisualElement.Add(outputPort);
            extensionContainer.Add(textVisualElement);
        }

        private void AddNewReply()
        {
            replyList.Add("Dialogue Reply");
            AddChoiceElement(replyList[replyList.Count - 1]);
        }
    }
}
