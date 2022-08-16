using Globals.Classes;
using IOEngine.Classes;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.TileAddons;
using static DataModels.Enums;

namespace TiledEngine.Classes
{
    internal class Chunk
    {
        private static readonly int ChunkSize = 64;
        private readonly TileManager _tileManager;
        private ContentManager _content;
        public int X { get; set; }
        public int Y { get; set; }
        
        private string _path => $"{SaveLoadManager.CurrentSave.MetaData.ChunkPath}/Chunk_{X}_{Y}";
        public Chunk(TileManager tileManager, int x, int y)
        {
            _tileManager = tileManager;
            X = x;
            Y = y;
        }

        public void LoadContent(ContentManager content)
        {
            _content = content;
        }

        public void Save( )
        {
            int startXTiles = X * Settings.TileSize;
            int startYTiles = Y * Settings.TileSize;

            int endXTiles = startXTiles + ChunkSize;
            int endYTiles = startYTiles + ChunkSize;
            BinaryWriter writer = SaveLoadManager.CreateWriter(_path);
            for (int z = 0; z < _tileManager.TileData.Count; z++)
            {
                for (int x = startXTiles; x < endXTiles; x++)
                {
                    for (int y = startYTiles; y < endYTiles; y++)
                    {
                        writer.Write(_tileManager.TileData[z][x, y].GID + 1);
                        writer.Write((int)_tileManager.TileData[z][x, y].X);
                        writer.Write((int)_tileManager.TileData[z][x, y].Y);

                    }
                }
            }
            SaveLoadManager.DestroyWriter(writer);
        }

        public void LoadSave()
        {
            BinaryReader reader = SaveLoadManager.CreateReader(_path); 
            int startXTiles = X * Settings.TileSize;
            int startYTiles = Y * Settings.TileSize;

            int endXTiles = startXTiles + ChunkSize;
            int endYTiles = startYTiles + ChunkSize;

            for (int z = 0; z < _tileManager.TileData.Count; z++)
            {
                for (int x = startXTiles; x < endXTiles; x++)
                {
                    for (int y = startYTiles; y < endYTiles; y++)
                    {
                        _tileManager.TileData[z][x, y] = new TileData((ushort)reader.ReadInt32(), (ushort)reader.ReadInt32(), (ushort)reader.ReadInt32(), (Layers)z);

                    }
                }
            }
            SaveLoadManager.DestroyReader(reader);
        }

        public void SetToDefault()
        {
            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}
