using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts
{
    public interface IChunkRenderer
    {
        public IEnumerator OnRegenerate();
        public void Draw();
        public void Destroy();
    }
}