using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class RepeatNode : DecoratorNode
    {
        int count = 0;
        int max = 0;

        public RepeatNode(Node _child, int repeats) : base(_child)
        {
            max = repeats;
        }

        public override void Init()
        {
            count = 0;
        }

        public override NodeReturnState Tick()
        {
            if (child == null)
                return NodeReturnState.Success;

            while (count < max)
            {
                switch (child.Tick())
                {
                    case NodeReturnState.Success:
                        count++;
                        break;
                    case NodeReturnState.Failure:
                        return NodeReturnState.Failure;                           
                    case NodeReturnState.Running:
                        return NodeReturnState.Running;
                }
            }

            return NodeReturnState.Success;
        }
    }
}