using System.Collections;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class SelectorNode : Node
    {
        private int currentIndex = 0;
        private bool isExecutingNode = false;
        private Node[] childNodes;

        public SelectorNode(params Node[] nodes)
        {
            childNodes = nodes;
        }

        public override void Init()
        {
            currentIndex = 0;
            isExecutingNode = false;
        }

        public override NodeReturnState Tick()
        {
            if (childNodes == null || childNodes.Length == 0 || CompletedAllNodes())
                return NodeReturnState.Success;

            if (!isExecutingNode)
            {
                childNodes[currentIndex].Init();
                isExecutingNode = true;
            }

            switch (childNodes[currentIndex].Tick())
            {
                case NodeReturnState.Success:
                    return NodeReturnState.Success;
                case NodeReturnState.Failure:
                    isExecutingNode = false;
                    currentIndex++;
                    return CompletedAllNodes() ? NodeReturnState.Failure : Tick();
                case NodeReturnState.Running:
                    return NodeReturnState.Running;
                default:
                    Debug.LogError("Unexpected node return state");
                    return NodeReturnState.Failure;
            }
        }

        private bool CompletedAllNodes() => currentIndex >= childNodes.Length;
    }
}