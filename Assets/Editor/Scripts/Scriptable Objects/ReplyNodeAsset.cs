using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor
{
    public class ReplyNodeAsset : DialogueNodeAsset
    {
        public List<ReplyData> replies;

        public void Initialize(string _title, string _text, List<ReplyData> _replies) 
        {
            title = _title;
            text = _text;
            replies = _replies;
        }
    }
}