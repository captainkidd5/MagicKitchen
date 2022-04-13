﻿using EntityEngine.Classes.HumanoidCreation;
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

namespace EntityEngine.Classes.PlayerStuff
{
    public class Player : HumanoidEntity
    {
        public byte Id { get; private set; }
        public readonly Vector2 StartingPosition = new Vector2(630, 500);
        private readonly PlayerContainer _playerContainer;

        private Direction DirectionFacing { get; set; }
        private Direction SecondaryDirectionFacing { get; set; }
        private bool WasMovingLastFrame { get; set; }


        public Light TestLight { get; set; }
        public Player(GraphicsDevice graphics, ContentManager content,PlayerContainer playerContainer, string name = "playerName") : base(graphics,content)
        {
            Name = name;
            Move(StartingPosition);
            StorageCapacity = 24;
            XOffSet = 8;
            YOffSet = 16;
            _playerContainer = playerContainer;
            InventoryHandler = new InventoryHandler(StorageCapacity);

        }
        public override void SwitchStage(string newStageName,  TileManager tileManager, ItemManager itemManager)
        {
            Flags.StagePlayerIn = newStageName;
            CurrentStageName = newStageName;
            base.SwitchStage(newStageName,tileManager, itemManager);
            _playerContainer.PlayerSwitchedStage(newStageName);
        }
        public override void LoadContent(ItemManager itemManager)
        {
            base.LoadContent( itemManager);
            IsInStage = true;
            UI.LoadPlayerInventory(StorageContainer);







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
            if (Controls.IsClicked)
            {
                //Item should not eject if any part of the ui is hovered
                if (UI.Cursor.HeldItem != null && !UI.IsHovered)
                {
                    DropCurrentlyHeldItemToWorld();
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
    }
}