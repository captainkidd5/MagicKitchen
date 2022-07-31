﻿using ItemEngine.Classes.StorageStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes
{
    internal class HumanoidInventoryHandler : InventoryHandler
    {
        public EquipmentStorageContainer EquipmentStorageContainer { get; set; }
        public int ArmorValue => EquipmentStorageContainer.GetArmorValue();
        public int MaxLumenValue => EquipmentStorageContainer.GetMaxLumens();

        public HumanoidInventoryHandler(int capacity) : base(capacity)
        {
            EquipmentStorageContainer = new EquipmentStorageContainer(8);
        }

        public void ReduceDurabilityOnEquippedArmor() => EquipmentStorageContainer.ReduceDurabilityOnEquippedArmor();
        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);

        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);

        }
    }
}
