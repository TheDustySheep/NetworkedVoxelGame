using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.AI
{
    public class BehaviourTree
    {
        protected Node mainNode;

        public BehaviourTree(Node _mainNode)
        {
            mainNode = _mainNode;
        }

        public void Init() => mainNode.Init();
        public void Tick()
        {
            switch (mainNode.Tick())
            {
                case NodeReturnState.Success:
                    mainNode.Init();
                    break;
                case NodeReturnState.Failure:
                    mainNode.Init();
                    break;
                case NodeReturnState.Running:
                    break;
                default:
                    mainNode.Init();
                    Debug.LogError("Unexpected node return state");
                    break;
            }
        }
    }
}