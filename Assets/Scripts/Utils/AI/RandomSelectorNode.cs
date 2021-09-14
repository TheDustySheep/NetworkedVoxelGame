using System.Collections;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class RandomSelectorNode : Node
    {
        private Node selectedNode;
        private Node[] childNodes;

        public RandomSelectorNode(params Node[] nodes)
        {
            childNodes = nodes;
        }

        public override void Init()
        {
            if (childNodes == null || childNodes.Length == 0)
            {
                selectedNode = null;
            }
            else
            {
                selectedNode = childNodes[Random.Range(0, childNodes.Length)];
            }
        }

        public override NodeReturnState Tick()
        {
            if (selectedNode == null)
                return NodeReturnState.Success;

            switch (selectedNode.Tick())
            {
                case NodeReturnState.Success:
                    return NodeReturnState.Success;
                case NodeReturnState.Failure:
                    return NodeReturnState.Failure;
                case NodeReturnState.Running:
                    return NodeReturnState.Running;
                default:
                    Debug.LogError("Unexpected node return state");
                    return NodeReturnState.Failure;
            }
        }
    }
}