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

        //Note, these are drawn in the order of their enums
        public enum BodyParts : byte
        {
            None = 0,
            Pants = 1,
            Shoes = 2,
            Shirt = 3,
            Arms = 4,
            Shoulders = 5,

            Head = 6,
            Eyes = 7,

            Hair = 8,
        }

        public enum EquipmentType : byte
        {
            None =0,
            Helmet = 1,
            Torso = 2,
            Legs = 3,
            Boots = 4,
            Trinket = 5
        }

        public static BodyParts? EquipmentTypeToBodyPart(EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.None:
                    return null;
                case EquipmentType.Helmet:
                    return BodyParts.Hair;
                case EquipmentType.Torso:
                    return BodyParts.Shirt;
                case EquipmentType.Legs:
                    return BodyParts.Pants;
                case EquipmentType.Boots:
                    return BodyParts.Shoes;
                case EquipmentType.Trinket:
                   return null;
            }
            return null;
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
