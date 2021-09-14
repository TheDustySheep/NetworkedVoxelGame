using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public abstract class Node
    {
        public virtual void Init() { }
        public abstract NodeReturnState Tick();
    }
}