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


        public void AddItem(Item item, ref int count)
        {
            //Try to find partially filled stackable item slot matching stackable item
            while (count > 0)
            {
                StorageSlot partiallyFilledSlot = Slots.FirstOrDefault(x => !x.Empty && x.Item.Id == item.Id && x.StoredCount < x.Item.MaxStackSize);
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
            //Try to find empty slot
            while (count > 0)
            {
                StorageSlot emptySlot = Slots.FirstOrDefault(x => x.Empty);
                //Inventory is completely full
                if (emptySlot == null)
                    return;
                if (!item.Stackable)
                {
                    emptySlot.AddUniqueItem(item);
                    count--;
                }
                else if (item.Stackable)
                {
                    while (count > 0 && (emptySlot.Add(item.Name)))
                    {
                        count--;
                    }
                }

            }

        }

        public void RemoveItem(Item item, ref int countToRemove)
        {
            while (countToRemove > 0)
            {
                StorageSlot slot = Slots.FirstOrDefault(x => x.Item.Id == item.Id);
                if (slot == null)
                    return;
                else
                {
                    slot.Remove(1);
                    countToRemove--;
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
        public Item Swap(ref Item itemToSwap, ref int count)
        {

            int newCount = StoredCount;
            Item item = Item;
            Item = itemToSwap;
            StoredCount = count;

            count = newCount;
            itemToSwap = item;

            return itemToSwap;
        }
        public bool AddUniqueItem(Item uniqueItem)
        {
            if (uniqueItem.Stackable)
                throw new Exception($"This method is not intended for stackable items");

            if (Item == null)
            {
                Item = uniqueItem;
                StoredCount++;
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

        public void Drop(int count)
        {
            if (Remove(count))
            {

            }
            OnItemChanged();
        }

        public void RightClickInteraction(ref Item heldItem, ref int count)
        {
            //Grabbing item from slot, no held item
            if (Item == null)
            {
                //no interaction
                return;
            }
           
            if(heldItem != null)
            {
                if (count > heldItem.MaxStackSize)
                    throw new Exception($"Should not be possible to be holding more than max stack size of item {heldItem}");
                if (Item.Id == heldItem.Id && Item.Stackable)
                {
                    //add 1 to held stack, if possible
                    if ((count < Item.MaxStackSize) && StoredCount > 0)
                    {
                        StoredCount--;
                        count++;
                    }
                    if (StoredCount == 0)
                        Item = null;
                    OnItemChanged();

                    return;

                }
            }
            else
            {
                //No held item, but items in slot, grab 1
                if ((count < Item.MaxStackSize) && StoredCount > 0)
                {
                    StoredCount--;
                    count++;
                    heldItem = Item;
                }
                if (StoredCount == 0)
                    Item = null;
                OnItemChanged();

                return;
            }
            
        }
        /// <summary>
        /// Performs different action based on if the cursor is holding an item, what type of
        /// item it is, whether or not the storage slot has an item, and what type it is
        /// </summary>
        public void LeftClickInteraction(ref Item heldItem, ref int count, bool shiftHeld)
        {
            //Grabbing item from slot, no held item
            if (heldItem == null)
            {
                heldItem = Item;
                if (shiftHeld)
                {
                    int countToRemove = StoredCount / 2;
                    count = countToRemove;
                    StoredCount = StoredCount - countToRemove;
                }
                else
                {
                    count = StoredCount;
                    Item = null;
                    StoredCount = 0;

                }

                OnItemChanged();

                return;
            }
            if (count > heldItem.MaxStackSize)
                throw new Exception($"Should not be possible to be holding more than max stack size of item {heldItem}");
            if (Empty)
            {
                Item = heldItem;
                StoredCount = count;
                heldItem = null;
                count = 0;
                OnItemChanged();

                return;
            }
            else if (Item.Id == heldItem.Id && Item.Stackable)
            {
                //deposit rest of held item stack into slot stack, until slot stack is full
                while ((StoredCount < Item.MaxStackSize) && count > 0)
                {
                    StoredCount++;
                    count--;
                }
                if (count == 0)
                    heldItem = null;
                OnItemChanged();

                return;

            }
            else if (!Item.Stackable || Item.Id != heldItem.Id)
            {
                //swap the two items. Same id, but unique (might have different durability or something) or just different id
                Swap(ref heldItem, ref count);
                OnItemChanged();

            }

        }
        protected virtual void OnItemChanged()
        {
            ItemChanged?.Invoke(Item, StoredCount);
        }
    }
}
