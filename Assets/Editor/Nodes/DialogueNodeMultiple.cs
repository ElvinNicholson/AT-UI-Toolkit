using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueNodeMultiple : DialogueNode
    {
        private List<Port> outputPortList;

        public override void Init(GraphView graphViewRef, Vector2 position)
        {
            base.Init(graphViewRef, position);

            outputPortList = new List<Port>();
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
            outputPortList.Add(outputPort);
            removeButton.clicked += () => RemoveReply(outputPort);

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

        private void RemoveReply(Port port)
        {
            graphView.DeleteElements(port.connections);
            outputPortList.Remove(port);
            extensionContainer.Remove(port.parent);
        }

        public override void DisconnectAllPort()
        {
            DisconnectPorts(inputContainer);
            foreach (Port port in outputPortList)
            {
                graphView.DeleteElements(port.connections);
            }
        }

        public override void Save()
        {
            base.Save();
        }
    }
}
