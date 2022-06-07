using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.Helpers
{
    public class TileLocationHelper
    {
        private readonly TileManager _tileManager;

        public TileLocationHelper(TileManager tileManager)
        {
            _tileManager = tileManager;
        }
        /// <summary>
        /// Gets a "ring" of clear points around a tile. Tile may span more than a tile. 
        /// </summary>
        /// <param name="requireAdjacency">Set to true to only return adjacent clear tiles</param>
        /// <returns></returns>
        public List<Point> GetSorroundingClearTilesAsPoints(Tile tileToGetAdjacentPointsOf,
            bool requireAdjacency = true)
        {
            List<Point> clearPoints = new List<Point>();
            Rectangle body = tileToGetAdjacentPointsOf.DestinationRectangle;
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);


            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            for (int i = -1; i < bodyTilesWide + 1; i++)
            {
                for (int j = -1; j < bodyTilesHigh + 1; j++)
                {

                    if (requireAdjacency)
                    {
                        //Top left, top right, bottom left, bottom right, ignore!
                        if ((i == -1 && j == -1) || (i == bodyTilesWide && j == -1)
                            || (i == -1 && j == bodyTilesHigh)
                            || (i == bodyTilesWide && j == bodyTilesHigh))
                        {
                            continue;
                        }
                    }
                    int newX = rectangleIndex.X + i;
                    int newY = rectangleIndex.Y + j;

                    if (_tileManager.PathGrid.X_IsValidIndex(newX) && _tileManager.PathGrid.Y_IsValidIndex(newY))
                    {
                        if (_tileManager.PathGrid.IsClear(newX, newY))
                        {

                            clearPoints.Add(new Point(newX, newY));
                        }
                    }

                }
            }

            return clearPoints;

        }

        internal bool MayPlaceForegroundTile(Rectangle destinationRectangleToPlace)
        {
            Rectangle body = destinationRectangleToPlace;
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);

            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            for (int i = 0; i < bodyTilesWide; i++)
            {
                for (int j = 0; j < bodyTilesHigh; j++)
                {


                    if (_tileManager.PathGrid.X_IsValidIndex(rectangleIndex.X + i) && _tileManager.PathGrid.Y_IsValidIndex(rectangleIndex.Y + j))
                    {
                        if (!_tileManager.PathGrid.IsClear(rectangleIndex.X + i, rectangleIndex.Y + j))
                            return false;
                    }

                }
            }
            return true;
        }
        internal bool MayPlaceBackgroundTile(Rectangle destinationRectangleToPlace, Layers currentTileLayer)
        {
            Rectangle body = destinationRectangleToPlace;
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);

            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            //Criteria for placing background tiles is different. All tiles in layers higher than desired placement must be empty.
            //We are not concerned if the tile we are replacing has an object (water edges have objects, but can be replaced)
            for (int z = _tileManager.Tiles.Count - 1; z > (int)currentTileLayer; z--)
            {

                for (int i = 0; i < bodyTilesWide; i++)
                {
                    for (int j = 0; j < bodyTilesHigh; j++)
                    {


                        if (_tileManager.X_IsValidIndex(rectangleIndex.X + i) && _tileManager.Y_IsValidIndex(rectangleIndex.Y + j))
                        {
                            if (_tileManager.GetTileFromPoint(new Point(rectangleIndex.X + i, rectangleIndex.Y + j),(Layers)z).GID >0)
                                return false;
                        }

                    }
                }
            }

            return true;
        }
        /// <summary>
        /// Updates the pathgrid for when an object will span more than a single tile
        /// </summary>
        internal void UpdateMultiplePathGrid(Rectangle body, GridStatus gridStatus)
        {
            //how many tiles this body spans
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);



            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            for (int i = 0; i < bodyTilesWide; i++)
            {
                for (int j = 0; j < bodyTilesHigh; j++)
                {
                    _tileManager.UpdateGrid(rectangleIndex.X + i, rectangleIndex.Y + j, gridStatus);

                }
            }
        }

        private static int GetTilesHigh(Rectangle body)
        {
            int high = (int)(((float)body.Height / (float)Settings.TileSize) + .5f);
            if (high < 1)
                high = 1;
            return high;
        }

        private static int GetTilesWide(Rectangle body)
        {
            int wide = (int)(((float)body.Width / (float)Settings.TileSize) + .5f);
            if (wide < 1)
                wide = 1;
            return wide;
        }

        /// <summary>
        /// Locates tile at given layer within search radius.
        /// Searches in an expanding grid outwards from given point
        /// Returns null if no tile found matching specified gid
        /// </summary>ec
        /// <param name="gid"></param>
        /// <param name="layerToSearch"></param>
        /// <param name="entityIndexPosition"></param>
        /// <param name="searchRadius">5 would mean searching five tiles in all directions</param>
        /// <returns></returns>
        public List<Point> LocateTile_RadialSearch(int gid, Layers layerToSearch, Point entityIndexPosition, int searchRadius)
        {
            List<Point> tilesFound = new List<Point>();
            int row = 1;
            int col = 1;
            int x_startIndex = -1;
            int y_startIndex = -1;
            //Expanding Search from center
            for (int x = x_startIndex; x < row + 1; x++)
            {
                if (_tileManager.X_IsValidIndex(entityIndexPosition.X + x))
                {
                    for (int y = y_startIndex; y < col + 1; y++)
                    {
                        if (_tileManager.Y_IsValidIndex(entityIndexPosition.Y + y))
                        {
                            Tile tile = _tileManager.GetTileFromPoint(new Point(entityIndexPosition.X + x,
                                entityIndexPosition.Y + y), layerToSearch);

                            if (tile.GID == gid)
                                tilesFound.Add(Vector2Helper.GetTileIndexPosition(tile.Position));

                        }

                    }
                }
                if (row == searchRadius)
                    break;
                if (x == row)
                {
                    x_startIndex--;
                    x = x_startIndex;
                    row++;

                    y_startIndex--;
                    col++;

                }
            }
            return tilesFound;
        }
    }
}
