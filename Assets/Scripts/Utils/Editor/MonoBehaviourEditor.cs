using UnityEngine;
using UnityEditor;

namespace Scripts.Utils
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ScriptableObjectEditor : Editor
    {
    }
}