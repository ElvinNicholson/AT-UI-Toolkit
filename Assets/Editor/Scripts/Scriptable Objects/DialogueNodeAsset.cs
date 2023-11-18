using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor
{
    public class DialogueNodeAsset : ScriptableObject
    {
        public string title;
        public string text;
        public DialogueNodeType type;
        [HideInInspector] public string nodeID;
        [HideInInspector] public Rect position;
    }
}

