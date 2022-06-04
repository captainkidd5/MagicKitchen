using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons.Actions;

namespace TiledEngine.Classes.TileAddons
{
    public abstract class LocateableTileAddon : DestructableTile
    {
     
        protected string Key;
        protected string SubKey;


        public LocateableTileAddon(Tile tile,
            IntermediateTmxShape intermediateTmxShape, string actionType) : base(tile, intermediateTmxShape, actionType)
        {
            RequireLoopBeforeDestruction = false;
        }

        public override void Load()
        {
           base.Load();
            Tile.TileManager.TileLocator.AddItem(Key, SubKey, Tile);
        }
        public override void CleanUp()
        {
            base.CleanUp();
            Tile.TileManager.TileLocator.RemoveItem(Key, SubKey, Tile);
        }
       
    }
}
