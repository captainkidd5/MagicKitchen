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

        public int MaxValue { get { return PathGrid.Weight.Length; } }
        public Navigator(string debugName)
        {
            DebugName = debugName;
        }


        public void Load(PathGrid pathGrid)
        {
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
        /// Returns velocity which moves towards the next point
        /// </summary>
        public void FollowPath(GameTime gameTime,Vector2 currentPos, ref Vector2 velocity)
        {
            //Reached destination!
            if (CurrentPath.Count == 0)
            {
                HasActivePath = false;
                return;
            }

            //Make sure next destination point is at the center of the tile, not the top left corner, so we add half the tile's width to 
            //both x and y
            if (MoveTowardsVector(GetCentralizedWorldPositionFromPathPoint(CurrentPath.Count - 1), currentPos, ref velocity, gameTime))
            {
                //reached the next tile
                CurrentPath.RemoveAt(CurrentPath.Count - 1);

            }
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
            CurrentPath = PathFinder.FindPath(Vector2Helper.WorldPositionToTilePositionAsPoint(currentPosition),
                Vector2Helper.WorldPositionToTilePositionAsPoint(targetTestPos), DebugName);
            if (CurrentPath == null)
                return false;
            return true;
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

        public virtual bool MoveTowardsVector(Vector2 goal, Vector2 currentPos, ref Vector2 velocity, GameTime gameTime)
        {
            // If we're already at the goal return immediatly

            if (Vector2Helper.WithinRangeOf(currentPos, goal, ErrorMargin))
                return true;

            // Find direction from current MainHull.Position to goal
            Vector2 direction = Vector2.Normalize(goal - currentPos);

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - currentPos)) + 1) < 0.1f)
                currentPos = goal;

            // Return whether we've reached the goal or not, leeway of 2 pixels 
            if (currentPos.X + ErrorMargin > goal.X && currentPos.Y - ErrorMargin < goal.X
               && currentPos.Y + ErrorMargin > goal.Y && currentPos.Y - ErrorMargin < goal.Y)
            {
                return true;
            }
            velocity = direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds * .05f;

            return false;
        }
    }
}
