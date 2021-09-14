using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.InventoryScripts
{
    public static class ToolFactory
    {
        public static Tool Generate(HeadType headType, RodType rodType)
        {
            Head head = new Head(headType);
            Rod rod = new Rod(rodType);

            return new Tool(head, rod);
        }
    }
}