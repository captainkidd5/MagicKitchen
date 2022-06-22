using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes
{
    public class WorldItemGeneratedEventArgs : EventArgs
    {
        public Item Item { get; set; }
        public int Count { get; set; }
        public Vector2 Position { get; set; }

        public WorldItemState WorldItemState { get; set; }

        public Vector2? JettisonDirection { get; set; }
    }
}
