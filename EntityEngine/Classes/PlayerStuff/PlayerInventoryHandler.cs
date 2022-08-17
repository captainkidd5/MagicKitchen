using EntityEngine.Classes.HumanoidStuff;
using Globals.Classes.Console;
using ItemEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class PlayerInventoryHandler : HumanoidInventoryHandler
    {
        public override Item HeldItem => UI.PlayerCurrentSelectedItem;
        //public int MaxLumenValue => EquipmentStorageContainer.GetMaxLumens();

        public override int HeldItemCount => throw new NotImplementedException();
        public PlayerInventoryHandler(int capacity) : base(capacity)
        {
        }
        public void RemoveDurabilityCommand(string[] args)
        {
            ReduceDurabilityOnEquippedArmor(int.Parse(args[0]));
        }

    }
}
