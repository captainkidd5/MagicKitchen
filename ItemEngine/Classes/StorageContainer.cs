using IOEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
                        while (worldItem.Count > 0 && (slot.Add(worldItem.Id)))
                        {
                            worldItem.Remove(1);
                        }
                    }
                }
                else
                {
                    if (slot.Empty)
                    {
                        slot.Add(worldItem.Id);
                        worldItem.Remove(1);
                        return;
                    }
                }
            }
        }

        public void AddItem(Item item, ref int count)
        {
            foreach (StorageSlot slot in Slots)
            {
                if (item.Stackable)
                {
                    if (slot.Item.Id == item.Id)
                    {
                        while (count > 0 && (slot.Add(item.Id)))
                        {
                            count--;
                        }
                    }
                }
                else
                {
                    if (slot.Empty)
                    {
                        slot.Add(item.Id);
                        count--;
                        return;
                    }
                }
            }
        }

    }

    public delegate void ItemChanged(int id);
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
            OnItemChanged(Item.Id);

            return itemToSwap;
        }

        public bool Add(int itemId)
        {
            if (itemId != Item.Id)
                throw new Exception($"{itemId} does not match {Item.Id}");
            if (StoredCount <= Item.MaxStackSize)
            {
                StoredCount++;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if able to remove {count} amount of items. Removes all of them if true. If unable to remove entire count, removes none of them.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool Remove(int count)
        {
            if(StoredCount - count > 0)
            {
                StoredCount -= count;
                return true;
            }
            return false;
                
        }
        /// <summary>
        /// Performs different action based on if the cursor is holding an item, what type of
        /// item it is, whether or not the storage slot has an item, and what type it is.
        /// </summary>
        /// <param name="itemToDeposit">Item currently held by cursor, may be null</param>
        /// <param name="pickUpItem">The cursor pick up action</param>
        /// <param name="dropItem">The cursor drop action</param>
        public void ClickInteraction(Item itemToDeposit, Action<Item> pickUpItem, Action dropItem)
        {

        }
        protected virtual void OnItemChanged(int id)
        {
            ItemChanged?.Invoke(id);
        }
    }
}
