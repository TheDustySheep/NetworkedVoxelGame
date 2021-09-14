using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Scripts.Utils;

namespace Scripts.Tests.Utils
{
    public class RunLengthEncoderTests
    {
        private static int[] InputData = new int[]
        {
            0, 0, 0, 0, 0, 0,
            1, 1, 1,
            2, 2, 2,
            0, 0, 0, 0,
        };

        [Test]
        public void TestEncodeCounts()
        {
            RunLengthEncoder.Encode(InputData, out var counts, out var values);
            
            Assert.AreEqual(6, counts[0]);
            Assert.AreEqual(3, counts[1]);
            Assert.AreEqual(3, counts[2]);
            Assert.AreEqual(4, counts[3]);
        }

        [Test]
        public void TestEncodeValues()
        {
            RunLengthEncoder.Encode(InputData, out var counts, out var values);

            Assert.AreEqual(0, values[0]);
            Assert.AreEqual(1, values[1]);
            Assert.AreEqual(2, values[2]);
            Assert.AreEqual(0, values[3]);
        }

        [Test]
        public void TestEncodeCountsLengths()
        {
            RunLengthEncoder.Encode(InputData, out var counts, out var values);

            Assert.AreEqual(4, counts.Length);
        }

        [Test]
        public void TestEncodeValuesLengths()
        {
            RunLengthEncoder.Encode(InputData, out var counts, out var values);

            Assert.AreEqual(4, values.Length);
        }

        [Test]
        public void TestDecodeValues()
        {
            RunLengthEncoder.Encode(InputData, out var counts, out var values);
            RunLengthEncoder.Decode(out var OutputData, counts, values);

            Assert.AreEqual(InputData, OutputData);
        }
    }
}