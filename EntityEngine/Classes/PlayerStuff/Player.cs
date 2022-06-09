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
using PhysicsEngine.Classes.Prefabs;

namespace EntityEngine.Classes.PlayerStuff
{
    public class Player : HumanoidEntity
    {
        public byte Id { get; private set; }
        public readonly Vector2 StartingPosition = Vector2Helper.GetWorldPositionFromTileIndex(126,134);
        private readonly PlayerManager _playerContainer;

        private bool WasMovingLastFrame { get; set; }


        internal ProgressManager ProgressManager { get;set; }

        protected HullBody FrontalSensor { get; set; }
        private Hook _hook;
        public Player(StageNPCContainer container, GraphicsDevice graphics, ContentManager content,PlayerManager playerContainer, string name = "playerName") : base(container, graphics,content)
        {
            Name = name;
            ScheduleName = "player1";
            Move(StartingPosition);
            StorageCapacity = 24;
            XOffSet = 8;
            YOffSet = 16;
            _playerContainer = playerContainer;
            InventoryHandler = new InventoryHandler(StorageCapacity);
            ProgressManager = new ProgressManager();
        }
        public override void SwitchStage(string newStageName,  TileManager tileManager, ItemManager itemManager)
        {
            Flags.StagePlayerIn = newStageName;
            CurrentStageName = newStageName;
            base.SwitchStage(newStageName,tileManager, itemManager);
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

            CommandConsole.RegisterCommand("Hook", "Creates hook at mouse location", CreateHookCommand);


        }
        private void CreateHookCommand(string[] args)
        {
            _hook = PhysicsManager.CreateHook(Controls.MouseWorldPosition);
        }
        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.Player },
            new List<Category>() { (Category)PhysCat.Solid, (Category)PhysCat.Grass, (Category)PhysCat.NPC, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item,
                (Category)PhysCat.NPCBigSensor, (Category)PhysCat.Portal }, OnCollides, OnSeparates, ignoreGravity:true,blocksLight:true, userData: this));


            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() {
                (Category)PhysCat.PlayerBigSensor }, new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.Portal,
                    (Category)PhysCat.Solid, (Category)PhysCat.NPC },
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);

            FrontalSensor = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, position, 16f, 16f, new List<Category>() {
                (Category)PhysCat.FrontalSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(FrontalSensor);
            // MainHullBody = HullBodies[0];

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
            base.Update(gameTime);
            UI.Cursor.PlayerPosition = Position;
            if (Controls.IsClickedWorld || Controls.WasGamePadButtonTapped(GamePadActionType.Y))
            {
                //Item should not eject if any part of the ui is hovered
                if (UI.Cursor.IsHoldingItem)
                {
                    DropHeldItem();
                }
            }
            if (_hook != null)
                _hook.Update(gameTime);
            if (UI.TalkingWindow.IsActive)
            {
                //EntityAnimator.
              
                Halt(true);
                DirectionMoving = UI.TalkingWindow.DirectionPlayerShouldFace;
                if (UI.TalkingWindow.WasJustActivated)
                    Animator.ChangeDirection(DirectionMoving, Position);
            }
            else
                Resume();


            FrontalSensor.Position = Position + GetFrontalSensorPositionFromEntityDirection();
            Controls.PlayerFrontalSensorPosition = FrontalSensor.Position;

            SelectItem(UI.PlayerCurrentSelectedItem);
        }
        protected override void CheckOnWarpStatus()
        {
            if(CurrentStageName != Flags.StagePlayerIn)
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
        }

        public override void LoadSave(BinaryReader reader)
        {
            base.LoadSave(reader);
            ProgressManager.LoadSave(reader);
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
