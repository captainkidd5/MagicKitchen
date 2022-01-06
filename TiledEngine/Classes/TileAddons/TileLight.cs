using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.Helpers;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.TileAddons
{
    internal class LightBody : TileBody
    {
        private Point _pointOffset;
        private float _lightRadius;

        public LightBody(Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape,
            string lightPropertyString, float lightRadius) : base(tile, tileManager, intermediateTmxShape)
        {

            _pointOffset = ParseLightString(lightPropertyString);
            _lightRadius = lightRadius;
        }
        public override void Load()
        {
            CreateBody(Tile.Position);

        }

        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, Tile.Position, 4f, new List<Category>() { Category.LightSource }, null, null, null,
               light: PhysicsManager.GetPointLight(new Vector2(Tile.Position.X + _pointOffset.X, Tile.Position.Y + _pointOffset.Y), true, 400),isSensor:true));
        }

        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private Point ParseLightString(string lightString)
        {

            return new Point(int.Parse(lightString.Split(',')[1]),
                int.Parse(lightString.Split(',')[2]));

        }
    }
}
