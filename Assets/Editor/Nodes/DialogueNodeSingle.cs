using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueNodeSingle : DialogueNode
    {
        public override void Init(Vector2 position)
        {
            base.Init(position);
        }

        public override void Draw()
        {
            base.Draw();

            // Output Port
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = "Next Dialogue";
            outputContainer.Add(outputPort);

            RefreshExpandedState();
        }
    }
}