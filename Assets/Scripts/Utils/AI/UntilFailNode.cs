using System.Collections;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class UntilFailNode : DecoratorNode
    {
        public UntilFailNode(Node child) : base(child) { }

        public override NodeReturnState Tick()
        {
            switch (child.Tick())
            {
                case NodeReturnState.Success:
                    child.Init();
                    return Tick();
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