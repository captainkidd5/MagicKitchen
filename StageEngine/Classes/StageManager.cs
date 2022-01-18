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
    public static class StageManager
    {
        private static ContentManager Content;
        private static GraphicsDevice Graphics;
        private static Camera2D Camera;

        private static Dictionary<string, Stage> Stages { get; set; }
        public static Stage CurrentStage { get; private set; }
        public static PenumbraComponent Penumbra { get; private set; }


        private static string StageSwitchingTo { get; set; }

        private static bool WasStageSwitchingLastFrame { get; set; }
        private static Vector2 NewPlayerPositionOnStageSwitch { get; set; }
        /// <summary>
        /// Should be called once per session
        /// </summary>
        public static void LoadContent(GraphicsDevice graphics, PenumbraComponent penumbra, ContentManager content, Camera2D camera)
        {
            Stages = new Dictionary<string, Stage>();
            Graphics = graphics;
            Penumbra = penumbra;
            Content = content;
            Camera = camera;
            List<StageData> stageData = content.Load<List<StageData>>("maps/StageData");

            foreach (StageData sd in stageData)
            {
                Stages.Add(sd.Name, new Stage(sd, content, graphics, camera, Penumbra));
            }

            if (Flags.FirstTimeLoad)
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
            CurrentStage.FirstEntryLoad();

            PlayerManager.Player1.LoadContent(Content, CurrentStage.TileManager, CurrentStage.ItemManager);
            CurrentStage.LoadPortals();
            PlayerManager.Player1.LoadToNewStage(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager);

            foreach (Character character in CharacterManager.AllCharacters)
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

        internal static void Unload()
        {

        }

        public static Stage GetStage(string stageName)
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
        public static void RequestSwitchStage(string newStage, Vector2 newPlayerPos)
        {
            UI.FadeIn(.00055f);

            StageSwitchingTo = newStage;
            NewPlayerPositionOnStageSwitch = newPlayerPos;
            Flags.Pause = true;

        }
        internal static void SwitchStage()
        {
            CurrentStage.SaveToIndividualFile();
            CurrentStage.Unload();

            CurrentStage = GetStage(StageSwitchingTo);

            if (CurrentStage == null)
                throw new Exception("Stage with name" + StageSwitchingTo + "does not exist");

            CurrentStage.LoadFromIndividualFile();
            CharacterManager.SwitchStage(StageSwitchingTo);
            Camera.Jump(PlayerManager.Player1.Position);
            StageSwitchingTo = null;
            Flags.IsStageLoading = false;
            Debug.Assert(NewPlayerPositionOnStageSwitch != Vector2.Zero, "New player position should not be zero");
            PlayerManager.Player1.Move(NewPlayerPositionOnStageSwitch);
            NewPlayerPositionOnStageSwitch = Vector2.Zero;

            WasStageSwitchingLastFrame = Flags.IsStageLoading;
            PlayerManager.Player1.LoadToNewStage(CurrentStage.Name, CurrentStage.TileManager, CurrentStage.ItemManager);
            Flags.Pause = false;
            UI.FadeOut(.00055f);

            Settings.Camera.LockBounds = CurrentStage.CamLock;

        }

        public static void Update(GameTime gameTime)
        {
            if (WasStageSwitchingLastFrame != Flags.IsStageLoading)
            {
                SwitchStage();
            }
            if (!Flags.Pause)
            {
                PortalManager.Update(gameTime);
                CharacterManager.Update(gameTime, CurrentStage.Name);
                CurrentStage.Update(gameTime);
                if (SoundFactory.AllowAmbientSounds && !SoundFactory.IsPlayingAmbient)
                    SoundFactory.PlayAmbientNoise(CurrentStage.Name);
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            CurrentStage.Draw(spriteBatch, gameTime);
        }

        public static void Save(BinaryWriter writer)
        {
            writer.Write(CurrentStage.Name);
        }

        public static void LoadSave(BinaryReader reader)
        {
            string name = reader.ReadString();
            CurrentStage = GetStage(name);
        }

        public static void CreateNewSave(BinaryWriter writer)
        {

        }
    }
}
