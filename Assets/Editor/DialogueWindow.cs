using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;

namespace DialogueEditor
{
    public class DialogueWindow : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowWindow()
        {
            GetWindow<DialogueWindow>("Dialogue Editor");
        }

        private void OnEnable()
        {
            AddGraphview();
        }

        private void AddGraphview()
        {
            DialogueGraphview graphview = new DialogueGraphview();
            graphview.StretchToParentSize();
            rootVisualElement.Add(graphview);
        }
    }
}
