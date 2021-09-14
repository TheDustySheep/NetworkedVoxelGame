using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class DebugLogNode : Node
    {
        string message;

        public DebugLogNode(string _message)
        {
            message = _message;
        }

        public override void Init() { }

        public override NodeReturnState Tick()
        {
            Debug.Log(message);
            return NodeReturnState.Success;
        }
    }
}