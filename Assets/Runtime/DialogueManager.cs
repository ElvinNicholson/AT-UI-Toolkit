using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private MainDialogueAsset dialogueAsset;
        [SerializeField] private DialogueUI dialogueUI;

        private DialogueNodeAsset currentNode;
        private int selectedReply;

        private void Start()
        {
            selectedReply = 0;
            currentNode = dialogueAsset.firstNode;
            dialogueUI.DisplayText(currentNode.text);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && currentNode.type != DialogueNodeType.REPLY)
            {
                NextNode();
            }
        }

        private void NextNode()
        {
            // Get next node
            switch (currentNode.type)
            {
                case DialogueNodeType.SINGLE:
                    SingleNodeAsset singleNodeAsset = currentNode as SingleNodeAsset;
                    currentNode = singleNodeAsset.nextNode;
                    break;

                case DialogueNodeType.REPLY:
                    ReplyNodeAsset replyNodeAsset = currentNode as ReplyNodeAsset;
                    currentNode = replyNodeAsset.replies[selectedReply].nextNode;
                    break;
            }

            switch (currentNode.type)
            {
                case DialogueNodeType.REPLY:
                    CreateReplyButtons();
                    break;

                case DialogueNodeType.END:
                    Debug.Log("End Dialogue");
                    dialogueUI.DisplayText("END OF DIALOGUE");
                    return;
            }

            dialogueUI.DisplayText(currentNode.text);
        }

        public void SetSelectedReply(int replyIndex)
        {
            dialogueUI.ClearReplyButton();
            selectedReply = replyIndex;
            NextNode();
        }

        private void CreateReplyButtons()
        {
            dialogueUI.CreateReplyButton(this, currentNode as ReplyNodeAsset);
        }
    }
}
