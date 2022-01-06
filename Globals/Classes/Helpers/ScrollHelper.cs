using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace Globals.Classes.Helpers
{
    public static class ScrollHelper
    {

        /// <summary>
        /// Changes scroll index based on direction of scroll
        /// </summary>
        /// <param name="direction">Direction to scroll</param>
        /// <param name="currentIndex">index to change from scroll</param>
        /// <param name="maxSize">.Length or .Count (do not subtract 1)</param>
        /// <returns>Returns new index after scroll</returns>
        public static int GetIndexFromScroll(Direction direction, int currentIndex, int maxSize)
        {
            if (maxSize == 0)
                return 0;

            if (direction == Direction.Up)
            {
                if (currentIndex > 0)
                    currentIndex--;
                else currentIndex = maxSize - 1;
            }
            else if (direction == Direction.Down)
            {
                if (currentIndex < maxSize - 1)
                    currentIndex++;
                else
                    currentIndex = 0;
            }
            else
            {
                throw new Exception($"Direction {direction.ToString()} invalid scroll value.");
            }
            return currentIndex;
        }

    }
}
