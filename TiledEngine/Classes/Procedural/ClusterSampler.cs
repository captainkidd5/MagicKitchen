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
    internal class ClusterSampler
    {
        private List<Point> _activeSamples;


        public ClusterSampler()
        {

            _activeSamples = new List<Point>();
        }



        public void Generate(TileGenerationData poissonData, Layers layerToCheckIfEmpty,
            TileManager tileManager)
        {
            for (int i = 0; i < poissonData.Tries; i++)
            {
                Point spawnPoint = new Point(Settings.Random.Next(0, tileManager.TileData[0].GetLength(0) - 1),
                Settings.Random.Next(0, tileManager.TileData[0].GetLength(0) - 1));

                int numberOfAdditionalSpawns = 0;

                while ((Settings.Random.Next(0, 100) < poissonData.OddsAdditionalSpawn) && numberOfAdditionalSpawns <= poissonData.MaxCluster)
                {
                    Point? point = tileManager.TileLocationHelper.RandomPointWithinRadius(spawnPoint, poissonData.MinDistance, poissonData.MaxDistance);
                    if (point != null)
                    {
                        if (tileManager.TileData[(byte)layerToCheckIfEmpty][point.Value.X, point.Value.Y].Empty &&
                            CheckIfTileIsAllowedTileType(poissonData, tileManager, point.Value))
                        {
                            tileManager.TileData[(byte)layerToCheckIfEmpty][point.Value.X, point.Value.Y].GID = poissonData.GID;
                            numberOfAdditionalSpawns++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// For example, limpet rock must be placed in shallow water
        /// </summary>
        /// <returns></returns>
        private bool CheckIfTileIsAllowedTileType(TileGenerationData poissonData, TileManager tileManager, Point newPoint)
        {
            bool correctTileType = false;
            foreach (string tileType in poissonData.AllowedTilingSets)
            {
                if (tileManager.IsTypeOfTile(tileType, newPoint))
                {
                    if (poissonData.OnlyCentralTilingTile)
                    {
                        if (TouchingAnyOtherTiles(poissonData, tileManager, newPoint,
                            tileManager.TileSetPackage.WangManager.WangSets[tileType].Set.ElementAt(15).Value[0].GID))
                        {
                            break;
                        }
                    }
                    correctTileType = true;
                    break;
                }

            }

            return correctTileType;
        }

        /// <summary>
        /// Returns if specified gid touches any other gid in a sorrounding area other than itself
        /// </summary>
        /// <param name="poissonData"></param>
        /// <param name="tileManager"></param>
        /// <param name="newPoint"></param>
        /// <param name="gidToCheckFor"></param>
        /// <returns></returns>
        private bool TouchingAnyOtherTiles(TileGenerationData poissonData, TileManager tileManager, Point newPoint, int gidToCheckFor)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Point sorroundingPoint = new Point(newPoint.X + i, newPoint.Y + j);
                    if (tileManager.IsValidPoint(sorroundingPoint))
                    {
                        int sorroundinGid = tileManager.TileData[0][sorroundingPoint.X, sorroundingPoint.Y].GID;
                        if (sorroundinGid != gidToCheckFor)
                        {
                            return true;
                        }

                    }
                }
            }

            return false;
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
