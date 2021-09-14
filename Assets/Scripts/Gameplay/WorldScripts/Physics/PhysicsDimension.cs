using Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.PhysicsScripts
{
    public class PhysicsDimension : MonoBehaviour
    {
        [SerializeField] GameObject physicsChunkPrefab;

        Queue<PhysicsChunk> inactiveChunks = new Queue<PhysicsChunk>();
        Dictionary<ChunkPos, PhysicsChunk> chunks = new Dictionary<ChunkPos, PhysicsChunk>();
        Indexer<VoxelType> indexer = new Indexer<VoxelType>();

        public int ActiveChunkCount => chunks.Count;
        public int InactiveChunkCount => inactiveChunks.Count;

        private void Awake()
        {
            indexer.UpdateIndex("World/Voxel Types");
        }

        private void OnEnable()
        {
            Signals.Get<OnChunkUpdate>().AddListener(OnChunkUpdate);
            Signals.Get<OnChunkDestroy>().AddListener(OnChunkDespawn);
        }

        private void OnDisable()
        {
            Signals.Get<OnChunkUpdate>().RemoveListener(OnChunkUpdate);
            Signals.Get<OnChunkDestroy>().RemoveListener(OnChunkDespawn);
        }

        public void OnChunkUpdate(ChunkPos chunkPos, ChunkData chunkData)
        {
            if (chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk.UpdateMesh(indexer, chunkPos, chunkData);
            }
            else
            {
                if (inactiveChunks.Count > 0)
                {
                    chunk = inactiveChunks.Dequeue();
                    chunk.gameObject.SetActive(true);
                    chunk.UpdateMesh(indexer, chunkPos, chunkData);
                }
                else
                {
                    chunk = Instantiate(physicsChunkPrefab, transform).GetComponent<PhysicsChunk>();
                    chunks.Add(chunkPos, chunk);
                    chunk.UpdateMesh(indexer, chunkPos, chunkData);
                }
            }
        }

        public void OnChunkDespawn(ChunkPos chunkPos)
        {
            if (chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk.Clear();
                chunk.gameObject.SetActive(false);
                inactiveChunks.Enqueue(chunk);
            }
        }
    }
}