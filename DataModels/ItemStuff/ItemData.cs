using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ItemStuff
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

        public int Price { get; set; }

        //Cooked vs uncooked pie
        public bool IsUnfinishedVersion { get; set; }
        public RecipeInfo RecipeInfo { get; set; }

        public void Load()
        {
            if(RecipeInfo != null)
            RecipeInfo.Name = Name; 
        }


    }
}
