﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;
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


        /// <summary>
        /// Makes sure point is between zero and max values provided
        /// </summary>
        /// <returns></returns>
        public static bool InBounds(Point point, int maxX, int maxY)
        {
            if (point.X < maxX && point.Y < maxY && point.X >= 0 && point.Y >= 0)
                return true;

            return false;
        }

    }
}
