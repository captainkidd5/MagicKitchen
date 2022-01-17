using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes.Helpers
{
    public static class RectangleHelper
    {
        public static Vector2 CenterRectangleOnScreen(Rectangle rectangle, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            Vector2 centerScreen = Settings.CenterScreen();
            return new Vector2(centerScreen.X - rectangle.Width * (float)scale / 2, centerScreen.Y - rectangle.Height / 2 * (float)scale);
        }
        public static Vector2 CenterRectangleOnScreen(int width, int height, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            Vector2 centerScreen = Settings.CenterScreen();
            return new Vector2(centerScreen.X - width / 2 * (float)scale, centerScreen.Y - height / 2 * (float)scale);
        }

        public static Vector2 PlaceTopRightScreen(Rectangle rectangleToPlace, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            return new Vector2(Settings.ScreenWidth - rectangleToPlace.Width * (float)scale, 0);
        }
        public static Vector2 PlaceTopRightScreen(int width, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            return new Vector2(Settings.ScreenWidth * (float)scale - width, 0);
        }

        public static Vector2 PlaceBottomLeftScreen(Rectangle rectangleToPlace, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            return new Vector2(0, Settings.ScreenHeight - rectangleToPlace.Height * (float)scale);
        }
        public static Vector2 PlaceBottomLeftScreen(int height, float? scale = null)
        {
            return new Vector2(0, Settings.ScreenHeight - height * (scale ?? Settings.GameScale));
        }

        public static Vector2 PlaceBottomRightScreen(Rectangle rectangleToPlace, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            return new Vector2(Settings.ScreenWidth - rectangleToPlace.Width * (float)scale, Settings.ScreenHeight - rectangleToPlace.Height * (float)scale);
        }
        public static Vector2 PlaceBottomRightScreen(int width, int height, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            return new Vector2(Settings.ScreenWidth - width * (float)scale, Settings.ScreenHeight - height * (float)scale);
        }

        public static Vector2 PlaceBottomCenterScreen(Rectangle rectangleToPlace, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            return new Vector2(Settings.ScreenWidth / 2 - rectangleToPlace.Width / 2 * (float)scale, Settings.ScreenHeight - rectangleToPlace.Height * (float)scale);
        }
        #region INNER
        public static Vector2 PlaceUpperLeftQuadrant(Rectangle backGroundRectangle, float? scale = null)
        {
            return new Vector2(backGroundRectangle.X + backGroundRectangle.Width / 4 * (float)scale,
                backGroundRectangle.Y + backGroundRectangle.Height / 4 * (float)scale);
        }

        public static Vector2 PlaceUpperRightQuadrant(Rectangle backGroundRectangle, Rectangle rectangleToPlace, float? scale = null)
        {
            return new Vector2(backGroundRectangle.X + backGroundRectangle.Width * (float)scale - backGroundRectangle.Width / 4 * (float)scale - rectangleToPlace.Width * (float)scale,
                backGroundRectangle.Y + backGroundRectangle.Height / 4 * (float)scale);
        }

        public static Vector2 PlaceBottomLeftQuadrant(Rectangle backGroundRectangle, float? scale = null)
        {
            scale = scale ?? Settings.GameScale;
            return new Vector2(backGroundRectangle.X + backGroundRectangle.Width / 4 * (float)scale,
                backGroundRectangle.Y + backGroundRectangle.Height * (float)scale - backGroundRectangle.Height / 4 * (float)scale);
        }

        public static Vector2 PlaceBottomRightQuadrant(Rectangle backGroundRectangle, Rectangle rectangleToPlace, float? scale = null)
        {
            return new Vector2(backGroundRectangle.X + backGroundRectangle.Width * (float)scale - backGroundRectangle.Width / 4 * (float)scale - rectangleToPlace.Width * (float)scale,
                backGroundRectangle.Y + backGroundRectangle.Height * (float)scale - backGroundRectangle.Height / 4 * (float)scale);
        }
        #endregion



        /// <summary>
        ///  <see cref="EscButton"/> 
        /// </summary>
        public static Vector2 PlaceRectangleAtTopRightOfParentRectangle(Rectangle parentRectangle, Rectangle rectangleToPlace)
        {
            return new Vector2(parentRectangle.X + parentRectangle.Width - rectangleToPlace.Width,
                parentRectangle.Y);
        }

        /// <summary>
        ///  <see cref="EscButton"/> 
        /// </summary>
        public static Vector2 PlaceRectangleAtBottomLeftOfParentRectangle(Rectangle parentRectangle, Rectangle rectangleToPlace)
        {
            return new Vector2(parentRectangle.X ,
                parentRectangle.Y + parentRectangle.Height - rectangleToPlace.Height * 2);
        }

        public static Rectangle GetEntireSourceRectangle(Texture2D texture)
        {
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }

        
    }
}
