using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons
{
    public abstract class LocateableTileAddon : TileBody
    {
     
        protected string Key;
        protected string SubKey;


        public LocateableTileAddon(Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape) : base(tile, tileManager, intermediateTmxShape)
        {
    
        }

        public override void Load()
        {
            base.Load();
            TileManager.TileLocator.AddItem(Key, SubKey, Tile);
        }
        public override void CleanUp()
        {
            base.CleanUp();
            TileManager.TileLocator.RemoveItem(Key, SubKey, Tile);
        }
       
    }
}
