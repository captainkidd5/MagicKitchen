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
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static DataModels.Enums;
using System.IO;
using Globals.Classes.Helpers;

namespace EntityEngine.Classes.PlayerStuff
{
    public class Player : HumanoidEntity
    {
        public byte Id { get; private set; }
        public readonly Vector2 StartingPosition = Vector2Helper.GetWorldPositionFromTileIndex(64,72);
        private readonly PlayerManager _playerContainer;

        private Direction DirectionFacing { get; set; }
        private Direction SecondaryDirectionFacing { get; set; }
        private bool WasMovingLastFrame { get; set; }

        public Light TestLight { get; set; }

        internal ProgressManager ProgressManager { get;set; }
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


        }

        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.Player },
            new List<Category>() { Category.Solid, Category.Grass,Category.NPC, Category.TransparencySensor, Category.Item, Category.NPCBigSensor, Category.Portal }, OnCollides, OnSeparates, ignoreGravity:true,blocksLight:true, userData: this));


            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() { Category.PlayerBigSensor }, new List<Category>() { Category.Item, Category.Portal,Category.Solid, Category.NPC },
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);
            // MainHullBody = HullBodies[0];

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
            if (Controls.IsClickedWorld)
            {
                //Item should not eject if any part of the ui is hovered
                if (UI.Cursor.HeldItem != null)
                {
                    DropCurrentlyHeldItemToWorld();
                    Controls.ClickActionTriggeredThisFrame = true;
                }
            }

            if (UI.TalkingWindow.IsActive)
            {
                //EntityAnimator.
                Halt(true);
                DirectionFacing = UI.TalkingWindow.DirectionPlayerShouldFace;
            }
            else
                Resume();
            if (UI.TalkingWindow.WasJustActived)
                Animator.ChangeDirection(DirectionFacing, Position);
        }

        protected override void DropCurrentlyHeldItemToWorld()
        {
            base.DropCurrentlyHeldItemToWorld();
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
            DirectionFacing = Controls.DirectionFacing;
            SecondaryDirectionFacing = Controls.SecondaryDirectionFacing;
            IsMoving = Controls.IsPlayerMoving;
            GetPlayerMovementDirectionAndVelocity(DirectionFacing);
            GetPlayerMovementDirectionAndVelocity(SecondaryDirectionFacing);
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
            //BehaviourManager.SwitchStage(TileManager);
            InventoryHandler.SwapItemManager(itemManager);
        }
    }
}
