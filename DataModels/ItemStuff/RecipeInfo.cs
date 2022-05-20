using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ItemStuff
{
    public enum CookAction
    {
        None = 0,
        Add = 1,
        Chop  = 2,
        Bake = 3,
        Mix = 4,
    }
    public class RecipeInfo
    {
        public string Name{ get; set; }

        //For example, a cheeze pizza recipe should show that dough is required, but will not display how to make dough
        //That should only be visible on the dough recipe page
        public bool ShownInParentRecipes { get; set; }
        //if true, player will not start out with this recipe unlocked
        public bool StartsLocked { get; set; }
        public CookAction CookAction { get; set; }

        //For example, "Add, then bake"
        public CookAction SecondAction { get; set; }
        public float CookTime { get; set; }

        //First ingredient is always base ingredient, if only two ingredients are added
        public List<CraftingIngredient> Ingredients { get; set; }
    }
}
