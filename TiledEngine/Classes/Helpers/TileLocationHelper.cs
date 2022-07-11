using DataModels.ItemStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.TileAddons;
using static DataModels.Enums;
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

        public bool IsAdjacentTo(TileData tileData, Vector2 position)
        {
            Vector2 tilePos = Vector2Helper.GetWorldPositionFromTileIndex(tileData.X, tileData.Y);
            float distance = Vector2.Distance(position, tilePos + new Vector2(8,8));

            return distance < Settings.TileSize * 1.5f;
           
        }
        public bool IsOnTopOf(TileData tileData, Vector2 position)
        {
            return tileData.GetPoint() ==  Vector2Helper.WorldPositionToTilePositionAsPoint(position);
        }
        /// <summary>
        /// Gets a "ring" of clear points around a tile. Tile may span more than a tile. 
        /// </summary>
        /// <param name="requireAdjacency">Set to true to only return adjacent clear tiles</param>
        /// <returns></returns>
        public List<Point> GetSorroundingClearTilesAsPoints(TileObject tileToGetAdjacentPointsOf,
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



        
        internal bool MayPlaceTile(Item item,Rectangle destinationRectangleToPlace, Layers currentTileLayer)
        {
            Rectangle body = destinationRectangleToPlace;
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);


            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            //Criteria for placing background tiles is different. All tiles in layers higher than desired placement must be empty.
            //We are not concerned if the tile we are replacing has an object (water edges have objects, but can be replaced)
            for (int z = _tileManager.TileData.Count - 1; z > (int)currentTileLayer; z--)
            {

                for (int i = 0; i < bodyTilesWide; i++)
                {
                    for (int j = 0; j < bodyTilesHigh; j++)
                    {

                        Point point = new Point(rectangleIndex.X + i, rectangleIndex.Y + j);

                        if (_tileManager.X_IsValidIndex(rectangleIndex.X + i) && _tileManager.Y_IsValidIndex(rectangleIndex.Y + j))
                        {

                            if (currentTileLayer > Layers.midground)
                            {
                                if (!_tileManager.PathGrid.IsClear(point.X, point.Y))
                                    return false;
                            }
                            ushort val = _tileManager.GetTileDataFromPoint(point, (Layers)z).Value.GID;

                            bool empty = _tileManager.GetTileDataFromPoint(point, (Layers)z).Value.Empty;
                            if (!empty)
                                return false;
                        }

             

                        if (IsPlacementTypeBlacklisted(point, item))
                            return false;

                    }
                }
            }

            return true;
        }

        /// <summary>
        /// If item has a specified list of allowed tile types (no value provided defaults to land ok), then if this tiles property
        /// is not contained in that list of allowed types, we shouldn't be allowed to place here.
        /// </summary>
        /// <param name="tilePoint"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsPlacementTypeBlacklisted(Point tilePoint, Item item)
        {
            string tilingSetValueString = _tileManager.GetTileDataFromPoint(tilePoint, Layers.background).Value.GetProperty(_tileManager.TileSetPackage,"tilingSet");

            if (!string.IsNullOrEmpty(tilingSetValueString))
            {
                AllowedPlacementTileType placementType = (AllowedPlacementTileType)Enum.Parse(typeof(AllowedPlacementTileType), tilingSetValueString);

                if (!item.AllowedPlacementTileTypes.Contains(placementType))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Updates the pathgrid for when an object will span more than a single tile
        /// </summary>
        internal void UpdateMultiplePathGrid(Rectangle body, GridStatus gridStatus)
        {
            //how many tiles this body spans
            int bodyTilesWide = GetTilesWide(body);
            int bodyTilesHigh = GetTilesHigh(body);

        

            //if x starts at 1605, and x.width == 16, then x spans two tiles


            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));

            //If tile body is less than or equal to tilesize, but is offset, it may still span 2 tiles
            if (bodyTilesWide == 1)
            {
                int remainderX = body.X / 16;
                int remainderX1 = (body.X + body.Width - 1) / 16;
                if (remainderX != remainderX1)
                    bodyTilesWide++;

            }
            if (bodyTilesHigh == 1)
            {
                int remainderX = body.Y / 16;
                int remainderX1 = (body.Y + body.Height - 1) / 16;
                if (remainderX != remainderX1)
                    bodyTilesHigh++;
            }

            for (int i = 0; i < bodyTilesWide; i++)
            {
                for (int j = 0; j < bodyTilesHigh; j++)
                {
                    int newX = rectangleIndex.X + i;
                    int newY = rectangleIndex.Y + j;  
                    if (newX >= 0 && rectangleIndex.Y >= 0)
                        _tileManager.UpdateGrid(newX, newY, gridStatus);

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
                        if (Y_IsValidIndex(entityIndexPosition.Y + y))
                        {
                            TileData? tile = _tileManager.GetTileDataFromPoint(new Point(entityIndexPosition.X + x,
                                entityIndexPosition.Y + y), layerToSearch);

                            if (tile.Value.GID == gid)
                                tilesFound.Add(tile.Value.GetPoint());

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

        /// <summary>
        /// Gets a random clear tile outside of the viewport, but within play area
        /// </summary>
        public Vector2? RandomClearPositionWithinRange(Random random, int tries = 10, string tileType = null)
        {

            Vector2 chosenPos = Vector2.Zero;

            Direction dir = Vector2Helper.GetRandomDirection();

            switch (dir)
            {
                case Direction.None:
                    throw new Exception($"Invalid spawn direction");
                    break;
                case Direction.Up:
                    chosenPos = new Vector2(random.Next(Settings.SpawnableAreaX, Settings.SpawnableAreaX + Settings.SpawnableAreaWidth),
                  random.Next(Settings.SpawnableAreaY, Settings.SpawnableAreaY + Settings.SpawnableAreaWidth - Settings.ActiveAreaWidth));
                    break;
                case Direction.Down:
                    chosenPos = new Vector2(random.Next(Settings.SpawnableAreaX, Settings.SpawnableAreaX + Settings.SpawnableAreaWidth),
                  random.Next(Settings.ActiveAreaY + Settings.ActiveAreaWidth, Settings.SpawnableAreaY + Settings.SpawnableAreaWidth));
                    break;
                case Direction.Left:
                    chosenPos = new Vector2(random.Next(Settings.SpawnableAreaX, Settings.ActiveAreaX),
                  random.Next(Settings.SpawnableAreaY, Settings.SpawnableAreaY + Settings.SpawnableAreaWidth));
                    break;
                case Direction.Right:
                    chosenPos = new Vector2(random.Next(Settings.ActiveAreaX + ActiveAreaWidth, Settings.SpawnableAreaX + Settings.SpawnableAreaWidth),
                  random.Next(Settings.SpawnableAreaY, Settings.SpawnableAreaY + Settings.SpawnableAreaWidth));
                    break;
            }

            for (int i = 0; i < tries; i++)
            {

                Point point = Vector2Helper.GetTileIndexPosition(chosenPos);
                if (!X_IsValidIndex(point.X) || !Y_IsValidIndex(point.Y))
                    continue;
                if (_tileManager.PathGrid.IsClear(point.X, point.Y))
                {
                    if (tileType != null)
                    {
                        if (!_tileManager.IsTypeOfTile(tileType, chosenPos))
                            return null;
                    }
                    return chosenPos;

                }
            }

            return null;
        }

        /// <summary>
        /// Ensures X index is greater than zero and less than bounds of grid
        /// </summary>
        internal bool X_IsValidIndex( int x)
        {
            if (x >= 0)
                if (x < _tileManager.TileData[0].GetLength(0))
                    return true;
            return false;
        }

        /// <summary>
        /// Ensures Y index is greater than zero and less than bounds of grid
        /// </summary>
        internal bool Y_IsValidIndex(int y)
        {
            if (y >= 0)
                if (y < _tileManager.TileData[0].GetLength(1))
                    return true;
            return false;
        }
    }
}
