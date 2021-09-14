using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class AlwaysRepeatNode : Node
    {
        public override void Init() { }
        public override NodeReturnState Tick() { return NodeReturnState.Running; }
    }
}