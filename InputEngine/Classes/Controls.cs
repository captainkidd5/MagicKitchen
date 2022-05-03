using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace InputEngine.Classes.Input
{

    public enum InputType
    {
        Pc = 0,
        Controller = 1
    }

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

        private static PcControls PcControls { get; set; }



        public static InputType InputType { get; set; }

        private static Camera2D Camera { get; set; }

        #region PLAYERMOVEMENT
        public static bool IsPlayerControllable { get; set; }
        public static bool IsPlayerMoving { get; set; }
        public static Direction DirectionFacing => PcControls.GetDirectionFacing();
        public static Direction SecondaryDirectionFacing => PcControls.SecondaryDirectionFacing;
        #endregion

        #region Buttons
        public static bool StartMenuPressed => PcControls.EscapePressed;
        public static List<Keys> TappedKeys => PcControls.TappedKeys;
        public static List<Keys> PressedKeys => PcControls.PressedKeys;
        #endregion

        #region CURSOR
        /// <summary>
        /// Gets cursor position RELATIVE TO WORLD. DO NOT USE FOR UI POSITIONING.
        /// </summary>
        public static Vector2 CursorWorldPosition => PcControls.MouseWorldPosition;

        /// <summary>
        /// Gets cursor position RELATIVE TO UI. DO NOT USE FOR WORLD POSITIONING.
        /// </summary>
        public static Vector2 CursorUIPosition => PcControls.MouseUIPosition;

        public static bool ScrollWheelIncreased => PcControls.ScrollWheelIncreased;
        public static bool ScrollWheelDecreased => PcControls.ScrollWheelDecreased;

        public static List<Keys> AcceptableKeysForTyping => PcControls.AcceptableKeysForTyping;
        public static Point CursorTileIndex { get; private set; }

        public static bool ClickActionTriggeredThisFrame;
        public static bool IsClicked => PcControls.DidClick && !ClickActionTriggeredThisFrame;

        //Will return true if UI is not currently hovered and controls are clicked
        public static bool IsClickedWorld => IsClicked && !IsUiHovered;

        public static bool IsUiHovered;
        public static bool IsRightClicked => PcControls.DidRightClick;

        public static bool WasKeyTapped(Keys key) => TappedKeys.Contains(key);

        public static bool IsKeyPressed(Keys key) => PressedKeys.Contains(key);

        //private static Collidable MouseBody { get; set; }

        #endregion
        public static void Load(Camera2D camera, GraphicsDevice graphics, ContentManager content)
        {
            Graphics = graphics;
            Content = content;
            Camera = camera;
            IsPlayerControllable = true;
            PcControls = new PcControls(camera, graphics);
            //Default is Pc controls
            PcControls = PcControls;
        }
        public static void Update(GameTime gameTime)
        {



            PcControls.Update(gameTime);
            ClickActionTriggeredThisFrame = false;
            if (IsPlayerControllable)
            {
                
                IsPlayerMoving = !(DirectionFacing == Direction.None);
                CursorTileIndex = GetTileIndexPosition();
            }
        }

        /// <summary>
        /// We need element type because the mouse hitbox is different for ui vs world.
        /// </summary>
        public static bool IsHovering(ElementType elementType, Rectangle rectangle)
        {
            return PcControls.IsHoveringRectangle(elementType, rectangle);
        }

        #region CURSOR_POSITITIONING


        /// <summary>
        /// Gets the x and y index of the tile underneath the cursor
        /// </summary>
        /// <returns></returns>
        public static Point GetTileIndexPosition()
        {
            return Vector2Helper.GetTileIndexPosition(CursorWorldPosition);
        }
        #endregion


        /// <summary>
        /// Called by typing box after every key press.
        /// </summary>
        public static void ClearUseableKeys()
        {
            PcControls.ClearRecentlyPressedKeys();
        }


    }
}
