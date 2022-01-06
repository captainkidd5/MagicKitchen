﻿using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.Helpers;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.TileAddons
{
    internal class TileBody : Collidable, ITileAddon
    {
        public Tile Tile { get; set; }
        protected readonly TileManager tileManager;
        protected IntermediateTmxShape IntermediateTmxShape { get; set; }
        public TileBody(Tile tile, TileManager tileManager, IntermediateTmxShape intermediateTmxShape)
        {
            Tile = tile;
            this.tileManager = tileManager;
            IntermediateTmxShape = intermediateTmxShape;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
           // throw new NotImplementedException();
        }


        public virtual void Load()
        {
            if(IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Basic)
            {
                AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,
               IntermediateTmxShape.Width, IntermediateTmxShape.Height, new List<Category>() { Category.Solid },
               new List<Category>() { Category.Player, Category.NPC, Category.PlayerBigSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else if(IntermediateTmxShape.TmxObjectType == TiledSharp.TmxObjectType.Ellipse)
            {
                AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Static, IntermediateTmxShape.HullPosition,IntermediateTmxShape.Radius,
                    new List<Category>() { Category.Solid }, new List<Category>() { Category.Player, Category.NPC, Category.PlayerBigSensor }, OnCollides, OnSeparates, blocksLight: IntermediateTmxShape.BlocksLight));
            }
            else
            {
                throw new Exception($"{IntermediateTmxShape.TmxObjectType} is not supported.");
            }
          
        }
        public override void Unload()
        {
            base.Unload();
        }

        public virtual void Interact()
        {
            throw new NotImplementedException();
        }
    }
}
