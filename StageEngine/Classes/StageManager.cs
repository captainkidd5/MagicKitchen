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
using ItemEngine.Classes;

namespace StageEngine.Classes
{
    public class StageManager : Component, ISaveable, ICommandRegisterable
    {
  
        private readonly PlayerManager _playerManager;



        public Player Player1 => _playerManager.Player1;
        private Camera2D _camera;
        private readonly PortalManager _portalManager;

        public Stage CurrentStage { get; private set; }





        public StageManager(GraphicsDevice graphics, ContentManager content,PlayerManager playerManager, Camera2D camera) : base(graphics, content)
        {

            _playerManager = playerManager;

            _camera = camera;
            _portalManager = new PortalManager(this);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            LoadStageData();
        }






        /// <summary>
        /// Pauses the game and begins the curtain phase
        /// </summary>
        /// <param name="newStage"></param>
        /// <exception cref="Exception"></exception>
        public void RequestSwitchStage(string newStage, Vector2 newPlayerPos)
        {
            UI.DropCurtain(UI.CurtainDropRate, new Action(EnterWOrld));
            Flags.Pause = true;

        }
        internal void EnterWOrld()
        {
            CurrentStage.SaveToStageFile();



            ItemFactory.WorldItemGenerated += CurrentStage.ItemManager.OnWorldItemGenerated;


                CurrentStage.LoadFromStageFile();


            _playerManager.LoadContent();
            _camera.Jump(Player1.Position);

            Flags.Pause = false;
            UI.RaiseCurtain(UI.CurtainDropRate);

            Settings.Camera.LockBounds = CurrentStage.CamLock;

        }

        public void Update(GameTime gameTime)
        {

            if (!Flags.Pause)
            {
                _portalManager.Update(gameTime);
                CurrentStage.Update(gameTime);

                _playerManager.Update(gameTime);


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
            TileLoader.Save(writer);
            _portalManager.Save(writer);
        }

        public void LoadSave(BinaryReader reader)
        {

            TileLoader.LoadSave(reader); 
            _portalManager.LoadSave(reader);
;
            CurrentStage.LoadFromStageFile();
 
            RequestSwitchStage(CurrentStage.Name, Player1.Position);
        }
        private void LoadStageData()
        {
            StageData stageData = content.Load<StageData>("maps/StageData");

         
          CurrentStage = new Stage(this,_playerManager, stageData, content, graphics, _camera);
            
        }
        public void CreateNewSave(BinaryWriter writer)
        {

            LoadStageData();
            CurrentStage.CreateNewSave();
           // _playerManager.LoadContent(
            _playerManager.Save(writer);


            TileLoader.Save(writer);
            _portalManager.Save(writer);
            _portalManager.CleanUp();

            TileLoader.Unload();

        }

        public void CleanUp()
        {
            CurrentStage.CleanUp();


            _portalManager.CleanUp();
            TileLoader.Unload();
            CurrentStage = null;

        }

        public void RegisterCommands()
        {
            //CurrentStage.RegisterCommands();
        }
    }
}
