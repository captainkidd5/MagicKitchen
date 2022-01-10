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

namespace MagicKitchen
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Camera2D Camera;


        private ContentManager MainMenuContentManager { get; set; }

        public static Player Player1 => PlayerManager.Player1;

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
            MainMenuContentManager = new ContentManager(Content.ServiceProvider);
            MainMenuContentManager.RootDirectory = "Content";
            Camera = new Camera2D(GraphicsDevice.Viewport);

            Settings.Load(_graphics, Camera, Window);

            Settings.SetResolution(Settings.ScreenWidth, Settings.ScreenHeight);
            //Todo: Change this to false if loading save
            Flags.FirstTimeLoad = true;
            PhysicsManager.Initialize(GraphicsDevice, Penumbra);





            Penumbra.Initialize();
            //Penumbra.SpriteBatchTransformEnabled = true;

            base.Initialize();

        }

        protected override void LoadContent()
        {



            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SaveLoadManager.InitialLoad();

            if (SaveLoadManager.IsSaveNameAvailable("testSave"))
            {
                SaveLoadManager.CreateNewSave("testSave");

            }

            CommandConsole.Load(consoleComponent);


            Clock.Load();
            List<StageData> stageData = Content.Load<List<StageData>>("Maps/StageData");
            MainFont = Content.Load<SpriteFont>("Fonts/Font");
            TextFactory.Load(MainFont);
            TileLoader.LoadContent(Content);
            ItemFactory.LoadContent(Content);
            EntityFactory.Load(Content);
            CharacterManager.LoadCharacterData(GraphicsDevice, Content);
            PlayerManager.LoadContent(GraphicsDevice, Content);
            PortalManager.IntialLoad();
            QuestManager.Load(GraphicsDevice, Content);

            StageManager.LoadContent(GraphicsDevice, Penumbra, Content, Camera);
            Controls.Load(Camera, GraphicsDevice, Content);
            SpriteFactory.LoadContent(GraphicsDevice, Content);


            Settings.SetResolution(1280, 720);
            PhysicsManager.LoadContent(Content, GraphicsDevice, MainFont);

            UserInterface.Load(GraphicsDevice, Content);
            UserInterface.RegisterCharacterClickEvents();
            RenderTargetManager.Load(GraphicsDevice);
            SoundFactory.Load(Content);
            Penumbra.OnVirtualSizeChanged(new PenumbraComponent.VirtualSizeChagnedEventArgs { VirtualWidth = 1280, VirtualHeight = 720 });

            // _graphics.IsFullScreen = true;
            // _graphics.ApplyChanges();
            SaveCurrentGame();
            Settings.DebugTexture = new Texture2D(GraphicsDevice, 1, 1);
            Settings.DebugTexture.SetData<Color>(new Color[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Controls.Update(gameTime);

            //Command console within ui should still be useable even if game is paused
            UserInterface.Update(gameTime);
            StageManager.Update(gameTime);

            if (!Flags.Pause)
            {
                PhysicsManager.Update(gameTime);
                SoundFactory.Update(gameTime, PlayerManager.Player1.Position);

            }
            Controls.UpdateCursor();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            RenderTargetManager.SetTarget(RenderTargetManager.MainTarget);
            if (Flags.EnableShadows && Flags.IsNightTime)
            {
                Penumbra.BeginDraw();
            }
            StageManager.Draw(_spriteBatch, gameTime);
            if (Flags.EnableShadows && Flags.IsNightTime)
            {
                Penumbra.Transform = Camera.GetTransform(GraphicsDevice);
                Penumbra.Draw(gameTime);

            }



            if (Flags.DebugVelcro)
                PhysicsManager.Draw(GraphicsDevice, Camera);
            UserInterface.Draw(_spriteBatch);

            RenderTargetManager.RemoveRenderTarget();
            // Everything between penumbra.BeginDraw and penumbra.Draw will be
            // lit by the lighting system.



            RenderTargetManager.DrawTarget(_spriteBatch, RenderTargetManager.MainTarget);



            //RenderTargetManager.SetTarget(RenderTargetManager.UITarget);
            RenderTargetManager.RemoveRenderTarget();

            //_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.getTransformAndSet(GraphicsDevice));
            //GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };


            //_spriteBatch.End();

            // UI.Draw(_spriteBatch);
            //if (GlobalFlags.DebugVelcro)
            //    PhysicsManager.Draw(GraphicsDevice, Camera);

            //RenderTargetManager.RemoveRenderTarget();

            base.Draw(gameTime);

            //_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.getTransformation(GraphicsDevice));

            //_spriteBatch.End();
            // UI.Draw(_spriteBatch);

        }



        public void SaveCurrentGame()
        {
            BinaryWriter writer = SaveLoadManager.GetCurrentSaveFileWriter();

            StageManager.Save(writer);


        }

        public void LoadSave()
        {
            BinaryReader reader = SaveLoadManager.GetCurrentSaveFileReader();
            StageManager.LoadSave(reader);
        }
    }
}
