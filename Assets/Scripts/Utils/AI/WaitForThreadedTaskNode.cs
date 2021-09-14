using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

namespace Scripts.Utils.AI
{
    public class WaitForThreadedTaskNode : Node
    {
        Action action;
        bool running = false;

        public WaitForThreadedTaskNode(Action _action)
        {
            action = _action;
        }

        public override void Init()
        {
            new Thread(() =>
            {
                running = true;
                action();
                running = false;
            }).Start();
        }

        public override NodeReturnState Tick()
        {
            if (running)
            {
                return NodeReturnState.Running;
            }
            else
            {
                return NodeReturnState.Success;
            }
        }
    }
}