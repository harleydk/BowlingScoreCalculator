using BowlingScoreCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BowlingGameScoreCalculator.Tests.UnitTests
{
    [TestClass]
    public class SingleFrameScoreCalculatorTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateScoreForFrameTest_frameNumberOutOfBounds()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{7,3})
            };

            // act/assert
            scoreCalculator.CalculateScoreForSingleFrame(2, playedFrames);
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_noStrikesGame()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{7,2}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,0})
            };

            // act
            int frame1Score = scoreCalculator.CalculateScoreForSingleFrame(1, playedFrames);
            int frame2Score = scoreCalculator.CalculateScoreForSingleFrame(2, playedFrames);

            // assert
            Assert.AreEqual(frame1Score, 9);
            Assert.AreEqual(frame2Score, 5);
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_singleFrameStrikeNoOtherFrames()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0})
            };

            // act
            int frame1Score = scoreCalculator.CalculateScoreForSingleFrame(1, playedFrames);

            // assert
            Assert.AreEqual(frame1Score, 10);
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_firstFrameIsAStrikeNextIsOpenFrame()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,1})
            };

            // act
            int frame1Score = scoreCalculator.CalculateScoreForSingleFrame(1, playedFrames);
            int frame2Score = scoreCalculator.CalculateScoreForSingleFrame(2, playedFrames);

            // assert
            Assert.AreEqual(frame1Score, 16);
            Assert.AreEqual(frame2Score, 6);
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_firstFrameIsAStrikeNextIsStrikeThirdIsOpen()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints( new int[]{5,1}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,1})
            };

            // act
            int frame1Score = scoreCalculator.CalculateScoreForSingleFrame(1, playedFrames);
            int frame2Score = scoreCalculator.CalculateScoreForSingleFrame(2, playedFrames);

            // assert
            Assert.AreEqual(frame1Score, 16);
            Assert.AreEqual(frame2Score, 6);
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_firstFrameIsSpareFrameSecondIsOpenFrame()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{9,1}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,3})
            };

            // act
            int frame1Score = scoreCalculator.CalculateScoreForSingleFrame(1, playedFrames);
            int frame2Score = scoreCalculator.CalculateScoreForSingleFrame(2, playedFrames);

            // assert
            Assert.AreEqual(frame1Score, 15);
            Assert.AreEqual(frame2Score, 8);
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_firstFrameIsAStrikeNextIsSpareThirdIsOpen()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,5}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,1})
            };

            // act
            int frame1Score = scoreCalculator.CalculateScoreForSingleFrame(1, playedFrames);
            int frame2Score = scoreCalculator.CalculateScoreForSingleFrame(2, playedFrames);
            int frame3Score = scoreCalculator.CalculateScoreForSingleFrame(3, playedFrames);

            // assert
            Assert.AreEqual(frame1Score, 20);
            Assert.AreEqual(frame2Score, 15);
            Assert.AreEqual(frame3Score, 6);
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_SpecificGame()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{3,7}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{8,2}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{8,1}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{3,4 }),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{7,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,5}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{3,2}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{2,5})
            };

            int totalScore = 0;
            int[] expectedResults = new[] { 20, 20, 18, 9, 17, 7, 7, 13, 5, 7 };
            for (int i = 0; i < playedFrames.Count; i++)
            {
                // act
                int frameScore = scoreCalculator.CalculateScoreForSingleFrame(i + 1, playedFrames);
                totalScore += frameScore;

                // assert
                Assert.AreEqual(frameScore, expectedResults[i]);
            }

            Assert.AreEqual(totalScore, 123);
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_PerfectGameOfOnlyStrikes()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>();
            for (int i = 0; i < 9; i++)
                playedFrames.Add(BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 0 }));
            playedFrames.Add(BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 10, 10 }));

            int totalScore = 0;
            int[] expectedResults = new[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 };
            for (int i = 0; i < playedFrames.Count; i++)
            {
                // act
                int frameScore = scoreCalculator.CalculateScoreForSingleFrame(i + 1, playedFrames);
                totalScore += frameScore;

                // assert
                Assert.AreEqual(frameScore, expectedResults[i]);
            }

            Assert.AreEqual(totalScore, 300);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateScoreForFrameTest_SuperflousFrame()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>();
            for (int i = 0; i < 11; i++)
                playedFrames.Add(BowlingFrame.CreateAndValidateFrameFromPoints(new int[] { 10, 10, 10 }));

            // act/assert
            int frameScore = scoreCalculator.CalculateScoreForSingleFrame(3, playedFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateScoreForFrameTest_TooManyFramePoints()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,4,3})
            };

            // act/assert
            int frameScore = scoreCalculator.CalculateScoreForSingleFrame(2, playedFrames);
        }

        [TestMethod()]
        public void TestCanAddBonusPointsFromNextFrames()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,4})
            };

            // act
            int bonusPoints = scoreCalculator.GetBonusPointsFromNextFrames(2, 2, playedFrames);

            // assert
            Assert.AreEqual(bonusPoints, 9);
        }
    }
}