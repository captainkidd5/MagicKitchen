﻿using Globals.Classes;
using ItemEngine.Classes;
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
        public ItemManager ItemManager { get; }

        public InventoryHandler(ItemManager itemManager, int capacity)
        {
            StorageContainer = new StorageContainer(capacity);
            ItemManager = itemManager;
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
            StorageContainer.RemoveItem(ItemFactory.GetItem(name), ref count);

            ItemManager.AddWorldItem(entityPosition,ItemFactory.GetItem(name), originalCount - count, jettisonVector);
        }

        public void DropItem(Vector2 entityPosition, Vector2 jettisonVector, Item item, int count)
        {
            int originalCount = count;
            StorageContainer.RemoveItem(item, ref count);

            ItemManager.AddWorldItem(entityPosition,item, originalCount - count, jettisonVector);
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
}
