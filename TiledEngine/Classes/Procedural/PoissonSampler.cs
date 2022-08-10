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
        private int _tries; //Maximum number of tries for a new point.
        private readonly TileManager _tileManager;
        private int _minDistance; //Minimum distance points must be separated from one another
        private int _maxDistance;//Maximum distance a point can be from at least one other point


        private List<Point> _activeSamples;
        public Rectangle GridRectangle { get; set; }


        public PoissonSampler(TileManager tileManager, int minDistance, int maxDistance, int tries)
        {
            _tileManager = tileManager;
            _minDistance = minDistance;
            _maxDistance = maxDistance;
            _activeSamples = new List<Point>();
            _tries = tries;
        }

        public void Generate(int gid, Layers layerToPlaceOn, Layers layerToCheckIfEmpty,
            TileManager TileManager, Random random, bool isCrop)
        {
            //generate first point randomly within grid
            _activeSamples.Add(new Point(random.Next(0, _tileManager.TileData[0].GetLength(0) - 1),
                random.Next(0, _tileManager.TileData[0].GetLength(0) - 1)));

            while (_activeSamples.Count > 0)
            {
                int sampleIndex = random.Next(0, _activeSamples.Count - 1); //pick random sample within activesample list
                Point sample = _activeSamples[sampleIndex];


                bool found = false;


                for (int k = 0; k < _tries; k++) //try MaxK times to find a valid point
                {

                    int newX = sample.X + _minDistance * ChanceHelper.GetNevagiveOrPositive1();
                    int newY = sample.Y + _minDistance * ChanceHelper.GetNevagiveOrPositive1();
                    Point newPoint = new Point(newX, newY);

                    if (_tileManager.IsValidPoint(newPoint))
                    {
                        if (_tileManager.TileData[(int)layerToPlaceOn][newPoint.X, newPoint.Y].Empty)
                        {
                            if (IsFarEnough(newPoint))
                            {
                               

                                    if (layerToCheckIfEmpty == Layers.background)
                                    {
                                        found = true;
                                        _activeSamples.Add(newPoint);
                                        _tileManager.TileData[(int)layerToPlaceOn][newPoint.X, newPoint.Y].GID = (ushort)gid;
                                    }
                                    else
                                    {
                                        if (_tileManager.TileData[(int)layerToPlaceOn][newPoint.X, newPoint.Y].Empty)
                                        {
                                            found = true;
                                            _activeSamples.Add(newPoint);
                                        _tileManager.TileData[(int)layerToPlaceOn][newPoint.X, newPoint.Y].GID = (ushort)gid;
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

   

        private bool IsFarEnough(Point sample)
        {
            int startingX = sample.X - _minDistance;
            int startingY = sample.Y - _minDistance;
            //Game1.Utility.EnsurePositive(ref startingX);
            //Game1.Utility.EnsurePositive(ref startingY);

            //int endingIndexX = sample.X + _minDistance;
            //int endingIndexY = sample.Y + _minDistance;
            //int max = _tileManager.TileData[0].GetLength(0) - 1;

            //Game1.Utility.EnsureNoMoreThanMax(ref endingIndexX, max);
            //Game1.Utility.EnsureNoMoreThanMax(ref endingIndexY, max);


            //for (int i = startingX; i < endingIndexX; i++)
            //{
            //    for (int j = startingY; j < endingIndexY; j++)
            //    {
            //        if (_tileManager.TileData[i,j].Empty)
            //        {
            //            return false;
            //        }
            //    }
            //}
            return true;
        }
    }
}
