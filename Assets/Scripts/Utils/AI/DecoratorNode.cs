using System.Collections;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public abstract class DecoratorNode : Node
    {
        protected Node child;

        public DecoratorNode(Node _child)
        {
            child = _child;
        }

        public override void Init()
        {
            child?.Init();
        }
    }
}