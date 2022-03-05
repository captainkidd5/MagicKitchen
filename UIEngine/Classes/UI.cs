﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using UIEngine.Classes.DebugStuff;
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

namespace UIEngine.Classes
{
    public enum GameDisplayState
    {
        None = 0,
        MainMenu = 1,
        InGame = 2
    }
    public static class UI
    {


        private static GraphicsDevice s_graphics;
        private static ContentManager s_content;

        private static float s_baseLayerDepth = .01f;
        public static float CurtainDropRate => Curtain.DropRate;

        public static GameDisplayState GameDisplayState { get; private set; } = GameDisplayState.MainMenu;
        private static GameDisplayState s_requestedGameState;

        private static List<InterfaceSection> s_activeSections;
        internal static Texture2D ButtonTexture { get; set; }
        internal static Texture2D GeneralInterfaceTexture { get; set; }

        internal static Color[] ButtonTextureDat;
        internal static Color[] GeneralInterfaceTexDat;
        public static bool IsHovered { get; private set; }

        private static List<InterfaceSection> s_standardSections { get; set; }
        private static List<InterfaceSection> s_mainMenuSections { get; set; }

        internal static CustomConsole CommandConsole { get; set; }
        public static DialogueWindow TalkingWindow { get; set; }

        internal static ToolBar ToolBar { get; set; }
        internal static ClockBar ClockBar { get; set; }
        
        internal static Curtain Curtain { get; set; }
        internal static EscMenu EscMenu { get; set; }
        internal static InventoryDisplay SecondaryInventoryDisplay { get; set; }

        internal static MainMenu MainMenu { get; set; }

        public static Cursor Cursor { get; set; }

        private static Game s_game;

        public static void Load(Game game, GraphicsDevice graphics, ContentManager content, ContentManager mainMenuContentManager)
        {
            s_game = game;
            s_graphics = graphics;
            s_content = content;
            ButtonTexture = content.Load<Texture2D>("UI/Buttons");
            ButtonTextureDat = new Color[ButtonTexture.Width * ButtonTexture.Height];
            ButtonTexture.GetData<Color>(ButtonTextureDat);

            GeneralInterfaceTexture = content.Load<Texture2D>("UI/GeneralInterface");
            GeneralInterfaceTexDat = new Color[GeneralInterfaceTexture.Width * GeneralInterfaceTexture.Height];
            GeneralInterfaceTexture.GetData<Color>(GeneralInterfaceTexDat);

            ToolBar = new ToolBar(null, graphics, content, null, s_baseLayerDepth);
            ClockBar = new ClockBar(null, graphics, content, null, s_baseLayerDepth);
            EscMenu = new EscMenu(null, graphics, content, null, s_baseLayerDepth);

            CommandConsole = new CustomConsole(null, graphics, content, null, s_baseLayerDepth);
            TalkingWindow = new DialogueWindow(null, graphics, content, null, s_baseLayerDepth);
            Curtain = new Curtain(null, graphics, content, null, s_baseLayerDepth);
            s_standardSections = new List<InterfaceSection>() { ToolBar, ClockBar, CommandConsole, TalkingWindow, Curtain, EscMenu };

            SecondaryInventoryDisplay = new InventoryDisplay(null, graphics, content, null, s_baseLayerDepth);
            Cursor = new Cursor();
            Cursor.LoadContent(content);

            MainMenu = new MainMenu(null, graphics, mainMenuContentManager, null, s_baseLayerDepth);
            s_mainMenuSections = new List<InterfaceSection>() { MainMenu, Curtain };
            s_activeSections = GetActiveSections();

            LoadCurrentSection();

        }
        public static void LoadPlayerInventory(StorageContainer playerStorageContainer)
        {
            ToolBar.Load(playerStorageContainer);

        }
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
                    section.IsActive = false;
                }
            }
        }
        public static void Update(GameTime gameTime)
        {
            IsHovered = false;
            if (s_requestedGameState != GameDisplayState.None && Curtain.FullyDropped )
                FinishChangeGameState();
            if (Controls.WasKeyTapped(Keys.F1))
            {
                Flags.Pause = !Flags.Pause;
                CommandConsole.Toggle();
            }
            if (Controls.WasKeyTapped(Keys.OemTilde))
            {
                Flags.Pause = !Flags.Pause;
                Globals.Classes.Console.CommandConsole.Toggle();

            }

            foreach (InterfaceSection section in s_activeSections)
            {
        
                    section.Update(gameTime);
                    if (section.Hovered)
                        IsHovered = true;
                

            }

           

            Cursor.Update(gameTime);

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
                    section.IsActive = true;
                }
            }
            else
            {
                foreach (InterfaceSection section in sectionsToReactive)
                {
                    section.IsActive = true;
                }
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            Cursor.Draw(spriteBatch);
            foreach (InterfaceSection section in s_activeSections)
            {
      
                    section.Draw(spriteBatch);

                
            }

            spriteBatch.End();

        }

        public static void DropCurtain(float rate) => Curtain.FadeIn(rate);
        public static void RaiseCurtain(float rate) => Curtain.FadeOut(rate);

        /// <summary>
        /// Returns a float value which is at least slightly larger than the given layerDepth
        /// </summary>
        internal static float IncrementLD(float parentLayerDepth)
        {
            float randomOffset = Settings.Random.Next(1, 999) * SpriteUtility.LayerMultiplier * .001f;
            float variedLayerDepth = parentLayerDepth + randomOffset;
            return variedLayerDepth;

        }

        private static void StartChangeGameState(GameDisplayState newState)
        {
            DropCurtain(CurtainDropRate);
            s_requestedGameState = newState;

           
        }
        private static void FinishChangeGameState()
        {
            Flags.IsStageLoading = false;
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
            SaveLoadManager.CurrentSave = saveFile;
            SaveLoadManager.Load(saveFile);
            StartChangeGameState(GameDisplayState.InGame);
        }
    }
}