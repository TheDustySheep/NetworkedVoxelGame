using Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.InventoryScripts
{
    public class Tool : Item
    {
        public Head head;
        public Rod rod;

        public float MiningRate => CalculateMiningRate();

        public Tool(Head _head, Rod _rod)
        {
            head = _head;
            rod = _rod;
        }

        private float CalculateMiningRate()
        {
            return 
            Mathf.Lerp(0.5f, 2.0f, head.Quality) *
            Mathf.Lerp(0.1f, 1.0f, head.Sharpness) *
            head.Type.headMaterial.UseSpeedMultiplier;
        }

        public override Sprite[] GetSprites()
        {
            return new Sprite[]
            {
                head.Type.sprite,
                rod.Type.sprite,
            };
        }
    }
}