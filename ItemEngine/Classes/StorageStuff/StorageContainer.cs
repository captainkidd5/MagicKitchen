using DataModels.ItemStuff;
using DataModels.MapStuff;
using Globals.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ItemEngine.Classes.StorageStuff

{

    public delegate void ItemAdded(Item item, int amtAdded);

    /// <summary>
    /// Does not include visuals
    /// </summary>
    public class StorageContainer : ISaveable
    {
        public event ItemAdded? ItemAdded;

        public int Capacity { get; private set; }
        public List<StorageSlot> Slots { get; protected set; }


        public StorageContainer(int capacity)
        {
            Capacity = capacity;
            AddSlots();
        }

        protected virtual void AddSlots()
        {
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
                        ItemAdded.Invoke(item, 1);
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
                    ItemAdded?.Invoke(item, 1);

                }
                else if (item.Stackable)
                {
                    int c = count;
                    while (count > 0 && (emptySlot.Add(item.Name)))
                    {
                        count--;
                        //ItemAdded?.Invoke(item, 1);

                    }
                    ItemAdded?.Invoke(item, c - count);

                }

            }

        }

        public void RemoveItem(int itemId, ref int countToRemove)
        {
            while (countToRemove > 0)
            {
                StorageSlot slot = Slots.FirstOrDefault(x => x.Item.Id == itemId);
                if (slot == null)
                    return;
                else
                {
                    slot.Remove(1);
                    countToRemove--;
                }
            }
        }
        public void RemoveItem(string name, ref int countToRemove)
        {
            while (countToRemove > 0)
            {
                StorageSlot slot = Slots.FirstOrDefault(x => x.Item != null && x.Item.Name == name);
                if (slot == null)
                    return;
                else
                {
                    slot.Remove(1);
                    countToRemove--;
                }
            }
        }

        public virtual void Save(BinaryWriter writer)
        {
           foreach(StorageSlot slot in Slots)
            {
                slot.Save(writer);
            }
        }

        public virtual void LoadSave(BinaryReader reader)
        {
            foreach (StorageSlot slot in Slots)
            {
                slot.LoadSave(reader);
            }
        }

        public void SetToDefault()
        {
            foreach (StorageSlot slot in Slots)
            {
                slot.SetToDefault();
            }
            Slots.Clear();
        }

        public int GetStoredCount(int id)
        {
            int totalCount = 0;
            foreach(StorageSlot slot in Slots)
            {
                if (!slot.Empty)
                {
                    if (slot.Item.Id == id)
                        totalCount += slot.StoredCount;

                }
            }
            return totalCount;
        }

        public Dictionary<string, int> GetItemStoredAsDictionary()
        {
            Dictionary<string, int> dictionaryToReturn = new Dictionary<string, int>();

            foreach (StorageSlot slot in Slots)
            {
                if (!slot.Empty)
                {
                    if (dictionaryToReturn.ContainsKey(slot.Item.Name))
                        dictionaryToReturn[slot.Item.Name] += slot.StoredCount;
                    else
                        dictionaryToReturn.Add(slot.Item.Name, slot.StoredCount);

                }
            }
            return dictionaryToReturn;
        }


    }

  
}
