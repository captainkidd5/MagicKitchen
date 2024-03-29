﻿using EntityEngine.Classes.BehaviourStuff;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextEngine;
using TiledEngine.Classes;
using UIEngine.Classes;
using static Globals.Classes.Settings;
using static DataModels.Enums;
using EntityEngine.Classes.ScriptStuff;
using DataModels.ScriptedEventStuff;
using SpriteEngine.Classes.Presets;
using ItemEngine.Classes.StorageStuff;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using SoundEngine.Classes;
using EntityEngine.Classes.ToolStuff;
using EntityEngine.ItemStuff;
using TiledEngine.Classes.TileAddons;
using DataModels;
using SpriteEngine.Classes.Animations.EntityAnimations;
using SpriteEngine.Classes.ParticleStuff;
using IOEngine.Classes;
using System.Linq;
using TiledEngine.Classes.PortalStuff;
using EntityEngine.Classes.StageStuff;

namespace EntityEngine.Classes
{


    /// <summary>
    /// Represents any character-like thing
    /// </summary>
    public abstract class Entity : Collidable, ISaveable
    {
        private readonly GraphicsDevice _graphics;
        private readonly ContentManager _content;

        //Movement
        public float BaseSpeed { get; protected set; } = 2.5f;
        protected Vector2 Velocity;
        internal float Speed { get; set; }
        protected byte StorageCapacity { get; set; }
        public Direction DirectionMoving { get; set; }
        public bool IsMoving { get; protected set; }
        protected bool ForceStop { get; private set; }
        //Entity gets the data representation of inventory while ui handles actual visuals

        internal Animator Animator { get; set; }
        protected Navigator Navigator { get; set; }

        public string Name { get; protected set; }
        internal string ScheduleName { get; set; }
        protected StatusIcon StatusIcon { get; set; }

        public bool IsUsingProgressBar => ProgressBarSprite != null;
        protected ProgressBarSprite ProgressBarSprite { get; private set; }



        private protected InventoryHandler InventoryHandler { get; set; }
        public StorageContainer StorageContainer => InventoryHandler.StorageContainer;



        public Point TileOn => Vector2Helper.GetTileIndexPosition(Position);



        private OverheadItemDisplay _overHeadItemDisplay { get; set; }

        internal ToolHandler ToolHandler { get; set; }
        public virtual byte  MaxHealth { get; protected set; } = 100;
        public virtual byte CurrentHealth { get; protected set; } = 100;

        public bool FlaggedForRemoval { get; set; }

        public HullBody DamageBody { get; protected set; }

        public string CurrentStageName { get; protected set; }

        protected StageManager StageManager { get; set; }

        protected TileManager TileManager => StageManager.AllStages[CurrentStageName].TileManager;
        protected ItemManager ItemManager => StageManager.AllStages[CurrentStageName].ItemManager;
        protected NPCContainer NPCContainer => StageManager.AllStages[CurrentStageName].NPCContainer;
        public Entity( GraphicsDevice graphics, ContentManager content) : base()
        {
          
            _graphics = graphics;
            _content = content;
          
             
        }


        public virtual void Initialize(string stageName, StageManager stagemanager)
        {
            base.Initialize();
            CurrentStageName = stageName;
            StageManager = stagemanager;
            Name = GetType().ToString();

            StorageCapacity = 6;
            Navigator = new Navigator(Name);
            Speed = BaseSpeed;

            InventoryHandler = new InventoryHandler(StorageCapacity);
            SoundModuleManager = new SoundModuleManager();


        }
        public virtual void Initialize(StageManager stageManager)
        {
            base.Initialize();
            Initialize(stageManager.CurrentStage.Name, stageManager);
            StatusIcon = new StatusIcon(new Vector2(XOffSet, YOffSet));

            CreateBody(Position);

            DirectionMoving = Direction.Down;

            _overHeadItemDisplay = new OverheadItemDisplay();

            ToolHandler = new ToolHandler(this, InventoryHandler);
            if (string.IsNullOrEmpty(CurrentStageName))
                CurrentStageName = StageManager.CurrentStage.Name;
            Navigator.Load(TileManager.PathGrid);

            InventoryHandler.SwapItemManager(ItemManager);

        }

        public virtual void MoveToPortal(string to, string from)
        {

            Portal portal = MapLoader.Portalmanager.AllPortals.FirstOrDefault(x => x.To == from && x.From == to);
            if (portal == null)
            {
                throw new Exception($"Unable to find portal for destination {to}");
            }

            Move(portal.Position);
        }
    
        public void SelectItem(Item item) => _overHeadItemDisplay.SelectItem(item, Position);
        public bool IsProgressComplete() => ProgressBarSprite.FullyCharged;

        public float LayerDepth => Animator.Layer;
        internal void AddProgressBar()
        {
            ProgressBarSprite = new ProgressBarSprite();
            ProgressBarSprite.Load(Position, .9f, new Vector2(-32, -32));
        }

        internal void RemoveProgressBar()
        {
            ProgressBarSprite = null;
        }

        internal virtual void ChangeSkinTone(Color newSkinTone)
        {

        }
        internal virtual void ChangeClothingColor(Type t, Color color)
        {

        }
      



        internal void LoadAnimations(Animator animator)
        {
            Animator = animator;
            Animator.Load(SoundModuleManager, Position);

            Animator.SoundPlayed += PlayStepSoundFromTile;
        }

        protected override void CreateBody(Vector2 position)
        {
            if (MainHullBody == null)
                MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.NPC },
                    new List<Category>() { (Category)PhysCat.PlayArea,(Category)PhysCat.Player, (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh,
                        (Category)PhysCat.Grass, (Category)PhysCat.TransparencySensor,  (Category)PhysCat.Portal,(Category)PhysCat.Tool,(Category)PhysCat.ArraySensor,
                    (Category)PhysCat.NPCBigSensor,(Category)PhysCat.Damage},
                    OnCollides, OnSeparates, ignoreGravity: true, blocksLight: true, userData: this, mass: 200f);

            BigSensorCollidesWithCategories = new List<Category>() { (Category)PhysCat.NPC, (Category)PhysCat.Player, (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh };
            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 16f, new List<Category>() { (Category)PhysCat.NPCBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);



        }

        public void SetNavigatorTarget(Vector2 pos) => Navigator.SetTarget(pos);
        public bool HasActivePath => Navigator.HasActivePath;
        public bool FollowPath(GameTime gameTime, ref Vector2 velocity) => Navigator.FollowPath(gameTime, Position, ref velocity);

        public Point? NearestClearPoint(Point point, int radius) => Navigator.NearestClearPoint(point, radius);
        public int GetPathDistance(Point start, Point end) => Navigator.PathDistance(start, end);

        public void ResetNavigator() => Navigator.Unload();
        public virtual bool FindPathTo(Vector2 otherPos)
        {

            if (Navigator.FindPathTo(Position, otherPos))
                return true;
            return false;
        }


        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //Collision logic changes based on current behaviour!
            if (fixtureB.CollisionCategories==((Category)PhysCat.Item) && fixtureA.CollisionCategories==((Category)PhysCat.Player))
            {
                WorldItem worldItem = (fixtureB.Body.Tag as WorldItem);
                InventoryHandler.GiveItem(worldItem);

                SoundFactory.PlayEffectPackage("ItemGrab");
            }
            return base.OnCollides(fixtureA, fixtureB, contact);

        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }



        public virtual void Halt(bool forceStop = false)
        {
            Velocity = Vector2.Zero;
            ForceStop = forceStop;
        }
        protected virtual void Resume()
        {
            ForceStop = false;
        }
      
        public SubmergenceLevel SubmergenceLevel { get; set; }
        public virtual new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SubmergenceLevel = SubmergenceLevel.None;
            Speed = BaseSpeed;
            if (TileManager.IsTypeOfTile("water", Position))
            {
                SubmergenceLevel = SubmergenceLevel.Shallow;
                Speed = BaseSpeed * .5f;

            }
            if (TileManager.IsTypeOfTile("deepWater", Position))
            {
                SubmergenceLevel = SubmergenceLevel.Deep;

                Speed = BaseSpeed * .25f;

            }
            StatusIcon.Update(gameTime, Position);

            if (ProgressBarSprite != null)
                ProgressBarSprite.Update(gameTime, Position);

            IsMoving = ((Velocity != Vector2.Zero));

            if (IsMoving && !ForceStop && !Animator.IsPerformingAnimation())
                DirectionMoving = UpdateDirection();

            //if (Animator.CurrentActionType != ActionType.Walking && Animator.CurrentActionType != ActionType.None)
            //    Velocity = Vector2.Zero;
            if (Animator.IsPerformingAnimation())
                Halt();
            //FinalMove(gameTime);


           // MainHullBody.Body.ApplyForce(Velocity * 100 * (float)gameTime.ElapsedGameTime.Milliseconds);








            //Position is changed so that our sprite knows where to draw, and position
            //snaps to where the physics system moved the main hullbody
            //Big sensor position just sticks on top of our main hullbody
            BigSensor.Position = Position;

            //If submerged, draw sprite much lower than if on land, to better create illusion
            if (SubmergenceLevel > SubmergenceLevel.None)
            {
                Animator.Update(gameTime, DirectionMoving, IsMoving, new Vector2(Position.X, Position.Y + 14), Speed / BaseSpeed);

            }
            else
            {
                Animator.Update(gameTime, DirectionMoving, IsMoving, new Vector2(Position.X, Position.Y), Speed / BaseSpeed);

            }
          

            _overHeadItemDisplay.Update(gameTime, Position, LayerDepth);
            ToolHandler.Update(gameTime);

        }

        protected virtual void FinalMove(GameTime gameTime)
        {
            MainHullBody.Body.LinearVelocity = Velocity * Speed * (float)gameTime.ElapsedGameTime.Milliseconds;

        }
        internal bool IsWater(Vector2 position)
        {
            return TileManager.IsTypeOfTile("water", position) || TileManager.IsTypeOfTile("deepWater", position);
        }
        protected void UseHeldItem() => ToolHandler.UseHeldItem();

        protected void ChargeHeldItem(GameTime gameTime, Vector2 aimPosition) => ToolHandler.ChargeHeldItem(gameTime, aimPosition);

        protected virtual void ActivateTool(Tool tool) => ToolHandler.ActivateTool(tool);




        protected virtual void DrawAnimator(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, SubmergenceLevel);




        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            StatusIcon.Draw(spriteBatch);

            if (ProgressBarSprite != null)
                ProgressBarSprite.Draw(spriteBatch);

            Animator.Draw(spriteBatch, SubmergenceLevel);
            DrawAnimator(spriteBatch);
            _overHeadItemDisplay.Draw(spriteBatch);



            ToolHandler.Draw(spriteBatch);
            if(LightsCollidable != null)
            {

            foreach(var light in LightsCollidable)
            {
                light.DrawEmitter(spriteBatch);
            }
            }

        }

        public virtual void DrawDebug(SpriteBatch spriteBatch)
        {
            Navigator.DrawDebug(spriteBatch, Color.White);
            //spriteBatch.DrawString(TextFactory.BitmapFont, $"{Name} \n {CurrentStageName}",
            //    CenteredPosition, Color.Red, 0f, Vector2.Zero, .5f, SpriteEffects.None, .99f);
        }

        protected void RemoveBigSensorCat(Category cat)
        {
            if (BigSensorCollidesWithCategories.Contains(cat))
            {
                BigSensorCollidesWithCategories.Remove(cat);
                BigSensor.Body.SetCollidesWith(Category.All);

                foreach (Category category in BigSensorCollidesWithCategories)
                    BigSensor.Body.SetCollidesWith(category);
            }
        }
        protected void AddBigSensorCat(Category cat)
        {
            if (BigSensorCollidesWithCategories.Contains(cat))
            {
                throw new Exception($"Big sensor already contains {cat}");
            }
            else
            {
                BigSensorCollidesWithCategories.Add(cat);

                foreach (Category category in BigSensorCollidesWithCategories)
                    BigSensor.Body.SetCollidesWith(category);
            }

        }
        protected void SetCollidesWith(List<Category> categories)
        {
            foreach (Category category in categories)
                BigSensor.Body.SetCollisionCategories(category);
        }

        /// <summary>
        /// Was previously not in player stage, now is. Activate main body and allow click interactions.
        /// </summary>
        protected virtual void RestoreEntityPhysics()
        {
            MainHullBody.Body.SetIsSensor(false);
            AddBigSensorCat((Category)PhysCat.Cursor);
            AddBigSensorCat((Category)PhysCat.TransparencySensor);

        }
        /// <summary>
        /// Was previously in player stage, no longer is. Disable collisions and remove click interactions.
        /// </summary>
        protected virtual void RemoveEntityPhysics()
        {
            MainHullBody.Body.SetIsSensor(true);

            //Shouldn't be able to click on entity when not in same stage.
            RemoveBigSensorCat((Category)PhysCat.Cursor);
            RemoveBigSensorCat((Category)PhysCat.TransparencySensor);

        }


        protected Direction UpdateDirection()
        {
            if (Velocity.X > .5f)
                return Direction.Right; //right
            else if (Velocity.X < -.5f)
                return Direction.Left; //left
            else if (Velocity.Y < .5f) // up
                return Direction.Up;
            else if (Velocity.Y > .5f)
                return Direction.Down;
            else
                return Direction.Down;
        }
        public virtual void ClickInteraction()
        {

        }

        public void PlayStepSoundFromTile()
        {

            string soundName = TileManager.GetStepSoundFromPosition(Position);
            if (string.IsNullOrEmpty(soundName))
                return;

            SoundModuleManager.PlaySpecificSound(soundName);

        }

        //Items

        public void GiveItem(WorldItem item) => InventoryHandler.GiveItem(item);
        public void GiveItem(Item item, ref int count) => InventoryHandler.GiveItem(item, ref count);

        public void GiveItem(string name, int count) => InventoryHandler.GiveItem(name, count);

        public void DropItem(string name, int count) => InventoryHandler.DropItem(Position, Vector2Helper.GetVectorFromDirection(DirectionMoving), name, count);
        public void DropItem(Item item, int count) => InventoryHandler.DropItem(Position, Vector2Helper.GetVectorFromDirection(DirectionMoving), item, count);


        protected virtual void DropCurrentlyHeldItemToWorld()
        {
            InventoryHandler.ItemManager.AddWorldItem(new Vector2(Position.X, Position.Y - YOffSet / 2), InventoryHandler.HeldItem, InventoryHandler.HeldItemCount, WorldItemState.Bouncing, Vector2Helper.GetVectorFromDirection(DirectionMoving));

        }


        internal void InteractWithTile(Point tilePoint, Layers tileLayer)
        {
            TileData? tile = TileManager.GetTileDataFromPoint(tilePoint, tileLayer);
            if (tile == null)
                throw new Exception($"No tile at {tilePoint} at layer {tileLayer.ToString()}!");
            ActionType? actionType = null;
           Action action = TileManager.TileObjects[tile.Value.GetKey()].Interact(ref actionType,false, InventoryHandler.HeldItem, CenteredPosition, DirectionMoving);
            if (actionType != null && IsJumpActionType(actionType.Value))
            {
                ReactToJumpActionType(actionType.Value);
            }
            //For now, entities immediately trigger tile effect
            action();
        }
        protected bool IsJumpActionType(ActionType actionType)
        {
            return actionType == ActionType.JumpUp || actionType == ActionType.JumpDown ||
                actionType == ActionType.JumpLeft || actionType == ActionType.JumpRight;
        }
        protected void ReactToJumpActionType(ActionType jumpActionType)
        {

            switch (jumpActionType)
            {
                case ActionType.JumpUp:
                    Move(new Vector2(Position.X, Position.Y - Settings.TileSize));
                    break;
                case ActionType.JumpDown:
                    Move(new Vector2(Position.X, Position.Y + Settings.TileSize));
                    break;
                case ActionType.JumpLeft:
                    Move(new Vector2(Position.X - Settings.TileSize, Position.Y));
                    break;
                case ActionType.JumpRight:
                    Move(new Vector2(Position.X + Settings.TileSize, Position.Y));
                    break;
                default:
                    throw new Exception($"Invalid actiontype provided");
            }
                
            if (TileManager.IsTypeOfTile("water", Position))
            {
                SoundModuleManager.PlayPackage("Splash");
            }

        }
        protected void PerformAction(Action action, Direction direction, ActionType actionType)
        {
            Animator.PerformAction(action, direction, actionType);
        }

        internal bool IsFacingTowardsOtherEntity(Vector2 otherEntityPos)
        {
            if (DirectionMoving == Vector2Helper.GetDirectionOfEntityInRelationToEntity(Position, otherEntityPos))
                return true;
            return false;
        }
        internal Direction FaceTowardsOtherEntity(Vector2 otherEntityPos)
        {
            DirectionMoving = Vector2Helper.GetDirectionOfEntityInRelationToEntity(Position, otherEntityPos);
            return DirectionMoving;
        }

        public void FaceDirection(Direction directionToFace)
        {
            DirectionMoving = directionToFace;
        }

        public virtual void Save(BinaryWriter writer)
        {

            writer.Write(CurrentStageName);
            Vector2Helper.WriteVector2(writer, Position);
            InventoryHandler.Save(writer);
            writer.Write(ScheduleName);
        }
        public virtual void LoadSave(BinaryReader reader)
        {
            CurrentStageName = reader.ReadString(); 
            Move(Vector2Helper.ReadVector2(reader));


            InventoryHandler.LoadSave(reader);
            ScheduleName = reader.ReadString();
        }
        public override void SetToDefault()
        {
            base.SetToDefault();
            Speed = BaseSpeed;
            StorageCapacity = 4;
            InventoryHandler.SetToDefault();
            CurrentHealth = MaxHealth;
        }
     
    }
}
