using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace TiledEngine.Classes.Helpers
{
    public static class TileLocationHelper
    {

        /// <summary>
        /// Gets a "ring" of clear points around a tile. Tile may span more than a tile. 
        /// </summary>
        /// <returns></returns>
        public static List<Point> GetAdjacentClearTilesAsPoints(PathGrid pathGrid, Tile tileToGetAdjacentPointsOf)
        {
            List<Point> clearPoints = new List<Point>();
            Rectangle body = tileToGetAdjacentPointsOf.DestinationRectangle;
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);

            if (bodyTilesWide < 1)
                bodyTilesWide = 1;

            if (bodyTilesHigh < 1)
                bodyTilesHigh = 1;
            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            for (int i = -1; i < bodyTilesWide; i++)
            {
                for (int j = -1; j < bodyTilesHigh; j++)
                {
                    int newX = rectangleIndex.X + i;
                    int newY = rectangleIndex.Y + j;
                    if (pathGrid.X_IsValidIndex(newX) && pathGrid.Y_IsValidIndex(newY))
                    {
                        if(pathGrid.IsClear(newX, newY))
                        {
                            clearPoints.Add(new Point(newX, newY)); 
                        }
                    }

                }
            }

            return clearPoints;

        }
        /// <summary>
        /// Updates the pathgrid for when an object will span more than a single tile
        /// </summary>
        internal static void UpdateMultiplePathGrid(TileManager tileManager,Rectangle body)
        {
            //how many tiles this body spans
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);
            if (bodyTilesWide < 1)
                bodyTilesWide = 1;

            if (bodyTilesHigh < 1)
                bodyTilesHigh = 1;
            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            for (int i = 0; i < bodyTilesWide; i++)
            {
                for (int j = 0; j < bodyTilesHigh; j++)
                {
                    tileManager.UpdateGrid(rectangleIndex.X + i, rectangleIndex.Y + j, GridStatus.Obstructed);

                }
            }
        }

        private static int GetTilesHigh(Rectangle body)
        {
            return (int)(((float)body.Height / (float)Settings.TileSize) + .5f);
        }

        private static int GetTilesWide(Rectangle body)
        {
            return (int)(((float)body.Width / (float)Settings.TileSize) + .5f);
        }
    }
}
