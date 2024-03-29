﻿using Globals.Classes;
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

        internal bool LeftClicked { get; 
            private set; }
        private bool _leftHeld;

        internal bool LeftHeld => _leftHeld && _tooLateForLeftClick;
        internal bool RightClicked { get; private set; }
        internal bool RightHeld { get; private set; }

        internal bool ScrollWheelIncreased { get; private set; }
        internal bool ScrollWheelDecreased { get; private set; }

        internal Vector2 UIPosition { get; private set; }
        internal Vector2 WorldPosition { get; private set; }

        private float _clickTimerThreshold = .2f;

        private SimpleTimer _leftClickHeldTimer;
        private bool _tooLateForLeftClick = false;

        private SimpleTimer _rightClickHeldTimer;
        private bool _tooLateForRightClick = false;
        internal MouseManager(Camera2D camera, GraphicsDevice graphics )
        {
            this.camera = camera;
            this.graphics = graphics;
            _leftClickHeldTimer = new SimpleTimer(_clickTimerThreshold);
            _rightClickHeldTimer = new SimpleTimer(_clickTimerThreshold);

        }
        internal void ControllerSetMouseUIPosition(Vector2 newPos)
        {
            
            UIPosition = newPos;
        }

        private Vector2 GetUIOffSet(Vector2 unadjustedPos)
        {
            Vector2 renderTargetResolution = new Vector2(RenderTargetManager.CurrentOverlayTarget.Width / (float)Settings.ScreenRectangle.Width, RenderTargetManager.CurrentOverlayTarget.Height / (float)Settings.ScreenRectangle.Height);

            return unadjustedPos * renderTargetResolution;
        }
        internal void Update(GameTime gameTime)
        {
            OldMouseState = NewMouseState;
            NewMouseState = Mouse.GetState();


            Controls.ClickActionTriggeredThisFrame = false;


            //Do not use mouse state to update cursor position if controller is connected
            //It is used elsewhere such as in inventory selection to draw the tooltip 
            //and updating here would mess up this positioning
            if (!Controls.ControllerConnected)
            {

                UIPosition = GetUIOffSet(new Vector2(NewMouseState.Position.X, NewMouseState.Position.Y));

            WorldPosition = camera.ScreenToWorld(UIPosition);



            WorldPosition = WorldPosition - RenderTargetManager.HalfScreen() + camera.GetZoomOffSetPatch();
            // float deltaTime = (float)gameTime.ElapsedGameTime.Milliseconds;




            UIRectangle = new Rectangle((int)UIPosition.X, (int)UIPosition.Y, 2, 2);
            WorldRectangle = new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, 2, 2);
            }


            HandleLeftClicks(gameTime);

            HandleRightClicks(gameTime);

            ScrollWheelIncreased = DidScrollIncrease();
            ScrollWheelDecreased = DidScrollDecrease();



        }

        /// <summary>
        /// Logic here will not register a click if it is held for more than <see cref="_clickTimerThreshold"/> and then released.
        /// This helps prevent accidental clicks which players may try to undo by not releasing the mouse button.
        /// </summary>
        /// <param name="gameTime"></param>
        private void HandleRightClicks(GameTime gameTime)
        {
           
            if (NewMouseState.RightButton == ButtonState.Released)
                _tooLateForRightClick = false;
            RightHeld = DidHoldButton(OldMouseState.RightButton, NewMouseState.RightButton);
            if (RightHeld)
                if (_rightClickHeldTimer.Run(gameTime))
                    _tooLateForRightClick = true;
            RightClicked = DidTapButton(OldMouseState.RightButton, NewMouseState.RightButton, _tooLateForRightClick);
            if (RightClicked)
                _rightClickHeldTimer.ResetToZero();
            if (_tooLateForRightClick && NewMouseState.RightButton == ButtonState.Released)
            {
                _tooLateForRightClick = false;
                _rightClickHeldTimer.ResetToZero();
            }
        }

        private void HandleLeftClicks(GameTime gameTime)
        {
          
            if (NewMouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released)
                _leftClickHeldTimer.Run(gameTime);
           
            _leftHeld = DidHoldButton(OldMouseState.LeftButton, NewMouseState.LeftButton);

            if (_leftHeld)
                if (_leftClickHeldTimer.Run(gameTime))
                    _tooLateForLeftClick = true;
            LeftClicked = DidTapButton(OldMouseState.LeftButton, NewMouseState.LeftButton, _tooLateForLeftClick);
            if(LeftClicked)
                _leftClickHeldTimer.ResetToZero();

            if (_tooLateForLeftClick && NewMouseState.LeftButton == ButtonState.Released)
            {
                _tooLateForLeftClick = false;
                _leftClickHeldTimer.ResetToZero();
            }
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

        private bool DidTapButton(ButtonState oldButtonState, ButtonState newButtonState, bool tooLate)
        {
           

            if (!tooLate && oldButtonState == ButtonState.Pressed &&
                newButtonState == ButtonState.Released)
                return true;
            return false;
        }
        private bool DidHoldButton(ButtonState oldButtonState, ButtonState newButtonState)
        {
          
            return false;
        }
       
        private bool DidScrollIncrease()
        {
            if (NewMouseState.ScrollWheelValue > OldMouseState.ScrollWheelValue)
                return true;
            return false;
        }
        private bool DidScrollDecrease()
        {
            if (NewMouseState.ScrollWheelValue < OldMouseState.ScrollWheelValue)
                return true;
            return false;
        }

    }
}
