using System.Collections;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class SucceederNode : DecoratorNode
    {
        public SucceederNode() : base(null) { }
        public SucceederNode(Node child) : base(child) { }

        public override NodeReturnState Tick()
        {
            if (child == null)
                return NodeReturnState.Success;

            switch (child.Tick())
            {
                case NodeReturnState.Success:
                    return NodeReturnState.Success;
                case NodeReturnState.Failure:
                    return NodeReturnState.Success;
                case NodeReturnState.Running:
                    return NodeReturnState.Running;
                default:
                    Debug.LogError("Unexpected node return state");
                    return NodeReturnState.Failure;
            }
        }
    }
}