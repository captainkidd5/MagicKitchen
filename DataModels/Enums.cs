using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public static class Enums
    {
        public enum Direction
        {
            None = 0,
            Up = 1,
            Down = 2,
            Left = 3,
            Right = 4
        }

        public enum SearchType
        {
            None = 0,
            Grid = 1,
            Radial = 2,
        }

    }
}
