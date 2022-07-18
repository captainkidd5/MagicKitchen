using DataModels.MapStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace ItemEngine.Classes.StorageStuff
{
    public class EquipmentStorageContainer : StorageContainer
    {

        public EquipmentSlot HelmetEquipmentSlot { get; private set; }
        public EquipmentSlot TorsoEquipmentSlot { get; private set; }

        public EquipmentSlot LegsEquipmentSlot { get; private set; }

        public EquipmentSlot BootsEquipmentSlot { get; private set; }




        public EquipmentStorageContainer(int capacity) : base(capacity)
        {
          
        }
        public void ReduceDurabilityOnEquippedArmor()
        {
            foreach (StorageSlot slot in Slots)
            {
                if (!slot.Empty)
                    slot.Item.RemoveDurability(slot.Item.ArmorValue);
            }
        }
        public int GetArmorValue()
        {
            int totalArmor = 0;
            foreach(StorageSlot slot in Slots)
            {
                if (!slot.Empty)
                    totalArmor += slot.Item.ArmorValue;
            }
            return totalArmor;
        }
        protected override void AddSlots()
        {
            Slots = new List<StorageSlot>();
            HelmetEquipmentSlot = new EquipmentSlot(EquipmentType.Helmet);
            TorsoEquipmentSlot = new EquipmentSlot(EquipmentType.Torso);

            LegsEquipmentSlot = new EquipmentSlot(EquipmentType.Legs);
            BootsEquipmentSlot = new EquipmentSlot(EquipmentType.Boots);
            Slots.Add(HelmetEquipmentSlot);
            Slots.Add(new EquipmentSlot(EquipmentType.Trinket));

            Slots.Add(TorsoEquipmentSlot);
            Slots.Add(new EquipmentSlot(EquipmentType.Trinket));

            Slots.Add(LegsEquipmentSlot);
            Slots.Add(new EquipmentSlot(EquipmentType.Trinket));

            Slots.Add(BootsEquipmentSlot);
            Slots.Add(new EquipmentSlot(EquipmentType.Trinket));


      

        }
    }
}
