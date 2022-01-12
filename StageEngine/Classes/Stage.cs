﻿using DataModels;
using EntityEngine.Classes.PlayerStuff;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
using IOEngine.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using Penumbra;
using SpriteEngine.Classes.RenderTargetStuff;
using System;
using System.Collections.Generic;
using System.IO;
using TiledEngine.Classes;
using EntityEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using Globals.Classes.Time;
using static Globals.Classes.Settings;

namespace StageEngine.Classes
{
    public class Stage : ISaveable
    {
        public string Name { get; private set; }
        internal bool InitialLoadDone { get; private set; }

        private readonly StageData _stageData;

        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private readonly Camera2D _camera;
        private readonly PenumbraComponent _penumbra;

        private string _ambientSoundPackageName => _stageData.AmbientSoundPackageName;
        internal TileManager TileManager { get; private set; }
        private Rectangle MapRectangle { get; set; }

        private string _pathExtension => Name + ".dat";

        public List<WorldItem> Items { get; set; }

        private Player Player1 { get; set; }

        public List<Entity> NPCs { get; set; }

        private PathGrid _pathGrid => TileManager.PathGrid;

        internal bool CamLock => _stageData.MapType == MapType.Exterior;
        public Stage(StageData stageData, ContentManager content,
            GraphicsDevice graphics, Camera2D camera, PenumbraComponent penumbra)
        {
            Name = stageData.Name;


            _stageData = stageData;

            _content = content;
            _graphics = graphics;
            _camera = camera;
            _penumbra = penumbra;
            Player1 = PlayerManager.Player1;
            TileManager = new TileManager(graphics, content, camera, penumbra);
            Items = new List<WorldItem>();
            NPCs = new List<Entity>();


        }



        public void Update(GameTime gameTime)
        {
            Clock.Update(gameTime);
            PlayerManager.Update(gameTime);
            _camera.Follow(Player1.Position, MapRectangle);

            TileManager.Update(gameTime);

            foreach (WorldItem item in Items)
            {
                item.Update(gameTime);
                if (item.FlaggedForRemoval)
                {
                    item.Unload();
                    Items.Remove(item);
                }

            }


        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            _penumbra.AmbientColor = Color.DarkSlateGray;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _camera.GetTransform(_graphics));
            _graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            PlayerManager.Draw(spriteBatch);
            CharacterManager.Draw(spriteBatch, Name);
            TileManager.Draw(spriteBatch);

            foreach (WorldItem item in Items)
                item.Draw(spriteBatch);

#if DEBUG
            if (Flags.DebugGrid)
                _pathGrid.DrawDebug(spriteBatch);
#endif

            spriteBatch.End();

        }
        /// <summary>
        /// Loads the portals into memory, should only be called ONCE per stage, per save
        /// </summary>
        public void LoadPortals()
        {
            TileLoader.LoadStagePortals(_stageData, TileManager);
            PortalManager.LoadNewStage(Name, TileManager);
        }
        /// <summary>
        /// Loads tiles into memory, then saves them, from tmx map. Should only be called ONCE per stage, per save
        /// </summary>
        public void FirstEntryLoad()
        {

            TileLoader.InitializeStage(_stageData, TileManager, _content);
            MapRectangle = TileManager.MapRectangle;

            InitialLoadDone = true;
            //Character Caspar = new Character(graphics, content, "Caspar") { Position = new Vector2(200, 400) };
            //Caspar.Load(content);
            //Caspar.LoadToNewStage(Name, TileManager.PathGrid);
            //NPCs.Add(Caspar);
            SaveToIndividualFile();
        }


        /// <summary>
        /// Saves to individual file, called whenever a player leaves a stage. 
        /// </summary>
        public void SaveToIndividualFile()
        {
            File.WriteAllText(SaveLoadManager.CurrentSave.StageFilePath + @"\" + _pathExtension, string.Empty);
            BinaryWriter stageWriter = SaveLoadManager.GetCurrentSaveFileWriter(@"\Stages\" + _pathExtension);
            Save(stageWriter);
            SaveLoadManager.DestroyWriter(stageWriter);
        }

        public void LoadFromIndividualFile()
        {
            BinaryReader stageReader = SaveLoadManager.GetCurrentSaveFileReader(@"\Stages\" + _pathExtension);
            LoadSave(stageReader);
            SaveLoadManager.DestroyReader(stageReader);
            MapRectangle = TileManager.MapRectangle;

        }
        public void Unload()
        {
            TileManager.Unload();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(InitialLoadDone);
            TileManager.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {
            InitialLoadDone = reader.ReadBoolean();
            TileManager.LoadSave(reader);
        }
    }
}
