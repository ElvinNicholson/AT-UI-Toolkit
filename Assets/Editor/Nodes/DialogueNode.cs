using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

namespace DialogueEditor
{
    public enum PortType
    {
        INPUT,
        OUTPUT
    }

    public class DialogueNode : Node
    {
        public string dialogueTitle;
        public string dialogueText;
        public string nodeID;
        public DialogueNodeType dialogueType;

        protected GraphView graphView;

        public Port inputPort;

        public virtual void Init(GraphView graphViewRef, Vector2 position)
        {
            dialogueTitle = "Dialogue Title";
            dialogueText = "Dialogue Text";
            graphView = graphViewRef;
            nodeID = Guid.NewGuid().ToString();
            SetPosition(new Rect(position, Vector2.zero));

            InitStyles();
        }

        public virtual void InitFromAsset(DialogueNodeAsset asset, GraphView graphViewRef)
        {
        }

        protected void InitStyles()
        {
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
            titleTextField.RegisterValueChangedCallback(evt => OnTitleValueChanged(evt.newValue));
            titleContainer.Insert(0, titleTextField);

            // Input Port
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Previous Dialogue";
            inputContainer.Add(inputPort);

            // Dialogue Text
            TextField dialogueTextField = new TextField();
            dialogueTextField.value = dialogueText;
            dialogueTextField.AddToClassList("de-node__text-field");
            dialogueTextField.AddToClassList("de-node__dialogue-text-field");
            dialogueTextField.AddToClassList("de-node__text-field__hidden");
            dialogueTextField.RegisterValueChangedCallback(evt => OnDialogueValueChanged(evt.newValue));
            extensionContainer.Add(dialogueTextField);
        }

        public virtual DialogueNodeAsset Save()
        {
            DialogueNodeAsset asset = new DialogueNodeAsset();
            return asset;
        }

        public virtual void OnPortConnect(PortType portType)
        {

        }

        public virtual void OnPortDisconnect(PortType portType)
        {
            
        }

        public virtual bool IsAllPortConnected()
        {
            return false;
        }

        public virtual void DisconnectAllPort()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        protected void DisconnectPorts(VisualElement portContainer)
        {
            foreach (Port port in portContainer.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public void SetColorRed()
        {
            titleContainer.style.backgroundColor = new StyleColor(new Color(0.8f, 0.2f, 0.2f));
        }

        private void OnTitleValueChanged(string newValue)
        {
            dialogueTitle = newValue;
        }

        private void OnDialogueValueChanged(string newValue)
        {
            dialogueText = newValue;
        }
    }
}
