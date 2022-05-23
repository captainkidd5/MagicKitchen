using DataModels.ItemStuff;
using DataModels.MapStuff;
using Globals.Classes;
using ItemEngine.Classes.CraftingStuff;
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
        public FuelStorageSlot FuelSlot { get; set; }

        public bool ContainsFuelItem => FuelSlot.StoredCount > 0;

        private ItemData _currentlyCraftableItem;
        public FuelMetre FuelTracker { get; set; }
        public CraftedItemMetre CraftedItemMetre { get; set; }
        public CraftingStorageContainer(CraftAction craftAction, int capacity,
            FurnitureData furnitureData = null) : base(capacity, furnitureData)
        {
            CraftAction = craftAction;
            OutputSlot = new StorageSlot();
            FuelSlot = new FuelStorageSlot();
            OutputSlot.ItemChanged += OutputSlotClicked;
            foreach (StorageSlot slot in Slots)
                slot.ItemChanged += AnyItemChanged;

            FuelSlot.ItemChanged += AnyItemChanged;

            OutputSlot.SetPlaceLock();
            FuelTracker = new FuelMetre();
            CraftedItemMetre = new CraftedItemMetre();
            CraftedItemMetre.ProgressDone += OnMetreCompleted;

        }
        private void OnMetreCompleted()
        {
            OutputSlot.RemovePlaceLock();
            OutputSlot.Add(_currentlyCraftableItem.Name);
            OutputSlot.SetPlaceLock();
            RemoveIngredientsFromInventoryToMakeItem(OutputSlot.Item);
      
            EvaluateOutputSlot();


        }
        public void TransferItemIntoFuel()
        {
            FuelTracker.AddFuel(FuelSlot.Item.FuelValue);

            FuelSlot.Remove(1);
        }
        private void GetCraftingRecipe()
        {
            ItemData itemData = ItemFactory.CraftingGuide.GetCraftedItem(CraftAction, Slots);
            _currentlyCraftableItem = itemData;
        }

        public void AnyItemChanged(Item item, int storedCount)
        {
            EvaluateOutputSlot();

        }

        private void EvaluateOutputSlot()
        {
            GetCraftingRecipe();
            if (_currentlyCraftableItem == null)
            {
                CraftedItemMetre.Reset();
                return;
            }
            if (_currentlyCraftableItem != null)
            {
                if(CraftedItemMetre.IdCurrentlyMaking != _currentlyCraftableItem.Id)
                    CraftedItemMetre.Reset();

                //may begin crafting again if output item is the same type, or it is empty
                if (OutputSlot.Item != null && OutputSlot.Item.Id == _currentlyCraftableItem.Id || OutputSlot.Empty)
                {
                    if (FuelTracker.CurrentFuel > 0)
                    {
                        CraftedItemMetre.Start(20, _currentlyCraftableItem.Id);
                    }
                }
            }
        }

        public void OutputSlotClicked(Item item, int storedCount)
        {
            GetCraftingRecipe();
            if (_currentlyCraftableItem == null)
            {
                CraftedItemMetre.Reset();
                return;
            }

            if (OutputSlot.Empty && !CraftedItemMetre.Active)
            {
                if (FuelTracker.CurrentFuel > 0)
                {
                    CraftedItemMetre.Start(20, _currentlyCraftableItem.Id);
                }
                return;
            }

            if (!OutputSlot.Empty)
            {
                if (OutputSlot.Item.Id == _currentlyCraftableItem.Id)
                    return;
                else
                {
                    if (FuelTracker.CurrentFuel > 0)
                        CraftedItemMetre.Start(20, _currentlyCraftableItem.Id);
                }
            }
        }

        /// <summary>
        /// Removes the materials from the inventory of entity crafting the item. Assumes that all materials exist,
        /// will not throw error if they do not
        /// </summary>
        /// <param name="item">The item to craft</param>
        private void RemoveIngredientsFromInventoryToMakeItem(Item item)
        {
            ItemData itemData = ItemFactory.GetItemData(item.Id);
            RecipeInfo recipeInfo = itemData.RecipeInfo;
            foreach (CraftingIngredient ingredient in recipeInfo.Ingredients)
            {
                int count = ingredient.Count;
                RemoveItem(ingredient.Name, ref count);
            }
        }
    }
}
