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

        protected override void AddSlots()
        {
            Slots = new List<StorageSlot>();
            HelmetEquipmentSlot = new EquipmentSlot(EquipmentType.Helmet);
            TorsoEquipmentSlot = new EquipmentSlot(EquipmentType.Torso);

            LegsEquipmentSlot = new EquipmentSlot(EquipmentType.Legs);
            BootsEquipmentSlot = new EquipmentSlot(EquipmentType.Boots);
            Slots.Add(HelmetEquipmentSlot);
            Slots.Add(TorsoEquipmentSlot);
            Slots.Add(LegsEquipmentSlot);
            Slots.Add(BootsEquipmentSlot);

            for (int i = 0; i < 4; i++)
            {
                Slots.Add(new EquipmentSlot(EquipmentType.Trinket));
            }

        }
    }
}
