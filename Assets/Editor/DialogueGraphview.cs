using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

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
            ContextualMenuManipulator singleContextMenu = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Dialogue Node", 
                actionEvent => AddElement(AddNode(DialogueNodeType.SINGLE, 
                viewTransform.matrix.inverse.MultiplyPoint(actionEvent.eventInfo.localMousePosition)))));
            this.AddManipulator(singleContextMenu);

            ContextualMenuManipulator multipleContextMenu = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Dialogue Reply Node", 
                actionEvent => AddElement(AddNode(DialogueNodeType.MULTIPLE, 
                viewTransform.matrix.inverse.MultiplyPoint(actionEvent.eventInfo.localMousePosition)))));
            this.AddManipulator(multipleContextMenu);

            // Dragging nodes
            this.AddManipulator(new SelectionDragger());

            // Rect select
            this.AddManipulator(new RectangleSelector());
        }

        private DialogueNode AddNode(DialogueNodeType type, Vector2 position)
        {
            DialogueNode newNode;

            if (type == DialogueNodeType.SINGLE)
            {
                newNode = new DialogueNodeSingle();
            }
            else
            {
                newNode = new DialogueNodeMultiple();
            }

            newNode.Init(position);
            newNode.Draw();

            return newNode;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port || startPort.node == port.node || startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
    }
}
