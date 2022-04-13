﻿using EntityEngine.Classes.Animators;
using EntityEngine.Classes.BehaviourStuff;
using EntityEngine.Classes.HumanoidCreation;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
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
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using static Globals.Classes.Settings;
using static DataModels.Enums;

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
        protected float StartingSpeed { get; set; } = 12f;
        protected Vector2 Velocity;
        protected float Speed { get; set; }
        protected int StorageCapacity { get; set; }
        public Direction DirectionMoving { get; set; }
        public bool IsMoving { get; protected set; }
        protected bool ForceStop { get; private set; }
        //Entity gets the data representation of inventory while ui handles actual visuals
        public string CurrentStageName { get; protected set; }
        internal Animator Animator { get; set; }
        protected Navigator Navigator { get; set; }

        public string TargetStage {get; internal protected set; }
        internal TileManager TileManager { get; set; }
        public string Name { get; protected set; }
        protected StatusIcon StatusIcon { get; set; }
        /// <summary>
        /// If entity is present at the current stage
        /// </summary>
        public bool IsInStage { get; 
            set; }
        protected Behaviour Behaviour { get; set; }

        private protected InventoryHandler InventoryHandler { get; set; }
        public StorageContainer StorageContainer => InventoryHandler.StorageContainer;

        //warp
        private WarpHelper _warpHelper;

        public Point TileOn => new Point((int)(Position.X / 16), (int)(Position.Y / 16));
        public bool IsWarping => _warpHelper.IsWarping;

        


        public Entity(GraphicsDevice graphics, ContentManager content) : base()
        {
            _graphics = graphics;
            _content = content;
            Name = GetType().ToString();

            StorageCapacity = 4;
            Navigator = new Navigator(Name);
            Speed = StartingSpeed;
            Behaviour = new WanderBehaviour(this, StatusIcon, Navigator,TileManager, null, null);
            _warpHelper = new WarpHelper(this);
            InventoryHandler = new InventoryHandler(StorageCapacity);

        }

        internal virtual void ChangeSkinTone(Color newSkinTone)
        {
            
        }
        internal virtual void ChangeClothingColor(Type t, Color color)
        {

        }
        public virtual void LoadContent(ItemManager itemManager)
        {

            CreateBody(Position);

            DirectionMoving = Direction.Down;
            StatusIcon = new StatusIcon(new Vector2(XOffSet, YOffSet));
            InventoryHandler.LoadContent(itemManager);
            TargetStage = CurrentStageName;


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newStageName"></param>
        /// <param name="isPlayerPresent"></param>
        public virtual void SwitchStage(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            if (CurrentStageName != Flags.StagePlayerIn)
            {
                RemoveEntityPhysics();

            }

            LoadToNewStage(newStageName, tileManager, itemManager);

        }
        public void StartWarp(string stageTo, Vector2 positionTo, TileManager tileManager, ItemManager itemManager, Direction directionToFace)
        {
            TileManager = tileManager;
            _warpHelper.StartWarp(Animator, stageTo, positionTo, directionToFace);
        }
        internal void LoadAnimations(Animator animator)
        {
            Animator = animator;
            Animator.Load(SoundModuleManager, this, Position);
        }

        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.NPC },
                new List<Category>() { Category.Solid, Category.Grass, Category.TransparencySensor, Category.Item, Category.Portal, Category.NPC }, OnCollides, OnSeparates,ignoreGravity:true, blocksLight: true, userData: this);

            BigSensorCollidesWithCategories = new List<Category>() { Category.Item, Category.Portal, Category.Solid };
            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 16f, new List<Category>() { Category.PlayerBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);

        }
        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
            //Collision logic changes based on current behaviour!
            Behaviour.OnCollides(fixtureA, fixtureB, contact);
            if (fixtureB.CollisionCategories.HasFlag(Category.Item) && fixtureA.CollisionCategories.HasFlag(Category.Player))
            {
                WorldItem worldItem = (fixtureB.Body.UserData as WorldItem);
                InventoryHandler.GiveItem(worldItem);
            }
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        public override void CleanUp()
        {
            base.CleanUp();
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
        protected virtual void UpdateBehaviour(GameTime gameTime)
        {
            if (!ForceStop)
                Behaviour.Update(gameTime, ref Velocity);
        }
        public virtual new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateBehaviour(gameTime);
            StatusIcon.Update(gameTime, Position);

            IsMoving = ((Velocity != Vector2.Zero));

            if (IsMoving && !ForceStop)
                DirectionMoving = UpdateDirection();


            MainHullBody.Body.LinearVelocity = Velocity * Speed * (float)gameTime.ElapsedGameTime.Milliseconds;

            //Position is changed so that our sprite knows where to draw, and position
            //snaps to where the physics system moved the main hullbody
            //Big sensor position just sticks on top of our main hullbody
            BigSensor.Position = Position;


            Animator.Update(gameTime, IsMoving, Position, DirectionMoving);

            if (_warpHelper.IsWarping)
                CheckOnWarpStatus();
        }

        public void ForceWarpTo(string stageTo, Vector2 position, TileManager tileManager, ItemManager itemManager)
        {
           Move(position);
            SwitchStage(stageTo, tileManager, itemManager);
            FaceDirection(Direction.Down);
        }
        private void CheckOnWarpStatus()
        {
            //if is warping and animator is now transparent, it means we're now ready to actually warp
            if (IsInStage)
            {
                if (Animator.IsTransparent())
                {
                    if (_warpHelper.FinishWarpAndFinalMove(Animator, TileManager, InventoryHandler.ItemManager))
                        RestoreEntityPhysics();
                    else
                        RemoveEntityPhysics();
                }
            }
            else
            {
                if (_warpHelper.FinishWarpAndFinalMove(Animator, TileManager, InventoryHandler.ItemManager))
                    RestoreEntityPhysics();
                else
                    RemoveEntityPhysics();
            }
            

        }

        public bool IsInSameStageAs(Entity otherEntity)
        {
            return CurrentStageName == otherEntity.CurrentStageName;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch);
#if DEBUG
            if (Flags.ShowEntityPaths)
                Behaviour.DrawDebug(spriteBatch);
#endif
        }

        public virtual void DrawDebug(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextFactory.DefaultFont, $"{Name} \n {CurrentStageName}",
                CenteredPosition, Color.Red, 0f, Vector2.Zero, .5f, SpriteEffects.None, .99f);
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
                BigSensor.Body.SetCollisionCategory(category);
        }

        /// <summary>
        /// Was previously not in player stage, now is. Activate main body and allow click interactions.
        /// </summary>
        protected virtual void RestoreEntityPhysics()
        {
            MainHullBody.Body.IsSensor = false;
            AddBigSensorCat(Category.Cursor);
            AddBigSensorCat(Category.TransparencySensor);

        }
        /// <summary>
        /// Was previously in player stage, no longer is. Disable collisions and remove click interactions.
        /// </summary>
        protected virtual void RemoveEntityPhysics()
        {
            MainHullBody.Body.IsSensor = true;
            //Shouldn't be able to click on entity when not in same stage.
            RemoveBigSensorCat(Category.Cursor);
            RemoveBigSensorCat(Category.TransparencySensor);

        }
        /// <summary>
        /// This should be called whenever a player switches stages. If this entity exists in the stage being switched TO, then its body should be loaded
        /// If the player is switching AWAY from this entity's stage, it should unload its body instead.s
        /// </summary>
        /// <param name="newStage"></param>
        /// <param name="fullyUnload">Characters which should have their positions tracked even when not present on the current stage should not unload their body
        /// instead they should just turn their bodies into sensors</param>
        /// 
        public void PlayerSwitchedStage(string newStage, bool fullyUnload = true)
        {
            IsInStage = CurrentStageName == newStage;

            if (IsInStage)
            {
                if (MainHullBody == null)
                    CreateBody(Position);
                else
                    RestoreEntityPhysics();
            }
            else if (MainHullBody != null)
            {
                if (fullyUnload)
                    CleanUp();

                else
                    RemoveEntityPhysics();
            }
        }

        protected virtual void LoadToNewStage(string newStage, TileManager tileManager, ItemManager itemManager)
        {
            CurrentStageName = newStage;
            Navigator.Unload();

            Navigator.Load(tileManager.PathGrid);
            TileManager = tileManager;
            Behaviour.SwitchStage(TileManager);
            InventoryHandler.SwapItemManager(itemManager);
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
            if (IsInStage)
            {
                string soundName = TileManager.GetStepSoundFromPosition(Position);
                if (string.IsNullOrEmpty(soundName))
                    return;

                SoundModuleManager.PlaySpecificSound(soundName);
            }
        }

        //Items

        public void GiveItem(WorldItem item) => InventoryHandler.GiveItem(item);
        public void GiveItem(Item item, ref int count) => InventoryHandler.GiveItem(item, ref count);

        public void GiveItem(string name, int count) => InventoryHandler.GiveItem(name, count);

        public void DropItem(string name, int count) => InventoryHandler.DropItem(Position, Vector2Helper.GetTossDirectionFromDirectionFacing(DirectionMoving), name, count);
        public void DropItem(Item item, int count) => InventoryHandler.DropItem(Position, Vector2Helper.GetTossDirectionFromDirectionFacing(DirectionMoving), item, count);


        protected virtual void DropCurrentlyHeldItemToWorld()
        {
            InventoryHandler.ItemManager.AddWorldItem(new Vector2(Position.X, Position.Y - YOffSet / 2), UI.Cursor.HeldItem, UI.Cursor.HeldItemCount, Vector2Helper.GetTossDirectionFromDirectionFacing(DirectionMoving));

        }
        

        internal void InteractWithTile(Point tilePoint, Layers tileLayer)
        {
            Tile tile = TileManager.GetTileFromPoint(tilePoint, tileLayer);
            if (tile == null)
                throw new Exception($"No tile at {tilePoint} at layer {tileLayer.ToString()}!");
            tile.Interact(false);
            
        }
        protected void PerformAction(ActionType actionType)
        {
            Animator.PerformAction(DirectionMoving, actionType);
        }

        internal bool IsFacingTowardsOtherEntity(Vector2 otherEntityPos)
        {
            if (DirectionMoving == Vector2Helper.GetDirectionOfEntityInRelationToEntity(Position, otherEntityPos))
                return true;
            return false;
        }
        internal void FaceTowardsOtherEntity(Vector2 otherEntityPos)
        {
            Direction directionToFace = Vector2Helper.GetDirectionOfEntityInRelationToEntity(Position, otherEntityPos);
            FaceDirection(directionToFace);
        }

        public void FaceDirection(Direction directionToFace)
        {
            Animator.ChangeDirection(directionToFace, Position);
            DirectionMoving = directionToFace;
        }

        public virtual void Save(BinaryWriter writer)
        {
            if (CurrentStageName != null)
                writer.Write(CurrentStageName);
            else
                writer.Write("LullabyTown");

            Vector2Helper.WriteVector2(writer, Position);

            InventoryHandler.Save(writer);
        }
        public virtual void LoadSave(BinaryReader reader)
        {
            //throw new NotImplementedException();
            CurrentStageName = reader.ReadString();
            Move(Vector2Helper.ReadVector2(reader));

            InventoryHandler.LoadSave(reader);
        }

        
    }
}