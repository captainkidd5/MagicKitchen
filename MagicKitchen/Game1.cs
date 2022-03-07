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
using EntityEngine.Classes.NPCStuff;
using Globals.Classes.Time;
using EntityEngine.Classes.NPCStuff.QuestStuff;
using SoundEngine;
using SoundEngine.Classes;
using QuakeConsole;
using Globals.Classes.Console;
using MagicKitchen.Classes.ConsoleStuff;

namespace MagicKitchen
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Camera2D Camera;


        private ContentManager _mainMenuContentManager;

        private StageManager _stageManager;

        private EntityManager _entityManager;

        private CommandList _commandList;
        public Player Player1 => _entityManager.Player1;

        public static SpriteFont MainFont { get; set; }

        public static PenumbraComponent Penumbra;

        private ConsoleComponent consoleComponent;
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
            _mainMenuContentManager = new ContentManager(Content.ServiceProvider);
            _mainMenuContentManager.RootDirectory = "Content";
            Camera = new Camera2D(GraphicsDevice.Viewport);

            Settings.Load(_graphics, Camera, Window);

            Settings.SetResolution(Settings.ScreenWidth, Settings.ScreenHeight);

            PhysicsManager.Initialize(Penumbra);




            Penumbra.Initialize();

            _entityManager = new EntityManager(GraphicsDevice, Content);
            _stageManager = new StageManager(GraphicsDevice, Content, _entityManager, Penumbra, Camera);
            //Penumbra.SpriteBatchTransformEnabled = true;
            _commandList = new CommandList();
            base.Initialize();

        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SaveLoadManager.FetchAllMetadata();

            CommandConsole.Load(consoleComponent);
            _commandList.Load();
            Clock.Load();
            List<StageData> stageData = Content.Load<List<StageData>>("Maps/StageData");
            MainFont = Content.Load<SpriteFont>("Fonts/Font");
            TextFactory.Load(MainFont);
            TileLoader.LoadContent(Content);
            ItemFactory.LoadContent(Content);
            EntityFactory.Load(Content);
            

            Controls.Load(Camera, GraphicsDevice, Content);
            SpriteFactory.LoadContent(GraphicsDevice, Content);


            Settings.SetResolution(1280, 720);
            PhysicsManager.LoadContent(Content, GraphicsDevice, MainFont);

            UI.Load(this,GraphicsDevice, Content, _mainMenuContentManager);
            RenderTargetManager.Load(GraphicsDevice);
            SoundFactory.Load(Content);
            Penumbra.OnVirtualSizeChanged(new PenumbraComponent.VirtualSizeChagnedEventArgs { VirtualWidth = 1280, VirtualHeight = 720 });
            
            SaveLoadManager.SaveCreated += OnSaveCreated;
            SaveLoadManager.SaveLoaded += OnSaveLoaded;
            SaveLoadManager.SaveSaved += OnSaveSaved;
            CommandConsole.RegisterCommand("save", "saves current game", SaveLoadManager.SaveGame);

            Settings.DebugTexture = new Texture2D(GraphicsDevice, 1, 1);
            Settings.DebugTexture.SetData<Color>(new Color[] { Color.White });
        }

       
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            Controls.Update(gameTime);

            if (UI.GameDisplayState == GameDisplayState.InGame)
            {
                _stageManager.Update(gameTime);

            }
            UI.Update(gameTime);
            if (!Flags.Pause)
            {
                PhysicsManager.Update(gameTime);
                SoundFactory.Update(gameTime, Player1.Position);

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            RenderTargetManager.SetTarget(RenderTargetManager.MainTarget);

            if (UI.GameDisplayState == GameDisplayState.InGame)
            {

                if (Flags.EnableShadows && Flags.IsNightTime)
                {
                    Penumbra.BeginDraw();
                }
                _stageManager.Draw(_spriteBatch, gameTime);
                if (Flags.EnableShadows && Flags.IsNightTime)
                {
                    Penumbra.Transform = Camera.GetTransform(GraphicsDevice);
                    Penumbra.Draw(gameTime);

                }

            }

            if (Flags.DebugVelcro)
                PhysicsManager.Draw(GraphicsDevice, Camera);
            UI.Draw(_spriteBatch);

            RenderTargetManager.RemoveRenderTarget();
            // Everything between penumbra.BeginDraw and penumbra.Draw will be
            // lit by the lighting system.

            RenderTargetManager.DrawTarget(_spriteBatch, RenderTargetManager.MainTarget);
            RenderTargetManager.RemoveRenderTarget();

            base.Draw(gameTime);



        }
        private void LoadInGameManagers()
        {
            _entityManager.LoadContent();
            _stageManager.LoadContent();
        }
        public void OnSaveCreated(object? sender, FileCreatedEventArgs e)
        {
            BinaryWriter writer = e.BinaryWriter;
            _stageManager.CreateNewSave(writer);
            SaveLoadManager.DestroyWriter(writer);
        }
        public void OnSaveLoaded(object? sender, FileLoadedEventArgs e)
        {
            LoadInGameManagers();
            BinaryReader reader = e.BinaryReader;
            _stageManager.LoadSave(reader);
            SaveLoadManager.DestroyReader(reader);
        }

        

        public void OnSaveSaved(object? sender, FileSavedEventArgs e)
        {
            CommandConsole.Append("Saving current game..");

            BinaryWriter writer = e.BinaryWriter;
            _stageManager.Save(writer);
            CommandConsole.Append("...Saved!");
        }
       
    }
}
