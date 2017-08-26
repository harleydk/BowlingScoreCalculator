using BowlingScoreCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BowlingScoreCalculator.TraditionalBowlingScoreCalculatorValidator;

namespace BowlingGameScoreCalculator.Tests.BowlingScoreCalculatorValidatorTests.IntegrationTests
{
    [TestClass]
    public class TestApiIntegrationTests
    {
        [TestMethod]
        public void TestCanGetBowlingTestDataFromRestApi()
        {
            // arrange
            TraditionalBowlingScoringCalculator traditionalBowlingScoringCalculator =
                new TraditionalBowlingScoringCalculator();
            TraditionalBowlingScoreCalculatorValidator traditionalBowlingScoreCalculatorValidator =
                new TraditionalBowlingScoreCalculatorValidator(traditionalBowlingScoringCalculator);

            // act
            string retrievedApiToken;
            TestGameDataDto testData = traditionalBowlingScoreCalculatorValidator.GetPlayedTestFramesFromRestApi(out retrievedApiToken);

            // assert
            Assert.IsTrue(testData.points.Length > 0);
            Assert.IsNotNull(retrievedApiToken);
        }

        [TestMethod]
        public void TestCanValidateBowlingScoreRules()
        {
            // arrange
            TraditionalBowlingScoringCalculator traditionalBowlingScoringCalculator =
                new TraditionalBowlingScoringCalculator();
            TraditionalBowlingScoreCalculatorValidator traditionalBowlingScoreCalculatorValidator =
                new TraditionalBowlingScoreCalculatorValidator(traditionalBowlingScoringCalculator);

            // act
            BowlingScoreValidatorResultEnum bowlingScoreValidatorResult = traditionalBowlingScoreCalculatorValidator.ValidateBowlingScoreRules();

            // assert
            if (bowlingScoreValidatorResult == BowlingScoreValidatorResultEnum.ScoreCalculatorValidationServiceInaccesible)
                Assert.Inconclusive("The REST service that's used to validate the bowlingscore-rules is currently unavailable");
            else
                Assert.AreEqual(bowlingScoreValidatorResult, BowlingScoreValidatorResultEnum.ScoreCalculatorWorksFine);
        }
    }
}