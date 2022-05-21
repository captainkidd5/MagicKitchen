﻿using DataModels.ItemStuff;
using ItemEngine.Classes.StorageStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes.CraftingStuff
{
    public class CraftingGuide
    {
        Dictionary<CraftAction, List<ItemData>> CraftActionDictionary;


        /// <summary>
        /// Returns the most complex satisfied recipe from the given recipes
        /// </summary>
        /// <returns></returns>
        public ItemData GetCraftedItem(CraftAction craftAction, List<StorageSlot> storageSlots)
        {
            List<ItemData> itemData = CraftActionDictionary[craftAction];
            ItemData itemDataToReturn = null;
            foreach(ItemData item in itemData)
            {
                if (Satisfied(storageSlots, item.RecipeInfo))
                {
                    if (itemDataToReturn == null)
                        itemDataToReturn = item;
                    if (IsMoreComplex(item.RecipeInfo, itemDataToReturn.RecipeInfo))
                        itemDataToReturn = item;
                }
            }
            return itemDataToReturn;
        }

        /// <summary>
        /// If a crafting recipe with more copmlexity is also satisfied, use that one instead
        /// </summary>
        private bool IsMoreComplex(RecipeInfo newInfo, RecipeInfo oldInfo)
        {
            return newInfo.Ingredients.Count > oldInfo.Ingredients.Count;
        }
        public bool Satisfied(List<StorageSlot> storageSlots, RecipeInfo recipeInfo)
        {
            foreach(CraftingIngredient ingredient in recipeInfo.Ingredients)
            {
                if(!storageSlots.Any(x => x.Item != null && x.Item.Name == ingredient.Name && x.StoredCount >= ingredient.Count))
                {
                    return false;
                }
            }
            foreach(StorageSlot slot in storageSlots)
            {
                //Dissalow extra ingredients
                if(slot.Item != null)
                if(!recipeInfo.Ingredients.Any(x => x.Name == slot.Item.Name))
                    { return false; }
            }
            return true;
        }

        public void LoadContent(List<ItemData> itemData)
        {
            CraftActionDictionary = new Dictionary<CraftAction, List<ItemData>>();
            foreach(ItemData item in itemData)
            {
                if(item.RecipeInfo != null)
                {
                    if (!CraftActionDictionary.ContainsKey(item.RecipeInfo.CraftAction))
                    {
                        CraftActionDictionary[item.RecipeInfo.CraftAction] = new List<ItemData>();
                    }
                    CraftActionDictionary[item.RecipeInfo.CraftAction].Add(item);
                }
            }
        }
    }
}