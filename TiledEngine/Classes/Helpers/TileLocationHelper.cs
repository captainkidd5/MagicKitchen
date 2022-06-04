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
        /// <param name="requireAdjacency">Set to true to only return adjacent clear tiles</param>
        /// <returns></returns>
        public static List<Point> GetSorroundingClearTilesAsPoints(PathGrid pathGrid, Tile tileToGetAdjacentPointsOf,
            bool requireAdjacency = true)
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
            for (int i = -1; i < bodyTilesWide + 1; i++)
            {
                for (int j = -1; j < bodyTilesHigh + 1; j++)
                {

                    if (requireAdjacency)
                    {
                        //Top left, top right, bottom left, bottom right, ignore!
                        if((i == -1 && j == -1) || (i == bodyTilesWide  && j == -1)
                            || (i == -1  && j == bodyTilesHigh )
                            || (i == bodyTilesWide && j == bodyTilesHigh))
                        {
                            continue;
                        }
                    }
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

        internal static bool MayPlaceTile(PathGrid pathGrid,Rectangle destinationRectangleToPlace)
        {
            Rectangle body = destinationRectangleToPlace;
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);

            if (bodyTilesWide < 1)
                bodyTilesWide = 1;

            if (bodyTilesHigh < 1)
                bodyTilesHigh = 1;
            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            for (int i = 0; i < bodyTilesWide; i++)
            {
                for (int j = 0; j < bodyTilesHigh ; j++)
                {


                    if (pathGrid.X_IsValidIndex(rectangleIndex.X + i) && pathGrid.Y_IsValidIndex(rectangleIndex.Y +j))
                    {
                        if (!pathGrid.IsClear(rectangleIndex.X + i, rectangleIndex.Y + j))
                            return false;
                    }

                }
            }
            return true;
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
