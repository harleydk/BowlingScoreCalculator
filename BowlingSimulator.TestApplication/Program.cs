using BowlingScoreCalculator;
using System;

namespace BowlingGameScoreValidator.TestApplication
{
    /// <summary>
    /// Tests an implementation of a bowling-score rule-set against a REST api.
    /// </summary>
    /// <remarks>
    /// ****************************
    /// We can talk about this, if you like:
    ///
    /// - Seperation of concerns, independence between scoring-calculator and validator; two different endeavors entirely.
    /// - Injection of score-calculator into validator-service, thus satisfying the dependency inversion principle.
    /// - My wife understands this code. Favoring readability and testability courtesy of business objects and natural language naming - though likely fewer lines needed if merely dealing with raw data.
    /// - There're many places where one might introduce abstractions - but given the _currently_ limited scope of the assignment, it would be overkill.
    ///
    /// *****************************
    ///</remarks>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(@"GREETINGS PROFESSOR FALKEN.");
            Console.WriteLine(@"SHALL WE PLAY A GAME?");
            Console.WriteLine(@"SIMULATING INDIVIDUAL GAMES OF BOWLING.");

            TraditionalBowlingScoringCalculator traditionalBowlingScoreCalculator = new TraditionalBowlingScoringCalculator();
            TraditionalBowlingScoreCalculatorValidator bowlingScoreValidator = new TraditionalBowlingScoreCalculatorValidator(traditionalBowlingScoreCalculator);

            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    BowlingScoreValidatorResultEnum calculatedScoresValidateCorrectly = bowlingScoreValidator.ValidateBowlingScoreRules();
                    switch (calculatedScoresValidateCorrectly)
                    {
                        case BowlingScoreValidatorResultEnum.ScoreCalculatorWorksFine:
                            Console.WriteLine($"Game {i} was scored and validated correctly.");
                            break;

                        case BowlingScoreValidatorResultEnum.ScoreCalculatorWorksNotSoMuch:
                            Console.WriteLine($"Could not validate scores for Game {i}.");
                            break;

                        case BowlingScoreValidatorResultEnum.ScoreCalculatorValidationServiceInaccesible:
                            Console.WriteLine($"Could not validate scores for Game {i} - validation-service could not be reached.");
                            break;
                    }

                    System.Threading.Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not validate scores for Game {i} - some error '{ex.Message}' got in the way.");
                }
            }

            Console.WriteLine("PRESS ANY KEY TO EXIT.");
            Console.ReadKey();
        }
    }
}