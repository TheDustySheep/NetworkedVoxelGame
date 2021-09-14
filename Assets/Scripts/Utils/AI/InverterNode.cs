using System.Collections;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(Node child) : base(child) { }

        public override NodeReturnState Tick()
        {
            switch (child.Tick())
            {
                case NodeReturnState.Success:
                    return NodeReturnState.Failure;
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