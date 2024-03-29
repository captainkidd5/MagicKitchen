﻿using EntityEngine.Classes.CharacterStuff;
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
using SpriteEngine.Classes;
using ItemEngine.Classes.StorageStuff;
using static DataModels.Enums;
using SpriteEngine.Classes.ShadowStuff;
using EntityEngine.Classes.NPCStuff;
using UIEngine.Classes;
using EntityEngine.Classes.StageStuff;

namespace EntityEngine.Classes.HumanoidStuff
{
    public class HumanoidEntity : NPC
    {

        public int Armor => (InventoryHandler as HumanoidInventoryHandler).ArmorValue;
        public byte Strength { get; set; }

        private float _damageImmunityTime = 1f;
        private SimpleTimer _damageImmunityTimer;
        public bool ImmunteToDamage { get; set; }
        public EquipmentStorageContainer EquipmentStorageContainer => (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer;

        public HumanoidEntity(GraphicsDevice graphics, ContentManager content) :
            base( graphics, content)
        {
          
            //  (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.HelmetEquipmentSlot.EquipmentChanged += (Animator as CustomizeableAnimator).OnEquipmentChanged;
        }

        public override void Initialize(string stageName, StageManager stagemanager)
        {
            base.Initialize(stageName, stagemanager);
            BodyPiece[] bodyPieces = new BodyPiece[]
         {
                    new Pants(0),
                    new Shoes(0),
                    new Shirt(0),
                    new Arms(0),
                    new Shoulders(0),

                    new Head(0),
                    new Eyes(0),

                      new Hair(0),

         };
            Animator = new CustomizeableAnimator(bodyPieces);
            InventoryHandler = new HumanoidInventoryHandler(StorageCapacity);
            _damageImmunityTimer = new SimpleTimer(_damageImmunityTime, true);
            Inspectable = true;
        }
        public override void TakeDamage(Entity source, int amt, Vector2? knockBack = null)
        {
            if (!ImmunteToDamage)
            {
                base.TakeDamage(source, amt - Armor, knockBack);

                //Don't want to reduce durabilty if a natural cause caused damage (e.x. hunger)
                if (source != null)
                    (InventoryHandler as HumanoidInventoryHandler).ReduceDurabilityOnEquippedArmor();
                ImmunteToDamage = true;
            }
        }
        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.NPC },
                new List<Category>() { (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh,  (Category)PhysCat.Player, (Category)PhysCat.PlayerBigSensor,
                    (Category)PhysCat.Grass, (Category)PhysCat.Item, (Category)PhysCat.Portal, (Category)PhysCat.FrontalSensor,(Category)PhysCat.ArraySensor}, OnCollides, OnSeparates, mass: 500f, ignoreGravity: true, blocksLight: true, userData: this));

            BigSensorCollidesWithCategories = new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.Portal,
                (Category)PhysCat.SolidHigh, (Category)PhysCat.SolidLow, (Category)PhysCat.PlayerBigSensor };

            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() { (Category)PhysCat.NPCBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);

            CreateArraySensorBody(position);
            CreateClickBox(XOffSet, YOffSet * -1, 16, 16);


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
            ChangeSkinTone(SpriteFactory.GetRandomSkinTone());
        }
        internal override void ChangeSkinTone(Color newSkinTone)
        {
            (Animator as CustomizeableAnimator).ChangeClothingColor(typeof(Head), newSkinTone);
            (Animator as CustomizeableAnimator).ChangeClothingColor(typeof(Arms), newSkinTone);

        }
        internal override void ChangeClothingColor(Type t, Color color) =>
            (Animator as CustomizeableAnimator).ChangeClothingColor(t, color);
        public override void Initialize(StageManager stageManager, Vector2? startPos, string name, bool standardAnimator = false)
        {
          
            Shadow = new Shadow(ShadowType.NPC, CenteredPosition, ShadowSize.Small, SpriteFactory.NPCSheet);
            base.Initialize(stageManager, startPos, name, standardAnimator);

            SubscribeEquipmentSlots();
            //XOffSet = 0;
            //YOffSet = 8;
            if (GetType() != typeof(Player))
            {
                LoadAnimations(Animator);
                LoadWardrobe();
            }

        }


        /// <summary>
        /// Will alert body parts when equipment is changed based on these events
        /// </summary>
        protected void SubscribeEquipmentSlots()
        {
            (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.HelmetEquipmentSlot.EquipmentChanged -= (Animator as CustomizeableAnimator).OnEquipmentChanged;
            (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.TorsoEquipmentSlot.EquipmentChanged -= (Animator as CustomizeableAnimator).OnEquipmentChanged;
            (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.LegsEquipmentSlot.EquipmentChanged -= (Animator as CustomizeableAnimator).OnEquipmentChanged;
            (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.BootsEquipmentSlot.EquipmentChanged -= (Animator as CustomizeableAnimator).OnEquipmentChanged;

            (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.HelmetEquipmentSlot.EquipmentChanged += (Animator as CustomizeableAnimator).OnEquipmentChanged;
            (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.TorsoEquipmentSlot.EquipmentChanged += (Animator as CustomizeableAnimator).OnEquipmentChanged;
            (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.LegsEquipmentSlot.EquipmentChanged += (Animator as CustomizeableAnimator).OnEquipmentChanged;
            (InventoryHandler as HumanoidInventoryHandler).EquipmentStorageContainer.BootsEquipmentSlot.EquipmentChanged += (Animator as CustomizeableAnimator).OnEquipmentChanged;
        }

        protected virtual void LoadWardrobe()
        {
            ChangeClothingColor(typeof(Hair), ColorHelper.GetRandomColor());
            Color shirtColor = ColorHelper.GetRandomColor();
            ChangeClothingColor(typeof(Shirt), shirtColor);
            ChangeClothingColor(typeof(Shoulders), shirtColor);



            ChangeSkinTone(SpriteFactory.GetRandomSkinTone());

            ChangeClothingColor(typeof(Pants), ColorHelper.GetRandomColor());
            ChangeClothingColor(typeof(Shoes), ColorHelper.GetRandomColor());
        }

        public override void SetToDefault()
        {
            base.SetToDefault();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ImmunteToDamage && _damageImmunityTimer.Run(gameTime))
            {
                ImmunteToDamage = false;
            }

        }

        public override void ClickInteraction()
        {
            StatusIcon.SetStatus(StatusIconType.None);

            FaceTowardsOtherEntity(Shared.PlayerPosition);
            UI.TalkingDirection = Vector2Helper.GetOppositeDirection(DirectionMoving);

            UI.ActivateSecondaryInventoryDisplay(null, StorageContainer);
            UI.StorageDisplayHandler.SecondaryStorageClosed += OnSecondaryInventoryClosed;

            UI.ActivateSecondaryEquipmentMenu(EquipmentStorageContainer);

            Halt(true);


        }
        /// <summary>
        /// Want npcs to be able to move again once their inventory is closed by the player
        /// </summary>
        protected virtual void OnSecondaryInventoryClosed()
        {
            Resume();
            UI.StorageDisplayHandler.SecondaryStorageClosed -= OnSecondaryInventoryClosed;

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
            SubscribeEquipmentSlots();

            base.LoadSave(reader);
            Animator.LoadSave(reader);
        }
    }
}
