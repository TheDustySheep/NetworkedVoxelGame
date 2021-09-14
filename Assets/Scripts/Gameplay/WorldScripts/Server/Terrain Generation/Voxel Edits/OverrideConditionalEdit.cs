using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    [CreateAssetMenu(menuName = "World/Terrain/Conditional Edit")]
    public class OverrideConditionalEdit : VoxelEditBase
    {
        [SerializeField] VoxelType ConditionType;
        [SerializeField] VoxelType OutputType;

        int conditionHashKey;
        int outputHashKey;

        private void Awake()
        {
            conditionHashKey = ConditionType.name.GetStableHashCode();
            outputHashKey = OutputType.name.GetStableHashCode();
        }

        public override VoxelData EditVoxel(VoxelData current)
        {
            if (current.HashKey == conditionHashKey)
                return new VoxelData(outputHashKey);
            else
                return current;
        }
    }
}