using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
using Scripts.Gameplay.WorldScripts;

namespace Scripts.Tests.Utils
{
    public class StaticWorldDataTests
    {
        [Test]
        public void ConvertGlobalVoxelToChunkPos()
        {
            Vector3Int[] Positions =
            {
                new Vector3Int(0, 0, (-StaticWorldData.CHUNK_SIZE * 2) - 1),
                new Vector3Int(0, 0, -StaticWorldData.CHUNK_SIZE * 2),
                new Vector3Int(0, 0, -StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0, -StaticWorldData.CHUNK_SIZE),
                new Vector3Int(0, 0, -1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0,  StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0,  StaticWorldData.CHUNK_SIZE),
                new Vector3Int(0, 0, (StaticWorldData.CHUNK_SIZE * 2) - 1),
                new Vector3Int(0, 0,  StaticWorldData.CHUNK_SIZE * 2),
            };

            Vector3Int[] Expected =
            {
                new Vector3Int(0, 0, -3),
                new Vector3Int(0, 0, -2),
                new Vector3Int(0, 0, -2),
                new Vector3Int(0, 0, -1),
                new Vector3Int(0, 0, -1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, 2),
            };

            for (int i = 0; i < Positions.Length; i++)
            {
                Assert.AreEqual(Expected[i], StaticWorldData.ConvectWorldVoxelToChunk(Positions[i]));
            }
        }

        [Test]
        public void ConvertGlobalVoxelToLocalVoxel()
        {
            Vector3Int[] Positions =
            {
                new Vector3Int(0, 0, (-StaticWorldData.CHUNK_SIZE * 2) - 1),
                new Vector3Int(0, 0, -StaticWorldData.CHUNK_SIZE * 2),
                new Vector3Int(0, 0, -StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0, -StaticWorldData.CHUNK_SIZE),
                new Vector3Int(0, 0, -1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0,  StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0,  StaticWorldData.CHUNK_SIZE),
                new Vector3Int(0, 0, (StaticWorldData.CHUNK_SIZE * 2) - 1),
                new Vector3Int(0, 0,  StaticWorldData.CHUNK_SIZE * 2),
            };

            Vector3Int[] Expected =
            {
                new Vector3Int(0, 0, StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, StaticWorldData.CHUNK_SIZE - 1),
                new Vector3Int(0, 0, 0),
            };

            for (int i = 0; i < Positions.Length; i++)
            {
                Assert.AreEqual(Expected[i], StaticWorldData.ConvectWorldVoxelToLocalVoxel(Positions[i]));
            }
        }
    }
}