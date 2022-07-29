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
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using TiledEngine.Classes.TileAddons.LightStuff;
using TiledEngine.Classes.TilePlacementStuff;
using TiledEngine.Classes.ZoneStuff;
using TiledSharp;
using UIEngine.Classes;
using static DataModels.Enums;
using static Globals.Classes.Settings;
using static TiledEngine.Classes.TileLoader;

namespace TiledEngine.Classes
{
    public class TileManager : Component, ISaveable, ILightDrawable
    {
        private TmxMap _tmxMap;
        //Top left of tilesheet is the tile selector. Very nice! Bro this is no longer tru u playin
        internal readonly Rectangle TileSelectorSourceRectangle = new Rectangle(16, 0, 16, 16);

        //How many tiles outside of the viewport should be rendered.
        //some tiles (trees, buildings) are quite large so we have to extend culling a bit so as to not cut them off!
        private readonly int _cullingLeeWay = 8;
        private readonly Camera2D _camera;

        private TilePlacementManager _tilePlacementManager;
        public PathGrid PathGrid { get; private set; }

        internal TileSetPackage TileSetPackage { get; private set; }
        internal Dictionary<int, float> OffSetLayersDictionary { get; private set; }
        public int MapWidth { get; private set; }
        public Rectangle MapRectangle { get; private set; }

        internal List<TileData[,]> TileData { get; private set; }
        public Dictionary<int, TileObject> TileObjects { get; private set; }

        public Dictionary<int, TileObject> DeadTileObjects { get; private set; }

        public ZoneManager ZoneManager { get; set; }



        public TileLocator TileLocator { get; private set; }
        internal PlacedOnItemManager PlacedItemManager { get; set; }
        internal TileLightManager TileLightManager { get; set; }
        private Sprite TileSelectorSprite { get; set; }

        public TileLocationHelper TileLocationHelper { get; set; }

        public bool JustResizedWindow { get; set; }
        public TileManager(GraphicsDevice graphics, ContentManager content, Camera2D camera) :
            base(graphics, content)
        {
            OffSetLayersDictionary = new Dictionary<int, float>();
            _camera = camera;
            TileLocator = new TileLocator();
            PlacedItemManager = new PlacedOnItemManager(this);
            TileLightManager = new TileLightManager();
            _tilePlacementManager = new TilePlacementManager(this);
            TileLocationHelper = new TileLocationHelper(this);

            TileObjects = new Dictionary<int, TileObject>();
            DeadTileObjects = new Dictionary<int, TileObject>();
            Settings.GameWindow.ClientSizeChanged -= Window_ClientSizeChanged;

            Settings.GameWindow.ClientSizeChanged += Window_ClientSizeChanged;

            ZoneManager = new ZoneManager();
        }

        /// <summary>
        /// Generic load, should only be called by <see cref="TileLoader.LoadTileManager(string, TileManager)"/>
        /// </summary>
        internal void LoadMap(TmxMap tmxMap, List<TileData[,]> tiles, int mapWidth, TileSetPackage tileSetPackage, bool newSave = true)
        {
            _tmxMap = tmxMap;

            TileSetPackage = tileSetPackage;

            MapWidth = mapWidth;

            PathGrid = new PathGrid(MapWidth, MapWidth);

            TileData = tiles;
            TileSelectorSprite = SpriteFactory.CreateWorldSprite(Vector2.Zero, TileSelectorSourceRectangle, TileSetPackage.BackgroundSpriteSheet);
            AssignProperties();
            if (newSave)
                ZoneManager.LoadZones(tmxMap);
        }


      

        private void AssignProperties()
        {

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
        public TileObject MouseOverTile { get; private set; }

        internal TileObject TileToInteractWith { get; set; }

        internal Layers TopLeftPrecedence;

        internal bool CheckPrecedence(Layers layer)
        {
            if (layer >= TopLeftPrecedence)
            {
                layer = TopLeftPrecedence;
                return true;

            }
            return false;
        }
        public void Update(GameTime gameTime)
        {
            TopLeftPrecedence = 0;
            DeadTileObjects.Clear();
            CalculateStartAndEndIndexes();
            CalculateMouseIndex();
            TileToInteractWith = null;
            foreach (var tileObject in TileObjects)
            {
                TileObject tileObj = tileObject.Value;
                //In the update loop of every tile object, a check is done to see if the tile object is within range of the viewport. If not, we
                //flag it for removal
                tileObj.Update(gameTime, PathGrid);
                if (tileObj.FlaggedForRemoval)
                {
                    tileObj.Unload();

                    DeadTileObjects.Add(tileObj.TileData.GetKey(), tileObj);
                }
            }
            foreach (var pair in DeadTileObjects)
            {
                TileObjects.Remove(pair.Key);
            }
            for (int z = 0; z < TileData.Count; z++)
            {
                for (int x = StartX; x < EndX; x++)
                {
                    for (int y = StartY; y < EndY; y++)
                    {
                        int key = TileData[z][x, y].GetKey();
                        if (!TileObjects.ContainsKey(key))
                        {
                            TileObjects.Add(key, new TileObject(this, TileData[z][x, y]));
                        }

                    }
                }

                //mouse over tile is the highest layered, non empty tile
                if (!TileData[z][MouseX, MouseY].Empty)
                {
                    TileObject mouseObj;
                    TileObjects.TryGetValue(TileData[z][MouseX, MouseY].GetKey(), out mouseObj);
                    if (mouseObj != null)
                        MouseOverTile = mouseObj;

                }
            }

            

            if (TileToInteractWith != null)
                TileSelectorSprite.Update(gameTime, new Vector2(TileToInteractWith.Position.X + TileToInteractWith.SourceRectangle.Width / 2 - 8,
                    TileToInteractWith.Position.Y +TileToInteractWith.SourceRectangle.Height - 16));
            CheckMouseTileInteractions(gameTime);



            _tilePlacementManager.Update(gameTime);
            JustResizedWindow = false;
        }
        public void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            JustResizedWindow = true;
            foreach (var obj in TileObjects)
            {
                obj.Value.Unload();
            }
            TileObjects.Clear();
        }
        public bool IsWithinUpdateRange(Vector2 entityPosition)
        {

            Point point = Vector2Helper.GetTileIndexPosition(entityPosition);
            if (point.X >= StartX && point.X < EndX)
            {
                if (point.Y >= StartY && point.Y < EndY)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsWithinUpdateRange(TileData tileData)
        {
            if (tileData.X >= StartX && tileData.X < EndX)
            {
                if (tileData.Y >= StartY && tileData.Y < EndY)
                {
                    return true;
                }
            }
            return false;
        }
        private void CheckMouseTileInteractions(GameTime gameTime)
        {
            if (MouseOverTile != null)
            {

                //moreover, if tile to interact with is real, we want to make sure that tile selector sprite draws here instead
                TileSelectorSprite.Update(gameTime, new Vector2(MouseOverTile.Position.X + MouseOverTile.SourceRectangle.Width/2 - 8,
                    MouseOverTile.Position.Y + MouseOverTile.SourceRectangle.Height - 16));


            }
        }

        /// <summary>
        /// Loops through all layers of current tile, if any of them are not the default cursor, we should be using that. If
        /// there are more than 1 distinct type, use the top level one.
        /// </summary>
        /// <param name="hoveredLayerTile"></param>
        private bool CheckIfCursorIconChangedFromTile(TileData hoveredLayerTile)
        {

            //CursorIconType iconType = hoveredLayerTile.GetCursorIconType(TileSetPackage);
            //if (hoveredLayerTile.WithinRangeOfPlayer && iconType != CursorIconType.None)
            //{
            //    if (UI.Cursor.CursorIconType != iconType)
            //    {
            //        UI.Cursor.ChangeCursorIcon(iconType);
            //    }
            //    return true;

            //}
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (var tileObject in TileObjects)
            {
                TileObject tileObj = tileObject.Value;
                if(!tileObj.TileData.Empty)
                  tileObj.Draw(spriteBatch, TileSetPackage.GetTexture(tileObj.TileData.GID));
            }

            if (Flags.ShowTileSelector)
                TileSelectorSprite.Draw(spriteBatch);
            _tilePlacementManager.Draw(spriteBatch);
        }
        public void DrawLights(SpriteBatch spriteBatch)
        {

            foreach (var tileObject in TileObjects)
            {
                TileObject tileObj = tileObject.Value;
                tileObj.DrawLights(spriteBatch);
            }

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




        public void SwitchGID(ushort newGid, TileData tileData)
        {
           // if(TileSetPackage.IsForeground(newGid))
               // newGid = (ushort)TileSetPackage.OffSetForegroundGID(newGid);
            
            tileData.GID = (ushort)(newGid + 1);
            TileData[(byte)tileData.Layer][tileData.X, tileData.Y] = tileData;
           // if (addProperty)
          //  {
                int key = tileData.GetKey();
                TileObject obj;
                TileObjects.TryGetValue(key, out obj);
                if(obj != null)
                {

                    TileObjects[key].FlaggedForRemoval = true;
                }
               

            //}
        }
        public TileData? GetTileFromWorldPosition(Vector2 position, Layers layer)
        {
            Point coord = Vector2Helper.GetTileIndexPosition(position);
            return GetTileDataFromPoint(coord, layer);
        }
        public TileData? GetTileDataFromPoint(Point point, Layers layer)
        {
            if (TileData.Count < (int)layer)
                throw new Exception("Tiles cannot be null");
            if (point.X >= TileData[(int)layer].GetLength(0) || point.X < 0)
            {
                //Debug.Assert(point.X > TileData[(int)layer].GetLength(0) || point.X < 0, $"{point.X} is outside the bounds of the array of length {TileData[(int)layer].GetLength(0)}");

                return null;
            }

            if (point.Y >= TileData[(int)layer].GetLength(1) || point.Y < 0)
            {
             //   Debug.Assert(point.Y > TileData[(int)layer].GetLength(1) || point.Y < 0, $"{point.Y} is outside the bounds of the array of length {TileData[(int)layer].GetLength(1)}");
                return null;

            }

            return TileData[(int)layer][point.X, point.Y];
        }



        internal bool X_IsValidIndex(int x) => TileLocationHelper.X_IsValidIndex(x);


        internal bool Y_IsValidIndex(int y) => TileLocationHelper.Y_IsValidIndex(y);

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
                TileData? tile = GetTileFromWorldPosition(position, (Layers)i);
                if (tile == null)
                    continue;

                string step = "step";
                TmxTilesetTile tmxTile = TileSetPackage.GetTmxTileSetTile(tile.Value.GID);
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
            writer.Write(MapWidth);
            writer.Write(TileData.Count);

            int xLength = TileData[0].GetLength(0);
            writer.Write(xLength);

            int yLength = TileData[0].GetLength(1);
            writer.Write(yLength);

            for (int z = 0; z < TileData.Count; z++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    for (int y = 0; y < yLength; y++)
                    {
                        writer.Write(TileData[z][x, y].GID + 1);
                        writer.Write((int)TileData[z][x, y].X);
                        writer.Write((int)TileData[z][x, y].Y);

                    }
                }
            }
            PlacedItemManager.Save(writer);
            TileLightManager.Save(writer);
            ZoneManager.Save(writer);
        }
        public void LoadSave(BinaryReader reader)
        {
            MapWidth = reader.ReadInt32();
            TileData = new List<TileData[,]>();
            int layerCount = reader.ReadInt32();
            int length0 = reader.ReadInt32();
            int length1 = reader.ReadInt32();

            for (int z = 0; z < layerCount; z++)
            {
                TileData.Add(new TileData[length0, length1]);
                for (int x = 0; x < length0; x++)
                {
                    for (int y = 0; y < length1; y++)
                    {
                        TileData[z][x, y] = new TileData((ushort)reader.ReadInt32(), (ushort)reader.ReadInt32(), (ushort)reader.ReadInt32(), (Layers)z);

                    }
                }
            }
            PlacedItemManager.LoadSave(reader);
            TileLightManager.LoadSave(reader);
            LoadMap(_tmxMap, TileData, MapWidth, TileLoader.TileSetPackage,false);
            ZoneManager.LoadSave(reader);

        }


        public bool IsTypeOfTile(string tileType, Vector2 position)
        {
            TileData? tile = GetTileFromWorldPosition(position, Layers.background);

            if (tile == null)
                return false;
            string tilingSetValue = tile.Value.GetProperty(TileSetPackage, "tilingSet");
            if (tilingSetValue == tileType)
                return true;

            return false;
        }
        public void CleanUp()
        {
            foreach (var tileObject in TileObjects)
            {
                tileObject.Value.Unload();
            }
            TileObjects.Clear();
            TileData.Clear();
            TileLocator.CleanUp();
            PlacedItemManager.CleanUp();
            TileLightManager.CleanUp();
        }

        public void SetToDefault( )
        {
            CleanUp();
        }
    }
}
