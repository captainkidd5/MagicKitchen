using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using TiledEngine.Classes.Helpers;


namespace TiledEngine.Classes.TileAddons
{
    internal class LightBody : TileBody
    {
        private Point _pointOffset;
        private float _lightRadius;
        private LightType _lightType;

        public LightBody(TileObject tile, IntermediateTmxShape intermediateTmxShape,
            string lightPropertyString) : base(tile, intermediateTmxShape)
        {

            _pointOffset = GetOffSet(lightPropertyString);
            _lightRadius = GetRadius(lightPropertyString);
            _lightType = GetLightType(lightPropertyString);
        }
        public override void Load()
        {
            CreateBody(Tile.Position + new Vector2(_pointOffset.X, _pointOffset.Y));
            AddLight(_lightType, new Vector2(_pointOffset.X, _pointOffset.Y), true,_lightRadius);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 4f,
                new List<Category>() { (Category)PhysCat.LightSource }, new List<Category>() { (Category)PhysCat.PlayerBigSensor }, null, null,
              isSensor:true));
        }

        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private Point GetOffSet(string lightString)
        {

            return new Point(int.Parse(lightString.Split(',')[1]),
                int.Parse(lightString.Split(',')[2]));

        }

        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private int GetRadius(string lightString)
        {

            return int.Parse(lightString.Split(',')[0]);

        }
        /// <summary>
        /// For use with the "lightSource" tile property
        /// </summary>
        private LightType GetLightType(string lightString)
        {

            return (LightType)Enum.Parse(typeof(LightType),lightString.Split(',')[3]);

        }
    }
}
