using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public enum IslandDistance
    {
        None = 0,
        Inner = 1,
        Mid = 2,
        Outer = 3
    }
    public class StageData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public int InsertionX { get; set; }
        public int InsertionY { get; set; }


        public string AmbientSoundPackageName { get; set; }

        public int MapWidth { get; set; }

        public Rectangle Bounds => new Rectangle(InsertionX * 16, InsertionY * 16, MapWidth * 16, MapWidth * 16);
        public void Load(int mapWidth)
        {
            MapWidth = mapWidth;
        }
    }
}
