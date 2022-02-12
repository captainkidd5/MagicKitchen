using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Globals.Classes.Settings;

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

        /// <summary>
        /// Returns direction of Entity 2 in comparison to Entity 1 (ex if entity 2 is right of entity 1, return direction.right)
        /// </summary>
        public static Direction GetDirectionOfEntityInRelationToEntity(Vector2 entity1, Vector2 entity2)
        {
            Direction directionToReturn = Direction.None;
            double degrees = MathHelper.ToDegrees((float)Math.Atan2(entity1.Y - entity2.Y, entity2.X - entity1.X)) ;
            degrees = (degrees + 360) % 360;
            if (degrees < 135 && degrees >= 45)
                return Direction.Up;
            else if (degrees < 315 && degrees >= 225)
                return Direction.Down;
            else if (degrees < 225 && degrees >= 135)
                return Direction.Left;
            else if ((degrees < 45 && degrees >= 0) || (degrees >= 315))
                return Direction.Right;

            if (directionToReturn == Direction.None)
                throw new Exception("Direction not found");
            return directionToReturn;
        }

        public static Direction GetOppositeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;

                default:
                    throw new Exception($"Directoin {dir} is invalid");
            }
        }
        /// <summary>
        /// Returns a scale to be used within a sprite. Basically a substiute for a destination rectangle
        /// </summary>
        /// <param name="currentSpriteSize">Unscaled size of your sprite</param>
        /// <param name="desiredSize">size to scale up to</param>
        /// <returns></returns>
        public static Vector2 GetScaleFromRequiredDimensions(Rectangle currentSpriteSize, Rectangle desiredSize)
        {
            return new Vector2(desiredSize.Width/currentSpriteSize.Width, desiredSize.Height/currentSpriteSize.Height);
        }

        public static void WriteVector2(BinaryWriter writer, Vector2 val)
        {
            writer.Write(val.X);
            writer.Write(val.Y);
        }

        public static Vector2 ReadVector2(BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
