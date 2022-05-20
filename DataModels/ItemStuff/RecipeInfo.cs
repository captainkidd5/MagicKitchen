using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ItemStuff
{
    public enum CraftAction
    {
        None = 0,
        Add = 1,
        Chop  = 2,
        Bake = 3,
        Mix = 4,
        Smelt = 5,
    }
    public class RecipeInfo
    {
        public string Name{ get; set; }


        //if true, player will not start out with this recipe unlocked
        public bool StartsLocked { get; set; }
        public CraftAction CraftAction { get; set; }
        public float CookTime { get; set; }

        public List<CraftingIngredient> Ingredients { get; set; }

   
    }
}
