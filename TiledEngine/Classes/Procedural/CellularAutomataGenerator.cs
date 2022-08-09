using Globals.Classes;
using Globals.Classes.Chance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.Procedural
{
    internal static class CellularAutomataGenerator
    {
        static float chanceToStartAlive = 0.45f;
        static int numberOfSteps = 6;
        static int birthLimit = 4;
        static int deathLimit = 4;

        private static bool[,] initialiseMap(bool[,] map)
        {
            int width = map.GetLength(0);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (ChanceHelper.RandomFloat(0, 1) < chanceToStartAlive)
                    {
                        map[x, y] = true;
                    }
                }
            }
            return map;
        }

        public static bool[,] GenerateMap(int width)
        {
            //Create a new map
            bool[,] cellmap = new bool[width, width];
            //Set up the map with random values
            cellmap = initialiseMap(cellmap);
            //And now run the simulation for a set number of steps
            for (int i = 0; i < numberOfSteps; i++)
            {
                cellmap = doSimulationStep(cellmap);
            }
            return cellmap;
        }
        //Returns the number of cells in a ring around (x,y) that are alive.
        private static int CountAliveNeighbors(bool[,] map, int x, int y)
        {
            int width = map.GetLength(0);
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int neighbour_x = x + i;
                    int neighbour_y = y + j;
                    //If we're looking at the middle point
                    if (i == 0 && j == 0)
                    {
                        //Do nothing, we don't want to add ourselves in!
                    }
                    //In case the index we're looking at it off the edge of the map
                    else if (neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= width || neighbour_y >= width)
                    {
                        count = count + 1;
                    }
                    //Otherwise, a normal check of the neighbour
                    else if (map[neighbour_x, neighbour_y])
                    {
                        count = count + 1;
                    }
                }
            }
            return count;
        }

        private static bool[,] doSimulationStep(bool[,] oldMap)
        {
            int width = oldMap.GetLength(0);
            bool[,] newMap = new bool[width, width];
            //Loop over each row and column of the map
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    int nbs = CountAliveNeighbors(oldMap, x, y);
                    //The new value is based on our simulation rules
                    //First, if a cell is alive but has too few neighbours, kill it.
                    if (oldMap[x, y])
                    {
                        if (nbs < deathLimit)
                        {
                            newMap[x, y] = false;
                        }
                        else
                        {
                            newMap[x, y] = true;
                        }
                    } //Otherwise, if the cell is dead now, check if it has the right number of neighbours to be 'born'
                    else
                    {
                        if (nbs > birthLimit)
                        {
                            newMap[x, y] = true;
                        }
                        else
                        {
                            newMap[x, y] = false;
                        }
                    }
                }
            }
            return newMap;
        }
    }
}
