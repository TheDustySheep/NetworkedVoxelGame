using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class DecisionNode : Node
    {
        public delegate bool Evaluate();
        protected Evaluate evaluate;

        Node trueNode;
        Node falseNode;

        Node executingNode;

        public DecisionNode(Node _trueNode, Node _falseNode, Evaluate _evaluate)
        {
            evaluate = _evaluate;
            trueNode = _trueNode;
            falseNode = _falseNode;
        }

        public override void Init()
        {
            executingNode = null;
        }

        public override NodeReturnState Tick()
        {
            if (executingNode == null)
            {
                if (evaluate.Invoke())
                {
                    executingNode = trueNode;
                }
                else
                {
                    executingNode = falseNode;
                }

                executingNode.Init();
            }

            return executingNode.Tick();
        }
    }
}