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
using Penumbra;
using EntityEngine.Classes.CharacterStuff;
using System.Diagnostics;
using SoundEngine.Classes;
using TiledEngine.Classes;
using UIEngine.Classes;
using EntityEngine.Classes;
using SoundEngine.Classes.SongStuff;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes.Console;

namespace StageEngine.Classes
{
    public class StageManager : Component, ISaveable, ICommandRegisterable
    {
  
        private readonly PlayerManager _playerManager;
        private readonly NPCManager _npcManager;
        private readonly string _startingStageName = "LullabyTown";

        public Player Player1 => _playerManager.Player1;
        private Camera2D _camera;
        private readonly PortalManager _portalManager;

        private Dictionary<string, Stage> Stages { get; set; }
        public Stage CurrentStage { get; private set; }
        public PenumbraComponent Penumbra { get; private set; }


        private string StageSwitchingTo { get; set; }

        private Vector2 NewPlayerPositionOnStageSwitch { get; set; }

        public StageManager(GraphicsDevice graphics, ContentManager content,PlayerManager playerManager, PenumbraComponent penumbra, Camera2D camera) : base(graphics, content)
        {

            Stages = new Dictionary<string, Stage>();
            _playerManager = playerManager;
            Penumbra = penumbra;
            _camera = camera;
            _portalManager = new PortalManager(this);
            _npcManager = new NPCManager(graphics,content);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            LoadStageData();
            _npcManager.LoadContent();
        }




        public Stage GetStage(string stageName)
        {
            Stage stage = Stages[stageName];
            if (stage == null)
                throw new Exception($"stage {stageName} not found");
            return stage;
        }

        /// <summary>
        /// Pauses the game and begins the curtain phase
        /// </summary>
        /// <param name="newStage"></param>
        /// <exception cref="Exception"></exception>
        public void RequestSwitchStage(string newStage, Vector2 newPlayerPos)
        {
            UI.DropCurtain(UI.CurtainDropRate, new Action(SwitchStage));

            StageSwitchingTo = newStage;
            NewPlayerPositionOnStageSwitch = newPlayerPos;
            Flags.Pause = true;

        }
        internal void SwitchStage()
        {
            SongManager.ChangePlaylist(StageSwitchingTo);
            CurrentStage.SaveToStageFile();
            CurrentStage.CleanUp();

            CurrentStage = GetStage(StageSwitchingTo);
            _npcManager.SwitchStage(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager);
            
            CurrentStage.LoadFromStageFile();
            if(_npcManager.CurrentContainer != null)
                _npcManager.CurrentContainer.CleanUp();
            _npcManager.CurrentContainer = CurrentStage.NPCContainer;
            if (CurrentStage == null)
                throw new Exception("Stage with name" + StageSwitchingTo + "does not exist");

            //CurrentStage.LoadFromStageFile();
            
            _camera.Jump(Player1.Position);
            StageSwitchingTo = null;
            Debug.Assert(NewPlayerPositionOnStageSwitch != Vector2.Zero, "New player position should not be zero");
            Player1.Move(NewPlayerPositionOnStageSwitch);
            NewPlayerPositionOnStageSwitch = Vector2.Zero;

            Player1.SwitchStage(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager);
            Flags.Pause = false;
            UI.RaiseCurtain(UI.CurtainDropRate);

            Settings.Camera.LockBounds = CurrentStage.CamLock;

        }

        public void Update(GameTime gameTime)
        {

            if (!Flags.Pause)
            {
                _portalManager.Update(gameTime);
                _playerManager.Update(gameTime);
                _npcManager.Update(gameTime);
                CurrentStage.Update(gameTime);
                if (SoundFactory.AllowAmbientSounds && !SoundFactory.IsPlayingAmbient)
                    SoundFactory.PlayAmbientNoise(CurrentStage.Name);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            CurrentStage.Draw(spriteBatch, gameTime, _npcManager.PersistentManager);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(CurrentStage.Name);
            CurrentStage.SaveToStageFile();
            _npcManager.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {
            string name = reader.ReadString();
            CurrentStage = GetStage(name);
            _npcManager.LoadSave(reader);
            _npcManager.LoadContent();
            //_player1.LoadContent(CurrentStage.ItemManager);
            //Still need to load all stages for portals and graph
            foreach (KeyValuePair<string, Stage> pair in Stages)
            {
                pair.Value.LoadFromStageFile();
                _npcManager.AssignCharactersToStages(pair.Value.Name, pair.Value.TileManager, pair.Value.ItemManager);
                if (pair.Value.Name != name)
                    pair.Value.Unload();
            }

           
            TileLoader.LoadFinished();
            RequestSwitchStage(CurrentStage.Name, Player1.Position);

            

        }
        private void LoadStageData()
        {
            List<StageData> stageData = content.Load<List<StageData>>("maps/StageData");

            foreach (StageData sd in stageData)
            {
                Stages.Add(sd.Name, new Stage(this,_playerManager, _npcManager, _portalManager, sd, content, graphics, _camera, Penumbra));
            }
        }
        public void CreateNewSave(BinaryWriter writer)
        {

            LoadStageData();
            writer.Write(_startingStageName);

            foreach (KeyValuePair<string, Stage> stage in Stages)
            {
                stage.Value.CreateNewSave();
                if(stage.Key != _startingStageName)
                stage.Value.Unload();


            }

            Stages.Clear();
        }

        public void CleanUp()
        {
           foreach(Stage stage in Stages.Values)
            {
                stage.CleanUp();
            }
            Stages.Clear();

            _npcManager.CleanUp();
            _portalManager.CleanUp();
            TileLoader.Unload();
            CurrentStage = null;

        }

        public void RegisterCommands()
        {
            _npcManager.RegisterCommands();
        }
    }
}
