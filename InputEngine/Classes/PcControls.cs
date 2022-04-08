using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace InputEngine.Classes
{
    internal class PcControls : IInput
    {
        private KeyboardManager KeyboardManager { get; set; }
        public MouseManager MouseManager {  get; private set; }
        public bool EscapePressed => KeyboardManager.WasKeyPressed(Keys.Escape);

        public bool DidClick => MouseManager.LeftClicked;
        public bool DidRightClick => MouseManager.RightClicked;
        public Vector2 MouseUIPosition => MouseManager.UIPosition;
        public Vector2 MouseWorldPosition => MouseManager.WorldPosition;
        public List<Keys> PressedKeys => KeyboardManager.PressedKeys;
        public List<Keys> TappedKeys => KeyboardManager.TappedKeys;
        public List<Keys> AcceptableKeysForTyping => KeyboardManager.UseableRecentKeys;
        /// <summary>
        /// Gets the direction the player is currently facing.
        /// </summary>
        public Direction GetDirectionFacing => KeyboardManager.PrimaryDirection;

        /// <summary>
        /// Gets the secondary direction the player is currently facing.
        /// </summary>
        public Direction SecondaryDirectionFacing => KeyboardManager.SecondaryDirection;


        public bool ScrollWheelIncreased => MouseManager.ScrollWheelIncreased;
        public bool ScrollWheelDecreased => MouseManager.ScrollWheelDecreased;

        public PcControls(Camera2D camera, GraphicsDevice graphics )
        {
            KeyboardManager = new KeyboardManager();
            MouseManager = new MouseManager(camera, graphics);
        }




        void IInput.Update(GameTime gameTime)
        {
            KeyboardManager.Update(gameTime);
            MouseManager.Update(gameTime);

        }

        


        bool IInput.IsHoveringRectangle(ElementType elementType, Rectangle rectangle)
        {
            if (elementType == ElementType.UI)
            {
                if (MouseManager.IsHoveringUIRectangle(rectangle))
                    return true;
            }
            else if (elementType == ElementType.World)
            {
                if (MouseManager.IsHoveringWorldRectangle(rectangle))
                    return true;
            }

            return false;
        }  
         void IInput.ClearRecentlyPressedKeys()
        {
            KeyboardManager.ClearUseableKeys();
        }

    }
}
