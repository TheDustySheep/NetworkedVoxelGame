using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class WaitForSecondsNode : Node
    {
        float length;
        float startTime;

        public WaitForSecondsNode(float timeSeconds)
        {
            length = timeSeconds;
        }

        public override void Init()
        {
            startTime = Time.time;
        }

        public override NodeReturnState Tick()
        {
            if (startTime + length < Time.time)
                return NodeReturnState.Success;
            else
                return NodeReturnState.Running;
        }
    }
}