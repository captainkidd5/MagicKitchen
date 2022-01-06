using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{

    public enum ItemType
    {
        Axe = 21,
        Hammer = 22,
        Shovel = 23,
        Sword = 25,
        Bow = 26,
        Ammunition = 27,
        Tree = 40

    }
    public class ItemData
    {
        public int Id { get; set; }

        public string Name { get; set; }
         
        public string Description { get; set; }

      

        public int MaxStackSize { get; set; }
        [ContentSerializer(Optional = true)]

        public int Price { get; set; }


    }
}
