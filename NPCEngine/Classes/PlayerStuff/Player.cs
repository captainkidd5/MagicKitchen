using EntityEngine.Classes.HumanoidCreation;
using EntityEngine.Classes.NPCStuff;
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
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.PlayerStuff
{
    public class Player : HumanoidEntity
    {
        public byte Id { get; private set; }
        public readonly Vector2 StartingPosition = new Vector2(440, 700);

        private Direction DirectionFacing { get; set; }
        private Direction SecondaryDirectionFacing { get; set; }
        private bool WasMovingLastFrame { get; set; }


        public Light TestLight { get; set; }
        public Player(GraphicsDevice graphics, ContentManager content, string name = "playerName") : base(graphics,content)
        {
            Name = name;
            Move(StartingPosition);
            StorageCapacity = 24;
            
        }

        public override void LoadContent(ContentManager content, TileManager tileManager, ItemManager itemManager)
        {
            base.LoadContent(content,  tileManager,  itemManager);
            IsInStage = true;
        }

        protected override void CreateBody(Vector2 position)
        {
            AddPrimaryBody(PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.Player },
            new List<Category>() { Category.Solid, Category.Grass, Category.TransparencySensor, Category.Item, Category.Portal }, OnCollides, OnSeparates,blocksLight:true, userData: this));


            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position, 16f, new List<Category>() { Category.PlayerBigSensor }, new List<Category>() { Category.Item, Category.Portal, Category.Solid, Category.PlayerBigSensor },
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);
            AddSecondaryBody(BigSensor);
            // MainHullBody = HullBodies[0];

        }



        public override void RestoreEntityPhysics()
        {
            //base.AddedToPlayerStage();
        }

        public override void RemoveEntityPhysics()
        {
            //base.RemovedFromPlayerStage();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        //Keep input updates separate from update because of multiplayer.
        public bool UpdateFromInput()
        {
            Velocity = Vector2.Zero;
            DirectionFacing = Controls.DirectionFacing;
            SecondaryDirectionFacing = Controls.SecondaryDirectionFacing;
            IsMoving = Controls.IsPlayerMoving;
            GetPlayerMovementDirectionAndVelocity(DirectionFacing);
            GetPlayerMovementDirectionAndVelocity(SecondaryDirectionFacing);

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
