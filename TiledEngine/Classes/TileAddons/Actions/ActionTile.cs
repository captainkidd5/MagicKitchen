using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.Actions
{
    internal class ActionTile : TileBody
    {
        public ActionTile(Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape) : base(tile, tileManager, intermediateTmxShape)
        {
        }
    }
}
