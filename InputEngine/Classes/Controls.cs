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
        Break = 2,
        Speech = 3,
        Door = 4,
        Ignite = 5,
        Dig = 6,
    }
    public static class Controls
    {
        private static GraphicsDevice Graphics;
        private static ContentManager Content;
        private static Camera2D Camera { get; set; }


        public static bool HasCursorTileIndexChanged => OldCursorTileIndex != CursorTileIndex;
        public static Point OldCursorTileIndex { get; private set; }

        public static Point CursorTileIndex { get; private set; }

        public static bool ClickActionTriggeredThisFrame { get; set; }
        public static bool IsUiHovered { get; set; }
        public static bool IsClicked => DidClick() && !ClickActionTriggeredThisFrame;

        //Will return true if UI is not currently hovered and controls are clicked
        public static bool IsClickedWorld => IsClicked && !IsUiHovered;
        public static bool IsRightClickedWorld => IsRightClicked && !IsUiHovered;

        public static bool IsPlayerControllable { get; set; }
        public static bool IsPlayerMoving { get; set; }
        private static KeyboardManager KeyboardManager { get; set; }
        private static MouseManager MouseManager { get; set; }
        public static bool EscapePressed => KeyboardManager.WasKeyPressed(Keys.Escape);
        public static bool IsSelectDown()
        {
            if (ControllerConnected)
            {
                return (_gamePadControls.IsActionHeld(GamePadActionType.Select));
            }
            return MouseManager.LeftHeld;
        }
        public static bool DidClick()
        {
            if (ControllerConnected)
            {
                if (_gamePadControls.WasActionTapped(GamePadActionType.Select))
                    return true;
            }
 
            return MouseManager.LeftClicked;
        }

        public static bool WasGamePadButtonTapped(GamePadActionType actionType) => _gamePadControls.WasActionTapped(actionType);
        public static bool DidGamePadSelectWorld => (WasGamePadButtonTapped(GamePadActionType.Select) && !IsUiHovered);

        public static bool IsRightClicked => MouseManager.RightClicked;

        public static void ControllerSetUIMousePosition(Vector2 newPos)
        {
            if (!ControllerConnected)
                throw new Exception($"Should not be setting mouse ui position if controller not connected");
            MouseManager.ControllerSetMouseUIPosition(newPos);
        } 
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
            if(ControllerConnected)
            {
                Direction direction = _gamePadControls.GetDirectionFacing();
                if (direction != Direction.None)
                    return direction;
            }
            
            return KeyboardManager.PrimaryDirection;
        }
        public static float ThumbStickRotation(Direction direction) => _gamePadControls.GetThumbStickRotation(direction);
        public static Vector2 ThumbStickVector(Direction direction) => _gamePadControls.GetThumbStickVector(direction);

        public static Vector2 WorldDistanceBetweenCursorAndVector(Vector2 other, Direction? direction = null)
        {
            if (ControllerConnected)
            {
                if (direction == null)
                    throw new Exception($"Gamepad must specify which stick to use");
              other.Normalize();
                return _gamePadControls.GetThumbStickVector(direction.Value) - other;
            }
            else
            {
                return MouseWorldPosition - other;
            }
        }
        /// <summary>
        /// Gets the secondary direction the player is currently facing.
        /// </summary>
        public static Direction SecondaryDirectionFacing => KeyboardManager.SecondaryDirection;

        public static bool ScrollWheelIncreased => MouseManager.ScrollWheelIncreased;
        public static bool ScrollWheelDecreased => MouseManager.ScrollWheelDecreased;

        private static GamepadControls _gamePadControls;
        public static bool ControllerConnected { get; private set; }

        public static void Load(Camera2D camera, GraphicsDevice graphics, ContentManager content)
        {
            Graphics = graphics;
            Content = content;
            Camera = camera;
            IsPlayerControllable = true;
            KeyboardManager = new KeyboardManager();
            MouseManager = new MouseManager(camera, graphics);
            _gamePadControls = new GamepadControls();

            GamePadCapabilities capabilities = GamePad.GetCapabilities(
                                              PlayerIndex.One);
            ControllerConnected = capabilities.IsConnected;
        }
    



        public static void Update(GameTime gameTime)
        {
            OldCursorTileIndex = CursorTileIndex;

            GamePadCapabilities capabilities = GamePad.GetCapabilities(
                                              PlayerIndex.One);
            ControllerConnected = capabilities.IsConnected;

            // If there a controller attached, handle it
            if (ControllerConnected)
            {
                _gamePadControls.Update(gameTime);
            }
            else
            {

            KeyboardManager.Update(gameTime);
            }
            MouseManager.Update(gameTime);

            if (IsPlayerControllable)
            {

                IsPlayerMoving = !(GetDirectionFacing() == Direction.None);
                CursorTileIndex = GetTileIndexPosition();
            }
            if(WasKeyTapped(Keys.N))
                Console.WriteLine("test");
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


        public static Vector2 PlayerFrontalSensorPosition { get; set; }
        /// <summary>
        /// Gets the x and y index of the tile underneath the cursor
        /// </summary>
        /// <returns></returns>
        public static Point GetTileIndexPosition()
        {
            if (ControllerConnected)
                return Vector2Helper.GetTileIndexPosition(PlayerFrontalSensorPosition);

            
            return Vector2Helper.GetTileIndexPosition(MouseWorldPosition);
        }
    }
}
