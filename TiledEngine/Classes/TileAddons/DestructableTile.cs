using DataModels.ItemStuff;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes.Helpers;
using UIEngine.Classes;

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
             new List<Category>() { (Category)PhysCat.Solid }, new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.NPC, (Category)PhysCat.FrontalSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition, IntermediateTmxShape.Radius,
                  new List<Category>() { (Category)PhysCat.Solid }, new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.NPC, (Category)PhysCat.FrontalSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if(IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Polygon)
            {

                AddPrimaryBody(PhysicsManager.CreatePolygonHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,new Vertices(IntermediateTmxShape.Vertices),
                  new List<Category>() { (Category)PhysCat.Solid }, new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.NPC, (Category)PhysCat.FrontalSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }

        }


        /// <summary>
        /// "Destructable - Rock,Good
        /// returns ToolTier.Good
        /// else returns ToolTier.None
        /// </summary>
        protected ToolTier GetToolTier()
        {
            string property = GetProperty("destructable");
            string[] split = property.Split(',');
            if(split.Length > 0)
            {
                string type = string.Empty;
                ToolTier toolTier = (ToolTier)Enum.Parse(typeof(ToolTier), split[1]);
                return toolTier;
            }
            return ToolTier.None;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PlayerInClickRange || PlayerInControllerActionRange)
                Tile.WithinRangeOfPlayer = true;
            if ((Tile.Sprite as AnimatedSprite).HasLoopedAtLeastOnce)
            {
                DestroyTileAndGetLoot();

            }

        }



        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);

        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        public override void Interact(bool isPlayer, Item heldItem)
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
