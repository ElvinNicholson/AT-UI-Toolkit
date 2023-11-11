using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor
{
    public class SingleNodeAsset : DialogueNodeAsset
    {
        public DialogueNodeAsset nextNode;

        public void Initialize(string _title, string _text, DialogueNodeAsset _nextNode)
        {
            title = _title;
            text = _text;
            nextNode = _nextNode;
        }
    }
}