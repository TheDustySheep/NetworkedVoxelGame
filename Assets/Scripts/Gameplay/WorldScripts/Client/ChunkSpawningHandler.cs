using Mirror;
using Scripts.Gameplay.WorldScripts.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Client
{
    public class ChunkSpawningHandler
    {
        Dictionary<ChunkPos, Chunk> chunks;
        Transform parent;
        ChunkRenderingSettings renderingSettings;
        ActiveChunkHandler activeChunkHandler;
        ChunkFactory chunkFactory;

        public ChunkSpawningHandler(Dictionary<ChunkPos, Chunk> _chunks, Transform _parent, ChunkRenderingSettings _renderingSettings, ActiveChunkHandler _activeChunkHandler)
        {
            chunks = _chunks;
            parent = _parent;
            renderingSettings = _renderingSettings;
            activeChunkHandler = _activeChunkHandler;

            chunkFactory = new ChunkFactory(_parent, _renderingSettings);

            NetworkClient.RegisterHandler<ChunkUpdateMessage>(OnChunkUpdate);
            NetworkClient.RegisterHandler<CompressedChunkUpdateMessage>(OnCompressedChunkUpdate);
        }

        public IEnumerator UpdateActiveChunks()
        {
            List<ChunkPos> chunksToDestroy = new List<ChunkPos>();
            List<Vector3Int> positions = new List<Vector3Int>();
            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while (true)
            {
                chunksToDestroy.Clear();
                positions.Clear();

                yield return activeChunkHandler.UpdateActiveChunks();

                BatchChunkUpdateRequestMessage message = new BatchChunkUpdateRequestMessage();
                
                int i = 0;

                foreach (var chunk in activeChunkHandler.activeChunks)
                {
                    if (i > 255)
                    {
                        break;
                    }

                    if (!chunks.ContainsKey(chunk))
                    {
                        positions.Add(chunk);
                        i++;
                    }
                }

                if (i > 0)
                {
                    message.positions = positions.ToArray();

                    NetworkClient.Send(message);
                }

                foreach (var chunk in chunks)
                {
                    if ((!chunk.Value.isBeingGenerated) && (!activeChunkHandler.activeChunks.Contains(chunk.Key)))
                    {
                        chunksToDestroy.Add(chunk.Key);
                    }
                }

                foreach (var chunkPos in chunksToDestroy)
                {
                    DestroyChunk(chunkPos);
                }

                yield return wait;
            }
        }

        private void OnChunkUpdate(ChunkUpdateMessage message)
        {
            ChunkData chunkData = message.Data;

            if (chunks.TryGetValue(message.ChunkPos, out var chunk))
            {
                chunk.UpdateChunk(chunkData);
            }
            else
            {
                SpawnChunk(message.ChunkPos, message.Data);
            }

            for (int i = 0; i < 6; i++)
            {
                if (chunks.TryGetValue(message.ChunkPos + StaticWorldData.OFFSETS[i], out var nchunk))
                {
                    nchunk.NeighbourUpdate();
                }
            }
        }

        private void OnCompressedChunkUpdate(CompressedChunkUpdateMessage message)
        {
            ChunkData chunkData = new ChunkData();
            ChunkDataCompressor.Decompress16(chunkData, message.data);

            if (chunks.TryGetValue(message.ChunkPos, out var chunk))
            {
                chunk.UpdateChunk(chunkData);
            }
            else
            {
                SpawnChunk(message.ChunkPos, chunkData);
            }

            for (int i = 0; i < 6; i++)
            {
                if (chunks.TryGetValue(message.ChunkPos + StaticWorldData.OFFSETS[i], out var nchunk))
                {
                    nchunk.NeighbourUpdate();
                }
            }
        }

        private void SpawnChunk(ChunkPos chunkPos, ChunkData chunkData)
        {
            chunks.Add(chunkPos, chunkFactory.SpawnChunk(chunkPos, chunkData));
            Utils.Signals.Get<OnChunkUpdate>().Dispatch(chunkPos, chunkData);
        }

        private void DestroyChunk(ChunkPos chunkPos)
        {
            if (chunks.ContainsKey(chunkPos))
            {
                chunks.Remove(chunkPos);
                Utils.Signals.Get<OnChunkDestroy>().Dispatch(chunkPos);

                for (int i = 0; i < 6; i++)
                {
                    if (chunks.TryGetValue(chunkPos + StaticWorldData.OFFSETS[i], out var nchunk))
                    {
                        nchunk.NeighbourUpdate();
                    }
                }
            }
        }
    }
}