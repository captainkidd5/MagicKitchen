using DataModels.ItemStuff;
using DataModels.MapStuff;
using Globals.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes.StorageStuff
{
    public class CraftingStorageContainer : StorageContainer
    {
        public CraftAction CraftAction { get; private set; }
        public StorageSlot OutputSlot { get; set; }

        private SimpleTimer _simpleTimer;

        private ItemData _currentlyCraftableItem;
        public CraftingStorageContainer(CraftAction craftAction, int capacity,
            FurnitureData furnitureData = null) : base(capacity, furnitureData)
        {
            CraftAction = craftAction;
            OutputSlot = new StorageSlot();
            OutputSlot.ItemGrabbedByEntity += OutputSlotClicked;
            foreach (StorageSlot slot in Slots)
                slot.ItemChanged += AnyItemChanged;

            OutputSlot.SetPlaceLock();
        }
        private void GetCraftingRecipe()
        {
            ItemData itemData = ItemFactory.CraftingGuide.GetCraftedItem(CraftAction, Slots);
            _currentlyCraftableItem = itemData;
        }
        public void AnyItemChanged(Item item, int storedCount)
        {
            GetCraftingRecipe();
            if (OutputSlot.Item != null && _currentlyCraftableItem != null && OutputSlot.Item.Id != _currentlyCraftableItem.Id)
            {
                OutputSlot.RemoveAll();
                if (_currentlyCraftableItem != null)
                {
                    OutputSlot.RemovePlaceLock();
                    OutputSlot.Add(_currentlyCraftableItem.Name);
                    OutputSlot.SetPlaceLock();


                }
            }
            else if (_currentlyCraftableItem != null)
            {
                if (OutputSlot.Item == null)
                {
                    OutputSlot.RemovePlaceLock();
                    OutputSlot.Add(_currentlyCraftableItem.Name);
                    OutputSlot.SetPlaceLock();
                }

            }

        }

        public void OutputSlotClicked(Item item, int storedCount)
        {
            if (item != null)
            {

                ItemData itemData = ItemFactory.GetItemData(item.Id);
                RecipeInfo recipeInfo = itemData.RecipeInfo;
                foreach (CraftingIngredient ingredient in recipeInfo.Ingredients)
                {
                    int count = ingredient.Count;
                    RemoveItem(ingredient.Name, ref count);
                }
               

            }
            if(item == null)
            {

            GetCraftingRecipe();
            if (_currentlyCraftableItem != null)
            {
                if (OutputSlot.Item == null)
                {
                    OutputSlot.RemovePlaceLock();
                    OutputSlot.Add(_currentlyCraftableItem.Name);
                    OutputSlot.SetPlaceLock();
                }

            }
            }


        }
    }
}
