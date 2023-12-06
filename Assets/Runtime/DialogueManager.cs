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
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartDialogue();
            }

            if (!dialogueUI.gameObject.activeSelf)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) && dialogueUI.IsTyping())
            {
                dialogueUI.SkipTyping();
            }
            else if (Input.GetMouseButtonDown(0) && currentNode.type != DialogueNodeType.REPLY)
            {
                NextNode();
            }
        }

        public void StartDialogue()
        {
            dialogueUI.gameObject.SetActive(true);
            currentNode = dialogueAsset.firstNode;
            DisplayCurrentNode();
        }

        public void ExitDialogue()
        {
            dialogueUI.gameObject.SetActive(false);
        }

        private void NextNode()
        {
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

            DisplayCurrentNode();
        }

        private void DisplayCurrentNode()
        {
            switch (currentNode.type)
            {
                case DialogueNodeType.REPLY:
                    CreateReplyButtons();
                    break;

                case DialogueNodeType.END:
                    ExitDialogue();
                    return;
            }

            dialogueUI.DisplayText(currentNode.title, currentNode.text);
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
