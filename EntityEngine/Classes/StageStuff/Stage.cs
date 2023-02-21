using DataModels;
using EntityEngine.Classes.PlayerStuff;
using EntityEngine.Classes.CharacterStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;

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
using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.Generators;
using SpriteEngine.Classes.ShadowStuff;
using EntityEngine.ItemStuff;
using SpriteEngine.Classes.ParticleStuff;
using TiledEngine.Classes.ZoneStuff;

namespace EntityEngine.Classes.StageStuff
{
    public class Stage : ISaveable, ILightDrawable
    {
        public string Name => _stageData.Name;

        private PlayerManager _playerManager;
        private StageData _stageData;
        private StageManager _stageManager;

        public NPCContainer NPCContainer { get; private set; }

        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private Camera2D _camera;


        private string _ambientSoundPackageName => _stageData.AmbientSoundPackageName;
        private Rectangle MapRectangle { get; set; }

        private string _pathExtension => Name + ".dat";

        internal ItemManager ItemManager { get; private set; }

        private Player Player1 => _playerManager.Player1;



        internal bool CamLock = true;

        public TileManager TileManager { get; private set; }
        public List<ILightDrawable> LightDrawables { get; set; }

        private FlotsamGenerator _flotsamGenerator;
        public Stage(StageData stageData,ContentManager content,
            GraphicsDevice graphics)
        {
            _stageData = stageData;

            _content = content;
            _graphics = graphics;

   
        }

        public void Initialize(Camera2D camera,
            StageManager stageManager, PlayerManager playerManager)
        {
            _camera = camera;
            _stageManager = stageManager;
            NPCContainer = new NPCContainer(_graphics, _content);
            NPCContainer.Initialize(_stageManager);
            TileManager = new TileManager(_graphics, _content);
            TileManager.Initialize(_camera);
            ItemManager = new ItemManager(Name, TileManager);

            _flotsamGenerator = new FlotsamGenerator(this);
            _playerManager = playerManager;
            LightDrawables = new List<ILightDrawable>() { TileManager, NPCContainer, _playerManager };
        }
        public void Update(GameTime gameTime)
        {
            Clock.Update(gameTime);
            _camera.Follow(Player1.Position, MapRectangle);
            TileManager.Update(gameTime);
            ItemManager.Update(gameTime);
            _flotsamGenerator.Update(gameTime);


           // NPCContainer.Update(gameTime);

            ParticleManager.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _camera.GetTransform(_graphics));
            _graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            TileManager.Draw(spriteBatch);

            ItemManager.Draw(spriteBatch);
            _playerManager.Draw(spriteBatch);
            //NPCContainer.Draw(spriteBatch);
#if DEBUG
           // if (SettingsManager.DebugGrid)
                //_pathGrid.DrawDebug(spriteBatch);
#endif
            ParticleManager.Draw(spriteBatch);

            spriteBatch.End();

        }

        public void DrawLights(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < LightDrawables.Count; i++)
                LightDrawables[i].DrawLights(spriteBatch);
        }

        public void CreateNewSave()
        {
            TileManager = MapLoader.CreateTileManagerFromTmxMap(_graphics, _stageData, _content);

            MapRectangle = TileManager.MapRectangle;

            SaveToStageFile();
        }


        /// <summary>
        /// Saves to individual file, called whenever a player leaves a stage. Note that stage data is saved separately from the main save data
        /// </summary>
        public void SaveToStageFile()
        {
            File.WriteAllText(SaveLoadManager.CurrentSave.MetaData.StageFilePath + @"\" + _pathExtension, string.Empty);
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

        public void Save(BinaryWriter writer)
        {
            TileManager.Save(writer);
            ItemManager.Save(writer);
            NPCContainer.Save(writer);

        }

        public void LoadSave(BinaryReader reader)
        {
            TileManager.LoadSave(reader);
            ItemManager.LoadSave(reader);
            ItemManager.LoadTileManager(TileManager);
            
            NPCContainer.LoadContent();

            NPCContainer.LoadSave(reader);

        }

 
        public void SetToDefault()
        {
            TileManager?.SetToDefault();
            ItemManager?.SetToDefault();
            NPCContainer?.SetToDefault();
            if (ItemManager != null)
                ItemFactory.WorldItemGenerated -= ItemManager.OnWorldItemGenerated;

        }
    }
}
