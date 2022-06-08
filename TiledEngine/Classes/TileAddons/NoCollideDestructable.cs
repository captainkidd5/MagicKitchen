using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons
{
    internal class NoCollideDestructable : DestructableTile
    {
        public NoCollideDestructable(Tile tile, IntermediateTmxShape intermediateTmxShape, string action) 
            : base(tile, intermediateTmxShape, action)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
