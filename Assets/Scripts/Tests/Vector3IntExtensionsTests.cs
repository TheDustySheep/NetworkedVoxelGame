using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Tests.Utils
{
    public class Vector3IntExtensionsTests
    {
        [Test]
        public void BitShiftLeftNewAssignment()
        {
            int bitShiftAmount = 7;
            int x = 5;
            int y = 7;
            int z = 9;

            Vector3Int a = new Vector3Int(x, y, z);
            Vector3Int b = a.BitShiftLeft(bitShiftAmount);

            x <<= bitShiftAmount;
            y <<= bitShiftAmount;
            z <<= bitShiftAmount;

            Assert.AreEqual(x, b.x);
            Assert.AreEqual(y, b.y);
            Assert.AreEqual(z, b.z);
        }

        [Test]
        public void BitShiftRightNewAssignment()
        {
            int bitShiftAmount = 7;
            int x = 5;
            int y = 7;
            int z = 9;

            Vector3Int a = new Vector3Int(x, y, z);
            Vector3Int b = a.BitShiftLeft(bitShiftAmount);

            x <<= bitShiftAmount;
            y <<= bitShiftAmount;
            z <<= bitShiftAmount;

            Assert.AreEqual(x, b.x);
            Assert.AreEqual(y, b.y);
            Assert.AreEqual(z, b.z);
        }

        [Test]
        public void BitShiftLeftOverride()
        {
            int bitShiftAmount = 7;
            int x = 5;
            int y = 7;
            int z = 9;

            Vector3Int a = new Vector3Int(x, y, z);
            a = a.BitShiftLeft(bitShiftAmount);

            x <<= bitShiftAmount;
            y <<= bitShiftAmount;
            z <<= bitShiftAmount;

            Assert.AreEqual(x, a.x);
            Assert.AreEqual(y, a.y);
            Assert.AreEqual(z, a.z);
        }

        [Test]
        public void BitShiftRightOverride()
        {
            int bitShiftAmount = 7;
            int x = 5;
            int y = 7;
            int z = 9;

            Vector3Int a = new Vector3Int(x, y, z);
            a = a.BitShiftLeft(bitShiftAmount);

            x <<= bitShiftAmount;
            y <<= bitShiftAmount;
            z <<= bitShiftAmount;

            Assert.AreEqual(x, a.x);
            Assert.AreEqual(y, a.y);
            Assert.AreEqual(z, a.z);
        }
    }
}