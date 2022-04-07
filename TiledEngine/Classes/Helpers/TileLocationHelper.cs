using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace TiledEngine.Classes.Helpers
{
    internal static class TileLocationHelper
    {
        /// <summary>
        /// Updates the pathgrid for when an object will span more than a single tile
        /// </summary>
        internal static void UpdateMultiplePathGrid(TileManager tileManager,Rectangle body)
        {
            //how many tiles this body spans
            int bodyTilesWide = (int)(((float)body.Width / (float)Settings.TileSize) + .5f);

            int bodyTilesHigh = (int)(((float)body.Height / (float)Settings.TileSize) + .5f);
            if (bodyTilesWide < 1)
                bodyTilesWide = 1;

            if (bodyTilesHigh < 1)
                bodyTilesHigh = 1;
            Point rectangleIndex = Vector2Helper.WorldPositionToTilePositionAsPoint(new Vector2(body.X, body.Y));
            for (int i = 0; i < bodyTilesWide; i++)
            {
                for (int j = 0; j < bodyTilesHigh; j++)
                {
                    tileManager.UpdateGrid( rectangleIndex.X + i, rectangleIndex.Y + j, GridStatus.Obstructed);

                }
            }
        }
    }
}
