using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BowlingScoreCalculator.Tests
{
    [TestClass()]
    public class GameScoreCalculatorTests
    {
        [TestMethod()]
        public void CalculateScoreForFrameTest_summationsForOngoingPlay()
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

            int[] pointsSummationsDuringPlay = new[] { 10, 30, 50, 67, 77, 91, 98, 108, 116, 123 };

            for (int i = 0; i < playedFrames.Count; i++)
            {
                scoreCalculator.AddPlayedBowlingFrame(playedFrames[i]);

                // assert
                int summarizedGameScore = scoreCalculator.CalculateTotalScoreForGame();
                Assert.AreEqual(summarizedGameScore, pointsSummationsDuringPlay[i]);
            }
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_summationsForFinishedGame()
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

            int[] pointsSummationsAfterGame = new[] { 20, 40, 58, 67, 84, 91, 98, 111, 116, 123 };

            for (int i = 0; i < playedFrames.Count; i++)
                scoreCalculator.AddPlayedBowlingFrame(playedFrames[i]);

            int totalPoints = 0;
            for (int j = 1; j <= playedFrames.Count; j++)
            {
                totalPoints += scoreCalculator.CalculateScoreForSingleFrame(j, playedFrames);
                Assert.AreEqual(totalPoints, pointsSummationsAfterGame[j - 1]);
            }
        }

        [TestMethod()]
        public void CalculateScoreForFrameTest_summationsForFinishedGame_variant()
        {
            // arrange
            TraditionalBowlingScoringCalculator scoreCalculator = new TraditionalBowlingScoringCalculator();
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{7,1}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{0,2}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{0,10}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{6,4}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0})
            };

            int[] pointsSummationsAfterGame = new[] { 8, 10, 26, 46, 66, 76 };

            for (int i = 0; i < playedFrames.Count; i++)
                scoreCalculator.AddPlayedBowlingFrame(playedFrames[i]);

            int totalPoints = 0;
            for (int j = 1; j <= playedFrames.Count; j++)
            {
                totalPoints += scoreCalculator.CalculateScoreForSingleFrame(j, playedFrames);
                Assert.AreEqual(totalPoints, pointsSummationsAfterGame[j - 1]);
            }
        }
    }
}