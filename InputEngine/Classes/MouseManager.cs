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
    internal class MouseManager
    {

        private readonly Camera2D camera;
        private readonly GraphicsDevice graphics;
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


        internal MouseManager(Camera2D camera, GraphicsDevice graphics )
        {
            this.camera = camera;
            this.graphics = graphics;

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




            UIRectangle = new Rectangle((int)UIPosition.X, (int)UIPosition.Y, 2, 2);
            WorldRectangle = new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, 2, 2);

            LeftClicked = DidLeftClick(OldMouseState, NewMouseState);
            LeftHeld = DidHoldLeft(OldMouseState, NewMouseState);

            RightClicked = DidRightClick(OldMouseState, NewMouseState);
            RightHeld = DidHoldRight(OldMouseState, NewMouseState);

            ScrollWheelIncreased = DidScrollIncrease(OldMouseState, NewMouseState);
            ScrollWheelDecreased = DidScrollDecrease(OldMouseState, NewMouseState);



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
