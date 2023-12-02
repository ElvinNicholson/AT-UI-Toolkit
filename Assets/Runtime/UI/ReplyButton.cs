using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueEditor
{
    public class ReplyButton : MonoBehaviour
    {
        private DialogueManager dialogueManagerRef;
        private int replyIndex;
        private Button button;
        private TextMeshProUGUI replyText;

        public void Init(DialogueManager dialogueManager, string text, int index)
        {
            dialogueManagerRef = dialogueManager;
            replyText = GetComponentInChildren<TextMeshProUGUI>(true);
            replyText.text = text;
            replyIndex = index;
        }

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            dialogueManagerRef.SetSelectedReply(replyIndex);
        }
    }
}