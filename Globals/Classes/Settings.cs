using DataModels.DialogueStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace Globals.Classes
{
    public static class Settings
    {

        public enum ElementType
        {
            UI = 1,
            World = 2
        }

        public static float GetLayerDepth(Layers layerDepths)
        {
            return (float)layerDepths * .1f;
        }
        public static bool WindowFocused = false;
        public static Random Random;
        private static GraphicsDeviceManager Graphics;

        public static readonly int TileSize = 16;

        #region SCREEN
        public static int ScreenWidth => Graphics.PreferredBackBufferWidth;
        public static int ScreenHeight => Graphics.PreferredBackBufferHeight;
        public static readonly int Gutter = 32;
        public static int GameScale = 1;

        public static float AspectRatio => (float)ScreenWidth / (float)ScreenHeight;

        public static float NativeWidth = 1280;
        public static float NativeHeight = 720;

        public static float PreferredAspect = NativeWidth / NativeHeight;
        public static Rectangle ScreenRectangle;

        public static Camera2D Camera;
        public static GameWindow GameWindow;

        //SpawnableArea is the area in which mobs may spawn. Subtracting the play area from this gives you a margin on sides of the screen
        //generally outside of viewport, but within a radius of the player
        public static readonly int SpawnableAreaWidth = 800;

        public static int SpawnableAreaX => (int)Camera.X - SpawnableAreaWidth / 2;
        public static int SpawnableAreaY => (int)Camera.Y - SpawnableAreaWidth / 2;

        public static readonly int ActiveAreaWidth = 600;

        public static int ActiveAreaX => (int)Camera.X - ActiveAreaWidth / 2;
        public static int ActiveAreaY => (int)Camera.Y - ActiveAreaWidth / 2;



        public static Rectangle GetVisibleRectangle()
        {

            Rectangle rect = new Rectangle(
                (int)Camera.X - (int)(ScreenWidth / 2 / Camera.Zoom),
                (int)Camera.Y - (int)(ScreenHeight / 2 / Camera.Zoom),
                (int)(ScreenWidth / Camera.Zoom), (int)(ScreenHeight / Camera.Zoom));
            return rect;
        }
        public static bool IsVisibleOnScreen(Vector2 position, int width = 16, int height = 16)
        {
            return ScreenRectangle.Intersects(new Rectangle((int)position.X, (int)position.Y, width, height));
        }

        /// <summary>
        /// 1x1 pixel texture for debugging things
        /// </summary>
        public static Texture2D DebugTexture { get; set; }
        public static void Load(GraphicsDeviceManager graphics, Camera2D camera, GameWindow gameWindow)
        {
            Random = new Random();
            Graphics = graphics;
            Camera = camera;
            GameWindow = gameWindow;

            DebugTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            DebugTexture.SetData<Color>(new Color[] { Color.White });
            SetResolution((int)NativeWidth, (int)NativeHeight);

        }
        public static void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            if (GameWindow != null)
            {
                GameWindow.ClientSizeChanged -= Window_ClientSizeChanged;
                SetResolution(GameWindow.ClientBounds.Width, GameWindow.ClientBounds.Height);
                GameWindow.ClientSizeChanged += Window_ClientSizeChanged;
            }

        }


        public static int BarHeight;
        public static int BarWidth;

        /// <summary>
        /// Will fill in black bars on the top and bottom of the screen (like movies do!) so that
        /// the aspect ratio is maintined.
        /// </summary>
        public static Rectangle GetScreenRectangle()
        {
            Rectangle screenRectangle;
            if (AspectRatio <= PreferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)((ScreenWidth / PreferredAspect) + 0.5f);
                BarHeight = (ScreenHeight - presentHeight) / 2;
                screenRectangle = new Rectangle(0, BarHeight, ScreenWidth, presentHeight);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)((ScreenHeight * PreferredAspect) + 0.5f);
                BarWidth = (ScreenWidth - presentWidth) / 2;
                screenRectangle = new Rectangle(BarWidth, 0, presentWidth, ScreenHeight);
            }
            return screenRectangle;
        }
        public static Vector2 CenterScreen => new Vector2(NativeWidth / 2, NativeHeight / 2);



        #endregion

        #region CAMERA
        public static float CameraZoom { get { return Camera.Zoom; } set { Camera.Zoom = value; } }



        #endregion

        public static void SetResolution(int width, int height)
        {

            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            ScreenRectangle = GetScreenRectangle();
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();
        }

        public static void ToggleFullscreen(bool? value)
        {
            if (value == null)
                Graphics.IsFullScreen = !Graphics.IsFullScreen;
            else
                Graphics.IsFullScreen = value.Value;
            //Graphics.ToggleFullScreen();
            Graphics.HardwareModeSwitch = false;
            //  Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            ScreenRectangle = GetScreenRectangle();

            Graphics.ApplyChanges();
        }



    }
}
