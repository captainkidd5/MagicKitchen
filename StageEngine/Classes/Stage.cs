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
using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.Generators;
using SpriteEngine.Classes.ShadowStuff;
using EntityEngine.ItemStuff;

namespace StageEngine.Classes
{
    public class Stage : ISaveable, ILightDrawable
    {
        public string Name { get; private set; }

        private readonly StageManager _stageManager;
        private readonly PlayerManager _playerManager;
        private readonly StageData _stageData;
        public StageNPCContainer NPCContainer { get; private set; }

        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private readonly Camera2D _camera;


        private string _ambientSoundPackageName => _stageData.AmbientSoundPackageName;
        internal TileManager TileManager { get; private set; }
        private Rectangle MapRectangle { get; set; }

        private string _pathExtension => Name + ".dat";

        internal ItemManager ItemManager { get; private set; }

        private Player Player1 => _playerManager.Player1;


        private PathGrid _pathGrid => TileManager.PathGrid;

        internal bool CamLock => _stageData.MapType == MapType.Exterior;

        public List<ILightDrawable> LightDrawables { get; set; }

        private FlotsamGenerator _flotsamGenerator;
        public Stage(StageManager stageManager,PlayerManager playerManager, NPCManager npcManager, StageData stageData, ContentManager content,
            GraphicsDevice graphics, Camera2D camera)
        {
            Name = stageData.Name;
            _stageManager = stageManager;
            _playerManager = playerManager;
            _stageData = stageData;
            NPCContainer = new StageNPCContainer(npcManager,graphics, content);
            _content = content;
            _graphics = graphics;
            _camera = camera;
            TileManager = new TileManager(graphics, content, camera, _stageData.MapType);

            ItemManager = new ItemManager(Name,TileManager);

            _flotsamGenerator = new FlotsamGenerator(ItemManager, TileManager);

            LightDrawables = new List<ILightDrawable>() {TileManager, NPCContainer, _playerManager };
        }



        public void Update(GameTime gameTime)
        {
            Clock.Update(gameTime);
            _camera.Follow(Player1.Position, MapRectangle);

            TileManager.Update(gameTime);

            ItemManager.Update(gameTime);
            _flotsamGenerator.Update(gameTime);


            NPCContainer.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, PersistentManager characterContainer)
        {


            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _camera.GetTransform(_graphics));
            _graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            TileManager.Draw(spriteBatch);
            characterContainer.Draw(spriteBatch);
            ItemManager.Draw(spriteBatch);
            _playerManager.Draw(spriteBatch);
            NPCContainer.Draw(spriteBatch);
#if DEBUG
            if (Flags.DebugGrid)
                _pathGrid.DrawDebug(spriteBatch);
#endif

            spriteBatch.End();

        }

        public void DrawLights(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < LightDrawables.Count; i++)
                LightDrawables[i].DrawLights(spriteBatch);
        }
        /// <summary>
        /// Loads tiles into memory, then saves them, from tmx map. Should only be called ONCE per stage, per save
        /// </summary>
        public void CreateNewSave()
        {

            TileLoader.CreateNewSave(_stageData, TileManager, _content);
            MapRectangle = TileManager.MapRectangle;
            //TileLoader.LoadStagePortals(_stageData, TileManager);
            //_portalManager.LoadNewStage(Name, TileManager);

            SaveToStageFile();
            TileManager.CleanUp();

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
            NPCContainer.LoadSave(reader);
            NPCContainer.LoadContent(Name, TileManager, ItemManager);

        }
        public void Unload()
        {
            ItemManager.CleanUp();
            NPCContainer.CleanUp();
            TileManager.CleanUp();
            ItemFactory.WorldItemGenerated -= ItemManager.OnWorldItemGenerated;


        }
        public void CleanUp()
        {
            TileManager.CleanUp();
            ItemManager.CleanUp();

            NPCContainer.CleanUp();
        }
    }
}
