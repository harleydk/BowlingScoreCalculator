using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculator
{
    /// <summary>
    /// Calculates scores for played frames of a bowling game.
    /// </summary>
    /// <seealso cref="https://en.wikipedia.org/wiki/Ten-pin_bowling#Traditional_scoring"/>
    public class TraditionalBowlingScoringCalculator
    {
        public const int MAX_NO_OF_FRAMES = 10;
        private IList<BowlingFrame> _playedFrames = new List<BowlingFrame>();

        /// <summary>
        /// "Implementerer en algoritme som fortløbende kan beregne summen for et spil, der følger 10-kegle reglerne til pointberegning i bowling."
        /// </summary>
        /// <remarks>
        /// We can only summarize the played frames to the _hitherto_ time in the game; we can't look ahead of time.
        /// So if a player starts out with a strike in the first frame, then drops suddenly dead, the total for that game remains a score of 10 -
        /// notwithstanding the sad fact there would have been some bonus-points lurking ahead.
        /// </remarks>
        public int CalculateTotalScoreForGame()
        {
            int scoreForGame = 0;
            for (int i = 0; i < _playedFrames.Count; i++)
                scoreForGame += CalculateScoreForSingleFrame(i + 1, _playedFrames);

            return scoreForGame;
        }

        public void AddPlayedBowlingFrame(BowlingFrame bowlingFrame)
        {
            _playedFrames.Add(bowlingFrame);
        }

        public int CalculateScoreForSingleFrame(int frameNumber, IList<BowlingFrame> playedFrames)
        {
            #region Guard Clause

            if (playedFrames.Count > MAX_NO_OF_FRAMES)
                throw new ArgumentException($"Invalid number of frames - max {MAX_NO_OF_FRAMES} is allowed.");

            if (frameNumber > playedFrames.Count || frameNumber > MAX_NO_OF_FRAMES)
                throw new ArgumentException("The frame-number is out of bounds.");

            #endregion Guard Clause

            BowlingFrame frameToCalculateScoreFor = playedFrames[frameNumber - 1];
            int framePoints = frameToCalculateScoreFor.Points.Sum();

            if (frameToCalculateScoreFor.Points[0] == 10 /* strike */)
                framePoints += GetBonusPointsFromNextFrames(2, frameNumber + 1, playedFrames);
            else if (framePoints == 10 /* spare */)
                framePoints += GetBonusPointsFromNextFrames(1, frameNumber + 1, playedFrames);

            return framePoints;
        }

        internal int GetBonusPointsFromNextFrames(int numberOfBonusPointsToAdd, int frameNumberToGrabBonusFrom, IList<BowlingFrame> playedFrames)
        {
            #region Guard Clause

            if (frameNumberToGrabBonusFrom > playedFrames.Count)
                return 0; // no played frames from whence to get the bonus.

            #endregion Guard Clause

            // lay out all remaining points as an integer-array and simply sum the next <numberOfBonusPointsToAdd> from them:
            List<int> pointsAvailable = new List<int>();
            for (int i = --frameNumberToGrabBonusFrom; i < playedFrames.Count; i++)
                pointsAvailable.AddRange(playedFrames[i].Points);

            if (numberOfBonusPointsToAdd > pointsAvailable.Count)
                return pointsAvailable.Sum(); // need more bonus points, but the game ended abruptly. We'll take what we can get.

            int bonusPoints = pointsAvailable.GetRange(0, numberOfBonusPointsToAdd).Sum();
            return bonusPoints;
        }
    }
}