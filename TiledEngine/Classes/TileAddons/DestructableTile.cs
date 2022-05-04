using InputEngine.Classes;
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
using UIEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{

    internal class DestructableTile : TileBody
    {

        public CursorIconType CursorIconType { get; private set; }
        public DestructableTile(Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape, string destructionType) : base(tile, tileManager, intermediateTmxShape)
        {
            CursorIconType = Cursor.GetCursorIconTypeFromString(destructionType.Split(',')[0]);
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
             new List<Category>() { Category.Solid }, new List<Category>() { Category.Player, Category.NPC, Category.FrontalSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition, IntermediateTmxShape.Radius,
                  new List<Category>() { Category.Solid }, new List<Category>() { Category.Player, Category.NPC,Category.FrontalSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }

        }






        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PlayerInClickRange || PlayerInControllerActionRange)
                Tile.WithinRangeOfPlayer = true;
            if ((Tile.Sprite as AnimatedSprite).HasLoopedAtLeastOnce)
            {
                if (TileLoader.TileLootManager.HasLootData(Tile.GID))
                    GenerateLoot();
                TileUtility.SwitchGid(Tile, TileManager, IndexLayer);
                TileManager.UpdateGrid(Tile.X, Tile.Y, GridStatus.Clear);


            }

        }



        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);

        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        public override void Interact(bool isPlayer)
        {
            if (isPlayer)
            {
                if (!PlayerInClickRange && !PlayerInControllerActionRange)
                    return;
            }

                (Tile.Sprite as AnimatedSprite).Paused = false;
            if (!IsPlayingASound)
            {
                PlaySound(CursorIconType.ToString());


            }


        }
    }
}
