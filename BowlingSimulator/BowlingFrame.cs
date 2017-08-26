using System;
using System.Linq;

namespace BowlingScoreCalculator
{
    /// <summary>
    /// A bowling game consists of a number of 'frames', which are containers for a number of rolls of the bowling ball; traditional bowling games consists of 10 frames.
    /// </summary>
    public class BowlingFrame
    {
        public int[] Points { get; private set; }

        private BowlingFrame(int[] points)
        {
            if (points.Length > 1 &&
                points.Length < 3 &&
                points[0] == 10
                && points[1] == 0)
            {
                Points = new int[] { 10 }; // throwing a strike effectively makes it a one-shot frame, so we don't need the 0.
            }
            else
                Points = points;
        }

        public static BowlingFrame CreateAndValidateFrameFromPoints(int[] points)
        {
            BowlingFrame newBowlingFrame = new BowlingFrame(points);
            ValidateFrame(points);

            return newBowlingFrame;
        }

        internal static void ValidateFrame(int[] points)
        {
            if (points == null || points.Length == 0 || points.Length > 3)
                throw new ArgumentException("Bad frame: no points.");

            // regular frame checks
            if (points.Any(point => point > 10))
                throw new ArgumentException("Bad frame: 10 is max score for a single point.");

            if (points.Length == 1 && points[0] < 10)
                throw new ArgumentException("Bad frame: Only one point, and it's not a strike? Bad frame.");

            if (points.Length == 2 && points.Sum() > 10)
                throw new ArgumentException("Bad frame: Two throws, totalling > 10? Bad frame.");

            // last frame checks - only the final may have 3 throws
            if (points.Length == 3)
            {
                // 3 points, first is not a strike so should be a spare
                if (points[0] != 10 && points.Take(2).Sum() != 10)
                    throw new ArgumentException("Bad frame: More than 3 points to a frame, first is not a strike but a spare isn't registered? Bad frame.");

                if (points[0] == 10 && points[1] != 10 && points.Skip(1).Sum() > 10)
                    throw new ArgumentException("Bad frame: 3 points, first is strike but next yield more than 10 points - bad frame.");
            }
        }
    }
}