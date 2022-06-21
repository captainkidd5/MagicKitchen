using EntityEngine.Classes.HumanoidCreation;
using EntityEngine.Classes.CharacterStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes;
using UIEngine.Classes;

using static DataModels.Enums;
using System.IO;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using tainicom.Aether.Physics2D.Dynamics;
using Globals.Classes.Console;
using ItemEngine.Classes.ToolStuff;
using SpriteEngine.Classes.ShadowStuff;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace EntityEngine.Classes.PlayerStuff
{
    public class Player : HumanoidEntity
    {
        public byte Id { get; private set; }
        public readonly Vector2 StartingPosition = Vector2Helper.GetWorldPositionFromTileIndex(126, 134);
        private readonly PlayerManager _playerContainer;

        private bool WasMovingLastFrame { get; set; }


        internal ProgressManager ProgressManager { get; set; }

        protected HullBody FrontalSensor { get; set; }

        private LumenHandler _lumenHandler;

        public int MaxLumens => _lumenHandler.MaxLumens;
        public int CurrentLumens => _lumenHandler.CurrentLumens;
        public bool Illuminated => _lumenHandler.Illuminated;

        protected HullBody LightSensor { get; set; }





        public Player(StageNPCContainer container, GraphicsDevice graphics, ContentManager content, PlayerManager playerContainer, string name = "playerName") : base(container, graphics, content)
        {
            Name = name;
            ScheduleName = "player1";
            Move(StartingPosition);
            StorageCapacity = 24;
            XOffSet = 0;
            YOffSet = 8;
            _playerContainer = playerContainer;
            InventoryHandler = new PlayerInventoryHandler(StorageCapacity);
            ProgressManager = new ProgressManager();
        }
        public override void SwitchStage(string newStageName, TileManager tileManager, ItemManager itemManager)
        {
            Flags.StagePlayerIn = newStageName;
            CurrentStageName = newStageName;
            base.SwitchStage(newStageName, tileManager, itemManager);
            // _playerContainer.PlayerSwitchedStage(newStageName);
        }
        public override void LoadContent(Vector2? startPos, string? name, bool standardAnimator = false)
        {
            base.LoadContent(startPos, name, standardAnimator);
            ProgressManager.LoadContent();
            UI.LoadPlayerInventory(StorageContainer);
            UI.LoadPlayerUnlockedRecipes(ProgressManager.UnlockedRecipes);

            UI.Cursor.ItemDropped -= DropHeldItem;

            UI.Cursor.ItemDropped += DropHeldItem;

            CommandConsole.RegisterCommand("tp", "teleports player to mouse position", TpCommand);

            ToolHandler = new PlayerToolHandler(this, InventoryHandler, _lumenHandler);
            _lumenHandler.Load();
        }
        private void TpCommand(string[] args)
        {
            Move(Controls.MouseWorldPosition);
        }

        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.Player },
            new List<Category>() { (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh,  (Category)PhysCat.Grass, (Category)PhysCat.NPC, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item,
                (Category)PhysCat.NPCBigSensor, (Category)PhysCat.Portal}, OnCollides, OnSeparates, ignoreGravity: true, blocksLight: true, userData: this));


            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() {
                (Category)PhysCat.PlayerBigSensor }, new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.Tool,(Category)PhysCat.Portal,
                    (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh,  (Category)PhysCat.NPC, (Category)PhysCat.LightSource },
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);

            FrontalSensor = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, position, 16f, 16f, new List<Category>() {
                (Category)PhysCat.FrontalSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(FrontalSensor);



            LightSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() {
                (Category)PhysCat.PlayerBigSensor }, new List<Category>() { (Category)PhysCat.LightSource },
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(LightSensor);
            
            _lumenHandler = new LumenHandler(LightSensor);
            //AddLight(LightType.Warm,new Vector2(XOffSet * -1, YOffSet * -1), 1f);
        }

        /// <summary>
        /// Frontal sensor needs to be a tile's distance in front of the entity, which depends on which
        /// direction the entity is facing
        /// </summary>
        /// <returns></returns>
        protected Vector2 GetFrontalSensorPositionFromEntityDirection()
        {
            switch (DirectionMoving)
            {
                case Direction.None:
                    return new Vector2(0, 16);

                case Direction.Up:
                    return new Vector2(0, -16);
                case Direction.Down:
                    return new Vector2(0, 16);

                case Direction.Left:
                    return new Vector2(-16, 0);

                case Direction.Right:
                    return new Vector2(16, 0);
                default: throw new Exception($"Invalid direction");
            }
        }

        protected override void RestoreEntityPhysics()
        {
            //base.AddedToPlayerStage();
        }

        protected override void RemoveEntityPhysics()
        {
            //base.RemovedFromPlayerStage();
        }
        protected override void UpdateBehaviour(GameTime gameTime)
        {
            // base.UpdateBehaviour(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            Resume();
            base.Update(gameTime);
            Shared.PlayerPosition = Position;
            UI.StatusPanel.HealthBar.SetProgressRatio((float)CurrentHealth / (float)MaxHealth);
            UI.StatusPanel.ManaBar.SetProgressRatio((float)CurrentLumens / (float)MaxLumens);
            if (Controls.IsClickedWorld || Controls.WasGamePadButtonTapped(GamePadActionType.Y))
            {
                //Item should not eject if any part of the ui is hovered
                if (UI.Cursor.IsHoldingItem)
                {
                    DropHeldItem();
                }
                else
                {
                    UseHeldItem();
                }
            }

            _lumenHandler.HandleLumens(gameTime);

            if (UI.TalkingWindow.IsActive)
            {
                //EntityAnimator.

                Halt(true);
                DirectionMoving = UI.TalkingWindow.DirectionPlayerShouldFace;
                if (UI.TalkingWindow.WasJustActivated)
                    Animator.ChangeDirection(DirectionMoving, Position);
            }
            //else
            //    Resume();


            FrontalSensor.Position = Position + GetFrontalSensorPositionFromEntityDirection();
            Controls.PlayerFrontalSensorPosition = FrontalSensor.Position;

            SelectItem(UI.PlayerCurrentSelectedItem);
        }


        protected override void CheckOnWarpStatus()
        {
            if (CurrentStageName != Flags.StagePlayerIn)
                base.CheckOnWarpStatus();
        }
        private void DropHeldItem()
        {
            DropCurrentlyHeldItemToWorld();
            Controls.ClickActionTriggeredThisFrame = true;
        }

        protected override void DropCurrentlyHeldItemToWorld()
        {
            InventoryHandler.ItemManager.AddWorldItem(new Vector2(Position.X, Position.Y - YOffSet / 2),
                UI.Cursor.HeldItem, UI.Cursor.HeldItemCount, WorldItemState.Bouncing,
                Vector2Helper.GetTossDirectionFromDirectionFacing(DirectionMoving));

            UI.Cursor.HeldItemCount = 0;
            UI.Cursor.HeldItem = null;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        //Keep input updates separate from update because of multiplayer.
        public bool UpdateFromInput()
        {
            if (!ForceStop)
            {

                Velocity = Vector2.Zero;
                Direction secondaryDirection = Controls.SecondaryDirectionFacing;
                IsMoving = Controls.IsPlayerMoving;
                GetPlayerMovementDirectionAndVelocity(Controls.DirectionFacing);
                GetPlayerMovementDirectionAndVelocity(secondaryDirection);
            }
      

            return (WasMovingLastFrame != IsMoving); //This frame's movement is different from last frames.
        }

        private void GetPlayerMovementDirectionAndVelocity(Direction direction)
        {

            switch (direction)
            {
                case Direction.Right:
                    Velocity.X = Speed;
                    DirectionMoving = Direction.Right;
                    break;

                case Direction.Left:
                    Velocity.X = -Speed;
                    DirectionMoving = Direction.Left;
                    break;

                case Direction.Down:
                    Velocity.Y = Speed;
                    DirectionMoving = Direction.Down;
                    break;

                case Direction.Up:
                    Velocity.Y = -Speed;
                    DirectionMoving = Direction.Up;
                    break;

                default:
                    break;

            }
        }

        public override void CleanUp()
        {
            base.CleanUp();
            ProgressManager.CleanUp();
        }

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            ProgressManager.Save(writer);
            _lumenHandler.Save(writer);
        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
            ProgressManager.LoadSave(reader);
            _lumenHandler.LoadSave(reader);
        }

        protected override void LoadToNewStage(string newStage, TileManager tileManager, ItemManager itemManager)
        {
            Navigator.Unload();


            TileManager = tileManager;

            Navigator.Load(TileManager.PathGrid);
            InventoryHandler.SwapItemManager(itemManager);
        }
    }
}
