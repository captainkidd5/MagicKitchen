using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataModels;
using Globals.Classes;
using IOEngine.Classes;
using System.IO;
using InputEngine.Classes.Input;
using SpriteEngine.Classes;
using PhysicsEngine.Classes;
using EntityEngine.Classes.PlayerStuff;

using EntityEngine.Classes.CharacterStuff;
using System.Diagnostics;
using SoundEngine.Classes;
using TiledEngine.Classes;
using UIEngine.Classes;
using EntityEngine.Classes;
using SoundEngine.Classes.SongStuff;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes.Console;
using ItemEngine.Classes;
using tainicom.Aether.Physics2D.Dynamics;
using System.Text.Json;
using System.Text.Json.Serialization;
using TiledEngine.Classes.PortalStuff;

namespace StageEngine.Classes
{
    public class StageManager : Component, ISaveable, ICommandRegisterable
    {

        private readonly PlayerManager _playerManager;



        public Player Player1 => _playerManager.Player1;
        private Camera2D _camera;

        public Stage CurrentStage { get; private set; }

        private Dictionary<string,Stage> _allStages { get; set; }

        private HullBody _playAreaBody;
        private HullBody _spawnAreaBody;


        public static Dictionary<string, StageData> AllStageData;

        public StageManager(GraphicsDevice graphics, ContentManager content, PlayerManager playerManager, Camera2D camera) : base(graphics, content)
        {

            _playerManager = playerManager;

            _camera = camera;
            _allStages = new Dictionary<string, Stage>();

        }

        public override void LoadContent()
        {
            string basePath = content.RootDirectory + "/Maps";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var files = Directory.GetFiles(basePath);
            string jsonString = string.Empty;

            foreach (var file in files)
                if (file.EndsWith("StageData.json"))
                {
                    jsonString = File.ReadAllText(file);
                    AllStageData = JsonSerializer.Deserialize<List<StageData>>(jsonString, options).ToDictionary(x => x.Name);
                    break;

                }
            base.LoadContent();
        }






        /// <summary>
        /// Pauses the game and begins the curtain phase
        /// </summary>
        /// <param name="newStage"></param>
        /// <exception cref="Exception"></exception>
        public void RequestSwitchStage(string newStage, Vector2 newPlayerPos)
        {
            UI.DropCurtain(UI.CurtainDropRate, new Action(EnterWorld));
            CurrentStage.SaveToStageFile();
            CurrentStage.CleanUp();
            CurrentStage = _allStages[newStage];
            CurrentStage.LoadFromStageFile();

            _playerManager.LoadContent(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager, AllStageData);
            foreach (Portal p in MapLoader.Portalmanager.AllPortals)
            {
                p.PortalClicked += OnPortalClicked;
            }
            Flags.Pause = true;

        }
        internal void EnterWorld()
        {

            ItemFactory.WorldItemGenerated += CurrentStage.ItemManager.OnWorldItemGenerated;

            _camera.Jump(Player1.Position);

            Flags.Pause = false;
            UI.RaiseCurtain(UI.CurtainDropRate);

            Settings.Camera.LockBounds = CurrentStage.CamLock;
            if (_playAreaBody != null)
                _playAreaBody.Destroy();
            if (Flags.DisplayPlayAreaCollisions)
            {

                _playAreaBody = PhysicsManager.CreateRectangularHullBody(BodyType.Static, _camera.position,
                    Settings.ActiveAreaWidth, Settings.ActiveAreaWidth, new List<Category>() { (Category)PhysCat.None },
                    new List<Category>() { },
                    null, null, isSensor: true);

                _spawnAreaBody = PhysicsManager.CreateRectangularHullBody(BodyType.Static, _camera.position,
                   Settings.SpawnableAreaWidth, Settings.SpawnableAreaWidth, new List<Category>() { (Category)PhysCat.PlayArea },
                   new List<Category>() { (Category)PhysCat.NPC },
                   null, null, isSensor: true);
            }

        }

        public void Update(GameTime gameTime)
        {

            if (!Flags.Pause)
            {
                CurrentStage.Update(gameTime);

                _playerManager.Update(gameTime);

                if (Flags.DisplayPlayAreaCollisions)
                {

                    _playAreaBody.Position = _camera.position;
                    _spawnAreaBody.Position = _camera.position;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            CurrentStage.Draw(spriteBatch, gameTime);
        }

        public void DrawLights(SpriteBatch spriteBatch)
        {
            CurrentStage.DrawLights(spriteBatch);
        }


        public void Save(BinaryWriter writer)
        {
            writer.Write(CurrentStage.Name);
            CurrentStage.SaveToStageFile();
        }

        public void LoadSave(BinaryReader reader)
        {
            SetToDefault();

            MapLoader.LoadContent(content);
            InitializeStages();

            CurrentStage = _allStages["TestIsland"];
            foreach(var kvp in _allStages)
            {
              
                    //Loading from stage file will add necessary portal data to map loader
                    kvp.Value.LoadFromStageFile();
                if (kvp.Value != CurrentStage)
                {
                    kvp.Value.CleanUp();
                }
              
            }
            //TODO
            //Do we need to unsubscribe from these somewhere on exiting game and loading separate save?
            foreach (Portal p in MapLoader.Portalmanager.AllPortals)
            {
                p.PortalClicked += OnPortalClicked;
            }
            RequestSwitchStage(CurrentStage.Name, Player1.Position);
        }

        public void CreateNewSave(BinaryWriter writer)
        {
            SetToDefault();
            InitializeStages();
            foreach(KeyValuePair<string, Stage> kvp in _allStages)
            {
                kvp.Value.CreateNewSave();
            }
            CurrentStage = _allStages["TestIsland"];
            _playerManager.Save(writer);

        }

        private void InitializeStages()
        {
            _allStages.Clear();
            foreach (var kvp in AllStageData)
            {
                StageData stageData = kvp.Value;
                Stage stage = new Stage(content, graphics, _camera, stageData, this, _playerManager);
                _allStages.Add(stageData.Name, stage);
                MapLoader.TileManagers.Add(stage.Name, stage.TileManager);
            }

          
            MapLoader.FillPortalGraph();

        }
        void OnPortalClicked(Portal p)
        {
            Portal returnPortal = MapLoader.Portalmanager.GetCorrespondingPortal(p);
            RequestSwitchStage(p.To, returnPortal.Position);
        }
        public void CleanUp()
        {
            CurrentStage.CleanUp();
            CurrentStage = null;

        }

        public void RegisterCommands()
        {
            //CurrentStage.RegisterCommands();
        }

        public void SetToDefault()
        {
            foreach (KeyValuePair<string, Stage> kvp in _allStages)
            {
                kvp.Value.SetToDefault();
            }
            MapLoader.TileManagers.Clear();
            _playerManager.SetToDefault();

        }
    }
}
