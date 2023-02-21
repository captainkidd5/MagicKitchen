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

namespace EntityEngine.Classes.StageStuff
{
    public class StageManager : Component, ISaveable, ICommandRegisterable
    {

        public PlayerManager PlayerManager { get; set; }



        public Player Player1 => PlayerManager.Player1;
        private Camera2D _camera;

        public Stage CurrentStage { get; private set; }

        public Dictionary<string,Stage> AllStages { get; private set; }

        private HullBody _playAreaBody;
        private HullBody _spawnAreaBody;


        public static Dictionary<string, StageData> AllStageData;
        public NPCContainer GlobalNPCContainer { get; private set; }

        private bool _firstLoad = true;
        public StageManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {

            AllStages = new Dictionary<string, Stage>();
            GlobalNPCContainer = new NPCContainer(graphics, content);
        }
        
        public void Initialize(PlayerManager playerManager, Camera2D camera)
        {
            PlayerManager = playerManager;

            _camera = camera;
          
            GlobalNPCContainer.Initialize(this);
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }

        public void LoadStageDataFromJson()
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
        }






        /// <summary>
        /// Pauses the game and begins the curtain phase
        /// </summary>
        /// <param name="newStage"></param>
        /// <exception cref="Exception"></exception>
        public void RequestSwitchStage(string newStage, Vector2 newPlayerPos)
        {
            UI.DropCurtain(UI.CurtainDropRate, new Action(()=> EnterWorld(newStage, newPlayerPos)));
           
            Flags.Pause = true;

        }
        private void EnterWorld(string newStage, Vector2 newPlayerPos)
        {
            CurrentStage.SaveToStageFile();
            CurrentStage.SetToDefault();
            CurrentStage = AllStages[newStage];
            
            CurrentStage.LoadFromStageFile();

            Player1.Move(newPlayerPos);
            foreach (Portal p in MapLoader.Portalmanager.AllPortals)
            {
                p.PortalClicked -= OnPortalClicked;

                p.PortalClicked += OnPortalClicked;
            }
            ItemFactory.WorldItemGenerated -= CurrentStage.ItemManager.OnWorldItemGenerated;

            ItemFactory.WorldItemGenerated += CurrentStage.ItemManager.OnWorldItemGenerated;

            _camera.Jump(Player1.Position);

            Flags.Pause = false;
            UI.RaiseCurtain(UI.CurtainDropRate);

            Settings.Camera.LockBounds = CurrentStage.CamLock;
            if (_playAreaBody != null)
                _playAreaBody.DestroyFromPhysicsWorld();
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
            _firstLoad = false;
        }

        public void Update(GameTime gameTime)
        {

            if (!Flags.Pause && !_firstLoad)
            {
                Shared.CurrentStageName = CurrentStage.Name;
                CurrentStage.Update(gameTime);

                PlayerManager.Update(gameTime);

                if (Flags.DisplayPlayAreaCollisions)
                {

                    _playAreaBody.Position = _camera.position;
                    _spawnAreaBody.Position = _camera.position;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(!_firstLoad)
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

            MapLoader.Initialize(content);
            InitializeStages();

            foreach(var kvp in AllStages)
            {
              
                    //Loading from stage file will add necessary portal data to map loader
                    kvp.Value.LoadFromStageFile();
                if (kvp.Value != CurrentStage)
                {
                    kvp.Value.SetToDefault();
                }
              
            }
   
            RequestSwitchStage(CurrentStage.Name, Player1.Position);
           // PlayerManager.LoadContent();
        }

        public void CreateNewSave(BinaryWriter writer)
        {
            //SetToDefault();
            InitializeStages();
            foreach(KeyValuePair<string, Stage> kvp in AllStages)
            {
                kvp.Value.CreateNewSave();
            }
            MapLoader.FillPortalGraph();
            MapLoader.CreatePortalObjects();
        }

        private void InitializeStages()
        {
            AllStages.Clear();
            foreach (var kvp in AllStageData)
            {
                StageData stageData = kvp.Value;
                Stage stage = new Stage(stageData,content, graphics);
                AllStages.Add(stageData.Name, stage);
            }
            CurrentStage = AllStages["TestIsland"];

            foreach (var kvp in AllStages)
            {
                kvp.Value.Initialize(_camera, this, PlayerManager);


            }


            MapLoader.FillPortalGraph();
            MapLoader.CreatePortalObjects();

        }
        void OnPortalClicked(Portal p)
        {
            Portal returnPortal = MapLoader.Portalmanager.GetCorrespondingPortal(p);
            RequestSwitchStage(p.To, returnPortal.Position + returnPortal.OffSetEntry);
        }


        public void RegisterCommands()
        {
            //CurrentStage.RegisterCommands();
        }

        public void SetToDefault()
        {
            CurrentStage.SetToDefault();
            CurrentStage = null;
            _firstLoad = true;
            foreach (KeyValuePair<string, Stage> kvp in AllStages)
            {
                kvp.Value.SetToDefault();
            }
            PlayerManager.SetToDefault();

        }
    }
}
