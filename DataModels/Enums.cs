using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public static class Enums
    {
        public enum Direction : byte
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

        public enum Layers : byte
        {
            background = 0, //zero
            midground = 1, //back
            buildings = 2, //sprite
            foreground = 3, //forward
            front = 4 //front
        }

        public enum BodyParts : byte
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

        public enum EquipmentType
        {
            None =0,
            Helmet = 1,
            Torso = 2,
            Legs = 3,
            Boots = 4,
            Trinket = 5
        }

        public enum ShadowSize
        {
            None = 0,
            Small = 1,
            Medium = 2,
            Large = 3
        }

    }
}
