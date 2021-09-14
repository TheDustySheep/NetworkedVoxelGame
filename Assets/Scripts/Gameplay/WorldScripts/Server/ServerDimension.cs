using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using Scripts.Utils;
using System;
using Mirror;
using Scripts.Gameplay.WorldScripts.Server.TerrainGeneration;

namespace Scripts.Gameplay.WorldScripts.Server
{
    public class ServerDimension : NetworkBehaviour
    {
        [SerializeField] TerrainGenerationSettings settings;

        public int ActiveChunks => chunks.Count;
        public int ChunksToSpawn => spawningHandler == null ? 0 : spawningHandler.ChunkToSpawnCount;
        public float AverageSpawnTime => spawningHandler == null ? 0 : spawningHandler.AverageChunkSpawnTime;
        public long TotalSpawnTime => spawningHandler == null ? 0 : spawningHandler.totalSpawningTime;
        public int TotalChunkSpawns => spawningHandler == null ? 0 : spawningHandler.totalSpawnedChunks;

        public int ClientRequestCount => chunkRequestHandler == null ? 0 : chunkRequestHandler.ActiveClientRequests;
        public int UpdateAllRequestCount => chunkRequestHandler == null ? 0 : chunkRequestHandler.ActiveUpdateAllRequests;

        public ServerDimensionSettings dimensionSettings;

        Dictionary<ChunkPos, Chunk> chunks = new Dictionary<ChunkPos, Chunk>();

        ChunkRequestHandler chunkRequestHandler;
        ChunkSpawningHandler spawningHandler;

        private void Awake()
        {
            spawningHandler = new ChunkSpawningHandler(chunks, this, settings);
            chunkRequestHandler = new ChunkRequestHandler(chunks, spawningHandler.SpawnChunk, dimensionSettings);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            chunkRequestHandler.OnServerStart();

            StartCoroutine(chunkRequestHandler.SendChunkUpdates());
            StartCoroutine(chunkRequestHandler.SendChunkUpdatesAll());
            StartCoroutine(spawningHandler.SpawnChunks());
        }

        /*
        public IEnumerator SendChunkData(NetworkConnection conn, ChunkPos chunkPos)
        {
            if (chunkPos.y < dimensionSettings.worldHeights.x || chunkPos.y > dimensionSettings.worldHeights.y)
                yield break;

            if (activeChunkRequests.TryGetValue(chunkPos, out var requestSet))
            {
                requestSet.Add(conn);
                yield break;
            }
            else
            {
                HashSet<NetworkConnection> connections = new HashSet<NetworkConnection>();
                connections.Add(conn);
                activeChunkRequests.Add(chunkPos, connections);
            }

            if (!chunks.ContainsKey(chunkPos))
            {
                yield return SpawnChunk(chunkPos);
            }

            if (chunks.TryGetValue(chunkPos, out var chunk))
            {

            }
        }

        public IEnumerator SendChunkData(ChunkPos chunkPos)
        {
            if (!chunks.ContainsKey(chunkPos))
            {
                
            }

            if (chunks.TryGetValue(chunkPos, out var chunk))
            {
                ChunkUpdateMessage message =
                    new ChunkUpdateMessage()
                    {
                        ChunkPos = chunkPos,
                        ChunkData = chunk.ChunkData.data
                    };

                NetworkServer.SendToAll(message);
            }
        }




        */
    }
}