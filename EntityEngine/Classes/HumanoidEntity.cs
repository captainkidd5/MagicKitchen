using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
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
using static Globals.Classes.Settings;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using SpriteEngine.Classes.Animations.BodyPartStuff;
using SpriteEngine.Classes.Animations.EntityAnimations;

namespace EntityEngine.Classes
{
    public class HumanoidEntity : Character
    {
       
        public HumanoidEntity(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            BodyPiece[] bodyPieces = new BodyPiece[]
           {
                    new Pants(0),
                    new Shoes(0),
                    new Shirt(0),
                    new Shoulders(0),
                    new Arms(0),
                    new Eyes(0),
                    new Head(0),
                      new Hair(1),

           };
            Animator = new CustomizeableAnimator(bodyPieces);
        }

        
        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.NPC },
                new List<Category>() { (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh,  (Category)PhysCat.Player, (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.Cursor,
                    (Category)PhysCat.Grass, (Category)PhysCat.Item, (Category)PhysCat.Portal, (Category)PhysCat.FrontalSensor}, OnCollides, OnSeparates, ignoreGravity: true, blocksLight:true, userData: this));

            BigSensorCollidesWithCategories = new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.Portal, (Category)PhysCat.SolidHigh, (Category)PhysCat.SolidLow, (Category)PhysCat.PlayerBigSensor};

            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() { (Category)PhysCat.NPCBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);

        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);  
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        internal void RandomizeColorCustomization()
        {
            ChangeSkinTone(EntityFactory.GetRandomSkinTone());
        }
        internal override void ChangeSkinTone(Color newSkinTone)
        {
            (Animator as CustomizeableAnimator).ChangeClothingColor(typeof(Head), newSkinTone);
            (Animator as CustomizeableAnimator).ChangeClothingColor(typeof(Arms), newSkinTone);

        }
        internal override void ChangeClothingColor(Type t, Color color) => 
            (Animator as CustomizeableAnimator).ChangeClothingColor(t, color);
        public override void LoadContent(EntityContainer entityContainer, Vector2? startPos, string? name, bool standardAnimator = false)
        {
            base.LoadContent(entityContainer, startPos, name, standardAnimator);
            LoadAnimations(Animator);
            GetClothingColors();

        }

        protected void GetClothingColors()
        {
            ChangeClothingColor(typeof(Hair), ColorHelper.GetRandomColor());
            Color shirtColor = ColorHelper.GetRandomColor();
            ChangeClothingColor(typeof(Shirt), shirtColor);
            ChangeClothingColor(typeof(Shoulders), shirtColor);



            ChangeSkinTone(EntityFactory.GetRandomSkinTone());

            ChangeClothingColor(typeof(Pants), ColorHelper.GetRandomColor());
            ChangeClothingColor(typeof(Shoes), ColorHelper.GetRandomColor());
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
            Animator.Save(writer);
        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
            Animator.LoadSave(reader);
        }
    }
}
