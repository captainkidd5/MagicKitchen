using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.Helpers;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{

    internal class DestructableTile : TileBody
    {
        private readonly Layers _layer;

        public CursorIconType CursorIconType { get; private set; }
        public DestructableTile(Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape, Layers layer, string destructionType) : base(tile, tileManager, intermediateTmxShape)
        {
            _layer = layer;
            CursorIconType = GetDestructionTypeFromString(destructionType);
            tile.CursorIconType = CursorIconType;
            if (tile.Sprite.GetType() == typeof(AnimatedSprite))
            {
                (tile.Sprite as AnimatedSprite).Paused = true;
            }
        }
        public override void Load()
        {
            if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height,
             new List<Category>() { Category.Solid }, new List<Category>() { Category.Player, Category.PlayerBigSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition, IntermediateTmxShape.Radius,
                  new List<Category>() { Category.Solid }, new List<Category>() { Category.Player, Category.PlayerBigSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }

        }
     





        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PlayerInClickRange)
                Tile.WithinRangeOfPlayer = true;
            if ((Tile.Sprite as AnimatedSprite).HasLoopedAtLeastOnce)
            {

                TileUtility.SwitchGid(Tile, TileManager, _layer);
                TileManager.UpdateGrid(Tile.X, Tile.Y, GridStatus.Clear);


            }

        }



        /// <summary>
        /// TODO: Use the entity's direction as the initial shuff direction. If direction is up or down, then do a random direction.
        /// </summary>
        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);

        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }



        private CursorIconType GetDestructionTypeFromString(string str)
        {
            return (CursorIconType)Enum.Parse(typeof(CursorIconType), str.Split(',')[0]);
        }

        public override void Interact()
        {
            if (PlayerInClickRange)
            {
                (Tile.Sprite as AnimatedSprite).Paused = false;
                if (!IsPlayingASound)
                {
                    PlaySound(CursorIconType.ToString());


                }

            }
        }
    }
}
