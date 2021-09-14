using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.EntityScripts
{
    public struct Colission
    {
        public Vector3 Self;
        public Vector3 Other;
        public Vector3 Normal;
        public bool IsColliding;
    }
}