using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Scripts.Utils;
using Scripts.Gameplay.WorldScripts.Server.TerrainGeneration;

namespace Scripts.Gameplay.WorldScripts.Server
{
    public class ChunkSpawningHandler
    {
        public float AverageChunkSpawnTime = 0f;
        public long totalSpawningTime = 0;
        public int totalSpawnedChunks = 0;

        public int ChunkToSpawnCount => chunksToSpawn.Count;

        Dictionary<ChunkPos, Chunk> chunks;

        HashSet<ChunkPos> chunksToSpawn = new HashSet<ChunkPos>();

        ServerDimension dimension;

        WorldGenerator terrainGenerator;

        Dictionary<ChunkPos, Dictionary<Vector3Int, VoxelEditBase>> unloadedChunkEdits = new Dictionary<ChunkPos, Dictionary<Vector3Int, VoxelEditBase>>();

        public ChunkSpawningHandler(Dictionary<ChunkPos, Chunk> _chunks, ServerDimension _dimension, TerrainGenerationSettings settings)
        {
            terrainGenerator = new WorldGenerator(settings);
            chunks = _chunks;
            dimension = _dimension;
        }

        public void SpawnChunk(ChunkPos chunkPos)
        {
            if (!chunks.ContainsKey(chunkPos))
                chunksToSpawn.Add(chunkPos);
        }

        public IEnumerator SpawnChunks()
        {
            var sw = new System.Diagnostics.Stopwatch();
            while (true)
            {
                if (chunksToSpawn.Count > 0)
                {
                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                    int maxSpawns = dimension.dimensionSettings.maxActiveChunkSpawnsPerTick;
                    ChunkPos[] spawnQueue = chunksToSpawn.ToArray();
                    Chunk[] newChunks = new Chunk[spawnQueue.Length];

                    chunksToSpawn.Clear();

                    Dictionary<Vector3Int, VoxelEditBase> edits = new Dictionary<Vector3Int, VoxelEditBase>();

                    yield return new WaitForThreadedTask(() =>
                    {
                        sw.Restart();
                        for (int i = 0; i < newChunks.Length; i++)
                        {
                            newChunks[i] = new Chunk(spawnQueue[i], terrainGenerator, ref edits);
                        }
                        sw.Stop();

                        OnTerrainGenerationEdit(edits);
                    });

                    totalSpawningTime += sw.ElapsedMilliseconds;
                    totalSpawnedChunks += newChunks.Length;
                    AverageChunkSpawnTime = (float)totalSpawningTime / (float)totalSpawnedChunks;

                    for (int i = 0; i < newChunks.Length; i++)
                    {
                        chunks.Add(spawnQueue[i], newChunks[i]);
                    }

                    ApplyEdits();
                }

                yield return null;
            }
        }

        public void OnTerrainGenerationEdit(Dictionary<Vector3Int, VoxelEditBase> edits)
        {
            foreach (var edit in edits)
            {
                ChunkPos chunkPos = StaticWorldData.ConvectWorldVoxelToChunk(edit.Key);
                Vector3Int localPos = StaticWorldData.ConvectWorldVoxelToLocalVoxel(edit.Key);

                if (unloadedChunkEdits.TryGetValue(chunkPos, out var chunkEdits))
                {
                    if (!chunkEdits.ContainsKey(localPos))
                        chunkEdits.Add(localPos, edit.Value);
                }
                else
                {
                    var localEdits = new Dictionary<Vector3Int, VoxelEditBase>();
                    localEdits.Add(localPos, edit.Value);
                    unloadedChunkEdits.Add(chunkPos, localEdits);
                }
            }
        }

        public void ApplyEdits()
        {
            List<ChunkPos> successEdits = new List<ChunkPos>();

            foreach (var chunkEdit in unloadedChunkEdits)
            {
                if (chunks.TryGetValue(chunkEdit.Key, out var chunk))
                {
                    foreach (var edit in chunkEdit.Value)
                    {
                        chunk.EditVoxel(edit.Key, edit.Value.EditVoxel(chunk.GetVoxel(edit.Key)));
                    }

                    successEdits.Add(chunkEdit.Key);
                }
            }

            foreach (var pos in successEdits)
            {
                unloadedChunkEdits.Remove(pos);
            }
        }
    }
}