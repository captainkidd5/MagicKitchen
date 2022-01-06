using EntityEngine.Classes.Animators;
using EntityEngine.Classes.BehaviourStuff;
using EntityEngine.Classes.HumanoidCreation;
using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
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
using System.Text;
using TextEngine;
using TiledEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes
{


    /// <summary>
    /// Represents any character-like thing
    /// </summary>
    public abstract class Entity : Collidable
    {
        private readonly GraphicsDevice graphics;
        private readonly ContentManager content;

        protected float StartingSpeed { get; set; } = 12f;


        //Entity gets the data representation of inventory while ui handles actual visuals
        public StorageContainer StorageContainer { get; set; }

        public bool IsMoving { get; protected set; }
        protected HullBody BigSensor { get; set; }
        public string CurrentStageName { get; protected set; }

        public float Speed { get; protected set; }

        protected Vector2 Velocity;
        public Direction DirectionMoving { get; set; }

        internal Animator EntityAnimator { get; set; }

        protected Navigator Navigator { get; set; }
        protected TileManager TileManager { get; set; }

        public string Name { get; protected set; }

        protected StatusIcon StatusIcon { get; set; }

        private const float TimeBetweenWarp = 6f;
        public SimpleTimer WarpTimer { get; private set; }
        public bool AbleToWarp { get; private set; } = true;

        /// <summary>
        /// If entity is present at the current stage
        /// </summary>
        public bool IsInStage { get; set; }

        protected Behaviour Behaviour { get; set; }

        protected List<Category> BigSensorCollidesWithCategories { get; set; }
        public Entity(GraphicsDevice graphics, ContentManager content) : base()
        {
            this.graphics = graphics;
            this.content = content;
            Name = this.GetType().ToString();
            Navigator = new Navigator(Name);
            Speed = StartingSpeed;
            WarpTimer = new SimpleTimer(TimeBetweenWarp, false);
            Behaviour = new WanderBehaviour(this, StatusIcon, Navigator, null);

        }
        public void Warp(string stageTo, Vector2 positionTo, TileManager tileManager)
        {
            Move(positionTo);
            LoadToNewStage(stageTo, tileManager);
            AbleToWarp = false;
            WarpTimer.SetNewTargetTime(TimeBetweenWarp);
            WarpTimer.ResetToZero();
            //IsInStage = CurrentStageName == stageTo;

        }
        internal void LoadAnimations(Animator animator)
        {
            EntityAnimator = animator;
            EntityAnimator.Load(SoundModuleManager, this, Position);
        }

        protected override void CreateBody(Vector2 position)
        {
            //base.CreateBody(position);

            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.Player },
                new List<Category>() { Category.Solid, Category.Grass, Category.TransparencySensor, Category.Item, Category.Portal }, OnCollides, OnSeparates, blocksLight: true, userData: this);

            BigSensorCollidesWithCategories = new List<Category>() { Category.Item, Category.Portal, Category.Solid };
            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 16f, new List<Category>() { Category.PlayerBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);

        }

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
            //Collision logic changes based on current behaviour!
           Behaviour.OnCollides(fixtureA,fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public virtual void Halt()
        {
            Velocity = Vector2.Zero;
        }
        public virtual new void Update(GameTime gameTime)
        {
            if (WarpTimer.Run(gameTime))
                AbleToWarp = true;
            base.Update(gameTime);

            Behaviour.Update(gameTime, ref Velocity);
            StatusIcon.Update(gameTime, Position);

            IsMoving = Velocity != Vector2.Zero;

            if (IsMoving)
            {
                DirectionMoving = UpdateDirection();

            }

            float deltaTime = (float)gameTime.ElapsedGameTime.Milliseconds;


            MainHullBody.Body.LinearVelocity = Velocity * Speed * deltaTime;

            //Position is changed so that our sprite knows where to draw, and position
            //snaps to where the physics system moved the main hullbody
            Move(new Vector2(MainHullBody.Position.X, MainHullBody.Position.Y));
            //Big sensor position just sticks on top of our main hullbody
            BigSensor.Position = Position;
            if (MainHullBody.Hull != null)
                MainHullBody.Hull.Position = Position;

            EntityAnimator.Update(gameTime, IsMoving, Position, DirectionMoving);

            //if(CurrentBehaviour == CurrentBehaviour.Wander && !Navigator.HasActivePath)
            //{

            //}
        }

       
        public bool IsInSameStageAs(Entity otherEntity)
        {
            return CurrentStageName == otherEntity.CurrentStageName;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            EntityAnimator.Draw(spriteBatch);

#if DEBUG
            if (Flags.ShowEntityPaths)
                Behaviour.DrawDebug(spriteBatch);
#endif

        }

        public virtual void DrawDebug(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextFactory.DefaultFont, $"{Name} \n {CurrentStageName}", CenteredPosition, Color.Red, 0f, Vector2.Zero, .5f, SpriteEffects.None, .99f);

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
        public void AddedToPlayerStage()
        {
            MainHullBody.Body.IsSensor = false;
            AddBigSensorCat(Category.Cursor);
        }
        /// <summary>
        /// Was previously in player stage, no longer is. Disable collisions and remove click interactions.
        /// </summary>
        public void RemovedFromPlayerStage()
        {
            MainHullBody.Body.IsSensor = true;
            //Shouldn't be able to click on entity when not in same stage.
            RemoveBigSensorCat(Category.Cursor);
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

            if (CurrentStageName == newStage)
            {

                if (MainHullBody == null)
                {
                    CreateBody(Position);

                }

                else
                {
                    AddedToPlayerStage();
                }
            }
            else if (MainHullBody != null)
            {

                if (fullyUnload)
                    Unload();

                else
                {
                    RemovedFromPlayerStage();
                }

            }
        }

        public virtual void LoadToNewStage(string newStage, TileManager tileManager)
        {
            CurrentStageName = newStage;
            Navigator.Unload();

            Navigator.Load(tileManager.PathGrid);
            TileManager = tileManager;
        }

        protected Direction UpdateDirection()
        {
            if (Velocity.X > .5f)
            {
                return Direction.Right; //right
            }
            else if (Velocity.X < -.5f)
            {
                return Direction.Left; //left
            }
            else if (Velocity.Y < .5f) // up
            {
                return Direction.Up;
            }

            else if (Velocity.Y > .5f)
            {
                return Direction.Down;
            }
            else
            {
                return Direction.Down;
            }


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
    }
}
