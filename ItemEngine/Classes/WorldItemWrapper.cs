﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.Filtering;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public class WorldItemWrapper : Collidable
    {
        private static readonly int _width = 16;
        private Item _item;

        //How many items this item represents

        public int Count { get; private set; }
        public Sprite Sprite { get; set; }

        public WorldItemWrapper(Item item, int count, Vector2 position)
        {
            _item = item;
            Count = count;
            Sprite = SpriteFactory.CreateWorldSprite(new Rectangle((int)_position.X, (int)_position.Y, _width, _width), Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet);
        }
        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.Player },
               new List<Category>() { Category.Solid, Category.Grass, Category.TransparencySensor, Category.Item, Category.Portal }, OnCollides, OnSeparates, blocksLight: true, userData: this);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Sprite.Update(gameTime, Position);
        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }

        private static readonly float directionMagnitude = 10;
        private Vector2 GetTossDirectionFromDirectionFacing(Direction directionFacing)
        {
            switch (directionFacing)
            {
                case Direction.Down:
                    return new Vector2(0, directionMagnitude);
                case Direction.Up:
                    return new Vector2(0, -directionMagnitude);
                case Direction.Left:
                    return new Vector2(-directionMagnitude, 0);
                case Direction.Right:
                    return new Vector2(directionMagnitude, 0);

                default:
                    throw new Exception(directionFacing.ToString() + " is invalid");
            }
        }
    }
}
