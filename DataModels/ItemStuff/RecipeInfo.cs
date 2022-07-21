using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ItemStuff
{

    public enum CraftingCategory
    {
        None = 0,
        Tool = 1,
        Placeable = 2,
        Refined = 3,
        Equipment = 4
    }
    public enum CraftAction
    {
        None = 0,
        Add = 1,
        Chop  = 2,
        Grill = 3,
        Smelt = 4,
        Craft = 5,
    }
    public class RecipeInfo
    {
        public string Name{ get; set; }


        //if true, player will not start out with this recipe unlocked
        public bool StartsLocked { get; set; }
        public CraftAction CraftAction { get; set; }
        public CraftingCategory CraftingCategory { get; set; }
        public float CookTime { get; set; }

        public List<CraftingIngredient> Ingredients { get; set; }

   
    }
}
