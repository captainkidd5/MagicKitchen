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
using Penumbra;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.RenderTargetStuff;
using StageEngine.Classes;
using System.Collections.Generic;
using System.IO;
using TextEngine;
using TiledEngine.Classes;
using EntityEngine.Classes;
using EntityEngine.Classes.CharacterStuff;
using Globals.Classes.Time;
using EntityEngine.Classes.CharacterStuff.QuestStuff;
using SoundEngine;
using SoundEngine.Classes;
using QuakeConsole;
using Globals.Classes.Console;
using MagicKitchen.Classes.ConsoleStuff;
using System;
using SoundEngine.Classes.SongStuff;
using InputEngine.Classes;

namespace MagicKitchen
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Camera2D Camera;


        private ContentManager _mainMenuContentManager;

        private StageManager _stageManager;


        private CommandList _commandList;

        private PlayerManager _playerManager;
        public Player Player1 => _playerManager.Player1;

        public static SpriteFont MainFont { get; set; }

        public static PenumbraComponent Penumbra;

        private ConsoleComponent consoleComponent;

        private FrameCounter _frameCounter;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.HardwareModeSwitch = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            Penumbra = new PenumbraComponent(this);
            consoleComponent = new ConsoleComponent(this);
            Components.Add(consoleComponent);
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Settings.Window_ClientSizeChanged;

        }
            
        protected override void Initialize()
        {
            IsFixedTimeStep = true;
            _mainMenuContentManager = new ContentManager(Content.ServiceProvider);
            _mainMenuContentManager.RootDirectory = "Content";
            Camera = new Camera2D(GraphicsDevice.Viewport);

            Settings.Load(_graphics, Camera, Window);

            Settings.SetResolution(Settings.ScreenWidth, Settings.ScreenHeight);

            PhysicsManager.Initialize(Penumbra);




            Penumbra.Initialize();
            _playerManager = new PlayerManager(GraphicsDevice, Content);
            _stageManager = new StageManager(GraphicsDevice, Content, _playerManager, Penumbra, Camera);
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
            MainFont = Content.Load<SpriteFont>("Fonts/Font");
            TextFactory.Load(Content);
            TileLoader.LoadContent(Content);
            ItemFactory.LoadContent(Content);
            EntityFactory.Load(Content);


            Controls.Load(Camera, GraphicsDevice, Content);
            RenderTargetManager.Load(GraphicsDevice);

            SpriteFactory.LoadContent(GraphicsDevice, Content);


            PhysicsManager.LoadContent(Content, GraphicsDevice, MainFont);
            SongManager.Load(Content);
            UI.Load(this, GraphicsDevice, Content, _mainMenuContentManager);
            _playerManager.LoadContent();

            SoundFactory.Load(Content);
            Penumbra.OnVirtualSizeChanged(new PenumbraComponent.VirtualSizeChagnedEventArgs {
                VirtualWidth = (int)Settings.NativeWidth,
                VirtualHeight = (int)Settings.NativeHeight
            });

            SaveLoadManager.SaveCreated += OnSaveCreated;
            SaveLoadManager.SaveLoaded += OnSaveLoaded;
            SaveLoadManager.SaveSaved += OnSaveSaved;
            UI.ReturnedToMainMenu += OnReturnToMainMenu;
            CommandConsole.RegisterCommand("save", "saves current game", SaveLoadManager.SaveGame);
            _frameCounter = new FrameCounter(4);
        }


        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            Controls.Update(gameTime);
            SongManager.Update(gameTime);
            if (UI.GameDisplayState == GameDisplayState.InGame)
            {
                _stageManager.Update(gameTime);

            }
            UI.Update(gameTime);
            if (!Flags.Pause)
            {
                PhysicsManager.Update(gameTime);

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
          
            RenderTargetManager.SetTarget(RenderTargetManager.MainTarget);
            GraphicsDevice.Clear(Color.Transparent);
            if (UI.GameDisplayState == GameDisplayState.InGame)
            {

                _stageManager.Draw(_spriteBatch, gameTime);

            }

            if (Flags.DebugVelcro)
                PhysicsManager.Draw(GraphicsDevice, Camera);
            _frameCounter.Update( gameTime.ElapsedGameTime.TotalSeconds);

            RenderTargetManager.RemoveRenderTarget();



            RenderTargetManager.SetTarget(RenderTargetManager.UITarget);
            GraphicsDevice.Clear(Color.Transparent);

            UI.Draw(_spriteBatch, _frameCounter.framerate);
            RenderTargetManager.RemoveRenderTarget();


            RenderTargetManager.DrawTarget(_spriteBatch, RenderTargetManager.UITarget);


            base.Draw(gameTime);



        }

        /// <summary>
        /// Note: if User either creates a new save, or loads an existing save, we know that it's no longer a first time boot up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSaveCreated(object? sender, FileCreatedEventArgs e)
        {
            Flags.IsNewGame = true;

            BinaryWriter writer = e.BinaryWriter;
            Clock.Save(writer);
            _playerManager.Save(writer);
            _stageManager.CreateNewSave(writer);
            SaveLoadManager.DestroyWriter(writer);
            Flags.FirstBootUp = false;

        }
        public void OnSaveLoaded(object? sender, FileLoadedEventArgs e)
        {
            //_playerManager.LoadContent();
            _stageManager.LoadContent();
            BinaryReader reader = e.BinaryReader;
            Clock.Load(reader);

            _playerManager.LoadSave(reader);
            _stageManager.LoadSave(reader);
            SaveLoadManager.DestroyReader(reader);
            Flags.FirstBootUp = false;
        }



        public void OnSaveSaved(object? sender, FileSavedEventArgs e)
        {
            CommandConsole.Append("Saving current game..");

            BinaryWriter writer = e.BinaryWriter;
            Clock.Save(writer);
            _playerManager.Save(writer);
            _stageManager.Save(writer);
            CommandConsole.Append("...Saved!");
            SaveLoadManager.DestroyWriter(writer);

        }

        public void OnReturnToMainMenu(object? sender, EventArgs e)
        {
            _stageManager.CleanUp();
        }

    }
}
