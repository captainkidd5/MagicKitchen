using EntityEngine.Classes.BehaviourStuff;
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
using SoundEngine.Classes;
using EntityEngine.Classes.ToolStuff;
using EntityEngine.ItemStuff;
using TiledEngine.Classes.TileAddons;
using DataModels;
using SpriteEngine.Classes.Animations.EntityAnimations;

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
        public float BaseSpeed { get; protected set; } = 3f;
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


        BehaviourManager BehaviourManager { get; set; }

        private protected InventoryHandler InventoryHandler { get; set; }
        public StorageContainer StorageContainer => InventoryHandler.StorageContainer;



        public Point TileOn => Vector2Helper.GetTileIndexPosition(Position);


        protected EntityContainer Container { get; set; }

        private OverheadItemDisplay _overHeadItemDisplay { get; set; }

        internal ToolHandler ToolHandler { get; set; }
        public byte MaxHealth { get; protected set; } = 100;
        public byte CurrentHealth { get; protected set; } = 100;

        public bool FlaggedForRemoval { get; set; }


        public Entity(GraphicsDevice graphics, ContentManager content) : base()
        {
            _graphics = graphics;
            _content = content;
            Name = GetType().ToString();

            StorageCapacity = 4;
            Navigator = new Navigator(Name);
            Speed = BaseSpeed;

            InventoryHandler = new InventoryHandler(StorageCapacity);

        }


        public void TakeDamage(int amt)
        {
            int newHealth = CurrentHealth - amt;
            if (newHealth <= 0)
                DestructionBehaviour();
            else
                CurrentHealth = (byte)newHealth;

        }

        protected virtual void DestructionBehaviour()
        {
            FlaggedForRemoval = true;
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
        public virtual void LoadContent(EntityContainer container)
        {
            Container = container;
            StatusIcon = new StatusIcon(new Vector2(XOffSet, YOffSet));

            CreateBody(Position);

            DirectionMoving = Direction.Down;
            BehaviourManager = new BehaviourManager(this, StatusIcon, Navigator, Container.TileManager);
            BehaviourManager.Load();
            _overHeadItemDisplay = new OverheadItemDisplay();

            ToolHandler = new ToolHandler(this, InventoryHandler);
            Navigator.Load(container.TileManager.PathGrid);

            BehaviourManager.SwitchStage(Container.TileManager);
            InventoryHandler.SwapItemManager(Container.ItemManager);
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
                        (Category)PhysCat.Grass, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Portal,(Category)PhysCat.Tool }, OnCollides, OnSeparates, ignoreGravity: true, blocksLight: true, userData: this);

            BigSensorCollidesWithCategories = new List<Category>() { (Category)PhysCat.NPC, (Category)PhysCat.Player, (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh };
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
        protected virtual void UpdateBehaviour(GameTime gameTime)
        {
            if (!ForceStop)
                BehaviourManager.Update(gameTime, ref Velocity);
        }
        public bool Submerged { get; private set; }
        public virtual new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Submerged = false;
            Speed = BaseSpeed;
            if (Container.TileManager.IsTypeOfTile("water", Position))
            {
                Submerged = true;
                Speed = BaseSpeed * .5f;

            }
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

            //If submerged, draw sprite much lower than if on land, to better create illusion
            if (Submerged)
            {
                Animator.Update(gameTime, DirectionMoving, IsMoving, new Vector2(Position.X, Position.Y + 14), Speed / BaseSpeed);

            }
            else
            {
                Animator.Update(gameTime, DirectionMoving, IsMoving, Position, Speed / BaseSpeed);

            }

            _overHeadItemDisplay.Update(gameTime, Position, LayerDepth);
            ToolHandler.Update(gameTime);

        }

        protected void UseHeldItem() => ToolHandler.UseHeldItem();

        protected void ChargeHeldItem(GameTime gameTime, Vector2 aimPosition) => ToolHandler.ChargeHeldItem(gameTime, aimPosition);

        protected virtual void ActivateTool(Tool tool) => ToolHandler.ActivateTool(tool);




        protected virtual void DrawAnimator(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, Submerged);




        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            StatusIcon.Draw(spriteBatch);

            if (ProgressBarSprite != null)
                ProgressBarSprite.Draw(spriteBatch);

            Animator.Draw(spriteBatch, Submerged);
            DrawAnimator(spriteBatch);
            _overHeadItemDisplay.Draw(spriteBatch);

#if DEBUG
            if (Flags.ShowEntityPaths)
                BehaviourManager.DrawDebug(spriteBatch);
#endif

            ToolHandler.Draw(spriteBatch);
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

            string soundName = Container.TileManager.GetStepSoundFromPosition(Position);
            if (string.IsNullOrEmpty(soundName))
                return;

            SoundModuleManager.PlaySpecificSound(soundName);

        }

        //Items

        public void GiveItem(WorldItem item) => InventoryHandler.GiveItem(item);
        public void GiveItem(Item item, ref int count) => InventoryHandler.GiveItem(item, ref count);

        public void GiveItem(string name, int count) => InventoryHandler.GiveItem(name, count);

        public void DropItem(string name, int count) => InventoryHandler.DropItem(Position, Vector2Helper.GetTossDirectionFromDirectionFacing(DirectionMoving), name, count);
        public void DropItem(Item item, int count) => InventoryHandler.DropItem(Position, Vector2Helper.GetTossDirectionFromDirectionFacing(DirectionMoving), item, count);


        protected virtual void DropCurrentlyHeldItemToWorld()
        {
            InventoryHandler.ItemManager.AddWorldItem(new Vector2(Position.X, Position.Y - YOffSet / 2), InventoryHandler.HeldItem, InventoryHandler.HeldItemCount, WorldItemState.Bouncing, Vector2Helper.GetTossDirectionFromDirectionFacing(DirectionMoving));

        }


        internal void InteractWithTile(Point tilePoint, Layers tileLayer)
        {
            TileData? tile = Container.TileManager.GetTileDataFromPoint(tilePoint, tileLayer);
            if (tile == null)
                throw new Exception($"No tile at {tilePoint} at layer {tileLayer.ToString()}!");
            ActionType? actionType = Container.TileManager.TileObjects[tile.Value.GetKey()].Interact(false, InventoryHandler.HeldItem, CenteredPosition, DirectionMoving);
            if (actionType != null && IsJumpActionType(actionType.Value))
            {
                ReactToJumpActionType(actionType.Value);
            }

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

            if (Container.TileManager.IsTypeOfTile("water", Position))
            {
                SoundModuleManager.PlayPackage("Splash");
            }

        }
        protected void PerformAction(Direction direction, ActionType actionType)
        {
            Animator.PerformAction(direction, actionType);
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


            Vector2Helper.WriteVector2(writer, Position);
            writer.Write(StorageCapacity);
            InventoryHandler.Save(writer);
            writer.Write(ScheduleName);
        }
        public virtual void LoadSave(BinaryReader reader)
        {
            Move(Vector2Helper.ReadVector2(reader));
            StorageCapacity = reader.ReadByte();

            InventoryHandler = new InventoryHandler(StorageCapacity);

            InventoryHandler.LoadSave(reader);
            ScheduleName = reader.ReadString();
        }
        public override void CleanUp()
        {
            base.CleanUp();
            Speed = BaseSpeed;
            StorageCapacity = 4;
            InventoryHandler.CleanUp();
            CurrentHealth = MaxHealth;
        }
        public virtual void SetToDefault()
        {
            CleanUp();
        }
    }
}
