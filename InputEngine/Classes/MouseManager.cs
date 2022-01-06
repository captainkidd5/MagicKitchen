using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.RenderTargetStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine;
using TextEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace InputEngine.Classes.Input
{
    internal class MouseManager : Collidable
    {

        private readonly Camera2D camera;
        private readonly GraphicsDevice graphics;
        private Rectangle CursorSourceRectangle = new Rectangle(32, 0, 32, 32);
        private MouseState OldMouseState { get; set; }
        private MouseState NewMouseState { get; set; }
        private Rectangle UIRectangle { get; set; }
        private Rectangle WorldRectangle { get; set; }

        internal bool LeftClicked { get; private set; }
        internal bool LeftHeld { get; private set; }

        internal bool RightClicked { get; private set; }
        internal bool RightHeld { get; private set; }

        internal bool ScrollWheelIncreased { get; private set; }
        internal bool ScrollWheelDecreased { get; private set; }

        internal Vector2 UIPosition { get; private set; }
        internal Vector2 WorldPosition { get; private set; }

        public Texture2D CursorTexture { get; set; }
        public Sprite CursorSprite { get; private set; }

        private Text MouseDebugText { get; set; }
        //public Sprite MyProperty { get; set; }

        internal MouseManager(Camera2D camera, GraphicsDevice graphics )
        {
            this.camera = camera;
            this.graphics = graphics;

        }

        internal void LoadContent(Texture2D cursorTexture)
        {
            CursorTexture = cursorTexture;
            CursorSprite = SpriteFactory.CreateUISprite(Vector2.Zero, CursorSourceRectangle,
                CursorTexture, Color.White, scale: 1f, layer:Settings.Layers.front);

            MouseDebugText = TextFactory.CreateUIText("test");
            CreateBody(WorldPosition);
        }

        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);

            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 16f, new List<Category>() { Category.Cursor },
                new List<Category>() { Category.Portal, Category.PlayerBigSensor, Category.NPCBigSensor }, OnCollides, OnSeparates, isSensor: true,friction:0f,mass:0f,restitution:0f, userData: this);

            AddPrimaryBody(MainHullBody);

            //MainHullBody = new HullBody(Position, false);
            //MainHullBody.XOffset = 0;
            //MainHullBody.YOffset = 0;

            //MainHullBody.Body = BodyFactory.CreateCircle(PhysicsManager.VelcroWorld, 16, 0f, position);
            //MainHullBody.Body.BodyType = BodyType.Static;
            //MainHullBody.Body.Restitution = 0f;
            //MainHullBody.Body.Friction = 0f;
            //MainHullBody.Body.Mass = 0f;
            //MainHullBody.Body.Inertia = 0;
            //MainHullBody.Body.SleepingAllowed = false;
            //MainHullBody.Body.CollisionCategories = Category.Cursor;
            //MainHullBody.Body.CollidesWith =  Category.Portal;
            //MainHullBody.Body.IgnoreGravity = true;
            //MainHullBody.Body.OnCollision += OnCollides;
            //MainHullBody.Body.OnSeparation += OnSeparates;
            //MainHullBody.Body.UserData = this;
            //MainHullBody.Body.IsSensor = true;

            //HullBodies.Add(MainHullBody);
            //MainHullBody.Position = new Vector2(600, 400);
            //BigSensor = new HullBody(this, Position, false);
            //BigSensor.XOffset = 0;
            //BigSensor.YOffset = 0;

            //BigSensor.Body = BodyFactory.CreateCircle(PhysicsManager.VelcroWorld, 16, 0f, position);
            //BigSensor.Body.BodyType = BodyType.Static;
            //BigSensor.Body.Restitution = 0f;
            //BigSensor.Body.Friction = 0f;
            //BigSensor.Body.Mass = 0f;
            //BigSensor.Body.Inertia = 0;
            //BigSensor.Body.SleepingAllowed = true;
            //BigSensor.Body.CollisionCategories = Category.EntityBigSensor;
            //BigSensor.Body.CollidesWith = Category.Item;
            //BigSensor.Body.IgnoreGravity = true;
            //BigSensor.Body.OnCollision += OnCollides;
            //BigSensor.Body.IsSensor = true;
            //BigSensor.Body.OnSeparation += OnSeparates;
            //BigSensor.Body.UserData = this;
            //HullBodies.Add(BigSensor);
        }

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //Console.WriteLine("test");
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
        }

        internal void Update(GameTime gameTime)
        {
            OldMouseState = NewMouseState;
            NewMouseState = Mouse.GetState();



            Vector2 renderTargetResolution = new Vector2(RenderTargetManager.CurrentOverlayTarget.Width/(float)Settings.ScreenRectangle.Width, RenderTargetManager.CurrentOverlayTarget.Height/ (float)Settings.ScreenRectangle.Height);

            UIPosition = new Vector2(NewMouseState.Position.X, NewMouseState.Position.Y) * renderTargetResolution;

            WorldPosition = camera.ScreenToWorld(UIPosition);



            WorldPosition = WorldPosition - RenderTargetManager.HalfScreen() +camera.GetZoomOffSetPatch();
           // float deltaTime = (float)gameTime.ElapsedGameTime.Milliseconds;


            Move(WorldPosition);
            MainHullBody.Position = WorldPosition;
           // MainHullBody.Body.LinearVelocity = new Vector2(200, 200) * deltaTime;

            UIRectangle = new Rectangle((int)UIPosition.X, (int)UIPosition.Y, 2, 2);
            WorldRectangle = new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, 2, 2);

            LeftClicked = DidLeftClick(OldMouseState, NewMouseState);
            LeftHeld = DidHoldLeft(OldMouseState, NewMouseState);

            RightClicked = DidRightClick(OldMouseState, NewMouseState);
            RightHeld = DidHoldRight(OldMouseState, NewMouseState);

            ScrollWheelIncreased = DidScrollIncrease(OldMouseState, NewMouseState);
            ScrollWheelDecreased = DidScrollDecrease(OldMouseState, NewMouseState);

            CursorSprite.Update(gameTime, UIPosition);

            if (Flags.DisplayMousePosition)
                MouseDebugText.UpdateText($"{UIPosition.X.ToString()} , {UIPosition.Y.ToString()}");

        }


        /// <summary>
        /// Swaps cursor texture
        /// </summary>
        /// <param name="newSourceRectangle">Leave null to put back as default</param>
        internal void SwapMouseSpriteRectangle(Rectangle? newSourceRectangle, Texture2D? texture = null)
        {
            Rectangle newRectangle = newSourceRectangle ?? CursorSourceRectangle;
            Texture2D textureToUse = texture ?? CursorTexture;
            CursorSprite.SwapSourceRectangle(newRectangle);
            CursorSprite.SwapTexture(textureToUse);
            CursorSprite.SwapScale(texture == null ? Settings.GameScale : Settings.GameScale);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            CursorSprite.Draw(spriteBatch);
            if (Globals.Classes.Flags.DisplayMousePosition)
                MouseDebugText.Draw(spriteBatch, new Vector2(UIPosition.X + 48, UIPosition.Y + 48));


        }

        internal bool IsHoveringUIRectangle(Rectangle rectangle)
        {
            if (UIRectangle.Intersects(rectangle))
                return true;
            return false;
        }

        internal bool IsHoveringWorldRectangle(Rectangle rectangle)
        {
            if (WorldRectangle.Intersects(rectangle))
                return true;
            return false;
        }

        private bool DidLeftClick(MouseState oldMouseState, MouseState newMouseState)
        {
            if (oldMouseState.LeftButton == ButtonState.Pressed &&
                NewMouseState.LeftButton == ButtonState.Released)
                return true;
            return false;
        }
        private bool DidHoldLeft(MouseState oldMouseState, MouseState newMouseState)
        {
            if (oldMouseState.LeftButton == ButtonState.Pressed &&
                NewMouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }
        private bool DidRightClick(MouseState oldMouseState, MouseState newMouseState)
        {
            if (oldMouseState.RightButton == ButtonState.Pressed &&
                NewMouseState.RightButton == ButtonState.Released)
                return true;
            return false;
        }
        private bool DidHoldRight(MouseState oldMouseState, MouseState newMouseState)
        {
            if (oldMouseState.RightButton == ButtonState.Pressed &&
                NewMouseState.RightButton == ButtonState.Pressed)
                return true;
            return false;
        }
        private bool DidScrollIncrease(MouseState oldMouseState, MouseState newMouseState)
        {
            if (newMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue)
                return true;
            return false;
        }
        private bool DidScrollDecrease(MouseState oldMouseState, MouseState newMouseState)
        {
            if (newMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue)
                return true;
            return false;
        }

    }
}
