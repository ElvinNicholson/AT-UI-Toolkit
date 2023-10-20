using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace DialogueEditor
{
    public class DialogueGraphview : GraphView
    {
        public DialogueGraphview()
        {
            AddStyleSheet();
            AddBackground();
            AddControls();
        }

        private void AddStyleSheet()
        {
            StyleSheet graphViewStyleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueGraphview.uss");
            StyleSheet nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueNode.uss");

            styleSheets.Add(graphViewStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }

        private void AddBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddControls()
        {
            // Middle click drag
            this.AddManipulator(new ContentDragger());

            // Scroll wheel zoom
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // Right click window
            ContextualMenuManipulator contextMenu = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add dialogue node", actionEvent => AddElement(AddNode(actionEvent.eventInfo.localMousePosition))));
            this.AddManipulator(contextMenu);

            // Dragging nodes
            this.AddManipulator(new SelectionDragger());

            // Rect select
            this.AddManipulator(new RectangleSelector());
        }

        private DialogueNode AddNode(Vector2 position)
        {
            DialogueNode newNode = new DialogueNode();

            newNode.Init(position);
            newNode.Draw();

            return newNode;
        }
    }
}
