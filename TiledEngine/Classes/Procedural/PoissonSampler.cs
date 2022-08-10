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
        public Rectangle GridRectangle { get; set; }


        public PoissonSampler(int minDistance, int maxDistance)
        {

            _activeSamples = new List<Point>();
        }

        public void Generate(PoissonData poissonData, Layers layerToCheckIfEmpty,
            TileManager tileManager)
        {
            //generate first point randomly within grid

            for (int i = 0; i < 120; i++)
            {

                _activeSamples.Add(new Point(Settings.Random.Next(0, tileManager.TileData[0].GetLength(0) - 1),
                    Settings.Random.Next(0, tileManager.TileData[0].GetLength(0) - 1)));
            }

            while (_activeSamples.Count > 0)
            {
                int sampleIndex = Settings.Random.Next(0, _activeSamples.Count - 1); //pick random sample within activesample list
                Point sample = _activeSamples[sampleIndex];


                bool found = false;


                for (int k = 0; k < poissonData.Tries; k++) //try MaxK times to find a valid point
                {

                    int newX = sample.X + poissonData.MinDistance * ChanceHelper.GetNevagiveOrPositive1();
                    int newY = sample.Y + poissonData.MinDistance * ChanceHelper.GetNevagiveOrPositive1();
                    Point newPoint = new Point(newX, newY);

                    if (tileManager.IsValidPoint(newPoint))
                    {
                        if (tileManager.TileData[(int)poissonData.LayersToPlace][newPoint.X, newPoint.Y].Empty)
                        {
                            if (IsFarEnough(tileManager, newPoint, poissonData.MinDistance, poissonData.MaxDistance))
                            {


                                if (layerToCheckIfEmpty == Layers.background)
                                {
                                    found = true;
                                    _activeSamples.Add(newPoint);
                                    tileManager.TileData[(int)poissonData.LayersToPlace][newPoint.X, newPoint.Y].GID = (ushort)poissonData.GID;
                                }
                                else
                                {
                                    if (tileManager.TileData[(int)poissonData.LayersToPlace][newPoint.X, newPoint.Y].Empty)
                                    {
                                        found = true;
                                        _activeSamples.Add(newPoint);
                                        tileManager.TileData[(int)poissonData.LayersToPlace][newPoint.X, newPoint.Y].GID = (ushort)poissonData.GID;
                                        break;
                                    }
                                }






                            }
                        }
                    }




                }

                if (!found)
                {
                    _activeSamples[sampleIndex] = _activeSamples[_activeSamples.Count - 1];
                    _activeSamples.RemoveAt(_activeSamples.Count - 1);
                }


            }

            return;

        }



        private bool IsFarEnough(TileManager tileManager, Point sample, int minDistance, int maxDistance)
        {
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
                    if (tileManager.PathGrid.Weight[i, j] == (byte)GridStatus.Obstructed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
