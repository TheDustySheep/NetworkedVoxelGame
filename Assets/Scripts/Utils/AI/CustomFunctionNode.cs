using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Utils.AI
{
    public class CustomFunctionNode : Node
    {
        Action func;

        public CustomFunctionNode(Action _func)
        {
            func = _func;
        }

        public override void Init() { }

        public override NodeReturnState Tick()
        {
            func?.Invoke();
            return NodeReturnState.Success;
        }
    }
}