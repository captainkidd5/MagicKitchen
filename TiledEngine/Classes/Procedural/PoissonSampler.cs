using DataModels.MapStuff;
using Globals.Classes;
using Globals.Classes.Chance;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.TileAddons;
using static DataModels.Enums;

namespace TiledEngine.Classes.Procedural
{
    internal class PoissonSampler
    {
        private const int CellSize = 16; //standard tile size
        //private int _minDistance; //Minimum distance points must be separated from one another
        //private int _maxDistance;//Maximum distance a point can be from at least one other point


        private List<Point> _activeSamples;


        public PoissonSampler()
        {

            _activeSamples = new List<Point>();
        }



        public void Generate(PoissonData poissonData, Layers layerToCheckIfEmpty,
            TileManager tileManager)
        {
            //generate first point randomly within grid
            List<Point> points = new List<Point>();
            for (int z = 0; z <8; z++)
            {
                _activeSamples.Add(new Point(Settings.Random.Next(0, tileManager.TileData[0].GetLength(0) - 1),
               Settings.Random.Next(0, tileManager.TileData[0].GetLength(0) - 1)));
            }


            while (_activeSamples.Count > 0)
            {
                List<Point> currentCluster = new List<Point>();

                int sampleIndex = Settings.Random.Next(0, _activeSamples.Count - 1); //pick random sample within activesample list
                Point spawnCenter = _activeSamples[sampleIndex];


                bool found = false;

                Point newPoint = new Point(0, 0);
                int currentClusterCount = 0;
                for (int k = 0; k < poissonData.Tries; k++) //try MaxK times to find a valid point
                {
                    if (currentClusterCount > poissonData.MaxCluster)
                        break;
                    int newX = spawnCenter.X + poissonData.MinDistance * ChanceHelper.GetNevagiveOrPositive1();
                    int newY = spawnCenter.Y + poissonData.MinDistance * ChanceHelper.GetNevagiveOrPositive1();
                    newPoint = new Point(newX, newY);

                    if (tileManager.IsValidPoint(newPoint))
                    {

                        bool correctTileType = CheckIfTileIsAllowedTileType(poissonData, tileManager, newPoint);
                        if (!correctTileType)
                            continue;

                        if (IsFarEnough(tileManager, newPoint, poissonData.MinDistance, poissonData.MaxDistance))
                        {


                            if (layerToCheckIfEmpty == Layers.background)
                            {
                                found = true;
                                if (currentClusterCount < poissonData.MaxCluster)
                                    _activeSamples.Add(newPoint);
                                tileManager.TileData[(int)poissonData.LayersToPlace][newPoint.X, newPoint.Y].GID = (ushort)poissonData.GID;
                                currentClusterCount++;
                            }
                            else
                            {
                                // if (tileManager.TileData[(int)poissonData.LayersToPlace][newPoint.X, newPoint.Y].Empty)
                                // {
                                found = true;
                                if(currentClusterCount < poissonData.MaxCluster)
                                _activeSamples.Add(newPoint);
                                tileManager.TileData[(int)poissonData.LayersToPlace][newPoint.X, newPoint.Y].GID = (ushort)poissonData.GID;
                                //}
                                currentClusterCount++;

                            }

                        }

                    }




                }

                if (!found)
                {
                    if (_activeSamples.Count < 1)
                        continue;

                    // _activeSamples[sampleIndex] = _activeSamples[_activeSamples.Count - 1];
                    _activeSamples.RemoveAt(sampleIndex);
                    continue;
                }

     
            }

            return;

        }

        /// <summary>
        /// For example, limpet rock must be placed in shallow water
        /// </summary>
        /// <returns></returns>
        private static bool CheckIfTileIsAllowedTileType(PoissonData poissonData, TileManager tileManager, Point newPoint)
        {
            bool correctTileType = false;
            foreach (string tileType in poissonData.AllowedTilingSets)
            {
                if (tileManager.IsTypeOfTile(tileType, newPoint))
                {
                    correctTileType = true;
                    break;
                }

            }

            return correctTileType;
        }

        private bool IsFarEnough(TileManager tileManager, Point sample, int minDistance, int maxDistance)
        {
            if (!tileManager.IsValidPoint(sample))
                return false;
            int startingX = Math.Abs(sample.X - minDistance);
            int startingY = Math.Abs(sample.Y - minDistance);


            int endingIndexX = sample.X + minDistance;
            int endingIndexY = sample.Y + minDistance;
            int max = tileManager.TileData[0].GetLength(0) - 1;

            if (endingIndexX >= max)
                endingIndexX = max - 1;
            if (endingIndexY >= max)
                endingIndexY = max - 1;



            for (int i = startingX; i < endingIndexX; i++)
            {
                for (int j = startingY; j < endingIndexY; j++)
                {
                    ushort gid = tileManager.TileData[3][i, j].GID;
                    if (tileManager.PathGrid.Weight[i, j] == (byte)GridStatus.Obstructed || !tileManager.TileData[3][i, j].Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
