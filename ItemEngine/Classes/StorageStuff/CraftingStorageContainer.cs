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
    public class CraftingStorageContainer : FurnitureStorageContainer
    {
        public CraftAction CraftAction { get; private set; }
        public OutputSlot OutputSlot { get; set; }
        public FuelStorageSlot FuelSlot { get; set; }

        public bool ContainsFuelItem => FuelSlot.StoredCount > 0;

        private ItemData _currentlyCraftableItem;
        public FuelMetre FuelMetre { get; set; }
        public CraftedItemMetre CraftedItemMetre { get; set; }
        public CraftingStorageContainer(CraftAction craftAction, int capacity,
            FurnitureData furnitureData = null) : base(capacity, furnitureData)
        {
            CraftAction = craftAction;
            OutputSlot = new OutputSlot();
            FuelSlot = new FuelStorageSlot();
            OutputSlot.ItemChanged += OutputSlotClicked;
            foreach (StorageSlot slot in Slots)
                slot.ItemChanged += AnyItemChanged;

            FuelSlot.ItemChanged += AnyItemChanged;

            OutputSlot.SetPlaceLock();
            FuelMetre = new FuelMetre();
            CraftedItemMetre = new CraftedItemMetre(FuelMetre);
            CraftedItemMetre.ProgressDone += OnMetreCompleted;

            Slots[furnitureData.OutputSlotIndex.Value] = OutputSlot;
            Slots[furnitureData.FuelSlotIndex.Value] = FuelSlot;

        }
        private void OnMetreCompleted()
        {
            OutputSlot.RemovePlaceLock();
            OutputSlot.Add(_currentlyCraftableItem.Name);
            OutputSlot.SetPlaceLock();
            ItemFactory.CraftingGuide.RemoveIngredientsFromInventoryToMakeItem(OutputSlot.Item, this);
      
            EvaluateOutputSlot();


        }
        public void TransferItemIntoFuel()
        {
            FuelMetre.AddFuel(FuelSlot.Item.FuelValue);

            FuelSlot.Remove(1);
        }
        private void GetCraftingRecipe()
        {
            //int fuelSlotIndex = Slots.IndexOf(FuelSlot);
            //int outputSlotIndex = Slots.IndexOf(OutputSlot);
            //Slots.RemoveAt(fuelSlotIndex);
            //Slots.RemoveAt(outputSlotIndex);

            ItemData itemData = ItemFactory.CraftingGuide.GetCraftedItem(CraftAction, Slots);
            _currentlyCraftableItem = itemData;
            //Slots.Insert(Slots.Count - 1, FuelSlot);
            //Slots.Insert(outputSlotIndex, OutputSlot);

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
                if ((OutputSlot.Item != null && OutputSlot.Item.Id == _currentlyCraftableItem.Id) || OutputSlot.Empty)
                {
                   
                        if (FuelMetre.CurrentFuel > 0)
                        {
                            if (CraftedItemMetre.Done)
                            {
                                CraftedItemMetre.Start((int)_currentlyCraftableItem.RecipeInfo.CookTime, _currentlyCraftableItem.Id);
                            }
                        }
                        else
                        {
                            CraftedItemMetre.Reset();
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
                if (FuelMetre.CurrentFuel > 0)
                {
                    CraftedItemMetre.Start((int)_currentlyCraftableItem.RecipeInfo.CookTime, _currentlyCraftableItem.Id);
                }
                return;
            }

            if (!OutputSlot.Empty)
            {
                if (OutputSlot.Item.Id == _currentlyCraftableItem.Id)
                    return;
                else
                {
                    if (FuelMetre.CurrentFuel > 0)
                        CraftedItemMetre.Start((int)_currentlyCraftableItem.RecipeInfo.CookTime, _currentlyCraftableItem.Id);
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
