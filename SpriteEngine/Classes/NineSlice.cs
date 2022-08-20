using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine;
using TextEngine.Classes;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes
{
    internal class NineSlice
    {
        internal int Width { get; private set; }
        internal int Height { get; private set; }


        public Rectangle Rectangle => new Rectangle((int)_position.X, (int)_position.Y, Width, Height);
        private Vector2 _position;



        private Vector2 _scale;

        private NSlicePiece[,] _nSlicePieces;
        private Texture2D _texture;
        internal Color Color { get; set; }

        private static readonly int _unit = 16;

        private static readonly Rectangle _topLeftCorner = new Rectangle(0, 0, 16, 16);
        private static readonly Rectangle _topEdge = new Rectangle(16, 0, 16, 16);
        private static readonly Rectangle _topRightCorner = new Rectangle(32, 0, 16, 16);

        private static readonly Rectangle _leftEdge = new Rectangle(0, 16, 16, 16);
        private static readonly Rectangle _center = new Rectangle(16, 16, 16, 16);
        private static readonly Rectangle _rightEdge = new Rectangle(32, 16, 16, 16);

        private static readonly Rectangle _bottomLeftCorner = new Rectangle(0, 32, 16, 16);
        private static readonly Rectangle _bottomEdge = new Rectangle(16, 32, 16, 16);
        private static readonly Rectangle _bottomRightCorner = new Rectangle(32, 32, 16, 16);
        private float _uiLayer;

        /// <summary>
        /// Create a dynamic UI rectangle of any size.
        /// </summary>
        internal NineSlice(Vector2 position, Texture2D texture, float layer,
            int width, int height, Color color, Vector2 scale)
        {

            SharedConstructor(position, texture, layer, color, scale);
            Width = width; 
            Height = height;

            BuildRectangle(position);
        }


        /// <summary>
        /// Create a dynamic UI rectangle to support given text.
        /// </summary>
        internal NineSlice(
            Vector2 position, Texture2D? texture,
            float layer, Text text, Color color, Vector2 scale)
        {
        
            SharedConstructor(position, texture, layer, color, scale);

            Width = (int)text.Width;
            Height = (int)text.Height;// + (int)TextFactory.SingleCharacterWidth() * 4;

            if (Height <= 16)
                Height = 48;
            BuildRectangle(_position);

        }

        public void Move(Vector2 newPosition)
        {
            BuildRectangle(newPosition);
        }
        private void SharedConstructor(Vector2 position, Texture2D? texture,
           float layer, Color color, Vector2 scale)
        {

            _position = position;
            Color = color;

            _texture = texture;
            _uiLayer = layer;
            _scale = scale;
        }
        private void BuildRectangle(Vector2 position)
        {
            if (Width % 16 != 0)
            {
                Width = Width + (16 - Width % 16);

                //throw new Exception($"Width must be multiple of 16");

            }
            if (Height % 16 != 0)
            {
                Height = Height + (16 - Height % 16);

                // throw new Exception($"Height must be multiple of 16");

            }
            int totalWidthSlices = Width / (int)(_unit * _scale.X);

            int totalHeightSlices = Height / (int)(_unit * _scale.Y);
            _nSlicePieces = new NSlicePiece[totalWidthSlices, totalHeightSlices];

            float xPos = _position.X;
            float yPos = _position.Y;
            for (int x = 0; x < totalWidthSlices; x++)
            {
                
                for (int y = 0; y < totalHeightSlices; y++)
                {
                    _nSlicePieces[x, y] = new NSlicePiece(GetRequiredRectangle(x, y, totalWidthSlices - 1, totalHeightSlices - 1),
                        new Vector2(xPos, yPos));
                    yPos += _unit * _scale.Y;
                }
                yPos = _position.Y;
                xPos += _unit * _scale.X;
            }


        }
        private Rectangle GetRequiredRectangle(int currentX, int currentY, int totalX, int totalY)
        {
            if (currentX == 0 && currentY == 0)
                return _topLeftCorner;
            if (currentX == 0 && currentY == totalY)
                return _bottomLeftCorner;
            if (currentX == 0 && currentY != 0)
                return _leftEdge;
            if (currentX == totalX && currentY == 0)
                return _topRightCorner;
            if (currentY == 0 && totalX != 0)
                return _topEdge;
            if (currentX == totalX && currentY == totalY)
                return _bottomRightCorner;
            if (currentX == totalX && currentY != 0)
                return _rightEdge;
            if (currentY == totalY && totalX != 0 && currentX != totalX)
                return _bottomEdge;

            return _center;

        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < _nSlicePieces.GetLength(0); x++)
            {

                for (int y = 0; y < _nSlicePieces.GetLength(1); y++)
                {
                    spriteBatch.Draw(_texture, _nSlicePieces[x,y].Position, _nSlicePieces[x, y].SourceRectangle, Color, 0f, Vector2.One, _scale, SpriteEffects.None, _uiLayer);

                }
            }

            
        }

        private Vector2 CenterTextHorizontal(int width, Text text)
        {
            float textWidth = text.Width;
            float newWidth = (float)width / 2f - textWidth / 2;
            return new Vector2(_position.X + newWidth, _position.Y);
        }
        private class NSlicePiece
        {
            public Rectangle SourceRectangle { get; private set; }
            public Vector2 Position { get; private set; }

            public NSlicePiece(Rectangle sourceRectangle, Vector2 position)
            {
                SourceRectangle = sourceRectangle;
                Position = position;
            }

            public void Move(Vector2 adjustedPosition)
            {
                Position = adjustedPosition;
            }
        }
    }
}
