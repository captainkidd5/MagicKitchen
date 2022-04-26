using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class DiningTable : Furniture
    {

        public int TotalSeatingCapacity { get; private set; } = 1;
        public int OccupiedSeatCount { get; internal set; }
        public bool SeatingAvailable => OccupiedSeatCount < TotalSeatingCapacity;


        public DiningTable(Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape) :
            base(tile, tileManager, intermediateTmxShape)
        {
            SubKey = "diningTable";
        }
        public override void Update(GameTime gameTime)
        {
            if (PlayerInClickRange && MouseHovering && Controls.IsClicked)
            {
                Console.WriteLine("Test");
            }
        }
        public bool SitDown()
        {
            if(SeatingAvailable)
            {
                OccupiedSeatCount++;
                return true;
            }
            return false;
        }

    }
}
