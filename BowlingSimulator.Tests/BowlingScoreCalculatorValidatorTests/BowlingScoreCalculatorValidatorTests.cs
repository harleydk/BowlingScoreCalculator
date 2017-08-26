using BowlingScoreCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using static BowlingScoreCalculator.TraditionalBowlingScoreCalculatorValidator;

namespace BowlingGameScoreCalculator.Tests.UnitTests
{
    [TestClass]
    public class BowlingScoreCalculatorValidatorTests
    {
        [TestMethod]
        public void TestCanConvertTestDataToBowlingFrames()
        {
            // arrange
            TraditionalBowlingScoringCalculator traditionalBowlingScoringCalculator =
                new TraditionalBowlingScoringCalculator();
            TraditionalBowlingScoreCalculatorValidator traditionalBowlingScoreCalculatorValidator =
                new TraditionalBowlingScoreCalculatorValidator(traditionalBowlingScoringCalculator);

            int[][] rawTestData =
            {
                new int[] {1, 2},
                new int[] {6, 3},
                new int[] {4, 6}
            };
            TraditionalBowlingScoreCalculatorValidator.TestGameDataDto testDataDto =
                new TraditionalBowlingScoreCalculatorValidator.TestGameDataDto
                {
                    points = rawTestData
                };

            // act
            IList<BowlingFrame> bowlingFrames =
                traditionalBowlingScoreCalculatorValidator.ConvertTestDataToBowlingFrames(testDataDto);

            // assert
            Assert.AreEqual(bowlingFrames.Count, 3);
            Assert.AreEqual(bowlingFrames[0].Points.Length, 2);
            Assert.AreEqual(bowlingFrames[0].Points[0], 1);
            Assert.AreEqual(bowlingFrames[2].Points[1], 6);
        }

        [TestMethod()]
        public void TestCanConstructCalculatedScoreObjectTest()
        {
            // arrange
            TraditionalBowlingScoringCalculator traditionalBowlingScoringCalculator =
                new TraditionalBowlingScoringCalculator();
            TraditionalBowlingScoreCalculatorValidator traditionalBowlingScoreCalculatorValidator =
                new TraditionalBowlingScoreCalculatorValidator(traditionalBowlingScoringCalculator);

            IList<BowlingFrame> playedFrames = new List<BowlingFrame>()
            {
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{3,7}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{8,2}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{8,1}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{10,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{3,4}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{7,0}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{5,5}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{3,2}),
                BowlingFrame.CreateAndValidateFrameFromPoints(new int[]{2,5})
            };

            // act
            CalculatedBowlingScoresDto calculatedBowlingScoresDto = traditionalBowlingScoreCalculatorValidator.ConstructScoresToValidate(playedFrames, traditionalBowlingScoringCalculator);

            // assert
            Assert.AreEqual(calculatedBowlingScoresDto.points.Length, 10);
            Assert.AreEqual(calculatedBowlingScoresDto.points.First(), 20);
            Assert.AreEqual(calculatedBowlingScoresDto.points.Last(), 123);
        }

        [TestMethod()]
        public void TestCanHandleErronous11thFrameIssue_strike()
        {
            // arrange
            TraditionalBowlingScoringCalculator traditionalBowlingScoringCalculator =
                new TraditionalBowlingScoringCalculator();
            TraditionalBowlingScoreCalculatorValidator traditionalBowlingScoreCalculatorValidator =
                new TraditionalBowlingScoreCalculatorValidator(traditionalBowlingScoringCalculator);

            TestGameDataDto testGameDataDto = new TestGameDataDto();
            int[][] testGamePoints = new int[11][];
            for (int i = 0; i <= 10; i++)
                testGamePoints[i] = new int[] { 10, 0 };
            testGamePoints[10] = new int[] { 10, 10 }; // simulates the 11th frame issue
            testGameDataDto.points = testGamePoints;

            // act
            traditionalBowlingScoreCalculatorValidator.HandleErronous11thFrameIssue(testGameDataDto);

            //assert
            Assert.IsTrue(testGameDataDto.points.Length == 10);
            Assert.AreEqual(testGameDataDto.points[9].Length, 3);
            Assert.IsTrue(testGameDataDto.points[9].All(point => point == 10));
        }

        [TestMethod()]
        public void TestCanHandleErronous11thFrameIssue_spare()
        {
            // arrange
            TraditionalBowlingScoringCalculator traditionalBowlingScoringCalculator =
                new TraditionalBowlingScoringCalculator();
            TraditionalBowlingScoreCalculatorValidator traditionalBowlingScoreCalculatorValidator =
                new TraditionalBowlingScoreCalculatorValidator(traditionalBowlingScoringCalculator);

            TestGameDataDto testGameDataDto = new TestGameDataDto();
            int[][] testGamePoints = new int[11][];
            for (int i = 0; i <= 10; i++)
                testGamePoints[i] = new int[] { 8, 2 };
            testGamePoints[10] = new int[] { 5, 0 }; // simulates the 11th frame issue
            testGameDataDto.points = testGamePoints;

            // act
            traditionalBowlingScoreCalculatorValidator.HandleErronous11thFrameIssue(testGameDataDto);

            //assert
            Assert.IsTrue(testGameDataDto.points.Length == 10);
            Assert.AreEqual(testGameDataDto.points[9].Length, 3);
            Assert.AreEqual(testGameDataDto.points[9].Sum(), 15);
        }
    }
}