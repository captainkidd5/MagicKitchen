using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.TileAddons;

namespace TiledEngine.Classes.Helpers
{
    public static class TileRectangleHelper
    {
        /// <summary>
        /// Cut out a rectangle from the tileset based on the tile GID.
        /// </summary>
        /// <param name="gid">Tile gid seen on the tileset</param>
        /// <param name="tileSetWidth">Width of tileset in units of tiles.</param>
        /// <returns></returns>
        public static Rectangle GetNormalSourceRectangle(int gid, int tileSetWidth)
        {
        
            int Column = gid % tileSetWidth;
            int Row = (int)Math.Floor((float)gid / (float)tileSetWidth);
            int tileWidth = Settings.TileSize;
            return new Rectangle(tileWidth * Column, tileWidth * Row, tileWidth, tileWidth);
        }

        /// <summary>
        /// Cut out a rectangle from the tileset based on the tile GID.
        /// </summary>
        /// <param name="gid">Tile gid seen on the tileset</param>
        /// <param name="tileSetDimension">Width of tileset in units of tiles.</param>
        /// <returns></returns>
        public static Rectangle GetBackgroundSourceRectangle(int gid, int tileSetDimension)
        {
            int Column = (gid % tileSetDimension);
            int Row = (int)Math.Floor((float)gid / (float)tileSetDimension);
            int tileWidth = Settings.TileSize;

            return new Rectangle((tileWidth * Column) + (3 * Column) + 1, (tileWidth * Row) + (3 * Row) + 1, tileWidth, tileWidth);
        }

        public static Rectangle GetDestinationRectangle(TileData tileData)
        {
            return new Rectangle((int)(tileData.X * Settings.TileSize),
                (int)(tileData.Y * Settings.TileSize), Settings.TileSize, Settings.TileSize);
        }

        public static Rectangle AdjustSourceRectangle(Rectangle oldRectangle, Rectangle newSourceRectangle)
        {
            Rectangle originalRectangle = oldRectangle;

            return new Rectangle(originalRectangle.X + newSourceRectangle.X, originalRectangle.Y + newSourceRectangle.Y,
                newSourceRectangle.Width, newSourceRectangle.Height);

        }

        public static Rectangle AdjustDestinationRectangle(Rectangle originalDestinationRectangle, Rectangle sourceRectangle)
        {
            return new Rectangle(originalDestinationRectangle.X + sourceRectangle.X, originalDestinationRectangle.Y + sourceRectangle.Y,
           sourceRectangle.Width, sourceRectangle.Height);
        }

       
       
    }
}
