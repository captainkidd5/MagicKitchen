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
        Door = 4
    }
    public static class Controls
    {
        private static GraphicsDevice Graphics;
        private static ContentManager Content;

        private static IInput PcControls { get; set; }
        private static IInput CurrentControls { get; set; }

        public static CursorIconType CursorIconType { get; set; }
        public static CursorIconType OldCursorIconType { get; set; }
        public static InputType InputType { get; set; }

        private static Camera2D Camera { get; set; }

        #region PLAYERMOVEMENT
        public static bool IsPlayerControllable { get; set; }
        public static bool IsPlayerMoving { get; set; }
        public static Direction DirectionFacing => CurrentControls.GetDirectionFacing;
        public static Direction SecondaryDirectionFacing => CurrentControls.SecondaryDirectionFacing;
        #endregion

        public static Item HeldItem { get; private set; }

        #region Buttons
        public static bool StartMenuPressed => CurrentControls.EscapePressed;
        public static List<Keys> TappedKeys => CurrentControls.TappedKeys;
        #endregion

        #region CURSOR
        /// <summary>
        /// Gets cursor position RELATIVE TO WORLD. DO NOT USE FOR UI POSITIONING.
        /// </summary>
        public static Vector2 CursorWorldPosition => CurrentControls.MouseWorldPosition;

        /// <summary>
        /// Gets cursor position RELATIVE TO UI. DO NOT USE FOR WORLD POSITIONING.
        /// </summary>
        public static Vector2 CursorUIPosition => CurrentControls.MouseUIPosition;

        public static bool ScrollWheelIncreased => CurrentControls.ScrollWheelIncreased;
        public static bool ScrollWheelDecreased => CurrentControls.ScrollWheelDecreased;

        public static List<Keys> AcceptableKeysForTyping => CurrentControls.AcceptableKeysForTyping;
        public static Point CursorTileIndex { get; private set; }
        public static bool IsClicked => CurrentControls.DidClick;
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
            CurrentControls = PcControls;
            Texture2D cursorTexture = Content.Load<Texture2D>("ui/MouseIcons");
            PcControls.Load(cursorTexture);
        }
        public static void Update(GameTime gameTime)
        {
            CursorIconType = CursorIconType.None;
            CurrentControls.Update(gameTime);

            if (IsPlayerControllable)
            {
                IsPlayerMoving = !(DirectionFacing == Direction.None);
                CursorTileIndex = GetTileIndexPosition();
            }
            UpdateCursor();
        }

        public static void UpdateCursor()
        {
            if (OldCursorIconType != CursorIconType)
            {
                CurrentControls.SwapCursorRectangle(GetCursorIconSourcRectangleFromType(CursorIconType), null);
            }
            OldCursorIconType = CursorIconType;
        }

        private static Rectangle GetCursorIconSourcRectangleFromType(CursorIconType ctype)
        {
            switch (ctype)
            {
                case CursorIconType.None:
                    return new Rectangle(0, 0, 32, 32);
                case CursorIconType.Selectable:
                    return new Rectangle(32, 0, 32, 32);
                case CursorIconType.Rock:
                    return new Rectangle(96, 0, 32, 32);
                case CursorIconType.Speech:
                    return new Rectangle(160, 0, 32, 32);
                case CursorIconType.Door:
                    return new Rectangle(96, 32, 32, 32);


                default:
                    goto case 0;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {

            CurrentControls.Draw(spriteBatch);

        }
        /// <summary>
        /// We need element type because the mouse hitbox is different for ui vs world.
        /// </summary>
        public static bool IsHovering(ElementType elementType, Rectangle rectangle)
        {
            return CurrentControls.IsHoveringRectangle(elementType,rectangle);
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
        public static bool WasKeyTapped(Keys key)
        {
            if (TappedKeys.Contains(key))
                return true;

            return false;
        }

        /// <summary>
        /// Called by typing box after every key press.
        /// </summary>
        public static void ClearUseableKeys()
        {
            CurrentControls.ClearRecentlyPressedKeys();
        }
        
        /// <summary>
        /// Have item shown under cursor
        /// </summary>
        public static void PickUp(Item item)
        {
                HeldItem = item;
                CurrentControls.SwapCursorRectangle(Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet);

        }
        public static void DropToSlot()
        {
            if (HeldItem == null)
                throw new Exception("tried to drop an item which doesn't exist");
            HeldItem = null;
            CurrentControls.SwapCursorRectangle(null, null);

        }
        public static void DropAndAddToWorld(Vector2 playerPosition, Direction directionFacing, List<Item> items)
        {
            if (HeldItem == null)
                throw new Exception("tried to drop an item which doesn't exist");
            HeldItem.Drop(playerPosition, directionFacing, items);
            HeldItem = null;
            CurrentControls.SwapCursorRectangle(null, null);
        }
    }
}
