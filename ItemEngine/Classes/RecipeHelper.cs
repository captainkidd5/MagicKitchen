using DataModels.ItemStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes
{
    public class RecipeHelper
    {
        List<RecipeInfo> _allRecipeInfo;
        public RecipeHelper()
        {
            _allRecipeInfo = new List<RecipeInfo>();
        }

        public void LoadContent(List<ItemData> itemData)
        {
            _allRecipeInfo.AddRange(itemData.Where(x => x.RecipeInfo != null).Select(x => x.RecipeInfo).ToList());
        }

        public RecipeInfo GetParentRecipe(RecipeInfo childRecipe)
        {
            return childRecipe.
        }
        public List<RecipeInfo> GetAllSubRecipes(RecipeInfo infoToTest)
        {
            List<RecipeInfo> returnInfo = new List<RecipeInfo>();
            
        }
    }
}
