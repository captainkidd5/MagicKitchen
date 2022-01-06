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
    public class StorageContainer : ISaveable
    {

        public int Capacity { get; private set; }
        public List<StorageSlot> Slots { get; private set; }
        public StorageContainer( int capacity)
        {
            Capacity = capacity;
            Slots = new List<StorageSlot>();

            for (int i = 0; i < Capacity; i++)
                Slots.Add(new StorageSlot());

        }

        public void ChangeCapacity(int newCapacity)
        {
            //TODO
        }
        /// <summary>
        /// Adds item count until capacity
        /// </summary>
        /// <returns>Returns item we wanted to add, it's total stack size will be reduced by the amount able to be added</returns>
        public bool FillUniqueItem(Item item)
        {

            foreach (StorageSlot slot in Slots)
                if (slot.AddUniqueItem(item))
                    return true;
            
            return false;
        }

        /// <summary>
        /// Adds item count until capacity. Use when you have no unique item in mind, you just want to generate x of item with id y.
        /// </summary>
        /// <returns>Returns amount unable to add</returns>
        public int FillStackableItem(int itemId, int countToAdd)
        {
            if (countToAdd < 1)
                throw new Exception($"Cannot add less than 1 item.");

            foreach (StorageSlot slot in Slots)
            {
                countToAdd = slot.TryFill(itemId, ref countToAdd);
                if (countToAdd == 0)
                    break;
            }
            return countToAdd;
        }
        /// <summary>
        /// Removes count of item
        /// </summary>
        /// <returns>Returns count of item unable to remove</returns>
        public Item RemoveStackableItem(int itemId, int count)
        {
            Item itemToReturn = null;
            foreach (StorageSlot slot in Slots)
            {
                itemToReturn = slot.Remove(itemId, ref count);
                if (count == 0)
                    return itemToReturn;
            }
                

            return itemToReturn;
        }



        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }

    public delegate void ItemChanged(int id);
    public class StorageSlot
    {
        public event ItemChanged ItemChanged;
        public Item Item { get; private set; }

        //just a shortcut to get the Item's stacksize
        public int StoredCount { get { if (Item == null) { return 0; } else return Item.StackSize; } }
        public bool Empty => Item == null;



        public StorageSlot()
        {
        }
        #region ADD

        /// <summary>
        /// When trying to add a stackable to a specific slot
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <returns></returns>
        public Item AddStackable(Item itemToAdd)
        {
            if (Item != null && Item.Id == itemToAdd.Id)
            {
                int amtAbleToAdd = Item.MaxStackSize - Item.StackSize;

                if (amtAbleToAdd > 0)
                {
                    if (itemToAdd.StackSize <= amtAbleToAdd)
                    {
                        //we can add everything
                        Item.StackSize += itemToAdd.StackSize;
                        itemToAdd.StackSize = 0;
                    }
                    else
                    {
                        //can't add everything, but can add some.
                        itemToAdd.StackSize += amtAbleToAdd;
                        Item.StackSize -= amtAbleToAdd;
                    }
                }

            }
            else
            {
                //slot was empty, just add the entire stack
                Item = itemToAdd;
            }
            OnItemChanged(Item.Id);

            return Item;
        }

        /// <summary>
        /// Adds item count until capacity, for use when you're not trying to add to a specific slot
        /// </summary>
        /// <returns>Returns amount unable to add</returns>
        public int TryFill(int itemId, ref int countToAdd)
        {
            int oldStoredCount = StoredCount;

            //Nothing is currently stored
            if (Item == null)
            {
                Item = ItemFactory.GenerateItem(itemId, 0, null);
                //Fire event for inventory ui to change its texture
            }
            else if(Item.Id != itemId)
            {
                return countToAdd;
            }

            int newAmtUnchecked = StoredCount + countToAdd;
            if (newAmtUnchecked > Item.MaxStackSize)
            {
                Item.StackSize = Item.MaxStackSize;
                countToAdd = newAmtUnchecked - Item.MaxStackSize;

            }
            else
            {
                Item.StackSize = newAmtUnchecked;
                countToAdd = 0;
            }

            if (oldStoredCount != StoredCount)
                OnItemChanged(Item.Id);

            return countToAdd;
        }

        /// <summary>
        /// Adds item count until capacity 
        /// </summary>
        /// <returns>Returns amount unable to add</returns>
        public bool AddUniqueItem(Item itemToAdd)
        {
            //Nothing is currently stored
            if (Item == null)
            {
                Item = itemToAdd;
                //Fire event for inventory ui to change its texture
                OnItemChanged(Item.Id);
                Item.StackSize = 1;
                return true;
            }
            return false;
        }
        #endregion

        #region REMOVE

        /// <summary>
        /// Removes and ALWAYS generates a new item instance for the world if this count > 0.
        /// </summary>
        public Item RemoveSingle()
        {
            Item itemToReturn = null;
            if (Item != null)
            {
                if (Item.Unique)
                {
                     itemToReturn = Item;
                    Remove(Item);
                }
                else {
                     itemToReturn = ItemFactory.GenerateItem(Item.Id, 1, null);
                    int countToRemove = 1;
                    Remove(Item.Id,ref countToRemove);
                    if (StoredCount == 0)
                    {
                        Item = null;
                        OnItemChanged((int)ItemType.None);


                    }
                }
            }
            return itemToReturn;         
        }

        /// <summary>
        /// Returns item if count is zero, else returns null
        /// </summary>
        public Item Remove(int id, ref int count)
        {
            if (StoredCount > 0)
            {
                int oldCount = StoredCount;
                if (Item != null)
                {
                    if (Item.Id == id)
                    {
                        int removeCountUnchecked = StoredCount - count;
                        //Tried to remove more than, or all of what slot has
                        if (removeCountUnchecked <= 0)
                        {
                            removeCountUnchecked = StoredCount;
                            count -= StoredCount;
                            Item.StackSize = 0;
                        }
                        else
                        {
                            Item.StackSize = removeCountUnchecked;
                            count = 0;
                        }
                    }
                }

                if (oldCount != StoredCount)
                    OnItemChanged(Item.Id);
            }

            if (count == 0)
                return Item;
            else
                return null;

        }


        public bool Remove(Item itemToRemove)
        {
            if (itemToRemove == null)
                throw new Exception("tried to remove a null item");
            if(Item == itemToRemove)
            {
                Item = null;
                OnItemChanged((int)ItemType.None);

                return true;
            }
            return false;
        }
        #endregion

        #region SWAP
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
        #endregion

        /// <summary>
        /// Performs different action based on if the cursor is holding an item, what type of
        /// item it is, whether or not the storage slot has an item, and what type it is.
        /// </summary>
        /// <param name="itemToDeposit">Item currently held by cursor, may be null</param>
        /// <param name="pickUpItem">The cursor pick up action</param>
        /// <param name="dropItem">The cursor drop action</param>
        public void ClickInteraction(Item itemToDeposit, Action<Item> pickUpItem, Action dropItem)
        {
            if (itemToDeposit == null)
            {
                if (!Empty)
                {
                    pickUpItem(Item);
                    Remove(Item);
                }

            }
            //else we are already holding something with our cursor
            else
            {
                if (itemToDeposit.Unique)
                {
                    //store unique item in empty slot, no problem
                    if (Empty)
                    {
                        AddUniqueItem(itemToDeposit);
                        dropItem();
                    }
                    //trying to swap unique item with stored, non-unique item
                    else
                        pickUpItem(Swap(itemToDeposit));
                    
                }
                //held item is not unique (it's stackable) so we have to check if the item we're clicking on is the same type.
                else if (!itemToDeposit.Unique)
                {
                    if (Empty)
                    {
                        AddStackable(itemToDeposit);
                        dropItem();

                    }
                    else if (Item.Unique || Item.Id != itemToDeposit.Id)
                    {
                        pickUpItem(Swap(itemToDeposit));
                    }
                    //trying to merge stacks
                    else if (Item.Id == itemToDeposit.Id)
                    {
                        AddStackable(itemToDeposit);
                        if (itemToDeposit.StackSize < 1)
                            dropItem();
                       
                    }
                }

            }
        }
        protected virtual void OnItemChanged(int id)
        {
            ItemChanged?.Invoke(id);
        }
    }
}
