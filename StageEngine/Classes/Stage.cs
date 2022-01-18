using DataModels;
using EntityEngine.Classes.PlayerStuff;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
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
using IOEngine.Classes;

namespace StageEngine.Classes
{
    public class Stage : ISaveable
    {
        public string Name { get; private set; }

        private readonly StageManager _stageManager;
        private readonly CharacterManager _characterManager;
        private readonly PlayerManager _playerManager;
        private readonly PortalManager _portalManager;
        private readonly StageData _stageData;

        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private readonly Camera2D _camera;
        private readonly PenumbraComponent _penumbra;

        private string _ambientSoundPackageName => _stageData.AmbientSoundPackageName;
        internal TileManager TileManager { get; private set; }
        private Rectangle MapRectangle { get; set; }

        private string _pathExtension => Name + ".dat";

        internal ItemManager ItemManager { get; private set; }

        private Player Player1 { get; set; }

        public List<Entity> NPCs { get; set; }

        private PathGrid _pathGrid => TileManager.PathGrid;

        internal bool CamLock => _stageData.MapType == MapType.Exterior;
        public Stage(StageManager stageManager,CharacterManager characterManager,PlayerManager playerManager,PortalManager portalManager, StageData stageData, ContentManager content,
            GraphicsDevice graphics, Camera2D camera, PenumbraComponent penumbra)
        {
            Name = stageData.Name;
            _stageManager = stageManager;
            _characterManager = characterManager;
           _playerManager = playerManager;
            _portalManager = portalManager;
            _stageData = stageData;

            _content = content;
            _graphics = graphics;
            _camera = camera;
            _penumbra = penumbra;
            Player1 = playerManager.Player1;
            TileManager = new TileManager(graphics, content, camera, penumbra);
            ItemManager = new ItemManager(Name);
            NPCs = new List<Entity>();


        }



        public void Update(GameTime gameTime)
        {
            Clock.Update(gameTime);
            _playerManager.Update(gameTime);
            _camera.Follow(Player1.Position, MapRectangle);

            TileManager.Update(gameTime);

            ItemManager.Update(gameTime);


        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            _penumbra.AmbientColor = Color.DarkSlateGray;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _camera.GetTransform(_graphics));
            _graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            _playerManager.Draw(spriteBatch);
            _characterManager.Draw(spriteBatch, Name);
            TileManager.Draw(spriteBatch);

            ItemManager.Draw(spriteBatch);

#if DEBUG
            if (Flags.DebugGrid)
                _pathGrid.DrawDebug(spriteBatch);
#endif

            spriteBatch.End();

        }
        /// <summary>
        /// Loads tiles into memory, then saves them, from tmx map. Should only be called ONCE per stage, per save
        /// </summary>
        public void CreateNewSave()
        {

            TileLoader.CreateNewSave(_stageData, TileManager, _content);
            MapRectangle = TileManager.MapRectangle;
            TileLoader.LoadStagePortals(_stageData, TileManager);
            _portalManager.LoadNewStage(Name, TileManager);

            SaveToStageFile();
        }


        /// <summary>
        /// Saves to individual file, called whenever a player leaves a stage. Note that stage data is saved separately from the main save data
        /// </summary>
        public void SaveToStageFile()
        {
            File.WriteAllText(SaveLoadManager.CurrentSave.StageFilePath + @"\" + _pathExtension, string.Empty);
            BinaryWriter stageWriter = SaveLoadManager.GetCurrentSaveFileWriter(@"\Stages\" + _pathExtension);
            Save(stageWriter);
            SaveLoadManager.DestroyWriter(stageWriter);
        }

        public void LoadFromStageFile()
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
            TileManager.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {
            TileManager.LoadSave(reader);
        }
    }
}
