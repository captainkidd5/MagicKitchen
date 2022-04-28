using DataModels;
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
        public List<int> UnlockedRecipes { get; private set; }

        public ProgressManager()
        {
            UnlockedRecipes = new List<int>();

        }
        public void LoadContent()
        {
            List<ItemData> itemsWithDefaultUnlockedRecipes = ItemFactory.ItemData.Where(x => x.RecipeInfo != null && !x.RecipeInfo.StartsLocked).ToList();
            foreach(ItemData item in itemsWithDefaultUnlockedRecipes)
                UnlockRecipe(item.Id);
        }
        public void UnlockRecipe(int id)
        {
            if (UnlockedRecipes.Contains(id))
                throw new Exception($"Recipe already unlocked");

            UnlockedRecipes.Add(id);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(UnlockedRecipes.Count);
            foreach(var item in UnlockedRecipes)
            {
                writer.Write(item);
            }
        }

        public void LoadSave(BinaryReader reader)
        {
            CleanUp();
            int unlockedRecipeCount = reader.ReadInt32();
            for(int i = 0; i < unlockedRecipeCount; i++)
                UnlockedRecipes.Add(reader.ReadInt32());
        }

        public void CleanUp()
        {
            UnlockedRecipes.Clear();
        }
    }
}
