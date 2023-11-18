using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor
{
    public class MainDialogueAsset : ScriptableObject
    {
        public List<DialogueNodeAsset> dialogueNodeAssets = new List<DialogueNodeAsset>();
        public DialogueNodeAsset firstNode;
    }
}