
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Globals.Classes;
using InputEngine.Classes.Input;
using UIEngine.Classes.TextStuff;
using QuakeConsole;
using UIEngine.Classes.SultanInterpreter;
using UIEngine.Classes.Storage;
using ItemEngine.Classes;
using UIEngine.Classes.MainMenuStuff;
using SpriteEngine.Classes;
using IOEngine.Classes;
using UIEngine.Classes.EscMenuStuff;
using System;
using UIEngine.Classes.ButtonStuff;
using Globals.Classes.Console;
using SoundEngine.Classes.SongStuff;
using UIEngine.Classes.ButtonStuff.SettingsMenuStuff;
using MonoGame.Extended.BitmapFonts;
using UIEngine.Classes.RecipeStuff;
using Globals.Classes.Helpers;
using System.Linq;
using static DataModels.Enums;
using InputEngine.Classes;
using DataModels.MapStuff;
using Globals.Classes.Time;
using TextEngine;
using ItemEngine.Classes.StorageStuff;
using UIEngine.Classes.StatusStuff;
using DataModels.DialogueStuff;
using UIEngine.Classes.Storage.ItemAlerts;
using UIEngine.Classes.CursorStuff;
using TextEngine.Classes;
using UIEngine.Classes.EquipmentMenuStuff;
using UIEngine.Classes.SplashScreens;

namespace UIEngine.Classes
{
    public enum GameDisplayState
    {
        None = 0,
        MainMenu = 1,
        InGame = 2,
        SplashScreens = 3,
    }
    public static class UI
    {


        private static GraphicsDevice s_graphics;
        private static ContentManager s_content;

        private static float s_baseLayerDepth = .01f;
        private static float[] LayeringDepths;

        internal static ButtonFactory ButtonFactory;
        public static float CurtainDropRate => Curtain.DropRate;

        public static GameDisplayState GameDisplayState { get; private set; }
        private static GameDisplayState s_requestedGameState;

        private static List<InterfaceSection> s_activeSections;
        /// <summary>
        /// "critical sections" are UI sections which take priority over everything else until dealt with. If this list has at least one element, complete update control is switched over to it.
        /// Ex: confirmation windows
        /// </summary>

        private static List<InterfaceSection> s_criticalSections;
        internal static Texture2D ButtonTexture { get; set; }
        internal static Texture2D GeneralInterfaceTexture { get; set; }

        internal static Color[] ButtonTextureDat;
        internal static Color[] GeneralInterfaceTexDat;

        private static bool s_isHovered;

        internal static StorageContainer PStorage => StorageDisplayHandler.PlayerInventoryDisplay.StorageContainer;
        public static bool IsHovered
        {
            get { return s_isHovered; }
            internal set { s_isHovered = value; Controls.IsUiHovered = value; }
        }

        private static List<InterfaceSection> s_standardSections { get; set; }
        private static List<InterfaceSection> s_mainMenuSections { get; set; }
        private static List<InterfaceSection> s_splashScreenSections { get; set; }


        private static TalkingWindow _talkingWindow { get; set; }

        internal static SettingsMenu SettingsMenu { get; set; }
        internal static ToolBar ToolBar { get; set; }
        internal static ClockBar ClockBar { get; set; }

        public static StatusPanel StatusPanel { get; set; }
        internal static Curtain Curtain { get; set; }
        internal static EscMenu EscMenu { get; set; }

        internal static RecipeBook RecipeBook { get; set; }

        internal static MainMenu MainMenu { get; set; }
        public static StorageDisplayHandler StorageDisplayHandler { get; set; }

        internal static ItemAlertManager ItemAlertManager { get; set; }
        internal static CursorInfoBox CursorInfoBox { get; set; }

        public static Cursor Cursor { get; set; }

        private static Game s_game;
        private static SplashScreen SplashScreen;
        public static Item PlayerCurrentSelectedItem => StorageDisplayHandler.PlayerSelectedItem;

        public static void RemoveCurrentlySelectedItem(int amt) => StorageDisplayHandler.RemovePlayerSelectedItem(amt);
        private static float _frontLayeringDepth;

        //Use this to make sure player isn't overloaded with rapid hover sounds
        internal static bool MayPlayButtonHoverSound;
        private static SimpleTimer s_buttonSoundTimer;
        private static readonly float s_buttonSoundInterval = .15f;
        public static void Load(Game game, GraphicsDevice graphics, ContentManager content, ContentManager mainMenuContentManager, ContentManager splashScreenContentManager)
        {
            if(Flags.DisplaySplashScreens)
            GameDisplayState = GameDisplayState.SplashScreens;
            else
                GameDisplayState = GameDisplayState.MainMenu;

            s_game = game;
            s_graphics = graphics;
            s_content = content;
            AssignLayeringDepths(ref LayeringDepths, s_baseLayerDepth, true);
            ButtonFactory = new ButtonFactory(graphics, content);
            ButtonTexture = content.Load<Texture2D>("UI/Buttons");
            ButtonTextureDat = new Color[ButtonTexture.Width * ButtonTexture.Height];
            ButtonTexture.GetData<Color>(ButtonTextureDat);

            GeneralInterfaceTexture = content.Load<Texture2D>("UI/GeneralInterface");
            GeneralInterfaceTexDat = new Color[GeneralInterfaceTexture.Width * GeneralInterfaceTexture.Height];
            GeneralInterfaceTexture.GetData<Color>(GeneralInterfaceTexDat);

            ToolBar = new ToolBar(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.Low));
            ClockBar = new ClockBar(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.Low));
            StatusPanel = new StatusPanel(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.Low));

            EscMenu = new EscMenu(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.Medium));
            RecipeBook = new RecipeBook(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.High));

            _talkingWindow = new TalkingWindow(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.High));
            SettingsMenu = new SettingsMenu(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.Front));
            Curtain = new Curtain(null, graphics, content, null, .95f);
            StorageDisplayHandler = new StorageDisplayHandler(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.High));
            ItemAlertManager = new ItemAlertManager(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.High));

            CursorInfoBox = new CursorInfoBox(null, graphics, content, null, GetLayeringDepth(UILayeringDepths.Front));

            s_standardSections = new List<InterfaceSection>() { ToolBar, ClockBar,StatusPanel, _talkingWindow,
                EscMenu, RecipeBook, StorageDisplayHandler, ItemAlertManager, CursorInfoBox };

            Cursor = new Cursor();
            Cursor.LoadContent(content);

            MainMenu = new MainMenu(null, graphics, mainMenuContentManager, null, GetLayeringDepth(UILayeringDepths.Back));
            s_mainMenuSections = new List<InterfaceSection>() { MainMenu, SettingsMenu };

            SplashScreen = new SplashScreen(null, graphics, splashScreenContentManager,Vector2.Zero, GetLayeringDepth(UILayeringDepths.Low));
            s_splashScreenSections = new List<InterfaceSection>() { SplashScreen };
            s_activeSections = GetActiveSections();
            s_criticalSections = new List<InterfaceSection>();
            Curtain.LoadContent();

            LoadCurrentSection();

            _frontLayeringDepth = GetLayeringDepth(UILayeringDepths.Front);

            s_buttonSoundTimer = new SimpleTimer(s_buttonSoundInterval, false);


           RaiseCurtain(CurtainDropRate);

        }
        internal static void LoadNewCursorInfo( List<string> text) => CursorInfoBox.LoadNewText( text);

        public static void ActivateSecondaryInventoryDisplay(FurnitureType t, StorageContainer storageContainer, bool displayWallet = false)
            => StorageDisplayHandler.ActivateSecondaryInventoryDisplay(t, storageContainer, displayWallet);

        public static void DeactivateSecondaryInventoryDisplay() => StorageDisplayHandler.DeactivateSecondaryDisplay();

        internal static void AssignLayeringDepths(ref float[] layeringDepths, float baseDepth, bool largeIncrement = false)
        {
            layeringDepths = new float[5];
            float tempDepth = baseDepth;
            for (int i = 0; i < 5; i++)
            {
                tempDepth = UI.IncrementLD(tempDepth, largeIncrement);
                layeringDepths[i] = tempDepth;
            }
        }
        private static float GetLayeringDepth(UILayeringDepths depth)
        {
            return LayeringDepths[(int)depth];
        }
        public static void LoadPlayerInventory(StorageContainer playerStorageContainer, EquipmentStorageContainer equipmentStorageContainer)
        {
            StorageDisplayHandler.Load(playerStorageContainer);
            EscMenu.LoadContent();
            EscMenu.EquipmentMenu.EquipmentDisplay.LoadNewEntityInventory(equipmentStorageContainer, false);
        }

        public static void LoadPlayerUnlockedRecipes(List<string> playerUnlockedrecipes) => RecipeBook.LoadAvailableRecipes(playerUnlockedrecipes);
        private static void LoadCurrentSection()
        {
            foreach (InterfaceSection section in s_activeSections)
                section.LoadContent();
        }

        private static void UnloadCurrentSection()
        {
            foreach (InterfaceSection section in s_activeSections)
                section.Unload();

        }

        private static List<InterfaceSection> GetActiveSections()
        {
            switch (GameDisplayState)
            {
                case GameDisplayState.None:
                    throw new System.Exception("Must have a game display state");
                case GameDisplayState.MainMenu:
                    return s_mainMenuSections;
                case GameDisplayState.InGame:
                    return s_standardSections;
                case GameDisplayState.SplashScreens:
                    return s_splashScreenSections;
                default:
                    throw new System.Exception("Must have a game display state");

            }
        }
        /// <summary>
        /// Deactivates all current sections not contained in passed in list
        /// </summary>
        /// <param name="sections">The sections to exclude from deactivation</param>
        public static void DeactivateAllCurrentSectionsExcept(List<InterfaceSection> sections)
        {
            foreach (InterfaceSection section in s_activeSections)
            {
                if (!sections.Contains(section))
                {
                    section.Deactivate();
                }
            }
        }
        public static void Update(GameTime gameTime)
        {
            IsHovered = false;
            //if (s_requestedGameState != GameDisplayState.None && Curtain.FullyDropped )
            //    FinishChangeGameState();
            if (Controls.WasKeyTapped(Keys.OemTilde))
            {
                CommandConsole.Toggle();

            }

            if (!MayPlayButtonHoverSound)
            {
                if (s_buttonSoundTimer.Run(gameTime))
                {
                    MayPlayButtonHoverSound = true;
                    s_buttonSoundTimer.ResetToZero();
                }
            }

            if (s_criticalSections.Count > 0)
            {
                for (int i = s_criticalSections.Count - 1; i >= 0; i--)
                {
                    InterfaceSection section = s_criticalSections[i];
                    if (section.FlaggedForCriticalRemoval)
                    {
                        section.FlaggedForCriticalRemoval = false;
                        s_criticalSections.RemoveAt(i);
                    }
                    else
                    {
                        section.Update(gameTime);

                    }

                }

            }
            else
            {
                foreach (InterfaceSection section in s_activeSections)
                {

                    section.Update(gameTime);



                }
            }
            Curtain.Update(gameTime);

            Cursor.Update(gameTime);

           
        }


        public static void AddCriticalSection(InterfaceSection section)
        {
            if (s_criticalSections.Contains(section))
                throw new Exception("Section already contained");
            s_criticalSections.Add(section);
        }

        public static void RemoveCriticalSection(InterfaceSection section)
        {
            if (s_criticalSections.Contains(section))
                section.FlaggedForCriticalRemoval = true;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionsToReactive">If list is not null, sections within list will be exempted from reactivation</param>
        public static void ReactiveSections(List<InterfaceSection> sectionsToReactive = null)
        {
            if (sectionsToReactive == null)
            {
                foreach (InterfaceSection section in s_activeSections)
                {
                    if (section.NormallyActivated)
                        section.Activate();
                }
            }
            else
            {
                foreach (InterfaceSection section in sectionsToReactive)
                {
                    if (section.NormallyActivated)
                        section.Activate();

                }
            }
        }
        public static void Draw(SpriteBatch spriteBatch, double frameRate)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            if(Flags.DisplayFPS)
                spriteBatch.DrawString(TextFactory.BitmapFont, frameRate.ToString(),Settings.CenterScreen, Color.Black, layerDepth: .99f);
            Cursor.Draw(spriteBatch);
            foreach (InterfaceSection section in s_activeSections)
            {

                section.Draw(spriteBatch);


            }
            Curtain.Draw(spriteBatch);
            spriteBatch.End();

        }

        public static void DropCurtain(float rate, Action actionOnDrop) => Curtain.FadeIn(rate, actionOnDrop);
        public static void RaiseCurtain(float rate) => Curtain.FadeOut(rate);

        /// <summary>
        /// Returns a float value which is at least slightly larger than the given layerDepth
        /// </summary>
        internal static float IncrementLD(float parentLayerDepth, bool largeIncrement)
        {
            float incrementAmount = 1f;

            if (largeIncrement)
                incrementAmount = 1f;
            float randomOffset = Settings.Random.Next(1, 999) * SpriteUtility.LayerMultiplier * incrementAmount;
            float variedLayerDepth = parentLayerDepth + randomOffset;
            return variedLayerDepth;

        }
        public static EventHandler<EventArgs> ReturnedToMainMenu;

        /// <summary>
        /// After curtain drops, event fired will cause main menu to have child components to cleanup and get rid of save-specific data so that
        /// a fresh save can be safely loaded in.
        /// set sender is stage to false if going to main menu from somewhere besides in game (splash screen)
        /// </summary>
        internal static void ReturnToMainMenu(bool senderIsStage = true)
        {
            s_requestedGameState = GameDisplayState.MainMenu;

            DropCurtain(CurtainDropRate,
                new Action(() =>
                {
                    SongManager.ChangePlaylist("MainMenu-Outer");
                    if (senderIsStage)
                        ReturnedToMainMenu.Invoke(null, null);
                    else
                        RaiseCurtain(CurtainDropRate);
                    FinishChangeGameState();

                }));

        }

        private static void FinishChangeGameState()
        {
            UnloadCurrentSection();
            GameDisplayState = s_requestedGameState;
            s_requestedGameState = GameDisplayState.None;
            s_activeSections = GetActiveSections();
            LoadCurrentSection();
        }

        internal static void Exit()
        {
            s_game.Exit();
        }

        public static void SaveGame(SaveFile saveFile)
        {
            SaveLoadManager.Save(saveFile);
        }

        public static void LoadGame(SaveFile saveFile)
        {

            DropCurtain(CurtainDropRate, new Action(() =>
            {
                SaveLoadManager.SetCurrentSave(saveFile.MetaData.Name);
                SaveLoadManager.Load(saveFile);
                s_requestedGameState = GameDisplayState.InGame;
                SaveLoadManager.SaveGame(null);

                FinishChangeGameState();

            }));
        }


        public static void LoadNewConversation(Dialogue dialogue) => _talkingWindow.LoadNewConversation(dialogue);
        public static Direction TalkingDirection { get => _talkingWindow.DirectionPlayerShouldFace; set => _talkingWindow.DirectionPlayerShouldFace = value; }

        public static bool IsTalkingWindowActive => _talkingWindow.IsActive;
        public static bool WasTalkingWindowJustActivated => _talkingWindow.WasJustActivated;
    }
}