using DataModels;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.Misc;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using TiledEngine.Classes.TilePlacementStuff;
using TiledEngine.Classes.ZoneStuff;
using TiledSharp;
using UIEngine.Classes;
using static DataModels.Enums;
using static Globals.Classes.Settings;
using static TiledEngine.Classes.TileLoader;

namespace TiledEngine.Classes
{
    public class TileManager : Component, ISaveable
    {

        //Top left of tilesheet is the tile selector. Very nice! Bro this is no longer tru u playin
        internal readonly Rectangle TileSelectorSourceRectangle = new Rectangle(16, 0, 16, 16);

        //How many tiles outside of the viewport should be rendered.
        //some tiles (trees, buildings) are quite large so we have to extend culling a bit so as to not cut them off!
        private readonly int _cullingLeeWay = 8;
        private readonly Camera2D _camera;
        internal readonly PenumbraComponent _penumbra;
        internal readonly ItemManager ItemManager;

        private TilePlacementManager _tilePlacementManager;
        public PathGrid PathGrid { get; private set; }

        internal TileSetPackage TileSetPackage { get; private set; }
        internal Dictionary<int, float> OffSetLayersDictionary { get; private set; }
        public int MapWidth { get; private set; }
        public Rectangle MapRectangle { get; private set; }

        internal List<Tile[,]> Tiles { get; private set; }

        public List<PortalData> Portals { get; private set; }

        public TileLocator TileLocator { get; private set; }
        internal PlacedOnItemManager PlacedItemManager { get; set; }

        private Sprite TileSelectorSprite { get; set; }

        public TileLocationHelper TileLocationHelper { get; set; }
        public MapType MapType { get; internal set; }
        public TileManager(GraphicsDevice graphics, ContentManager content, Camera2D camera, PenumbraComponent penumbra, MapType mapType, ItemManager itemManager) :
            base(graphics, content)
        {
            OffSetLayersDictionary = new Dictionary<int, float>();
            Portals = new List<PortalData>();
            MapType = mapType;
            ItemManager = itemManager;
            _camera = camera;
            _penumbra = penumbra;
            TileLocator = new TileLocator();
            PlacedItemManager = new PlacedOnItemManager(this);
            _tilePlacementManager = new TilePlacementManager(this);
            TileLocationHelper = new TileLocationHelper(this);
        }

        /// <summary>
        /// Generic load, should only be called by <see cref="TileLoader.LoadTileManager(string, TileManager)"/>
        /// </summary>
        internal void CreateNewSave(TileManager tileManager, List<Tile[,]> tiles, int mapWidth, TileSetPackage tileSetPackage)
        {

            TileSetPackage = tileSetPackage;

            MapWidth = mapWidth;

            PathGrid = new PathGrid(MapWidth, MapWidth);

            Tiles = tiles;
            TileSelectorSprite = SpriteFactory.CreateWorldSprite(Vector2.Zero, TileSelectorSourceRectangle, TileSetPackage.BackgroundSpriteSheet);
            AssignProperties();
        }

        /// <summary>
        /// Grabs all of the objects from tmx map LAYER "Portal"
        /// </summary>
        internal List<PortalData> LoadPortals(TmxMap tmxMap)
        {
            LoadZones(tmxMap);
            TmxObjectGroup portals;

            tmxMap.ObjectGroups.TryGetValue("Portal", out portals);
            foreach (TmxObject portal in portals.Objects)
            {
                Portals.Add(new PortalData(new Rectangle((int)portal.X, (int)portal.Y, (int)portal.Width, (int)portal.Height),
                    portal.Properties["from"], portal.Properties["to"], int.Parse(portal.Properties["xOffSet"]), int.Parse(portal.Properties["yOffSet"]),
                    (Direction)Enum.Parse(typeof(Direction), portal.Properties["directionToFace"]), bool.Parse(portal.Properties["Click"])));
            }
            return Portals;
        }
        public List<SpecialZone> LoadZones(TmxMap tmxMap)
        {
            TmxObjectGroup zones;

            tmxMap.ObjectGroups.TryGetValue("SpecialZone", out zones);
            List<SpecialZone> zonesList = new List<SpecialZone>();
            foreach (TmxObject specialZone in zones.Objects)
            {
                SpecialZone zone = new SpecialZone(specialZone.Properties.ElementAt(0).Key, specialZone.Properties.ElementAt(0).Value, new Rectangle(
                    (int)specialZone.X,
                    (int)specialZone.Y,
                    (int)specialZone.Width,
                    (int)specialZone.Height));
                zonesList.Add(zone);
            }
            return zonesList;
        }

        private void AssignProperties()
        {
            for (int z = 0; z < Tiles.Count; z++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    for (int y = 0; y < MapWidth; y++)
                    {
                        TileUtility.AssignProperties(Tiles[z][x, y], (Layers)z, wang: false);
                    }
                }
            }

            MapRectangle = new Rectangle(0, 0, Settings.TileSize * MapWidth, Settings.TileSize * MapWidth);
        }




        #region INDEX VARIABLES
        private int StartX { get; set; }
        private int StartY { get; set; }
        private int EndX { get; set; }

        private int EndY { get; set; }

        private int MouseX { get; set; }
        private int MouseY { get; set; }
        #endregion
        public Tile MouseOverTile { get; private set; }

        internal Tile TileToInteractWith { get; set; }
        public void Update(GameTime gameTime)
        {
            CalculateStartAndEndIndexes();
            CalculateMouseIndex();
            TileToInteractWith = null;
            for (int z = 0; z < Tiles.Count; z++)
            {
                for (int x = StartX; x < EndX; x++)
                {
                    for (int y = StartY; y < EndY; y++)
                    {
                        Tiles[z][x, y].Update(gameTime, PathGrid);
                    }
                }
                //Mouse over tile is the highest layered, non empty tile
                if (!Tiles[z][MouseX, MouseY].Empty)
                    MouseOverTile = Tiles[z][MouseX, MouseY];
            }
            CheckMouseTileInteractions();



            TileSelectorSprite.Update(gameTime, new Vector2(MouseOverTile.DestinationRectangle.X, MouseOverTile.DestinationRectangle.Y));
            _tilePlacementManager.Update(gameTime);
        }

        private void CheckMouseTileInteractions()
        {
            if (TileToInteractWith != null)
            {
                if (Controls.IsClickedWorld || Controls.DidGamePadSelectWorld)
                    TileToInteractWith.Interact(true, UI.PlayerCurrentSelectedItem);
            }
        }

        /// <summary>
        /// Loops through all layers of current tile, if any of them are not the default cursor, we should be using that. If
        /// there are more than 1 distinct type, use the top level one.
        /// </summary>
        /// <param name="hoveredLayerTile"></param>
        private bool CheckIfCursorIconChangedFromTile(Tile hoveredLayerTile)
        {

            CursorIconType iconType = hoveredLayerTile.GetCursorIconType();
            if (hoveredLayerTile.WithinRangeOfPlayer && iconType != CursorIconType.None)
            {
                if (UI.Cursor.CursorIconType != iconType)
                {
                    UI.Cursor.ChangeCursorIcon(iconType);
                }
                return true;

            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int z = 0; z < Tiles.Count; z++)
            {
                for (int x = StartX; x < EndX; x++)
                {
                    for (int y = StartY; y < EndY; y++)
                    {
                        Tiles[z][x, y].Draw(spriteBatch, TileSetPackage.GetTexture(Tiles[z][x, y].GID));

                    }
                }
            }
            if (Flags.ShowTileSelector)
                TileSelectorSprite.Draw(spriteBatch);
            _tilePlacementManager.Draw(spriteBatch);
        }



        /// <summary>
        /// Change grid value at specified index.
        /// </summary>
        public void UpdateGrid(int indexI, int indexJ, GridStatus newValue)
        {
            PathGrid.UpdateGrid(indexI, indexJ, newValue);
        }

        /// <summary>
        /// This method will cull tiles, so that only the tiles within the screen are updated/drawn.
        /// </summary>
        private void CalculateStartAndEndIndexes()
        {

            int screenHalfTiles = (int)(Settings.ScreenWidth / _camera.Zoom / 2 / Settings.TileSize);

            StartX = (int)(_camera.X / Settings.TileSize) - screenHalfTiles - _cullingLeeWay;
            if (StartX < 0)
                StartX = 0;

            StartY = (int)(_camera.Y / Settings.TileSize) - screenHalfTiles - _cullingLeeWay;
            if (StartY < 0)
                StartY = 0;

            EndX = (int)(_camera.X / Settings.TileSize) + screenHalfTiles + _cullingLeeWay;
            if (EndX > MapWidth)
                EndX = MapWidth;

            EndY = (int)(_camera.Y / Settings.TileSize) + screenHalfTiles + _cullingLeeWay;
            if (EndY > MapWidth)
                EndY = MapWidth;
        }

        /// <summary>
        /// Ensures that mouse indices given from controls are within the bounds of the current map. 
        /// </summary>
        private void CalculateMouseIndex()
        {
            MouseX = Controls.CursorTileIndex.X;
            MouseY = Controls.CursorTileIndex.Y;
            if (MouseX >= MapWidth)
                MouseX = MapWidth - 1;
            if (MouseY >= MapWidth)
                MouseY = MapWidth - 1;
        }

        public Tile GetTileFromTileKey(int key)
        {
            int x = (key >> 18) & 0x3fff;
            int y = (key >> 4) & 0x3fff;
            int layer = (key >> 0) & 0xf;
            return Tiles[layer][x, y];
        }

        public Tile GetTileFromWorldPosition(Vector2 position, Layers layer)
        {
            Point coord = Vector2Helper.GetTileIndexPosition(position);
            return GetTileFromPoint(coord, layer);
        }
        public Tile GetTileFromPoint(Point point, Layers layer)
        {
            if (Tiles.Count < (int)layer)
                throw new Exception("Tiles cannot be null");
            if (point.X >= Tiles[(int)layer].GetLength(0) || point.X < 0)
            {
                Debug.Assert(point.X > Tiles[(int)layer].GetLength(0) || point.X < 0, $"{point.X} is outside the bounds of the array of length {Tiles[(int)layer].GetLength(0)}");

                return null;
            }

            if (point.Y >= Tiles[(int)layer].GetLength(1) || point.Y < 0)
            {
                Debug.Assert(point.Y > Tiles[(int)layer].GetLength(1) || point.Y < 0, $"{point.Y} is outside the bounds of the array of length {Tiles[(int)layer].GetLength(1)}");
                return null;

            }

            return Tiles[(int)layer][point.X, point.Y];
        }




        /// <summary>
        /// Ensures X index is greater than zero and less than bounds of grid
        /// </summary>
        internal bool X_IsValidIndex(int x)
        {
            if (x >= 0)
                if (x < Tiles[0].GetLength(0))
                    return true;
            return false;
        }

        /// <summary>
        /// Ensures Y index is greater than zero and less than bounds of grid
        /// </summary>
        internal bool Y_IsValidIndex(int y)
        {
            if (y >= 0)
                if (y < Tiles[0].GetLength(1))
                    return true;
            return false;
        }
        /// <summary>
        /// Gets the tile underfoot the Npcs position, and returns the sound at layer 0, if exists
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public string GetStepSoundFromPosition(Vector2 position)
        {
            //Use the top layer sound if available (ex: grass should be used over dirt)
            for (int i = 1; i >= 0; i--)
            {
                Tile tile = GetTileFromWorldPosition(position, (Layers)i);
                if (tile == null)
                    continue;

                string step = "step";
                TmxTilesetTile tmxTile = TileSetPackage.GetTmxTileSetTile(tile.GID);
                if (tmxTile != null)
                {
                    if (tmxTile.Properties.TryGetValue(step, out step))
                        return step;
                }

            }

            return string.Empty;
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write((int)MapType);
            writer.Write(MapWidth);
            writer.Write(Tiles.Count);

            int xLength = Tiles[0].GetLength(0);
            writer.Write(xLength);

            int yLength = Tiles[0].GetLength(1);
            writer.Write(yLength);

            for (int z = 0; z < Tiles.Count; z++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    for (int y = 0; y < yLength; y++)
                    {
                        writer.Write(Tiles[z][x, y].GID + 1);
                        writer.Write(Tiles[z][x, y].X);
                        writer.Write(Tiles[z][x, y].Y);

                    }
                }
            }
            PlacedItemManager.Save(writer);
        }
        public void LoadSave(BinaryReader reader)
        {
            MapType mapType = (MapType)reader.ReadInt32();
            MapWidth = reader.ReadInt32();
            Tiles = new List<Tile[,]>();
            int layerCount = reader.ReadInt32();
            int length0 = reader.ReadInt32();
            int length1 = reader.ReadInt32();

            for (int z = 0; z < layerCount; z++)
            {
                Tiles.Add(new Tile[length0, length1]);
                for (int x = 0; x < length0; x++)
                {
                    for (int y = 0; y < length1; y++)
                    {
                        Tiles[z][x, y] = new Tile(this, reader.ReadInt32(), (Layers)z, z, reader.ReadInt32(), reader.ReadInt32());

                    }
                }
            }
            PlacedItemManager.LoadSave(reader);
            //Todo: this is sus
            CreateNewSave(this, Tiles, MapWidth, TileLoader.GetPackageFromMapType(mapType));

        }

        public void CleanUp()
        {
            for (int z = 0; z < Tiles.Count; z++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    for (int y = 0; y < MapWidth; y++)
                    {
                        //Remove any hullbodies or penumbra lights
                        Tiles[z][x, y].Unload();
                    }
                }
            }
            Tiles.Clear();
            TileLocator.CleanUp();
            PlacedItemManager.CleanUp();
        }
    }
}
