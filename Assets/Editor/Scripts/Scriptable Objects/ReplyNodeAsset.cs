using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor
{
    public class ReplyNodeAsset : DialogueNodeAsset
    {
        [SerializeField]
        public List<ReplyData> replies = new List<ReplyData>();
    }
}