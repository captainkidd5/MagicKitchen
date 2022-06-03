using EntityEngine.Classes.Animators;
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
using static Globals.Classes.Settings;
using static DataModels.Enums;
using EntityEngine.Classes.ScriptStuff;
using DataModels.ScriptedEventStuff;
using SpriteEngine.Classes.Presets;
using ItemEngine.Classes.StorageStuff;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

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
        protected float StartingSpeed { get; set; } = 3f;
        protected Vector2 Velocity;
        internal float Speed { get; set; }
        protected int StorageCapacity { get; set; }
        public Direction DirectionMoving { get; set; }
        public bool IsMoving { get; protected set; }
        protected bool ForceStop { get; private set; }
        //Entity gets the data representation of inventory while ui handles actual visuals
        public string CurrentStageName { get; protected set; }
        internal Animator Animator { get; set; }
        protected Navigator Navigator { get; set; }

        public string TargetStage { get; internal protected set; }
        internal TileManager TileManager { get; set; }
        public string Name { get; protected set; }
        internal string ScheduleName { get; set; }
        protected StatusIcon StatusIcon { get; set; }

        public bool IsUsingProgressBar => ProgressBarSprite != null;
        protected ProgressBarSprite ProgressBarSprite { get; private set; }

        /// <summary>
        /// If entity is present at the current stage
        /// </summary>
        public bool IsInStage
        {
            get;
            set;
        }
        BehaviourManager BehaviourManager { get; set; }

        private protected InventoryHandler InventoryHandler { get; set; }
        public StorageContainer StorageContainer => InventoryHandler.StorageContainer;

        //warp
        private WarpHelper _warpHelper;

        public Point TileOn => Vector2Helper.GetTileIndexPosition(Position);
        public bool IsWarping => _warpHelper.IsWarping;

        protected StageNPCContainer Container { get; }

        private OverheadItemDisplay _overHeadItemDisplay { get; set; }
        public Entity(StageNPCContainer container, GraphicsDevice graphics, ContentManager content) : base()
        {
            _graphics = graphics;
            _content = content;
            Name = GetType().ToString();

            StorageCapacity = 4;
            Navigator = new Navigator(Name);
            Speed = StartingSpeed;

            _warpHelper = new WarpHelper(this);
            InventoryHandler = new InventoryHandler(StorageCapacity);
            Container = container;

        }
        public void SelectItem(Item item) => _overHeadItemDisplay.SelectItem(item, Position);
        public bool IsProgressComplete() => ProgressBarSprite.Done;

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
        public void InjectScript(SubScript subscript) => BehaviourManager.InjectScript(subscript);

        internal virtual void ChangeSkinTone(Color newSkinTone)
        {

        }
        internal virtual void ChangeClothingColor(Type t, Color color)
        {

        }
        public virtual void LoadContent()
        {
            StatusIcon = new StatusIcon(new Vector2(XOffSet, YOffSet));

            CreateBody(Position);

            DirectionMoving = Direction.Down;
            TargetStage = CurrentStageName;
            BehaviourManager = new BehaviourManager(this, StatusIcon, Navigator, TileManager);
            BehaviourManager.Load();
            _overHeadItemDisplay = new OverheadItemDisplay();



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
                IsInStage = false;
                RemoveEntityPhysics();

            }
            else
            {
                IsInStage = true;
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
            if (MainHullBody == null)
                MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.NPC },
                    new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.Solid, (Category)PhysCat.Grass, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Portal }, OnCollides, OnSeparates, ignoreGravity: true, blocksLight: true, userData: this);

            BigSensorCollidesWithCategories = new List<Category>() { (Category)PhysCat.NPC, (Category)PhysCat.Player, (Category)PhysCat.Solid };
            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 16f, new List<Category>() { (Category)PhysCat.PlayerBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);



        }


        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //Collision logic changes based on current behaviour!
            BehaviourManager.OnCollides(fixtureA, fixtureB, contact);
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.Item) && fixtureA.CollisionCategories.HasFlag((Category)PhysCat.Player))
            {
                WorldItem worldItem = (fixtureB.Body.Tag as WorldItem);
                InventoryHandler.GiveItem(worldItem);
            }
            return base.OnCollides(fixtureA, fixtureB, contact);

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
                BehaviourManager.Update(gameTime, ref Velocity);
        }
        public virtual new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateBehaviour(gameTime);
            StatusIcon.Update(gameTime, Position);

            if (ProgressBarSprite != null)
                ProgressBarSprite.Update(gameTime);

            IsMoving = ((Velocity != Vector2.Zero));

            if (IsMoving && !ForceStop)
                DirectionMoving = UpdateDirection();


            MainHullBody.Body.LinearVelocity = Velocity * Speed * (float)gameTime.ElapsedGameTime.Milliseconds;

            //Position is changed so that our sprite knows where to draw, and position
            //snaps to where the physics system moved the main hullbody
            //Big sensor position just sticks on top of our main hullbody
            BigSensor.Position = Position;


            Animator.Update(gameTime, IsMoving, Position);

            _overHeadItemDisplay.Update(gameTime, Position, LayerDepth);


            if (_warpHelper.IsWarping)
                CheckOnWarpStatus();
        }

        public void ForceWarpTo(string stageTo, Vector2 position, TileManager tileManager, ItemManager itemManager)
        {
            Move(position);
            SwitchStage(stageTo, tileManager, itemManager);
            FaceDirection(Direction.Down);
        }
        protected virtual void CheckOnWarpStatus()
        {

            if (_warpHelper.FinishWarpAndFinalMove(Animator, TileManager, InventoryHandler.ItemManager))
                RestoreEntityPhysics();
            else
                RemoveEntityPhysics();



        }

        public bool IsInSameStageAs(Entity otherEntity)
        {
            return CurrentStageName == otherEntity.CurrentStageName;
        }

        public bool IsInPlayerStage()
        {
            return CurrentStageName == Flags.StagePlayerIn;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            StatusIcon.Draw(spriteBatch);

            if (ProgressBarSprite != null)
                ProgressBarSprite.Draw(spriteBatch);

            Animator.Draw(spriteBatch);

            _overHeadItemDisplay.Draw(spriteBatch);

#if DEBUG
            if (Flags.ShowEntityPaths)
                BehaviourManager.DrawDebug(spriteBatch);
#endif
        }

        public virtual void DrawDebug(SpriteBatch spriteBatch)
        {
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
            // CurrentStageName = newStage;
            Navigator.Unload();

            Navigator.Load(Container.GetPathGrid(newStage));
            TileManager = tileManager;

            BehaviourManager.SwitchStage(TileManager);
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
            InventoryHandler.ItemManager.AddWorldItem(new Vector2(Position.X, Position.Y - YOffSet / 2), InventoryHandler.HeldItem, InventoryHandler.HeldItemCount, Vector2Helper.GetTossDirectionFromDirectionFacing(DirectionMoving));

        }


        internal void InteractWithTile(Point tilePoint, Layers tileLayer)
        {
            Tile tile = TileManager.GetTileFromPoint(tilePoint, tileLayer);
            if (tile == null)
                throw new Exception($"No tile at {tilePoint} at layer {tileLayer.ToString()}!");
            tile.Interact(false, InventoryHandler.HeldItem);

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
            writer.Write(ScheduleName);
        }
        public virtual void LoadSave(BinaryReader reader)
        {
            //throw new NotImplementedException();
            CurrentStageName = reader.ReadString();
            Move(Vector2Helper.ReadVector2(reader));

            InventoryHandler.LoadSave(reader);
            ScheduleName = reader.ReadString();
        }


    }
}
