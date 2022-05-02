using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
using ItemEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.PlayerStuff
{
    /// <summary>
    /// A class used to save the state of player progress. For now that is:
    ///Recipes
    /// </summary>
    internal class ProgressManager : ISaveable
    {
        public List<string> UnlockedRecipes { get; private set; }

        public ProgressManager()
        {
            UnlockedRecipes = new List<string>();

        }
        public void LoadContent()
        {
            List<ItemData> itemsWithDefaultUnlockedRecipes = ItemFactory.ItemData.Where(x => x.RecipeInfo != null && !x.RecipeInfo.StartsLocked).ToList();
            foreach (ItemData item in itemsWithDefaultUnlockedRecipes)
                UnlockRecipe(item.Name);
        }
        public void UnlockRecipe(string name)
        {
            if (UnlockedRecipes.Contains(name))
                throw new Exception($"Recipe already unlocked");

            UnlockedRecipes.Add(name);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(UnlockedRecipes.Count);
            foreach (var item in UnlockedRecipes)
            {
                writer.Write(item);
            }
        }

        public void LoadSave(BinaryReader reader)
        {
            CleanUp();
            int unlockedRecipeCount = reader.ReadInt32();
            for (int i = 0; i < unlockedRecipeCount; i++)
            {
                string name = reader.ReadString();
                UnlockedRecipes.Add(name);
                ItemFactory.RecipeHelper.UnlockNewRecipe(name);
            }
                
        }

        public void CleanUp()
        {
            UnlockedRecipes.Clear();
        }
    }
}