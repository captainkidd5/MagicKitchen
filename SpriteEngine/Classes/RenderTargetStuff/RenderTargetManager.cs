using Globals.Classes;
using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteEngine.Classes.RenderTargetStuff
{
    public static class RenderTargetManager
    {
        private static GraphicsDevice Graphics;

        public static RenderTarget2D DayTargetOverlay;
        public static RenderTarget2D NightTargetOverlay;

        public static RenderTarget2D CurrentOverlayTarget;

        public static RenderTarget2D MainTarget;

        public static RenderTarget2D UITarget;

        public static void Load(GraphicsDevice graphics)
        {
            Graphics = graphics;
            DayTargetOverlay = new RenderTarget2D(
                graphics,
                graphics.PresentationParameters.BackBufferWidth,
                graphics.PresentationParameters.BackBufferHeight);
            NightTargetOverlay = new RenderTarget2D(
                graphics,
                graphics.PresentationParameters.BackBufferWidth,
                graphics.PresentationParameters.BackBufferHeight);

            MainTarget = new RenderTarget2D(
                graphics,
                graphics.PresentationParameters.BackBufferWidth,
                graphics.PresentationParameters.BackBufferHeight);

            UITarget = new RenderTarget2D(
                graphics,
                graphics.PresentationParameters.BackBufferWidth,
                graphics.PresentationParameters.BackBufferHeight);

            CurrentOverlayTarget = DayTargetOverlay;
        }


   

        public static void SetTarget(RenderTarget2D target)
        {
 
            Graphics.SetRenderTarget(target);

        }

        public static void RemoveRenderTarget()
        {

            Graphics.SetRenderTarget(null);

        }


        public static void DrawTarget(SpriteBatch spriteBatch, RenderTarget2D renderTarget2D)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(MainTarget, Settings.ScreenRectangle, Color.White);
            spriteBatch.Draw(UITarget, Settings.ScreenRectangle, Color.White);

            spriteBatch.End();
        }


        private static RenderTarget2D GetTargetFromDayStatus(DayStatus dayStatus)
        {
            return dayStatus == DayStatus.DayTime ? DayTargetOverlay : NightTargetOverlay;
        }

        public static Vector2 HalfScreen()
        {
            return new Vector2(CurrentOverlayTarget.Width, CurrentOverlayTarget.Height) / 2;
        }
    }
}
