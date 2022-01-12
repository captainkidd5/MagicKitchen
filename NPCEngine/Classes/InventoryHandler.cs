using ItemEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    internal class InventoryHandler
    {
        public StorageContainer StorageContainer { get; private set; }

        public InventoryHandler(int capacity)
        {
            StorageContainer = new StorageContainer(capacity);
        }

        /// <summary>
        /// Gives as much of world item as possible to entity. May not give all or any.Remainder can be found in world item passed in
        /// </summary>
        public void GiveItem(WorldItem worldItem)
        {
            StorageContainer.AddItem(worldItem);
        }
        /// <summary>
        /// Gives as much of item as possible to entity. May not give all or any.Remainder can be found in count
        /// </summary>
        public void GiveItem(Item item,ref  int count)
        {
            StorageContainer.AddItem(item,ref count);
        }

        public void GiveItem(string name, int count)
        {
            StorageContainer.AddItem(ItemFactory.GetItem(name), ref count);

        }

       

    }
}
