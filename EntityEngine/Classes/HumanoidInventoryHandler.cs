using ItemEngine.Classes.StorageStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    internal class HumanoidInventoryHandler : InventoryHandler
    {
        public EquipmentStorageContainer EquipmentStorageContainer { get; set; }
        public HumanoidInventoryHandler(int capacity) : base(capacity)
        {
            EquipmentStorageContainer = new EquipmentStorageContainer(8);
        }
    }
}
