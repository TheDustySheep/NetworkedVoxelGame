using System.Collections;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class SequenceNode : Node
    {
        private int currentIndex = 0;
        private bool isExecutingNode = false;
        private Node[] childNodes;
        
        public SequenceNode(params Node[] nodes)
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
                    isExecutingNode = false;
                    currentIndex++;
                    return CompletedAllNodes() ? NodeReturnState.Success : Tick();
                case NodeReturnState.Failure:
                    return NodeReturnState.Failure;
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