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

        public static RenderTarget2D LightsTarget;

        public static RenderTarget2D CurrentOverlayTarget;

        public static RenderTarget2D MainTarget;

        public static RenderTarget2D UITarget;

        public static RenderTarget2D UILightsAffectableTarget;

        public static void Load(GraphicsDevice graphics)
        {
            Graphics = graphics;

            LightsTarget = new RenderTarget2D(
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
            UILightsAffectableTarget = new RenderTarget2D(
               graphics,
               graphics.PresentationParameters.BackBufferWidth,
               graphics.PresentationParameters.BackBufferHeight);
            CurrentOverlayTarget = MainTarget;
        }


   

        public static void SetTarget(RenderTarget2D target)
        {
 
            Graphics.SetRenderTarget(target);

        }

        public static void RemoveRenderTarget()
        {

            Graphics.SetRenderTarget(null);

        }


        public static void DrawTarget(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            spriteBatch.Draw(UITarget, Settings.ScreenRectangle, Color.White);

            spriteBatch.End();
        }


        public static Vector2 HalfScreen()
        {
            return new Vector2(CurrentOverlayTarget.Width, CurrentOverlayTarget.Height) / 2;
        }
    }
}
