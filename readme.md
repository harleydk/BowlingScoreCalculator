## A bowling score calculator

This repository comprises a C# implementation of a 10-pin bowling score calculator. The goal of this implementation has been to promote a readable and maintainable solution. There is no doubt the code could be easily compressed, yet the focus has been on making the implementation easily understandable (to a human).

Also implemented is a score-calculator validator, which may be used to validate the score-calculator implementation, against a dedicated REST api for this designated purpore.

Also included is a unit-test project, covering both projects.

## Example use of bowling score calculator.

The bowling score calculator computes a total score from the number of played frames of the bowling game - using the traditional 10-pin bowling score rules.

New it up...

TraditionalBowlingScoringCalculator traditionalBowlingScoreCalculator = new TraditionalBowlingScoringCalculator();

... then add frames ...

AddPlayedBowlingFrame(BowlingFrame bowlingFrame);

... and retrieve the total score for the game, with each added frame or at the end of the game:

int totalScore = GetGameScore();

"GetGameScore()" returns the current score, based on the current game points played. Use "CalculateScoreForSingleFrame()" to get the points of the individual frames of the game - though pay in mind that since some scores yield a bonus-score - 'strikes' and 'spares', i.e. - it's not possible to get the individual frame score until the next frame(s) have been played out, or the game has ended.


