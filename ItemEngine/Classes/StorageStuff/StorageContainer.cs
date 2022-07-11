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
        public List<StorageSlot> Slots { get; private set; }

        private Wallet _wallet;

        public StorageContainer(int capacity)
        {
            Capacity = capacity;
            Slots = new List<StorageSlot>();
            _wallet = new Wallet();
            for (int i = 0; i < Capacity; i++)
            {
                Slots.Add(new StorageSlot());
            }

        }

        public bool CanAfford(int amt) => _wallet.CanAfford(amt);
        public int WithdrawCoins(int amt) => _wallet.Withdraw(amt);
        public void DepositCoins(int amt) => _wallet.Deposit(amt);
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
                    while (count > 0 && (emptySlot.Add(item.Name)))
                    {
                        count--;
                        ItemAdded?.Invoke(item, 1);

                    }
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

        public void Save(BinaryWriter writer)
        {
            _wallet.Save(writer);
           foreach(StorageSlot slot in Slots)
            {
                slot.Save(writer);
            }
        }

        public void LoadSave(BinaryReader reader)
        {
            _wallet.LoadSave(reader);
            foreach (StorageSlot slot in Slots)
            {
                slot.LoadSave(reader);
            }
        }

        public void CleanUp()
        {
            _wallet.CleanUp();
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

        public void SetToDefault( )
        {
            _wallet.SetToDefault();
            foreach (StorageSlot slot in Slots)
            {
                slot.SetToDefault();
            }
        }
    }

  
}
