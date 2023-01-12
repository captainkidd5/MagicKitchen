using DataModels;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using InputEngine.Classes.Input;
using IOEngine.Classes;
using ItemEngine.Classes;
using UIEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.RenderTargetStuff;
using System.Collections.Generic;
using System.IO;
using TextEngine;
using TiledEngine.Classes;
using EntityEngine.Classes;
using EntityEngine.Classes.CharacterStuff;
using Globals.Classes.Time;
using SoundEngine;
using SoundEngine.Classes;
using QuakeConsole;
using Globals.Classes.Console;
using Core.Classes.ConsoleStuff;
using System;
using SoundEngine.Classes.SongStuff;
using InputEngine.Classes;
using DataModels.QuestStuff;
using TextEngine.Classes;
using SpriteEngine.Classes.ParticleStuff.WeatherStuff;
using System.Diagnostics;
using Globals.XPlatformHelpers;
using UIEngine.Classes.QuestLogStuff;
using EntityEngine.Classes.StageStuff;

namespace Core
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Camera2D Camera;


        private ContentManager _mainMenuContentManager;
        private ContentManager _splashScreenContentManager;

        private StageManager _stageManager;

        private CommandList _commandList;

        private PlayerManager _playerManager;

        private QuestManager _questManager;
        public Player Player1 => _playerManager.Player1;

        public static SpriteFont MainFont { get; set; }


        private ConsoleComponent consoleComponent;

        private FrameCounter _frameCounter;
        public Game1()
        {
            //throw new Exception("Hit");
            Debug.WriteLine($"device type is{Globals.Classes.Flags.DeviceType} ");
            _graphics = new GraphicsDeviceManager(this);
            _graphics.HardwareModeSwitch = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            consoleComponent = new ConsoleComponent(this);
            Components.Add(consoleComponent);
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Settings.Window_ClientSizeChanged;

            Activated += IsActivated;
            Deactivated += IsDeactivated;

            AssetLocator.GetFiles = Directory.GetFiles;
            //DesktopGl doesn't need any extension
            AssetLocator.GetStaticFileDirectory = (string path) => { return ""; };
            AssetLocator.GetContentFileDirectory = () => { return ""; };
        }

        private void IsDeactivated(object sender, EventArgs e)
        {
            Settings.WindowFocused = false;
        }

        private void IsActivated(object sender, EventArgs e)
        {
            Settings.WindowFocused = true;
        }


        protected override void Initialize()
        {
            IsFixedTimeStep = true;
            _mainMenuContentManager = new ContentManager(Content.ServiceProvider);
            _mainMenuContentManager.RootDirectory = "Content";

            _splashScreenContentManager = new ContentManager(Content.ServiceProvider);
            _splashScreenContentManager.RootDirectory = "Content";
            Camera = new Camera2D(GraphicsDevice.Viewport);

            Settings.Load(_graphics, Camera, Window);

            Settings.SetResolution(Settings.ScreenWidth, Settings.ScreenHeight);

            PhysicsManager.Initialize();

            Window.Title = "Magic ";



            _stageManager = new StageManager(GraphicsDevice, Content);
          //  _stageManager.Initialize(_playerManager, Camera);
            _playerManager = new PlayerManager(GraphicsDevice, Content);
           // _playerManager.Initialize(_stageManager);
            _stageManager.PlayerManager = _playerManager;
            //Penumbra.SpriteBatchTransformEnabled = true;
            _commandList = new CommandList();

            
            base.Initialize();

        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SaveLoadManager.FetchAllMetadata();
            SettingsManager.LoadSettings();
            CommandConsole.Load(consoleComponent);
            _commandList.Load();
            _stageManager.RegisterCommands();
            Clock.Load();

            //This is the android content issue
          //  MainFont = Content.Load<SpriteFont>("Fonts/Font");

            TextFactory.Load(Content);
            LanguageManager.Load(Content);
            Scheduler.Load(Content);
            MapLoader.Initialize(Content);
            ItemFactory.LoadContent(Content);

            EntityFactory.Load(Content);

            _questManager = new QuestManager();
            _questManager.Load(Content);

            Controls.Load(Camera, GraphicsDevice, Content);

            RenderTargetManager.Load(GraphicsDevice);

            SpriteFactory.LoadContent(GraphicsDevice, Content);


            PhysicsManager.LoadContent(Content, GraphicsDevice, MainFont);

            SongManager.Load(Content);
            UI.Load(this, GraphicsDevice, Content, _mainMenuContentManager, _splashScreenContentManager);
            UI.LoadQuests(_questManager);
            SoundFactory.Load(Content);

            SaveLoadManager.SaveCreated += OnSaveCreated;
            SaveLoadManager.SaveLoaded += OnSaveLoaded;
            SaveLoadManager.SaveSaved += OnSaveSaved;
            UI.ReturnedToMainMenu += OnReturnToMainMenu;
            //CommandConsole.RegisterCommand("save", "saves current game", SaveLoadManager.SaveGame);
            _frameCounter = new FrameCounter(4);

            WeatherManager.SetWeather(WeatherType.SandStorm, false);

        }


        protected override void Update(GameTime gameTime)
        {
  
            //if (Settings.WindowFocused)
            //   {

            Controls.Update(gameTime);
            SongManager.Update(gameTime);
            if (UI.GameDisplayState == GameDisplayState.InGame)
            {
                _stageManager.Update(gameTime);

            }
            WeatherManager.Update(gameTime);
            UI.Update(gameTime);
            if (!Flags.Pause)
            {
                PhysicsManager.Update(gameTime);

            }

            base.Update(gameTime);
            //}

        }

        protected override void Draw(GameTime gameTime)
        {
            //  if (Settings.WindowFocused)
            // {

            if (SettingsManager.IsNightTime)
            {
                if (UI.GameDisplayState == GameDisplayState.InGame)
                {
                    RenderTargetManager.SetTarget(RenderTargetManager.LightsTarget);
                    GraphicsDevice.Clear(Color.Transparent);



                    _spriteBatch.Begin(blendState: BlendState.Additive, transformMatrix: Settings.Camera.GetTransform(GraphicsDevice));
                    _stageManager.DrawLights(_spriteBatch);
                    _spriteBatch.End();
                }
                else if (UI.GameDisplayState == GameDisplayState.MainMenu)
                {
                    RenderTargetManager.SetTarget(RenderTargetManager.LightsTarget);
                    GraphicsDevice.Clear(Color.Transparent);
                    _spriteBatch.Begin(blendState: BlendState.Additive);
                    UI.DrawLights(_spriteBatch);
                    _spriteBatch.End();
                }


            }

            RenderTargetManager.SetTarget(RenderTargetManager.MainTarget);
            GraphicsDevice.Clear(Color.Transparent);

            if (UI.GameDisplayState == GameDisplayState.InGame)
            {

                _stageManager.Draw(_spriteBatch, gameTime);

            }

            if (SettingsManager.DebugVelcro)
                PhysicsManager.Draw(GraphicsDevice, Camera);
            _frameCounter.Update(gameTime.ElapsedGameTime.TotalSeconds);

            RenderTargetManager.RemoveRenderTarget();
            GraphicsDevice.Clear(Color.Transparent);
            if (SettingsManager.IsNightTime)
            {

                RenderTargetManager.SetTarget(RenderTargetManager.UILightsAffectableTarget);
                GraphicsDevice.Clear(Color.Transparent);
                WeatherManager.Draw(_spriteBatch);
                UI.DrawLightsAffectable(_spriteBatch);
                RenderTargetManager.RemoveRenderTarget();
                GraphicsDevice.Clear(Color.Transparent);
            }

            RenderTargetManager.SetTarget(RenderTargetManager.UITarget);
            GraphicsDevice.Clear(Color.Transparent);
            if (UI.GameDisplayState == GameDisplayState.MainMenu && !SettingsManager.IsNightTime)
            {
                UI.DrawLightsAffectable(_spriteBatch);
            }
            UI.Draw(_spriteBatch, _frameCounter.framerate);
           
            RenderTargetManager.RemoveRenderTarget();

            if (SettingsManager.IsNightTime)
            {
                SpriteFactory.LightEffect.Parameters["MaskTexture"].SetValue(RenderTargetManager.LightsTarget);
                _spriteBatch.Begin(blendState: BlendState.AlphaBlend, effect: SpriteFactory.LightEffect);
                _spriteBatch.Draw(RenderTargetManager.MainTarget, Settings.ScreenRectangle, Color.Red);
                _spriteBatch.End();

            }
            else
            {
                _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                _spriteBatch.Draw(RenderTargetManager.MainTarget, Settings.ScreenRectangle, Color.White);
                _spriteBatch.End();
            }

            if (UI.GameDisplayState == GameDisplayState.MainMenu)
            {
                if (SettingsManager.IsNightTime)
                {
                    SpriteFactory.LightEffect.Parameters["MaskTexture"].SetValue(RenderTargetManager.LightsTarget);

                    _spriteBatch.Begin(blendState: BlendState.AlphaBlend, effect: SpriteFactory.LightEffect);
                    // UI.DrawLightsAffectable(_spriteBatch);
                    _spriteBatch.Draw(RenderTargetManager.UILightsAffectableTarget, Settings.ScreenRectangle, Color.Red);

                    _spriteBatch.End();
                }
            }

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);


            _spriteBatch.Draw(RenderTargetManager.UITarget, Settings.ScreenRectangle, Color.White);

            _spriteBatch.End();




            base.Draw(gameTime);

            //  }


        }

        /// <summary>
        /// Note: if User either creates a new save, or loads an existing save, we know that it's no longer a first time boot up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSaveCreated(object? sender, FileCreatedEventArgs e)
        {
            Flags.IsNewGame = true;

            _stageManager.LoadStageDataFromJson();


            BinaryWriter writer = e.BinaryWriter;
            Clock.SetToDefault();
            Clock.Save(writer);
            _stageManager.CreateNewSave(writer);
            _stageManager.Initialize(_playerManager, Camera);
            _stageManager.GlobalNPCContainer.Save(writer);

            _playerManager.Initialize(_stageManager);
            _playerManager.Save(writer);
            _playerManager.LoadContent();


            MapLoader.Portalmanager.Save(writer);

            _playerManager.Player1.GiveItem("Wooden_Hook", 1);
            SaveLoadManager.DestroyWriter(writer);
            Flags.FirstBootUp = false;
            _stageManager.EnterWorld(_stageManager.CurrentStage.Name, Player1.Position);

        }
        public void OnSaveLoaded(object? sender, FileLoadedEventArgs e)
        {
            //_playerManager.LoadContent();
            _stageManager.LoadContent();
            BinaryReader reader = e.BinaryReader;

            Clock.Load(reader);
            _stageManager.LoadSave(reader);
            _stageManager.GlobalNPCContainer.LoadSave(reader);
            _playerManager.LoadSave(reader);
           
            MapLoader.Portalmanager.LoadSave(reader);

            SaveLoadManager.DestroyReader(reader);
            Flags.FirstBootUp = false;
            WeatherManager.SetWeather(WeatherType.SandStorm, true);

        }



        public void OnSaveSaved(object? sender, FileSavedEventArgs e)
        {
           // CommandConsole.Append("Saving current game..");

            BinaryWriter writer = e.BinaryWriter;
            Clock.Save(writer);
            _stageManager.Save(writer);
            _stageManager.GlobalNPCContainer.Save(writer);
            _playerManager.Save(writer);
      
            MapLoader.Portalmanager.Save(writer);

            //CommandConsole.Append("...Saved!");
            SaveLoadManager.DestroyWriter(writer);

        }

        public void OnReturnToMainMenu(object? sender, EventArgs e)
        {
            _stageManager.SetToDefault();
            UI.RaiseCurtain(UI.CurtainDropRate);
        }

    }
}
