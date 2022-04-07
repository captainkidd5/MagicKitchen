using Globals;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsEngine.Classes.Pathfinding
{
    public enum GridStatus
    {
        Clear = 1,
        Obstructed = 0,
    }


    /// <summary>
    /// A class which lays out a grid of 0's and 1's for every base tile on the map. If any sort of impassable tile object resides on that square,
    /// this class should be aware of it and update its grid status to 1 (obstructed). Conversely, if an object is cleared from that location,
    /// this classs should be aware of it and update gridstatus back to 0 (unobstructed).
    /// </summary>
    public class PathGrid : IDebuggable
    {

        private Rectangle Size;

        public byte[,] Weight { get; private set; }


        /// <param name="mapTilesWide">The width of the map, in TILES</param>
        /// <param name="mapTilesHigh">The height of the map, in TILES</param>
        public PathGrid(int mapTilesWide, int mapTilesHigh)
        {
            Size = new Rectangle(0, 0, mapTilesWide, mapTilesHigh);
            Weight = new byte[mapTilesWide, mapTilesHigh];
            for (int i = 0; i < mapTilesWide; i++)
            {
                for (int j = 0; j < mapTilesHigh; j++)
                {
                    Weight[i, j] = (int)GridStatus.Clear;
                }
            }

        }

        //0 empty, 1 obstructed
        public void UpdateGrid(int indexI, int indexJ, GridStatus newValue)
        {
            if (indexI >= 0)
            {
                if (indexJ >= 0)
                {
                    if (indexI < Weight.GetLength(0) && indexJ < Weight.GetLength(1))
                        Weight[indexI, indexJ] = (byte)newValue;
                    return;
                }

            }
            //throw new IndexOutOfRangeException("Must specify two indicies which are within the bounds of " + Weight.GetLength(0) + " and " + Weight.GetLength(1));
        }


        /// <returns>Returns true if specified index is not obstructed</returns>
        public bool IsClear(int indexI, int indexJ)
        {
            if (indexI >= 0)
            {
                if (indexJ >= 0)
                {
                    if (indexI < Weight.GetLength(0) && indexJ < Weight.GetLength(1))
                    {
                        if (Weight[indexI, indexJ] == (int)GridStatus.Clear)
                            return true;
                        return false;
                    }

                }

            }
            throw new IndexOutOfRangeException("Must specify two indicies which are within the bounds of " + Weight.GetLength(0) + " and " + Weight.GetLength(1));
        }

        public void DrawDebug(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Weight.GetLength(0); i++)
            {
                for (int j = 0; j < Weight.GetLength(1); j++)
                {
                    //if (Weight[i, j] == (int)GridStatus.Obstructed)
                        //spriteBatch.Draw(Settings.DebugTexture, new Rectangle(i * Settings.TileSize, j * Settings.TileSize, Settings.TileSize, Settings.TileSize), null, Color.Red, 0f, Vector2.One, SpriteEffects.None, layerDepth: .99f);
                }
            }
        }

    }
}
