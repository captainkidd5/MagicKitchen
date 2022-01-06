using Microsoft.Xna.Framework;
using Penumbra;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.TileAddons;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.Helpers
{
    public static class TileLightSourceHelper
    {

        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        public static void AddJustLightSource(Tile tile,TileManager tileManager, string lightPropertyString, float lightRadius, OnCollisionHandler? cHandler = null, OnSeparationHandler? sHandler = null)
        {

            tile.Addons.Add(new LightBody(tile,tileManager, null, lightPropertyString, lightRadius));

        }

        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private static Point ParseLightString(string lightString)
        {

            return new Point(int.Parse(lightString.Split(',')[1]),
                int.Parse(lightString.Split(',')[2]));
            
        }
        /// <summary>
        /// Default tile collision behaviour (no response)
        /// </summary>
        private static void BasicOnCollilsion(Fixture a, Fixture b, Contact contact)
        {
            //Console.WriteLine("On Collision");
        }

        /// <summary>
        /// Default tile separation behaviour (no response)
        /// </summary>
        private static void BasicOnSeparation(Fixture a, Fixture b, Contact contact)
        {
            //  Console.WriteLine("On Separation");
        }
    }
}
