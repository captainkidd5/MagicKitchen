using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.ItemStuff
{
    internal class WorldItemSeed
    {

        public ushort ItemId { get; set; }
        public Vector2 Position { get; set; }
        public float TimeCreated { get; set; }
        public ushort Count { get; set; }
        public ushort? CurrentDurability { get; set; }
    }
}
