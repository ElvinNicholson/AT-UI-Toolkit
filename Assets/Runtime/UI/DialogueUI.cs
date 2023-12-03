using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DialogueEditor
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI dialogueTitle;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private GameObject replyButtonPrefab;
        [SerializeField] private Transform replyButtonParent;

        public void DisplayText(string title, string text)
        {
            dialogueTitle.text = title;
            dialogueText.text = text;
        }

        public void CreateReplyButton(DialogueManager dialogueManager, ReplyNodeAsset replyNodeAsset)
        {
            for (int i = 0; i < replyNodeAsset.replies.Count; i++)
            {
                Vector2 position = replyButtonParent.position + new Vector3(0, 75 * (replyNodeAsset.replies.Count - i));
                ReplyButton replyButton = Instantiate(replyButtonPrefab, position, Quaternion.identity, replyButtonParent).GetComponent<ReplyButton>();
                replyButton.Init(dialogueManager, replyNodeAsset.replies[i].replyText, i);
            }
        }

        public void ClearReplyButton()
        {
            foreach (Transform child in replyButtonParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
