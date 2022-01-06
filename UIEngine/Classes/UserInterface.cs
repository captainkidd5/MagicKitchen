
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using UIEngine.Classes.DebugStuff;
using Microsoft.Xna.Framework.Input;
using Globals.Classes;
using EntityEngine.Classes.PlayerStuff;
using InputEngine.Classes.Input;
using StageEngine.Classes;
using UIEngine.Classes.TextStuff;
using EntityEngine.Classes.NPCStuff;
using QuakeConsole;
using UIEngine.Classes.SultanInterpreter;

namespace UIEngine.Classes
{
    public static class UserInterface
    {


        private static  GraphicsDevice s_graphics;
        private static  ContentManager s_content;




        private static List<InterfaceSection> s_activeSections;
        internal static Texture2D ButtonTexture { get; set; }
        internal static Texture2D GeneralInterfaceTexture { get; set; }
        //test
        internal static bool IsHovered { get; set; }

        private static List<InterfaceSection> s_standardSections { get; set; }

        internal static CustomConsole CommandConsole { get; set; }
        internal static DialogueWindow TalkingWindow { get; set; }

        internal static ToolBar ToolBar { get; set; }
        internal static ClockBar ClockBar { get; set; }

        internal static Curtain Curtain { get; set; }




        public static void Load( GraphicsDevice graphics, ContentManager content  ,Texture2D buttonsTexture, Texture2D generalInterface)
        {
            s_graphics = graphics;
            s_content = content;
            ButtonTexture = buttonsTexture;
            GeneralInterfaceTexture = generalInterface;


            ToolBar = new ToolBar(graphics, content, null);
            ClockBar = new ClockBar(graphics, content,  null);

            ToolBar.Load(null);

            CommandConsole = new CustomConsole(graphics, content, null);
            TalkingWindow = new DialogueWindow(graphics, content,null);
            Curtain = new Curtain(graphics, content, null);
            s_standardSections = new List<InterfaceSection>() { ToolBar, ClockBar, CommandConsole, TalkingWindow, Curtain };


            s_activeSections = s_standardSections;

            foreach (InterfaceSection section in s_standardSections)
            {
                section.Load();
            }
        }
        /// <summary>
        /// Allows the talking window to respond to character clicks and load in their dialogue trees
        /// </summary>
        public static void RegisterCharacterClickEvents()
        {
            foreach(Character character in CharacterManager.AllCharacters)
            {
                TalkingWindow.RegisterCharacterClickEvent(character);
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
                if(Controls.IsClicked && Controls.HeldItem != null)
                {
                    Controls.DropAndAddToWorld(PlayerManager.Player1.Position, PlayerManager.Player1.DirectionMoving, StageManager.CurrentStage.Items);
                }
                if(Controls.IsClicked && TalkingWindow.IsActive && !TalkingWindow.Hovered)
                {
                    ReactiveSections();
                    TalkingWindow.IsActive = false;
                }
            }
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

            foreach (InterfaceSection section in s_activeSections)
            {
                if (section.IsActive)
                {
                    section.Draw(spriteBatch);

                }
            }

                Controls.Draw(spriteBatch);
            spriteBatch.End();
            //RenderTargetManager.RemoveRenderTarget();
            //RenderTargetManager.DrawMainTarget(spriteBatch);
        }

        public static void FadeIn(float rate) => Curtain.FadeIn(rate);
        public static void FadeOut(float rate) => Curtain.FadeOut(rate);
       
    }
}
