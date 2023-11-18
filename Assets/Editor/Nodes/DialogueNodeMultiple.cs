using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueNodeMultiple : DialogueNode
    {
        public List<string> replyList;
        public List<Port> outputPortList;
        public List<string> nextNodeIDs;

        public override void Init(GraphView graphViewRef, Vector2 position)
        {
            dialogueType = DialogueNodeType.REPLY;
            base.Init(graphViewRef, position);

            outputPortList = new List<Port>();
            replyList = new List<string>();
        }

        public override void InitFromAsset(DialogueNodeAsset asset, GraphView graphViewRef)
        {
            ReplyNodeAsset replyNodeAsset = asset as ReplyNodeAsset;
            dialogueTitle = replyNodeAsset.title;
            dialogueText = replyNodeAsset.text;
            dialogueType = DialogueNodeType.REPLY;
            graphView = graphViewRef;
            nodeID = replyNodeAsset.nodeID;
            SetPosition(replyNodeAsset.position);

            InitStyles();

            outputPortList = new List<Port>();
            replyList = new List<string>();
            nextNodeIDs = new List<string>();

            DrawFromAsset();

            foreach (ReplyData reply in replyNodeAsset.replies)
            {
                replyList.Add(reply.replyText);
                nextNodeIDs.Add(reply.nextNodeID);
                AddChoiceElement(replyList[replyList.Count - 1]);
            }
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

        private void DrawFromAsset()
        {
            base.Draw();

            // Add Choice Button
            Button addChoiceButton = new Button();
            addChoiceButton.text = "Add Dialogue Reply";
            addChoiceButton.clicked += AddNewReply;
            addChoiceButton.AddToClassList("de-node__button");

            extensionContainer.Add(addChoiceButton);

            RefreshExpandedState();
        }

        private void AddChoiceElement(string dialogue)
        {
            int index = replyList.Count - 1;

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
            replyTextField.RegisterValueChangedCallback(evt => OnReplyTextFieldValueChanged(evt.newValue, index));

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

        public override DialogueNodeAsset Save()
        {
            ReplyNodeAsset asset = ScriptableObject.CreateInstance<ReplyNodeAsset>();
            asset.title = dialogueTitle;
            asset.text = dialogueText;
            asset.type = dialogueType;
            asset.nodeID = nodeID;
            asset.position = GetPosition();

            // Loop through each output port in replies
            for (int i = 0; i < outputPortList.Count; i++)
            {
                ReplyData replyData = new ReplyData();
                replyData.replyText = replyList[i];

                var edges = outputPortList[i].connections;
                foreach (Edge edge in edges)
                {
                    Port nextNodeInputPort = edge.input;
                    DialogueNode nextNode = nextNodeInputPort.parent.GetFirstOfType<DialogueNode>();
                    replyData.nextNodeID = nextNode.nodeID;
                }

                asset.replies.Add(replyData);
            }

            return asset;
        }

        private void OnReplyTextFieldValueChanged(string newValue, int index)
        {
            replyList[index] = newValue;
        }
    }
}
