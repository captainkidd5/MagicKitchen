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
using EntityEngine.Classes.NPCStuff;
using System.Diagnostics;
using SoundEngine.Classes;
using TiledEngine.Classes;
using UIEngine.Classes;
using EntityEngine.Classes;

namespace StageEngine.Classes
{
    public class StageManager : Component, ISaveable
    {
  
        private readonly EntityManager _entityManager;

        private Player _player1 => _entityManager.Player1;
        private Camera2D _camera;
        private readonly PortalManager _portalManager;

        private Dictionary<string, Stage> Stages { get; set; }
        public Stage CurrentStage { get; private set; }
        public PenumbraComponent Penumbra { get; private set; }


        private string StageSwitchingTo { get; set; }

        private bool WasStageSwitchingLastFrame { get; set; }
        private Vector2 NewPlayerPositionOnStageSwitch { get; set; }

        public StageManager(GraphicsDevice graphics, ContentManager content,EntityManager entityManager, PenumbraComponent penumbra, Camera2D camera) : base(graphics, content)
        {

            Stages = new Dictionary<string, Stage>();
            _entityManager = entityManager;
            Penumbra = penumbra;
            _camera = camera;
            _portalManager = new PortalManager(this, _entityManager);
        }

        public override void Load()
        {
            base.Load();
            LoadStageData();

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
            UI.DropCurtain(UI.CurtainDropRate);

            StageSwitchingTo = newStage;
            NewPlayerPositionOnStageSwitch = newPlayerPos;
            Flags.Pause = true;

        }
        internal void SwitchStage()
        {
            CurrentStage.SaveToStageFile();
            CurrentStage.Unload();

            CurrentStage = GetStage(StageSwitchingTo);
            CurrentStage.LoadFromStageFile();

            if (CurrentStage == null)
                throw new Exception("Stage with name" + StageSwitchingTo + "does not exist");

            //CurrentStage.LoadFromStageFile();
            _entityManager.SwitchStage(StageSwitchingTo);
            _camera.Jump(_player1.Position);
            StageSwitchingTo = null;
            Flags.IsStageLoading = false;
            Debug.Assert(NewPlayerPositionOnStageSwitch != Vector2.Zero, "New player position should not be zero");
            _player1.Move(NewPlayerPositionOnStageSwitch);
            NewPlayerPositionOnStageSwitch = Vector2.Zero;

            WasStageSwitchingLastFrame = Flags.IsStageLoading;
            _player1.LoadToNewStage(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager);
            Flags.Pause = false;
            UI.RaiseCurtain(UI.CurtainDropRate);

            Settings.Camera.LockBounds = CurrentStage.CamLock;

        }

        public void Update(GameTime gameTime)
        {
            if (WasStageSwitchingLastFrame != Flags.IsStageLoading)
            {
                SwitchStage();
            }
            if (!Flags.Pause)
            {
                _portalManager.Update(gameTime);
                _entityManager.Update(gameTime);
                CurrentStage.Update(gameTime);
                if (SoundFactory.AllowAmbientSounds && !SoundFactory.IsPlayingAmbient)
                    SoundFactory.PlayAmbientNoise(CurrentStage.Name);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            CurrentStage.Draw(spriteBatch, gameTime);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(CurrentStage.Name);
            CurrentStage.SaveToStageFile();

        }

        public void LoadSave(BinaryReader reader)
        {
            string name = reader.ReadString();
            CurrentStage = GetStage(name);

            //Still need to load all stages for portals and graph
            foreach (KeyValuePair<string, Stage> pair in Stages)
            {
                pair.Value.LoadFromStageFile();
                if (pair.Value.Name != name)
                    pair.Value.Unload();
            }

            _player1.LoadContent(content, CurrentStage.TileManager, CurrentStage.ItemManager);
            _player1.LoadToNewStage(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager);

            foreach (Character character in _characterManager.Entities)
            {
                Stage stage = Stages[character.CurrentStageName];
                if (stage == null)
                    throw new Exception($"Stage {character.CurrentStageName} does not exist, check to make sure" +
                        $"both a tmx map with name and npcdata stage name match");
                character.LoadContent(content, stage.TileManager, stage.ItemManager);
                character.LoadToNewStage(stage.Name, stage.TileManager, stage.ItemManager);
                stage.NPCs.Add(character);
                character.PlayerSwitchedStage(CurrentStage.Name, false);
            }

            TileLoader.LoadFinished();
            _camera.Jump(_player1.Position);

        }
        private void LoadStageData()
        {
            List<StageData> stageData = content.Load<List<StageData>>("maps/StageData");

            foreach (StageData sd in stageData)
            {
                Stages.Add(sd.Name, new Stage(this, _characterManager, _playerManager, _portalManager, sd, content, graphics, _camera, Penumbra));
            }
        }
        public void CreateNewSave(BinaryWriter writer)
        {

            LoadStageData();
            string startingStageName = "LullabyTown";
            writer.Write(startingStageName);

            foreach (KeyValuePair<string, Stage> stage in Stages)
            {
                stage.Value.CreateNewSave();
                if(stage.Key != startingStageName)
                stage.Value.Unload();


            }
            Stages.Clear();
        }
    }
}
