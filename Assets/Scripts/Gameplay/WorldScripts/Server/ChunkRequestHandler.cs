using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Scripts.Utils;
using Scripts.Gameplay.WorldScripts.Networking;

namespace Scripts.Gameplay.WorldScripts.Server
{
    public class ChunkRequestHandler
    {
        private static bool USE_COMPRESSED_CHUNK_DATA = true;

        public int ActiveClientRequests => clientChunkRequests.Count;
        public int ActiveUpdateAllRequests => updateAllRequests.Count;

        Dictionary<ChunkPos, Chunk> chunks;

        Dictionary<ChunkPos, HashSet<NetworkConnection>> clientChunkRequests = new Dictionary<ChunkPos, HashSet<NetworkConnection>>();

        HashSet<ChunkPos> updateAllRequests = new HashSet<ChunkPos>();

        Action<ChunkPos> spawnChunkCallback;

        ServerDimensionSettings settings;

        public ChunkRequestHandler(Dictionary<ChunkPos, Chunk> _chunks, Action<ChunkPos> _spawnChunkCallback, ServerDimensionSettings _settings)
        {
            spawnChunkCallback = _spawnChunkCallback;
            chunks = _chunks;
            settings = _settings;
        }

        public void OnServerStart()
        {
            NetworkServer.RegisterHandler<ChunkUpdateRequestMessage>(OnChunkRequest);
            NetworkServer.RegisterHandler<BatchChunkUpdateRequestMessage>(OnBatchChunkRequest);
            NetworkServer.RegisterHandler<VoxelUpdateRequestMessage>(OnVoxelUpdateRequest);
        }

        public IEnumerator SendChunkUpdates()
        {
            Queue<ChunkPos> chunksToSend = new Queue<ChunkPos>();
            while (true)
            {
                chunksToSend.Clear();

                int count = 0;
                foreach (var chunkPos in clientChunkRequests.Keys)
                {
                    if (count >= settings.maxChunkUpdatesSendPerTick)
                        break;

                    if (chunks.ContainsKey(chunkPos))
                    {
                        chunksToSend.Enqueue(chunkPos);
                        count++;
                    }
                }

                count = 0;
                while (count < settings.maxChunkUpdatesSendPerTick && chunksToSend.Count > 0)
                {
                    ChunkPos pos = chunksToSend.Dequeue();
                    if (chunks.TryGetValue(pos, out var chunk))
                    {
                        if (clientChunkRequests.TryGetValue(pos, out var connections))
                        {
                            CompressedChunkUpdateMessage message = new CompressedChunkUpdateMessage(pos, chunk.Data);

                            foreach (var conn in connections)
                            {
                                conn.Send(message);
                            }

                            clientChunkRequests.Remove(pos);
                        }
                    }
                    count++;
                }

                yield return null;
            }
        }

        public IEnumerator SendChunkUpdatesAll()
        {
            while (true)
            {
                bool doRemove = false;
                ChunkPos pos = new ChunkPos();

                foreach (var chunkPos in updateAllRequests)
                {
                    if (chunks.TryGetValue(chunkPos, out var chunk))
                    {
                        if (USE_COMPRESSED_CHUNK_DATA)
                        {
                            CompressedChunkUpdateMessage message = new CompressedChunkUpdateMessage(chunkPos, chunk.Data);
                            NetworkServer.SendToAll(message);
                        }
                        else
                        {
                            ChunkUpdateMessage message = new ChunkUpdateMessage(chunkPos, chunk.Data);
                            NetworkServer.SendToAll(message);
                        }

                        doRemove = true;
                        pos = chunkPos;
                        break;
                    }
                }

                if (doRemove)
                {
                    updateAllRequests.Remove(pos);
                }

                yield return null;
            }
        }

        public void OnChunkRequest(NetworkConnection conn, ChunkUpdateRequestMessage message)
        {
            if (clientChunkRequests.TryGetValue(message.ChunkPos, out var set))
            {
                set.Add(conn);
            }
            else
            {
                HashSet<NetworkConnection> connections = new HashSet<NetworkConnection>();
                connections.Add(conn);
                clientChunkRequests.Add(message.ChunkPos, connections);

                spawnChunkCallback?.Invoke(message.ChunkPos);
            }
        }

        public void OnBatchChunkRequest(NetworkConnection conn, BatchChunkUpdateRequestMessage message)
        {
            if (message.positions == null)
                return;

            foreach (var pos in message.positions)
            {
                if (clientChunkRequests.TryGetValue(pos, out var set))
                {
                    set.Add(conn);
                }
                else
                {
                    HashSet<NetworkConnection> connections = new HashSet<NetworkConnection>();
                    connections.Add(conn);
                    clientChunkRequests.Add(pos, connections);

                    spawnChunkCallback?.Invoke(pos);
                }
            }
        }

        public void OnVoxelUpdateRequest(VoxelUpdateRequestMessage message)
        {
            Vector3Int chunkPos = StaticWorldData.ConvectWorldVoxelToChunk(message.pos);
            Vector3Int localPos = StaticWorldData.ConvectWorldVoxelToLocalVoxel(message.pos);

            Debug.Log($"Editing Global: {message.pos} Chunk: {chunkPos} at Local: {localPos}");

            if (chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk.EditVoxel(localPos, message.data);
                updateAllRequests.Add(chunkPos);
            }
        }
    }
}