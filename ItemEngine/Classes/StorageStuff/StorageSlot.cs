using DataModels.ItemStuff;
using Globals.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes.StorageStuff
{
    public delegate void ItemChanged(Item item, int storedCount);
    public delegate void ItemGrabbedByEntity(Item item, int storedCount);

    public class StorageSlot : ISaveable
    {
        public event ItemChanged ItemChanged;
        public event ItemGrabbedByEntity ItemGrabbedByEntity;
        public Item Item { get; private set; }

        //just a shortcut to get the Item's stacksize
        public int StoredCount { get; private set; }
        public bool Empty => Item == null;

        //If set to true, items placed in this slot may appear on top of tiles (Furniture, for example)
        public bool HoldsVisibleFurnitureItem { get; set; } = false;

        //if true, player may not place new items into this slot
        public bool PlaceLocked { get; protected set; }

        public List<int> BlacklistedItems { get; set; }
        public StorageSlot()
        {
        }
        public void SetPlaceLock()
        {
            if (PlaceLocked)
                throw new Exception($"Slot already place locked");
            PlaceLocked = true;
        }
        /// <summary>
        /// Blacklisted items may not be placed in this slot
        /// </summary>
        private bool IsItemBlackListed(ushort itemId)
        {
            return BlacklistedItems != null && BlacklistedItems.Contains(itemId);
        }

        public void RemovePlaceLock()
        {
            if (!PlaceLocked)
                throw new Exception($"Slot was not locked");

            PlaceLocked = false;
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
            if (IsItemBlackListed(uniqueItem.Id))
                return false;

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

        public virtual bool Add(string itemName)
        {
            if (PlaceLocked ||  IsItemBlackListed(ItemFactory.GetItemData(itemName).Id))
                return false;

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

            if (StoredCount - count >= 0)
            {
                StoredCount -= count;
                if (StoredCount < 1)
                {
                    Item = null;

                }
                OnItemChanged();

                return true;
            }
            OnItemChanged();

            return false;

        }

        public void RemoveAll()
        {
            StoredCount = 0;
            Item = null;
            OnItemChanged();
        }

        protected virtual bool MayPlaceItem(ushort itemIdToTryToPlace)
        {
            if (PlaceLocked || IsItemBlackListed(itemIdToTryToPlace))
                return false;
            return true;
        }
        public void RightClickInteraction(ref Item heldItem, ref int heldCount)
        {
         

            if (heldItem != null)
            {
                if (!MayPlaceItem(heldItem.Id))
                    return;
                if (StoredCount > heldItem.MaxStackSize)
                    throw new Exception($"Should not be possible to be add more than max stack size of item {heldItem}");
                if (Item == null)
                {
                    Item = heldItem;
                    StoredCount++;
                    heldCount--;
                    if (heldCount == 0)
                        heldItem = null;
                    OnItemChanged();
                    return;
                }
                if (Item.Id == heldItem.Id)
                {
                    //add 1 to held stack, if possible
                    if ((StoredCount < Item.MaxStackSize) && heldCount > 0)
                    {
                        StoredCount++;
                        heldCount--;
                    }
                    if (heldCount == 0)
                        heldItem = null;
                    OnItemChanged();

                    return;

                }
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

                OnItemGrabbed();
                if (shiftHeld && StoredCount > 1)
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
                OnItemGrabbed();

                OnItemChanged();

                return;
            }
            if (count > heldItem.MaxStackSize)
                throw new Exception($"Should not be possible to be holding more than max stack size of item {heldItem}");
            if (Empty)
            {
                if (!MayPlaceItem(heldItem.Id))
                    return;
                Item = heldItem;
                StoredCount = count;
                heldItem = null;
                count = 0;
                OnItemChanged();

                return;
            }
            else if (Item.Id == heldItem.Id && Item.Stackable)
            {
                if (!MayPlaceItem(heldItem.Id))
                    return;
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
                if (!MayPlaceItem(heldItem.Id))
                    return;
                //swap the two items. Same id, but unique (might have different durability or something) or just different id
                Swap(ref heldItem, ref count);
                OnItemChanged();

            }

        }

        public ItemDataDTO ExportItemDataDTO()
        {
            if (Item == null)
                return null;

            return new ItemDataDTO() { ItemData = ItemFactory.GetItemData(Item.Id), Count = StoredCount };
        }
        protected virtual void OnItemChanged()
        {
            ItemChanged?.Invoke(Item, StoredCount);
        }

        protected virtual void OnItemGrabbed()
        {
            ItemGrabbedByEntity?.Invoke(Item, StoredCount);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(StoredCount);
            if (StoredCount > 0)
            {
                Item.Save(writer);
            }
            writer.Write(PlaceLocked);
        }

        public void LoadSave(BinaryReader reader)
        {
            StoredCount = reader.ReadInt32();
            if (StoredCount > 0)
            {
                Item item = new Item();
                item.LoadSave(reader);

                Item = item;
            }
            PlaceLocked = reader.ReadBoolean();
            //Call on item changed here to update UI with loaded changes, otherwise ui doesn't show anything until 
            //slot is interacted with again
            OnItemChanged();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void SetToDefault( )
        {
            StoredCount = 0;
            Item = null;
            PlaceLocked = false;
        }
    }
}
