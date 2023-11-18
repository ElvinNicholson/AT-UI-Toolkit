using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor
{
    [System.Serializable]
    public class ReplyData
    {
        public string replyText;
        public DialogueNodeAsset nextNode;
        [HideInInspector] public string nextNodeID;
    }
}
