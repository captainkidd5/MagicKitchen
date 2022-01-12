using IOEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ItemEngine.Classes
{
    /// <summary>
    /// Does not include visuals
    /// </summary>
    public class StorageContainer
    {
        public int Capacity { get; private set; }
        public List<StorageSlot> Slots { get; set; }
        public StorageContainer(int capacity)
        {
            Capacity = capacity;
            Slots = new List<StorageSlot>();
            for (int i = 0; i < Capacity; i++)
            {
                Slots.Add(new StorageSlot());
            }
        }
        public void AddItem(WorldItem worldItem)
        {
            foreach (StorageSlot slot in Slots)
            {
                if (worldItem.Stackable)
                {
                    if (slot.Item.Id == worldItem.Id)
                    {
                        while (worldItem.Count > 0 && (slot.Add(worldItem.Name)))
                        {
                            worldItem.Remove(1);
                        }
                    }
                }
                else
                {
                    if (slot.Empty)
                    {
                        slot.Add(worldItem.Name);
                        worldItem.Remove(1);
                        return;
                    }
                }
            }
        }

        public void AddItem(Item item, ref int count)
        {
            while (count > 0)
            {
                StorageSlot partiallyFilledSlot = Slots.FirstOrDefault(x => !x.Empty && x.StoredCount < x.Item.MaxStackSize);
                if (partiallyFilledSlot != null)
                {
                    while (count > 0 && (partiallyFilledSlot.Add(item.Name)))
                    {
                        count--;
                    }
                }
                else
                    break;
            }
            if (count > 0)
            {
                StorageSlot emptySlot = Slots.FirstOrDefault(x => x.Empty);
                //Inventory is completely full
                if (emptySlot == null)
                    return;
                if (!item.Stackable)
                {
                    emptySlot.AddUniqueItem(item);
                    return;
                }
                while (count > 0 && (emptySlot.Add(item.Name)))
                {
                    count--;
                }
            }

        }

    }

    public delegate void ItemChanged(Item item, int storedCount);
    public class StorageSlot
    {
        public event ItemChanged ItemChanged;
        public Item Item { get; private set; }

        //just a shortcut to get the Item's stacksize
        public int StoredCount { get; private set; }
        public bool Empty => Item == null;



        public StorageSlot()
        {
        }

        /// <summary>
        /// The ol' switcheroo
        /// </summary>
        public Item Swap(Item itemToSwap)
        {
            Item item = Item;
            Item = itemToSwap;
            itemToSwap = item;
            OnItemChanged();

            return itemToSwap;
        }
        public bool AddUniqueItem(Item uniqueItem)
        {
            if (!uniqueItem.Stackable)
                throw new Exception($"This method is not intended for stackable items");

            if (Item == null)
            {
                Item = uniqueItem;
                OnItemChanged();
                return true;
            }
            return false;
        }

        public bool Add(string itemName)
        {

            if (Item == null)
            {
                Item = ItemFactory.GetItem(itemName);
                OnItemChanged();
            }
            if (itemName != Item.Name)
                throw new Exception($"{itemName} does not match {Item.Name}");
            if (StoredCount <= Item.MaxStackSize)
            {
                StoredCount++;
                OnItemChanged();

                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if able to remove {count} amount of items. Removes all of them if true. If unable to remove entire count, removes none of them
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool Remove(int count)
        {
            if (StoredCount - count > 0)
            {
                StoredCount -= count;
                if (StoredCount < 1)
                {
                    OnItemChanged();
                    Item = null;
                }
                return true;
            }
            return false;

        }
        /// <summary>
        /// Performs different action based on if the cursor is holding an item, what type of
        /// item it is, whether or not the storage slot has an item, and what type it is
        /// </summary>
        public void ClickInteraction(Item itemToDeposit, ref int count, Action<Item> pickUpItem, Action dropItem)
        {
            if (count > itemToDeposit.MaxStackSize)
                throw new Exception($"Should not be possible to be holding more than max stack size of item {itemToDeposit}");
            if (Empty)
            {
                Item = itemToDeposit;
                StoredCount = count;
                return;
            }
            else if (Item.Id == itemToDeposit.Id && Item.Stackable)
            {
                //deposit rest of held item stack into slot stack, until slot stack is full
                while ((StoredCount < Item.MaxStackSize) && count > 0)
                {
                    StoredCount++;
                    count--;
                }
                return;

            }
            else if (!Item.Stackable)
            {
                //swap the two items. Same id, but unique (might have different durability or something)
                Swap(itemToDeposit);

            }

        }
        protected virtual void OnItemChanged()
        {
            ItemChanged?.Invoke(Item, StoredCount);
        }
    }
}
