using DataModels.ItemStuff;
using Microsoft.Xna.Framework;
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
            return _allRecipeInfo.FirstOrDefault(x => x.Name == childRecipe.BaseIngredient);
        }
        public List<RecipeInfo> GetAllSubRecipes(RecipeInfo infoToTest)
        {
            List<RecipeInfo> returnInfo = new List<RecipeInfo>();
            returnInfo.Add(infoToTest);
            while(true)
            {
                infoToTest = GetParentRecipe(infoToTest);
                if (infoToTest == null || !infoToTest.ShownInParentRecipes)
                    break;
                returnInfo.Add(infoToTest);
            }
            returnInfo.Reverse();
            return returnInfo;
            
        }

        public Rectangle GetCookActionRectangleFromAction(CookAction cookAction)
        {
            switch (cookAction)
            {
                case CookAction.None:
                    break;
                case CookAction.Add:
                    break;
                case CookAction.Chop:
                    break;
                    //little oven icon
                case CookAction.Bake:
                    return new Rectangle(352, 208, 16, 16);
            }
            throw new Exception($"Cook action {cookAction.ToString()} not supported");
        }
    }
}