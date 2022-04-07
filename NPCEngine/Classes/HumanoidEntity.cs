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
using System.IO;
using System.Text;
using TiledEngine.Classes;
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
            EntityAnimator = new CustomizeableAnimator(bodyPieces);
        }

        
        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.NPC },
                new List<Category>() { Category.Solid,Category.Player,Category.PlayerBigSensor,Category.Cursor, Category.Grass, Category.Item, Category.Portal }, OnCollides, OnSeparates, ignoreGravity: true, blocksLight:true, userData: this));

            BigSensorCollidesWithCategories = new List<Category>() { Category.Item, Category.Portal, Category.Solid, Category.PlayerBigSensor };

            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() { Category.NPCBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);

        }

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);  
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        internal void RandomizeColorCustomization()
        {
            ChangeSkinTone(EntityFactory.GetRandomSkinTone());
        }
        internal void ChangeSkinTone(Color newSkinTone)
        {
            (EntityAnimator as CustomizeableAnimator).ChangeClothingColor(typeof(Head), newSkinTone);
            (EntityAnimator as CustomizeableAnimator).ChangeClothingColor(typeof(Arms), newSkinTone);

        }
        internal void ChangeClothingColor(Type t, Color color) => 
            (EntityAnimator as CustomizeableAnimator).ChangeClothingColor(t, color);
        public override void LoadContent(ItemManager itemManager )
        {
            base.LoadContent(itemManager);
            LoadAnimations(EntityAnimator);



        }

        public override void CleanUp()
        {
            base.CleanUp();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            EntityAnimator.Save(writer);
        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
            EntityAnimator.LoadSave(reader);
        }
    }
}
