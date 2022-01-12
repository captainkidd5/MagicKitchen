
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

namespace UIEngine.Classes
{
    public static class UI
    {


        private static  GraphicsDevice s_graphics;
        private static  ContentManager s_content;




        private static List<InterfaceSection> s_activeSections;
        internal static Texture2D ButtonTexture { get; set; }
        internal static Texture2D GeneralInterfaceTexture { get; set; }

        internal static Color[] ButtonTextureDat;
        internal static Color[] GeneralInterfaceTexDat;
        //test
        internal static bool IsHovered { get; set; }

        private static List<InterfaceSection> s_standardSections { get; set; }

        internal static CustomConsole CommandConsole { get; set; }
        public static DialogueWindow TalkingWindow { get; set; }

        internal static ToolBar ToolBar { get; set; }
        internal static ClockBar ClockBar { get; set; }

        internal static Curtain Curtain { get; set; }

        internal static InventoryDisplay SecondaryInventoryDisplay { get; set; }


        public static Cursor Cursor { get; set; }


        public static void Load(GraphicsDevice graphics, ContentManager content, StorageContainer playerStorageContainer)
        {
            s_graphics = graphics;
            s_content = content;
            ButtonTexture = content.Load<Texture2D>("UI/Buttons");
            ButtonTextureDat = new Color[ButtonTexture.Width * ButtonTexture.Height];
            ButtonTexture.GetData<Color>(ButtonTextureDat);

            GeneralInterfaceTexture = content.Load<Texture2D>("UI/GeneralInterface");
            GeneralInterfaceTexDat = new Color[GeneralInterfaceTexture.Width * GeneralInterfaceTexture.Height];
            GeneralInterfaceTexture.GetData<Color>(GeneralInterfaceTexDat);

            ToolBar = new ToolBar(null,graphics, content, null);
            ClockBar = new ClockBar(null,graphics, content,  null);

            ToolBar.Load(playerStorageContainer);

            CommandConsole = new CustomConsole(null,graphics, content, null);
            TalkingWindow = new DialogueWindow(null, graphics, content,null);
            Curtain = new Curtain(null, graphics, content, null);
            s_standardSections = new List<InterfaceSection>() { ToolBar, ClockBar, CommandConsole, TalkingWindow, Curtain };

            SecondaryInventoryDisplay = new InventoryDisplay(null, graphics, content, null);
            s_activeSections = s_standardSections;
            Cursor = new Cursor();
            Cursor.LoadContent(content);
            foreach (InterfaceSection section in s_standardSections)
            {
                section.Load();
            }
        }


        /// <summary>
        /// Deactivates all current sections not contained in passed in list
        /// </summary>
        /// <param name="sections">The sections to exclude from deactivation</param>
        public static void DeactivateAllCurrentSectionsExcept(List<InterfaceSection> sections)
        {
            foreach(InterfaceSection section in s_activeSections)
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
                if (section.IsActive)
                {

                section.Update(gameTime);
                if (section.Hovered)
                    IsHovered = true;
                }

            }


            //Reached end of update loop and nothing was hovered
            if (!IsHovered)
            {

                if(Controls.IsClicked && TalkingWindow.IsActive && !TalkingWindow.Hovered)
                {
                    ReactiveSections();
                    TalkingWindow.IsActive = false;
                }
            }
            Cursor.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionsToReactive">If list is not null, sections within list will be exempted from reactivation</param>
        public static void ReactiveSections(List<InterfaceSection> sectionsToReactive = null)
        {
            if(sectionsToReactive == null)
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
         //   RenderTargetManager.SetToMainTarget();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            Cursor.Draw(spriteBatch);
            foreach (InterfaceSection section in s_activeSections)
            {
                if (section.IsActive)
                {
                    section.Draw(spriteBatch);

                }
            }

            spriteBatch.End();
            //RenderTargetManager.RemoveRenderTarget();
            //RenderTargetManager.DrawMainTarget(spriteBatch);
        }

        public static void FadeIn(float rate) => Curtain.FadeIn(rate);
        public static void FadeOut(float rate) => Curtain.FadeOut(rate);
       
    }
}
