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

        public enum Layers
        {
            background = 0, //zero
            midground = 1, //back
            buildings = 2, //sprite
            foreground = 3, //forward
            front = 4 //front
        }

        public enum BodyParts
        {
            None = 0,
            Hat = 10,
            Hair = 9,
            Eyes = 8,
            Head = 7,
            Shoulders = 6,
            Arms = 5,
            Shirt = 4,
            Shoes = 3,
            Pants = 2
        }

    }
}
