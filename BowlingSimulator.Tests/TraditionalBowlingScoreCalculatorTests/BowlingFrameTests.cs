using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BowlingScoreCalculator.Tests
{
    [TestClass()]
    public class BowlingFrameTests
    {
        [TestMethod()]
        public void BowlingFrameTest_superfluousZerosRemoved()
        {
            // arrange/act
            BowlingFrame bowlingFrame = BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 0 });

            // assert
            Assert.IsTrue(bowlingFrame.Points.Length == 1); // unnecessary 0 removed
            Assert.AreEqual(bowlingFrame.Points[0], 10);
        }

        [TestMethod()]
        public void CanValidateFrame_goodFrame()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 5, 2 });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CanValidateFrame_invalidRange()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 11, 2 });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CanValidateFrame_noPoints()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CanValidateFrame_strikeAndPointInSameFrame()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 2 });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CanValidateFrame_onlyOnePoint()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 5 });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CanValidateFrame_lastFrameBad()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 8, 7 });
        }

        [TestMethod()]
        public void CanValidateFrame_lastFrameGood()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 8, 1 });
        }

        [TestMethod()]
        public void CanValidateFrame_lastFrameSpare()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 8, 2 });
        }

        [TestMethod()]
        public void CanValidateFrame_lastFrameSpareThenStrike()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 8, 2, 10 });
        }

        [TestMethod()]
        public void CanValidateFrame_optimalLastFrame()
        {
            // arrange/act/assert
            BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 10, 10 });
        }
    }
}