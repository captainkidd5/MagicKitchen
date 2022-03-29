﻿using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
            if(tileSetWidth != 100)
                Console.WriteLine("test");
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
            //if (gid == -1 || gid == 0)
            //    return NoTextureRectangle;
            int Column = (gid % tileSetDimension);
            int Row = (int)Math.Floor((float)gid / (float)tileSetDimension);
            int tileWidth = Settings.TileSize;

            return new Rectangle((tileWidth * Column) + (3 * Column) + 1, (tileWidth * Row) + (3 * Row) + 1, tileWidth, tileWidth);


        }

        public static Rectangle GetDestinationRectangle(Tile tile)
        {
            return new Rectangle((int)(tile.X * Settings.TileSize),
                (int)(tile.Y * Settings.TileSize), Settings.TileSize, Settings.TileSize);
        }

        public static Rectangle AdjustSourceRectangle(Rectangle oldRectangle, Rectangle newSourceRectangle)
        {
            Rectangle originalRectangle = oldRectangle;

            return new Rectangle(originalRectangle.X + newSourceRectangle.X, originalRectangle.Y + newSourceRectangle.Y,
                newSourceRectangle.Width, newSourceRectangle.Height);

        }

        public static Rectangle AdjustDestinationRectangle(Tile tile, Rectangle sourceRectangle)
        {
            return new Rectangle(tile.DestinationRectangle.X + sourceRectangle.X, tile.DestinationRectangle.Y + sourceRectangle.Y,
           sourceRectangle.Width, sourceRectangle.Height);
        }

       
    }
}
