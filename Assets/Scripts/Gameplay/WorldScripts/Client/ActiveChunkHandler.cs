using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Client
{
    public class ActiveChunkHandler
    {
        public HashSet<Vector3Int> activeChunks = new HashSet<Vector3Int>();

        Utils.ScriptableReference.Vector3Reference playerPosition;
        ClientLegacy.ClientDimensionSettings settings;
        Vector3Int lastPosition = new Vector3Int(99999999, 99999999, 99999999);

        public ActiveChunkHandler(ClientLegacy.ClientDimensionSettings _settings, Utils.ScriptableReference.Vector3Reference _playerPosition)
        {
            settings = _settings;
            playerPosition = _playerPosition;
        }

        public IEnumerator UpdateActiveChunks()
        {
            Vector3Int pos = ConvectToChunk(playerPosition.value);

            if (pos != lastPosition)
            {
                ComputeShader shader = settings.activeChunkShader;
                int kernelIndex = shader.FindKernel("CSMain");

                int chunkHeightCount = -settings.worldHeights.x + settings.worldHeights.y + 1;

                //Create Buffers
                ComputeBuffer chunkBuffer = new ComputeBuffer(1024 * chunkHeightCount, sizeof(int) * 3, ComputeBufferType.Append);
                ComputeBuffer chunkCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

                chunkBuffer.SetCounterValue(0);

                shader.SetInt("playerPosX", pos.x);
                shader.SetInt("playerPosY", pos.z);
                shader.SetInt("undergroundDepth", settings.worldHeights.x);
                shader.SetFloat("renderDistance", settings.renderDistance);

                shader.SetBuffer(kernelIndex, "chunks", chunkBuffer);

                shader.Dispatch(kernelIndex, 1, chunkHeightCount, 1);

                yield return null;

                //Retrieve Data
                ComputeBuffer.CopyCount(chunkBuffer, chunkCountBuffer, 0);
                int[] chunkCountArray = { 0 };
                chunkCountBuffer.GetData(chunkCountArray);
                int numChunks = chunkCountArray[0];

                Vector3Int[] _chunks = new Vector3Int[numChunks];
                chunkBuffer.GetData(_chunks, 0, 0, numChunks);

                //Release Buffers
                chunkBuffer.Release();
                chunkCountBuffer.Release();

                activeChunks.Clear();
                for (int i = 0; i < _chunks.Length; i++)
                {
                    activeChunks.Add(_chunks[i]);
                }
            }

            lastPosition = pos;
        }

        private Vector3Int ConvectToChunk(Vector3 playerPos)
        {
            Vector3Int _pos = new Vector3Int(
                Mathf.FloorToInt(playerPos.x), 
                Mathf.FloorToInt(playerPos.y), 
                Mathf.FloorToInt(playerPos.z));

            if (_pos.x < 0)
                _pos.x -= 15;
            if (_pos.y < 0)
                _pos.y -= 15;
            if (_pos.z < 0)
                _pos.z -= 15;

            Vector3Int pos = _pos / StaticWorldData.CHUNK_SIZE;

            return pos;
        }
    }
}