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
            foreach (StorageSlot slot in Slots)
                slot.ItemChanged += AnyItemChanged;
        }

        public void AnyItemChanged(Item item, int storedCount)
        {
            ItemData itemData = ItemFactory.CraftingGuide.GetCraftedItem(CraftAction, Slots);
            _currentlyCraftableItem = itemData;
            OutputSlot.RemoveAll();
            OutputSlot.Add(_currentlyCraftableItem.Name);
        }
    }
}
