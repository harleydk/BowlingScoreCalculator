using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace BowlingScoreCalculator
{
    /// <summary>
    /// Validates a bowling score calculator against a REST api, from whence we get a series of points and validates their calculated scores.
    /// </summary>
    /// <remarks>
    /// The REST api validates scores by way of traditional 10-pin bowling scoring rules.
    /// </remarks>
    public class TraditionalBowlingScoreCalculatorValidator
    {
        /// <summary>
        /// Data transfer object, used in getting test-data from the validator api-service.
        /// </summary>
        internal class TestGameDataDto

        {
            public int[][] points;
            public string token;
        }

        /// <summary>
        /// Data transfer object, used in sending data to the validator api-service, for validation.
        /// </summary>
        internal class CalculatedBowlingScoresDto
        {
            public int[] points;
            public string token;
        }

        private readonly TraditionalBowlingScoringCalculator _scoreCalculator;

        /// <summary>
        /// Token used in coupling requests of test-data with response of a calculated result.
        /// </summary>
        private string _apiValidationToken;

        public TraditionalBowlingScoreCalculatorValidator(TraditionalBowlingScoringCalculator traditionalBowlingScoreCalculator)
        {
            #region Guard clause

            if (traditionalBowlingScoreCalculator == null)
                throw new ArgumentException("traditionalBowlingScoreCalculator can't be null.");

            #endregion Guard clause

            _scoreCalculator = traditionalBowlingScoreCalculator;
        }

        public BowlingScoreValidatorResultEnum ValidateBowlingScoreRules()
        {
            try
            { 
                TestGameDataDto playedFramesTestData = GetPlayedTestFramesFromRestApi(out _apiValidationToken);
                IList<BowlingFrame> playedBowlingFrames = ConvertTestDataToBowlingFrames(playedFramesTestData);

                CalculatedBowlingScoresDto calculatedBowlingGameScoreDto = ConstructScoresToValidate(playedBowlingFrames, _scoreCalculator);
                BowlingScoreValidatorResultEnum validationResult = ValidateCalculatedGameScoreAgainstApi(calculatedBowlingGameScoreDto);

                return validationResult;
            }
            catch (WebException)
            {
                return BowlingScoreValidatorResultEnum.ScoreCalculatorValidationServiceInaccesible;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets bowling test-data from a REST api.
        /// </summary>
        /// <remarks>
        /// Also retrieved from the rest api is an api-token, that we save for when we'll validate the calculated scores against the same service.
        /// </remarks>
        internal TestGameDataDto GetPlayedTestFramesFromRestApi(out string apiToken)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["BowlingScoreCalculatorValidationServiceUrl"];
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                    throw new WebException("The REST api used for retrieving test-data didn't respond in an orderly fashion");

                TestGameDataDto parsedServiceData = Newtonsoft.Json.JsonConvert.DeserializeObject<TestGameDataDto>(response.Content.ReadAsStringAsync().Result);
                HandleErronous11thFrameIssue(parsedServiceData);
                apiToken = parsedServiceData.token;
                return parsedServiceData;
            }
            catch (Newtonsoft.Json.JsonException)
            {
                throw new WebException("The test-data retrived from the REST api could not be converted into something meaningful.");
            }
            catch (Exception)
            {
                throw new WebException("The REST api used for retrieving test-data didn't respond in an orderly fashion");
            }
        }

        /// <summary>
        /// The REST api returns an extra 11th frame in case of a strike or spare in the 10th. That's not as per the traditonal scoring rule,
        /// so we'll need make up for that.
        /// </summary>
        internal void HandleErronous11thFrameIssue(TestGameDataDto parsedServiceData)
        {
            if (parsedServiceData.points.Length > 10 &&
                parsedServiceData.points[9].Sum() == 10)
            {
                // combine 10th and 11th frame and remove the superfluous frame.
                if (parsedServiceData.points[9][0] == 10 /* strike */)
                {
                    parsedServiceData.points[9] = new int[] {
                       10,
                        parsedServiceData.points[10][0],
                        parsedServiceData.points[10][1] };
                }
                else /* spare */
                {
                    parsedServiceData.points[9] = new int[] {
                       parsedServiceData.points[9][0],
                        parsedServiceData.points[9][1],
                        parsedServiceData.points[10][0] };
                }

                Array.Resize(ref parsedServiceData.points, 10);
            }
        }

        internal IList<BowlingFrame> ConvertTestDataToBowlingFrames(TestGameDataDto playedFramesPocos)
        {
            IList<BowlingFrame> playedFrames = new List<BowlingFrame>();
            for (int i = 0; i < playedFramesPocos.points.Length; i++)
            {
                BowlingFrame playedFrame = BowlingFrame.CreateAndValidateFrameFromPoints(playedFramesPocos.points[i]);
                playedFrames.Add(playedFrame);
            }

            return playedFrames;
        }

        internal CalculatedBowlingScoresDto ConstructScoresToValidate(IList<BowlingFrame> playedBowlingFrames, TraditionalBowlingScoringCalculator scoreCalculator)
        {
            CalculatedBowlingScoresDto validationObject = new CalculatedBowlingScoresDto()
            {
                token = _apiValidationToken,
                points = new int[playedBowlingFrames.Count]
            };

            int summarizedGameScore = 0;
            for (int i = 0; i < playedBowlingFrames.Count; i++)
            {
                summarizedGameScore += _scoreCalculator.CalculateScoreForSingleFrame(i + 1, playedBowlingFrames);
                validationObject.points[i] = summarizedGameScore;
            }

            return validationObject;
        }

        private BowlingScoreValidatorResultEnum ValidateCalculatedGameScoreAgainstApi(CalculatedBowlingScoresDto calculatedBowlingScoresDTO)
        {
            string calculatedBowlingScoresAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(calculatedBowlingScoresDTO);
            StringContent content = new StringContent(calculatedBowlingScoresAsJson, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            string url = ConfigurationManager.AppSettings["BowlingScoreCalculatorValidationServiceUrl"];
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (!response.IsSuccessStatusCode)
                throw new WebException("The REST api used for retrieving test-data didn't respond in an orderly fashion");

            JObject responseObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            bool scoresValidatedSuccesfully = (bool)responseObject["success"];

            if (scoresValidatedSuccesfully)
                return BowlingScoreValidatorResultEnum.ScoreCalculatorWorksFine;
            else
                return BowlingScoreValidatorResultEnum.ScoreCalculatorValidationServiceInaccesible;
        }
    }
}