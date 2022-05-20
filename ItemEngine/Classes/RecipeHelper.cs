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

        private List<RecipeInfo> _unlockedRecipes;
        public RecipeHelper()
        {
            _allRecipeInfo = new List<RecipeInfo>();
            _unlockedRecipes = new List<RecipeInfo>();
        }
        public void UnlockNewRecipe(string recipeName)
        {
            _unlockedRecipes.Add(_allRecipeInfo.FirstOrDefault(x => x.Name == recipeName));
        }
        public void LoadContent(List<ItemData> itemData)
        {
            _allRecipeInfo.AddRange(itemData.Where(x => x.RecipeInfo != null).Select(x => x.RecipeInfo).ToList());
        }

        public RecipeInfo GetParentRecipe(RecipeInfo childRecipe)
        {
            return null;
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

        /// <summary>
        /// Returns the item which would be created by combining the given ingredients
        /// </summary>
        /// <param name="ingredientsSelected">The ingredients placed in the crafting menu</param>
        /// <param name="availableRecipes">Recipes unlocked by the player</param>
        /// <returns></returns>
        public ItemData Cook(List<ItemDataDTO> ingredientsSelected)
        {
            List<string> ingredientNames = new List<string>();

            List<RecipeInfo> recipes = _unlockedRecipes;
            foreach(ItemDataDTO itemData in ingredientsSelected)
            {
                ingredientNames.Add(itemData.ItemData.Name);
               // recipes = recipes.Where(x => x.Ingredients.Contains(itemData.ItemData.Name)).ToList();
            }
            if (recipes.Count == 1)
                return ItemFactory.GetItemData(recipes[0].Name);
            else
                throw new Exception($"Recipe not found, or too many recipes found");

          
        }
    }
}