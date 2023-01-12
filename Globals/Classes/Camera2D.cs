using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Globals.Classes
{
    public class Camera2D : ISaveable
    {
        private readonly Viewport _viewPort;

        private Vector2 _origin { get; set; }
        public Vector2 position;
        private float _zoom { get; set; }
        private float _rotation { get; set; }

        private Rectangle _viewPortRectangle;

     
        public float Zoom { get { return _zoom; } set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } }

        public float X { get { return position.X; } }
        public float Y { get { return position.Y; } }

        public bool LockBounds { get; set; }

        public Camera2D(Viewport viewport)
        {
          SetToDefault();
            _origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            _viewPort = viewport;
        }
        public void SetToDefault()
        {
            Zoom = 3.0f;
            _rotation = 0.0f;
            position = Vector2.Zero;
            LockBounds = true;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix(Vector2.Zero));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition - new Vector2(_viewPort.X, _viewPort.Y) , Matrix.Invert(GetViewMatrix(Vector2.One))) ;
        }
        public void Jump(Vector2 jumpToPos)
        {
            position = jumpToPos;
            
        }
        /// <summary>
        /// Used to offset mouse world position when camera is zoomed
        /// </summary>
        /// <returns></returns>
        public Vector2 GetZoomOffSetPatch()
        {
            switch (Zoom)
            {
                case 1:
                    return Vector2.Zero;
                case 2:
                    return new Vector2(120, 60);
                case 3:
                    return new Vector2(164, 80);
                case 4:
                    return new Vector2(180, 92);
                default:
                    return Vector2.Zero;
            }
        }
        /// <summary>
        /// Used to correctly draw aether2d debugger when camera is zoomed
        /// </summary>
        /// <returns></returns>
        public Vector2 GetDebuggerOffSetPatch()
        {
            switch (Zoom)
            {
                case 1:
                    return Vector2.Zero;
                case 2:
                    return new Vector2(120, 60);
                case 3:
                    return new Vector2(240, 120);

                case 4:
                    return new Vector2(360, 180);
                default:
                    return Vector2.Zero;
            }
        }
        const int camera_smoothing = 10;
        const float camera_frame_size_hor = 4;
        const float camera_frame_size_ver = 3;
        public void Follow(Vector2 amount, Rectangle mapRectangle)
        {
           

            //float camera_target_x = Math.Min(Math.Max(X, amount.X - camera_frame_size_hor), amount.X + camera_frame_size_hor);
            //float camera_target_y = Math.Min(Math.Max(Y, amount.Y - camera_frame_size_ver), amount.Y + camera_frame_size_ver);
            //position.X = ((X) * (camera_smoothing - 1) + camera_target_x) / camera_smoothing;
            //position.Y = ((Y) * (camera_smoothing - 1) + camera_target_y) / camera_smoothing;


            // position = new Vector2((Math.Abs(amount.X) < 0.5f) ? amount.X :
            //     (float)Math.Round(position.X), (Math.Abs(amount.Y) < 0.5f) ? position.Y
            //    : (float)Math.Round(position.Y));

            //position = new Vector2((float)Math.Round(position.X), (float)Math.Round(position.Y));
            position = amount;
            _viewPortRectangle = new Rectangle((int)(mapRectangle.X + Settings.ScreenWidth / 2 / Zoom),
              (int)(mapRectangle.Y + Settings.ScreenHeight / 2 / Zoom),
                (int)(mapRectangle.Width - Settings.ScreenWidth / 2 / Zoom),
                (int)(mapRectangle.Height - Settings.ScreenHeight/ 2 / Zoom));

            if (LockBounds)
            {
                if (position.X < this._viewPortRectangle.X - Settings.BarWidth * Zoom)
                {
                    position.X = this._viewPortRectangle.X - Settings.BarWidth * Zoom;
                }
                if (position.X > this._viewPortRectangle.Width + Settings.BarWidth * Zoom)
                {
                    position.X = this._viewPortRectangle.Width + Settings.BarWidth * Zoom;
                }
                if (position.Y < this._viewPortRectangle.Y)
                {
                    position.Y = this._viewPortRectangle.Y;
                }
                if (position.Y > this._viewPortRectangle.Height)
                {
                    position.Y = this._viewPortRectangle.Height;
                }
            }
        }

        /// <summary>
        /// Will have camera "float" towards a desired point.
        /// </summary>
        /// <param name="amount">Position to float to</param>
        private void Lerp(Vector2 amount)
        {
            position.X = MathHelper.Lerp(X, amount.X, .5f);
            position.Y = MathHelper.Lerp(Y, amount.Y, .5f);
        }


        public Matrix GetTransform(GraphicsDevice graphicsDevice)
        {

            return Matrix.CreateTranslation(
                new Vector3(-X, -Y, 0)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                Matrix.CreateTranslation(
                    new Vector3(graphicsDevice.Viewport.Width * 0.5f,
            graphicsDevice.Viewport.Height * 0.5f, 0));
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-position * parallax, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-_origin, 0.0f)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(_origin, 0.0f));
        }
 
        public void Save(BinaryWriter writer)
        {
            writer.Write(Zoom);
        }

        public void LoadSave(BinaryReader reader)
        {
          Zoom = reader.ReadSingle();
        }

    

        public void LoadContent()
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
