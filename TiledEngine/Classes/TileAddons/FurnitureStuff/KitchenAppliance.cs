using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class KitchenAppliance : Furniture
    {
        public KitchenAppliance(Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape, string actionType)
            : base(tile, tileManager, intermediateTmxShape, actionType)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}
