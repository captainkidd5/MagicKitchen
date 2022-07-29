using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.PathFinding.PatherFinder;
using PhysicsEngine.Classes.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsEngine.Classes.Pathfinding
{
    public class Navigator
    {
        private string DebugName { get; set; }
        private List<PathFinderNode> CurrentPath { get; set; }
        private Vector2 TargetPosition { get; set; }
        private PathFinderFast PathFinder { get; set; }
        private PathGrid PathGrid { get; set; }
        public bool HasActivePath { get; private set; }

        private int ErrorMargin { get; set; } = 2;


        public int MaxValue { get { return PathGrid.Weight.GetLength(0); } }

        public bool IsClear(Point tilePoint) => PathGrid.IsClear(tilePoint.X, tilePoint.Y);

        public Point? NearestClearPoint(Point startingPoint, int searchRadius) => PathGrid.NearestClearPointTo(startingPoint, searchRadius);

        public Navigator(string debugName)
        {
            DebugName = debugName;
        }


        public void Load(PathGrid pathGrid)
        {
            if (pathGrid == null)
                throw new Exception("pathgrid may not be null");
            PathGrid = pathGrid;
            PathFinder = new PathFinderFast(PathGrid.Weight);

    
        }


        public void Unload()
        {
            HasActivePath = false;
            if(CurrentPath != null)
               CurrentPath.Clear();
            TargetPosition = Vector2.Zero;
        }
        /// <summary>
        /// Returns true if reached final destination
        /// </summary>
        public bool FollowPath(GameTime gameTime,Vector2 currentPos, ref Vector2 velocity)
        {
            //Reached destination!
            if (CurrentPath.Count == 0)
            {
                HasActivePath = false;
                return true;
            }

            //Make sure next destination point is at the center of the tile, not the top left corner, so we add half the tile's width to 
            //both x and y
            if (Vector2Helper.MoveTowardsVector(GetCentralizedWorldPositionFromPathPoint(CurrentPath.Count - 1),
                currentPos, ref velocity, gameTime, ErrorMargin))
            {
                //reached the next tile
                CurrentPath.RemoveAt(CurrentPath.Count - 1);

            }
            return false;
        }
        public void SetTarget(Vector2 targetPosition)
        {
            TargetPosition = targetPosition;
            HasActivePath = true;
        }

        /// <summary>
        /// Returns a vector2 representing the tile in worldspace from the path point. Gets the center of the tile.
        /// </summary>
        /// <param name="pathIndex"></param>
        /// <returns></returns>
        private Vector2 GetCentralizedWorldPositionFromPathPoint(int pathIndex)
        {
            Vector2 posNoAddition = Vector2Helper.GetWorldPositionFromTileIndex(CurrentPath[pathIndex].X, CurrentPath[pathIndex].Y);
            return new Vector2(posNoAddition.X + Settings.TileSize / 2, posNoAddition.Y + Settings.TileSize /2);
        }
        /// <summary>
        /// Sets the current path property with a list of coordinates indicating open tiles
        /// </summary>
        /// <returns>Returns false if unable to find valid path</returns>
        public bool FindPathTo(Vector2 currentPosition, Vector2 targetTestPos)
        {
            PathFinder.SearchLimit = 100;
            CurrentPath = PathFinder.FindPath(Vector2Helper.WorldPositionToTilePositionAsPoint(currentPosition),
                Vector2Helper.WorldPositionToTilePositionAsPoint(targetTestPos), DebugName);
            if (CurrentPath == null)
                return false;
            return true;
        }

        /// <summary>
        /// Returns number of points required between two positions
        /// </summary>

        /// <returns>Returns -1 if no available path found</returns>
        public int PathDistance(Point currentPosition, Point targetTestPos)
        {
            List<PathFinderNode> pathList = PathFinder.FindPath(currentPosition, targetTestPos, DebugName);
            if (pathList == null)
                return -1;
            else
                return pathList.Count;
        }

        /// <summary>
        /// Draws a line between each position on the NPCs current path, resulting in a long path line representing their path. Does not draw already-traversed path
        /// </summary>
        public void DrawDebug(SpriteBatch spriteBatch, Color color)
        {
            if (HasActivePath)
                for (int i = 0; i < CurrentPath.Count - 1; i++)
                    LineUtility.DrawLine(null, spriteBatch, GetCentralizedWorldPositionFromPathPoint(i), GetCentralizedWorldPositionFromPathPoint(i + 1), color);             
        }


    }
}
