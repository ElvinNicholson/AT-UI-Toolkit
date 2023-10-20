using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

namespace DialogueEditor
{
    public class DialogueNode : Node
    {
        public string dialogueTitle { get; set; }
        public string dialogueText { get; set; }
        public List<string> choiceList { get; set; }

        public virtual void Init(Vector2 position)
        {
            dialogueTitle = "Dialogue Title";
            dialogueText = "Dialogue Text";

            choiceList = new List<string>();
            choiceList.Add("Dialogue Choice");

            SetPosition(new Rect(position, Vector2.zero));

            extensionContainer.AddToClassList("de-node__extension-container");
        }

        public virtual void Draw()
        {
            // Title
            TextField titleTextField = new TextField();
            titleTextField.value = dialogueTitle;
            titleTextField.AddToClassList("de-node__text-field");
            titleTextField.AddToClassList("de-node__name-text-field");
            titleTextField.AddToClassList("de-node__text-field__hidden");
            titleContainer.Insert(0, titleTextField);

            // Input Port
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Previous Dialogue";
            inputContainer.Add(inputPort);

            // Dialogue Text
            VisualElement bodyVisualElement = new VisualElement();
            bodyVisualElement.style.flexDirection = FlexDirection.Column;
            bodyVisualElement.AddToClassList("de-node__visual-element");

            TextField dialogueTextField = new TextField();
            dialogueTextField.value = dialogueText;
            dialogueTextField.AddToClassList("de-node__text-field");
            dialogueTextField.AddToClassList("de-node__dialogue-text-field");
            dialogueTextField.AddToClassList("de-node__text-field__hidden");
            bodyVisualElement.Add(dialogueTextField);

            // Add Choice Button
            Button addChoiceButton = new Button();
            addChoiceButton.text = "Add Dialogue Choice";
            addChoiceButton.clicked += AddNewChoice;
            addChoiceButton.AddToClassList("de-node__button");

            bodyVisualElement.Add(addChoiceButton);
            extensionContainer.Add(bodyVisualElement);

            // Dialogue Choices
            foreach (string choice in choiceList)
            {
                AddChoiceElement(choice);
            }

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
            removeButton.text = "X";
            removeButton.AddToClassList("de-node__button");

            // Dialogue Choice
            TextField choiceTextField = new TextField();
            choiceTextField.AddToClassList("de-node__text-field");
            choiceTextField.AddToClassList("de-node__dialogue-text-field");
            choiceTextField.AddToClassList("de-node__choice-text-field");
            choiceTextField.AddToClassList("de-node__text-field__hidden");
            choiceTextField.value = dialogue;

            // Output Port
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = "";
            outputPort.AddToClassList("de-node__output-port");

            textVisualElement.Add(removeButton);
            textVisualElement.Add(choiceTextField);
            textVisualElement.Add(outputPort);
            extensionContainer.Add(textVisualElement);
        }

        private void AddNewChoice()
        {
            choiceList.Add("Dialogue Choice");
            AddChoiceElement(choiceList[choiceList.Count - 1]);
        }
    }
}
