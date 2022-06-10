using Globals.Classes;
using ItemEngine.Classes;
using ItemEngine.Classes.StorageStuff;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    internal class InventoryHandler : ISaveable
    {
        public StorageContainer StorageContainer { get; private set; }
        public ItemManager ItemManager { get; private set; }
        public virtual Item HeldItem => StorageContainer.Slots[0] != null ? StorageContainer.Slots[0].Item : null;
        public virtual int HeldItemCount => StorageContainer.Slots[0].StoredCount;
        public InventoryHandler( int capacity)
        {
            StorageContainer = new StorageContainer(capacity);
        }

        public void SwitchStage(ItemManager itemManager)
        {
            ItemManager = itemManager;

        }
        /// <summary>
        /// Need reference to new stage items
        /// </summary>
        /// <param name="newManager"></param>
        public void SwapItemManager(ItemManager newManager)
        {
            ItemManager = newManager;
        }
        /// <summary>
        /// Gives as much of world item as possible to entity. May not give all or any.Remainder can be found in world item passed in
        /// </summary>
        public void GiveItem(WorldItem worldItem)
        {
            int count = worldItem.Count;
            StorageContainer.AddItem(worldItem.Item, ref count);
            worldItem.Remove(worldItem.Count - count);
        }
        /// <summary>
        /// Gives as much of item as possible to entity. May not give all or any.Remainder can be found in count
        /// </summary>
        public void GiveItem(Item item, ref int count)
        {
            StorageContainer.AddItem(item, ref count);
        }

        public void GiveItem(string name, int count)
        {
            StorageContainer.AddItem(ItemFactory.GetItem(name), ref count);

        }

        public void DropItem(Vector2 entityPosition,Vector2 jettisonVector, string name, int count)
        {
            int originalCount = count;
            StorageContainer.RemoveItem(ItemFactory.GetItem(name).Id, ref count);

            ItemManager.AddWorldItem(entityPosition,ItemFactory.GetItem(name), originalCount - count, WorldItemState.Bouncing, jettisonVector);
        }

        public void DropItem(Vector2 entityPosition, Vector2 jettisonVector, Item item, int count)
        {
            int originalCount = count;
            StorageContainer.RemoveItem(item.Id, ref count);

            ItemManager.AddWorldItem(entityPosition,item, originalCount - count, WorldItemState.Bouncing, jettisonVector);
        }
        public bool CanAfford(int amt) => StorageContainer.CanAfford(amt);
        public int Withdraw(int amt) => StorageContainer.WithdrawCoins(amt);
        public void Deposit(int amt) => StorageContainer.DepositCoins(amt);
        public void Save(BinaryWriter writer)
        {
            StorageContainer.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {
            StorageContainer.LoadSave(reader);
        }

        public void CleanUp()
        {
            ItemManager.CleanUp();
        }
    }
}
