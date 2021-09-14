using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts
{
    public struct VoxelData
    {
        public int HashKey;
        public int State;

        public VoxelData(int hashKey)
        {
            HashKey = hashKey;
            State = 0;
        }

        public VoxelData(string key)
        {
            HashKey = key.GetStableHashCode();
            State = 0;
        }
    }
}