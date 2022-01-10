﻿using EntityEngine.Classes.Animators;
using EntityEngine.Classes.HumanoidCreation;
using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes
{
    public class HumanoidEntity : Entity
    {
       
        public HumanoidEntity(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {

        }

        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.NPC },
                new List<Category>() { Category.Solid, Category.Grass, Category.TransparencySensor, Category.Item, Category.Portal }, OnCollides, OnSeparates,blocksLight:true, userData: this));

            BigSensorCollidesWithCategories = new List<Category>() { Category.Item, Category.Portal, Category.Solid, Category.PlayerBigSensor };

            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() { Category.NPCBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);

        }

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);

            if (fixtureB.CollisionCategories == Category.Item)
            {
                Item item = fixtureB.Body.UserData as Item;


                if (item.Unique)
                {
                    if (GiveUniqueItem(item, 1) == 0)
                    {
                        item.PickUp();
                    }
                }
                else
                {
                    int amtAbleToRemove = GiveStackableItem(item.Id, item.StackSize);
                    if (amtAbleToRemove == 0)
                    {
                        item.PickUp();
                    }
                    else
                    {
                        item.StackSize -= amtAbleToRemove;
                    }
                }


            }

         
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            BodyPiece[] bodyPieces = new BodyPiece[]
                {
                    new Pants(0),
                    new Shoes(0),
                    new Shirt(0),
                    new Arms(0),
                    new Eyes(0),
                    new Head(0),
                      new Hair(0),

                };
            CustomizeableAnimator customizeableAnimator = new CustomizeableAnimator(bodyPieces);
            LoadAnimations(customizeableAnimator);

        }

        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
         //  MainHullBody.Hull.Position = Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
