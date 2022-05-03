using Globals.Classes;
using Globals.Classes.Helpers;
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
    public enum CursorIconType
    {
        None = 0,
        Selectable = 1,
        Rock = 2,
        Speech = 3,
        Door = 4,
        Ignite = 5,
    }
    public static class Controls
    {
        private static GraphicsDevice Graphics;
        private static ContentManager Content;
        private static Camera2D Camera { get; set; }
        public static Point CursorTileIndex { get; private set; }

        public static bool ClickActionTriggeredThisFrame;
        public static bool IsUiHovered;
        public static bool IsClicked => DidClick() && !ClickActionTriggeredThisFrame;

        //Will return true if UI is not currently hovered and controls are clicked
        public static bool IsClickedWorld => IsClicked && !IsUiHovered;
        public static bool IsPlayerControllable { get; set; }
        public static bool IsPlayerMoving { get; set; }
        private static KeyboardManager KeyboardManager { get; set; }
        private static MouseManager MouseManager { get; set; }
        public static bool EscapePressed => KeyboardManager.WasKeyPressed(Keys.Escape);

        public static bool DidClick()
        {
            if (_controllerConnected)
            {
                if (_gamePadControls.WasActionTapped(GamePadActionType.Select))
                    return true;
            }
            return MouseManager.LeftClicked;
        }

        public static bool GamePadButtonTapped(GamePadActionType actionType) => _gamePadControls.WasActionTapped(actionType);
        public static bool IsRightClicked => MouseManager.RightClicked;
        public static Vector2 MouseUIPosition => MouseManager.UIPosition;
        public static Vector2 MouseWorldPosition => MouseManager.WorldPosition;
        public static bool WasKeyTapped(Keys key) => TappedKeys.Contains(key);

        public static bool IsKeyPressed(Keys key) => PressedKeys.Contains(key);
        public static List<Keys> PressedKeys => KeyboardManager.PressedKeys;
        public static List<Keys> TappedKeys => KeyboardManager.TappedKeys;
        public static List<Keys> AcceptableKeysForTyping => KeyboardManager.UseableRecentKeys;
        /// <summary>
        /// Gets the direction the player is currently facing.
        /// </summary>
        /// 
        public static Direction DirectionFacing => GetDirectionFacing();
        private static Direction GetDirectionFacing() 
        {
            if(_controllerConnected)
            {
                Direction direction = _gamePadControls.GetDirectionFacing();
                if (direction != Direction.None)
                    return direction;
            }
            
            return KeyboardManager.PrimaryDirection;
        }

        /// <summary>
        /// Gets the secondary direction the player is currently facing.
        /// </summary>
        public static Direction SecondaryDirectionFacing => KeyboardManager.SecondaryDirection;

        public static bool ScrollWheelIncreased => MouseManager.ScrollWheelIncreased;
        public static bool ScrollWheelDecreased => MouseManager.ScrollWheelDecreased;

        private static GamepadControls _gamePadControls;
        private static bool _controllerConnected;

        public static void Load(Camera2D camera, GraphicsDevice graphics, ContentManager content)
        {
            Graphics = graphics;
            Content = content;
            Camera = camera;
            IsPlayerControllable = true;
            KeyboardManager = new KeyboardManager();
            MouseManager = new MouseManager(camera, graphics);
            _gamePadControls = new GamepadControls();
        }
    



        public static void Update(GameTime gameTime)
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(
                                              PlayerIndex.One);
            _controllerConnected = capabilities.IsConnected;

            // If there a controller attached, handle it
            if (_controllerConnected)
            {
                _gamePadControls.Update(gameTime);
            }
            KeyboardManager.Update(gameTime);
            MouseManager.Update(gameTime);
            if (IsPlayerControllable)
            {

                IsPlayerMoving = !(GetDirectionFacing() == Direction.None);
                CursorTileIndex = GetTileIndexPosition();
            }
        }




        public static bool IsHovering(ElementType elementType, Rectangle rectangle)
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
        public static void ClearUseableKeys() => KeyboardManager.ClearUseableKeys();
  
        /// <summary>
        /// Gets the x and y index of the tile underneath the cursor
        /// </summary>
        /// <returns></returns>
        public static Point GetTileIndexPosition()
        {
            return Vector2Helper.GetTileIndexPosition(MouseWorldPosition);
        }
    }
}
