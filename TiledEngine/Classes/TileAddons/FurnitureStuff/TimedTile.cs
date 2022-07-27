using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class TimedTile : TileBody
    {
        public float TimeCreated { get; set; }
        public TimedTile(TileObject tile, IntermediateTmxShape intermediateTmxShape) : base(tile, intermediateTmxShape)
        {
            TimeCreated = Clock.TotalTime;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            string property = Tile.GetProperty("swapTo", true);
            float ttl = int.Parse(property.Split(",")[1].Split(":")[1]);
            if(TimeCreated + ttl < Clock.TotalTime)
            {
                ushort newGid = (ushort)int.Parse(property.Split(",")[0]);
                if (Tile.TileManager.TileSetPackage.IsForeground(Tile.TileData.GID))
                    newGid = (ushort)Tile.TileManager.TileSetPackage.OffSetBackgroundGID(newGid);
                Tile.TileManager.SwitchGID(newGid, Tile.TileData);
            }
        }
        
    }
}
