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
        Bake = 3
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
        public float CookTime { get; set; }
        public string BaseIngredient { get; set; }
        public string SupplementaryIngredient { get; set; }
    }
}
