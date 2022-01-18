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

namespace StageEngine.Classes
{
    public class StageManager : Component
    {
        private  Camera2D _camera;
        private readonly CharacterManager _characterManager;
        private readonly PlayerManager _playerManager;
        private readonly PortalManager _portalManager;

        public StageManager(GraphicsDevice graphics, ContentManager content,
            CharacterManager characterManager,PlayerManager playerManager, PenumbraComponent penumbra, Camera2D camera) : base(graphics, content)
        {
            _characterManager = characterManager;
            _playerManager = playerManager;
            Stages = new Dictionary<string, Stage>();
            Penumbra = penumbra;
            _camera = camera;
            _portalManager = new PortalManager(this, playerManager);
            _portalManager.IntialLoad();
        }

        public override void Load()
        {
            base.Load();
            List<StageData> stageData = content.Load<List<StageData>>("maps/StageData");

            foreach (StageData sd in stageData)
            {
                Stages.Add(sd.Name, new Stage(this, _characterManager, _playerManager,_portalManager, sd, content, graphics, _camera, Penumbra));
            }

            CurrentStage.FirstEntryLoad();

            _playerManager.Player1.LoadContent(content, CurrentStage.TileManager, CurrentStage.ItemManager);
            CurrentStage.LoadPortals();
            _playerManager.Player1.LoadToNewStage(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager);

            foreach (Character character in _characterManager.AllCharacters)
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
        }

        private  Dictionary<string, Stage> Stages { get; set; }
        public  Stage CurrentStage { get; private set; }
        public  PenumbraComponent Penumbra { get; private set; }


        private  string StageSwitchingTo { get; set; }

        private  bool WasStageSwitchingLastFrame { get; set; }
        private  Vector2 NewPlayerPositionOnStageSwitch { get; set; }


        public  Stage GetStage(string stageName)
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
        public  void RequestSwitchStage(string newStage, Vector2 newPlayerPos)
        {
            UI.FadeIn(.00055f);

            StageSwitchingTo = newStage;
            NewPlayerPositionOnStageSwitch = newPlayerPos;
            Flags.Pause = true;

        }
        internal  void SwitchStage()
        {
            CurrentStage.SaveToIndividualFile();
            CurrentStage.Unload();

            CurrentStage = GetStage(StageSwitchingTo);

            if (CurrentStage == null)
                throw new Exception("Stage with name" + StageSwitchingTo + "does not exist");

            CurrentStage.LoadFromIndividualFile();
            _characterManager.SwitchStage(StageSwitchingTo);
            _camera.Jump(_playerManager.Player1.Position);
            StageSwitchingTo = null;
            Flags.IsStageLoading = false;
            Debug.Assert(NewPlayerPositionOnStageSwitch != Vector2.Zero, "New player position should not be zero");
            _playerManager.Player1.Move(NewPlayerPositionOnStageSwitch);
            NewPlayerPositionOnStageSwitch = Vector2.Zero;

            WasStageSwitchingLastFrame = Flags.IsStageLoading;
            _playerManager.Player1.LoadToNewStage(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager);
            Flags.Pause = false;
            UI.FadeOut(.00055f);

            Settings.Camera.LockBounds = CurrentStage.CamLock;

        }

        public  void Update(GameTime gameTime)
        {
            if (WasStageSwitchingLastFrame != Flags.IsStageLoading)
            {
                SwitchStage();
            }
            if (!Flags.Pause)
            {
                _portalManager.Update(gameTime);
                _characterManager.Update(gameTime, CurrentStage.Name);
                CurrentStage.Update(gameTime);
                if (SoundFactory.AllowAmbientSounds && !SoundFactory.IsPlayingAmbient)
                    SoundFactory.PlayAmbientNoise(CurrentStage.Name);
            }
        }

        public  void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            CurrentStage.Draw(spriteBatch, gameTime);
        }

        public  void Save(BinaryWriter writer)
        {
            writer.Write(CurrentStage.Name);
        }

        public  void LoadSave(BinaryReader reader)
        {
            string name = reader.ReadString();
            CurrentStage = GetStage(name);
        }

        public  void CreateNewSave(BinaryWriter writer)
        {
            CurrentStage = GetStage("LullabyTown");
            foreach (KeyValuePair<string, Stage> stage in Stages)
            {
                if (stage.Value != CurrentStage)
                {
                    stage.Value.FirstEntryLoad();
                    stage.Value.LoadPortals();

                    stage.Value.Unload();
                }

            }
        }
    }
}
