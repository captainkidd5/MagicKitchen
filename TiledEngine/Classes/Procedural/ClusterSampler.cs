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
        /// Require central tile would meaning spawning things in water should not spawn on partial water tiles, only the central water tile
        /// Trees for example wouldn't look right jutting out of a water edge tile
        /// <returns></returns>
        private static bool CheckIfTileIsAllowedTileType(TileGenerationData poissonData, TileManager tileManager, Point newPoint)
        {
            bool correctTileType = false;
            foreach (string tileType in poissonData.AllowedTilingSets)
            {

                if (tileManager.IsTypeOfTile(tileType, newPoint))
                {
                    if (poissonData.OnlyCentralTilingTile)
                    {
                        int wangGId = tileManager.TileSetPackage.WangManager.WangSets[tileType].Set.ElementAt(15).Value[0].GID;
                        if (tileManager.TileData[0][newPoint.X, newPoint.Y].GID == wangGId)
                        {
                            correctTileType = true;
                            break;
                        }
                        else
                        {
                            ushort id = tileManager.TileData[0][newPoint.X, newPoint.Y].GID;
                            Console.WriteLine("test");
                        }

                    }
                    else
                    {
                        correctTileType = true;
                        break;
                    }
                   
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
