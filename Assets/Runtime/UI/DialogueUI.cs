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

        [Header("Variables")]
        [SerializeField] private float timePerChar;

        private string currentText;
        private bool isTyping;

        public void DisplayText(string title, string text)
        {
            StopAllCoroutines();
            dialogueTitle.text = title;
            currentText = text;

            StartCoroutine(TypeText(currentText));
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

        private IEnumerator TypeText(string text)
        {
            dialogueText.text = "";
            isTyping = true;

            foreach (char c in text.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(timePerChar);
            }

            isTyping = false;
        }

        public void SkipTyping()
        {
            StopAllCoroutines();
            isTyping = false;
            dialogueText.text = currentText;
        }

        public bool IsTyping()
        {
            return isTyping;
        }
    }
}
