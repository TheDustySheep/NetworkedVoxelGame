using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.ScriptableReference
{
    public class ScriptableReference<T> : ScriptableObject
    {
        public T value;
    }
}