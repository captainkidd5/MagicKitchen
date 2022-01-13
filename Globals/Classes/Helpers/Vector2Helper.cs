using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes.Helpers
{
    public static class Vector2Helper
    {
        public static Vector2 GetVector2FromRectangle(Rectangle rectangle)
        {
            return new Vector2((int)rectangle.X, (int)rectangle.Y);
        }

        public static Point GetTileIndexPosition(Vector2 worldPosition)
        {
            int x = (int)worldPosition.X / Settings.TileSize;
            if (x < 0)
                x = 0;
            int y = (int)worldPosition.Y / Settings.TileSize;
            if (y < 0)
                y = 0;
            return new Point(x, y);
        }

        public static Vector2 GetWorldPositionFromTileIndex(int tileX, int tileY)
        {
            return new Vector2(tileX * Settings.TileSize, tileY * Settings.TileSize);
        }
        public static Vector2 WorldPositionToTilePosition(Vector2 worldPos)
        {
            return new Vector2((int)(worldPos.X / Settings.TileSize), (int)(worldPos.Y / Settings.TileSize));
        }
        public static Point WorldPositionToTilePositionAsPoint(Vector2 worldPos)
        {
            return new Point((int)(worldPos.X / Settings.TileSize), (int)(worldPos.Y / Settings.TileSize));
        }

        /// <summary>
        /// Checks if two given positions are close to one another
        /// </summary>
        /// <param name="marginOfErrorInPixels">Larger the value, the less precise you have to be</param>
        /// <returns></returns>
        public static bool WithinRangeOf(Vector2 currentPos, Vector2 goal, int marginOfErrorInPixels = 2)
        {
            if (currentPos.X + marginOfErrorInPixels > goal.X && currentPos.X - marginOfErrorInPixels < goal.X
                && currentPos.Y + marginOfErrorInPixels > goal.Y && currentPos.Y - marginOfErrorInPixels < goal.Y)
            {
                return true;
            }
            return false;
        }
    }
}
