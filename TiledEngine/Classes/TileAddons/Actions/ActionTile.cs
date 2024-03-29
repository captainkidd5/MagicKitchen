﻿using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using UIEngine.Classes;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons.Actions
{

    public class ActionTile : TileBody
    {
        
        public ActionTile(TileObject tile,  IntermediateTmxShape intermediateTmxShape, string actionType) : base(tile, intermediateTmxShape)
        {
           
        }
        protected override List<Category> GetCollisionCategories()
        {
            if (IndexLayer < Layers.buildings)
                return new List<Category>() { (Category)PhysCat.ClickBox };
            else
                return new List<Category>() {  (Category)PhysCat.ClickBox };


        }
        protected override bool OnClickBoxCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnClickBoxCollides(fixtureA, fixtureB, contact);
        }
        protected override List<Category> GetCategoriesCollidesWith()
        {
            return new List<Category>() { (Category)PhysCat.Cursor, (Category)PhysCat.PlayerBigSensor };
        }
        protected virtual bool ReturnIsSensor()
        {
            return false;
        }
        public override void Load()
        {
            List<Category> categoriesCollidesWith =GetCategoriesCollidesWith();
            List<Category> collisionCategories = GetCollisionCategories();

            if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height,
             collisionCategories, categoriesCollidesWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight, mass: 0f, isSensor: ReturnIsSensor())); ;
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition, IntermediateTmxShape.Radius,
                collisionCategories, categoriesCollidesWith, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight, mass:0f, isSensor: ReturnIsSensor()));
            }
            else if (IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Polygon)
            {

                AddPrimaryBody(PhysicsManager.CreatePolygonHullBody(BodyType.Dynamic, IntermediateTmxShape.HullPosition, new Vertices(IntermediateTmxShape.Vertices),
                  GetCollisionCategories(), GetCategoriesCollidesWith(), OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight, isSensor: ReturnIsSensor()));
            }
            else
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }
            Move(IntermediateTmxShape.HullPosition);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(IntermediateTmxShape.HullPosition);
     
            if (WithinRangeOfPlayer(Controls.ControllerConnected))
            {
                Tile.WithinRangeOfPlayer = true;
                if (IsHovered(Controls.ControllerConnected))
                {
                    if(Tile.TileManager.CheckPrecedence((Layers)Tile.TileData.Layer))
                        AlterCursorAndAlertTile();

                }

            }

        }



        protected virtual void AlterCursorAndAlertTile()
        {
            UI.Cursor.ChangeCursorIcon(Tile.GetCursorIconType());
            Tile.AlertTileManagerCursorIconChanged();
        }

        /// <summary>
        /// "Destructable - Rock,Good
        /// returns ToolTier.Good
        /// else returns ToolTier.None
        /// </summary>
        protected virtual ToolTier GetRequiredToolTier()
        {
            string property = Tile.GetProperty("action", true);
            string[] split = property.Split(',');
            if (split.Length > 0)
            {
                string type = string.Empty;
                ToolTier toolTier = (ToolTier)Enum.Parse(typeof(ToolTier), split[2]);
                return toolTier;
            }
            return ToolTier.None;
        }

        protected virtual ItemType GetRequiredItemType()
        {
            string property = Tile.GetProperty("action", true);
            string[] split = property.Split(',');
            if (split.Length > 0)
            {
                string type = string.Empty;
                ItemType toolTier = (ItemType)Enum.Parse(typeof(ItemType), split[1]);
                return toolTier;
            }
            return ItemType.None;
        }

        protected bool MeetsItemRequirements(Item item)
        {
            return item != null && item.ItemType == GetRequiredItemType() && item.ToolTier >= GetRequiredToolTier();
        }
        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            OnClickBoxCollides(fixtureA, fixtureB, contact);
            return base.OnCollides(fixtureA, fixtureB, contact);


        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            OnClickBoxSeparates(fixtureA, fixtureB, contact);
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        public override Action Interact(ref ActionType? actionType, bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)

        {
            if (isPlayer)
            {
                if (!PlayerInClickRange)
                    return null;
            }
            return base.Interact(ref actionType, isPlayer, heldItem, entityPosition, directionEntityFacing);

        }
    }
}
