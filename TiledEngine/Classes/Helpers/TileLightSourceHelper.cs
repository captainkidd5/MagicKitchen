using Microsoft.Xna.Framework;
using Penumbra;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes.TileAddons;


namespace TiledEngine.Classes.Helpers
{
    public static class TileLightSourceHelper
    {

        ///// <summary>
        ///// For use with the "lightSource" tile property
        ///// </summary>
        public static void AddJustLightSource(TileObject tile, TileManager tileManager, string lightPropertyString)
        {

            tile.Addons.Add(new LightBody(
                tile, new IntermediateTmxShape(
                    TiledSharp.TmxObjectType.Ellipse,
                    tile.DestinationRectangle, tile.Position, 3f),
                lightPropertyString));

            TileProgressBar progressBar = new TileProgressBar(tile);
           // progressBar.Load();

            tile.Addons.Add(progressBar);

        }

    }
}
