using ItemEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class PlayerInventoryHandler : InventoryHandler
    {
        public override Item HeldItem => UI.PlayerCurrentSelectedItem;
        public override int HeldItemCount => throw new NotImplementedException();
        public PlayerInventoryHandler(int capacity) : base(capacity)
        {
        }


    }
}
