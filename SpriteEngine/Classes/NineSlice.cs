using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
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
        private List<Rectangle> _combinedRectangle;
        private List<Vector2> _rectanglePositions;
        private Texture2D _texture;
        internal Color Color { get; set; }

        private readonly int _unit = 16;

        private readonly Rectangle _topLeftCorner = new Rectangle(0, 0, 16, 16);
        private readonly Rectangle _topEdge = new Rectangle(16, 0, 16, 16);
        private readonly Rectangle _topRightCorner = new Rectangle(32, 0, 16, 16);

        private readonly Rectangle _leftEdge = new Rectangle(0, 16, 16, 16);
        private readonly Rectangle _center = new Rectangle(16, 16, 16, 16);
        private readonly Rectangle _rightEdge = new Rectangle(32, 16, 16, 16);

        private readonly Rectangle _bottomLeftCorner = new Rectangle(0, 32, 16, 16);
        private readonly Rectangle _bottomEdge = new Rectangle(16, 32, 16, 16);
        private readonly Rectangle _bottomRightCorner = new Rectangle(32, 32, 16, 16);
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
            int totalRequiredWidth = width;
            int totalRequireHeight = height;
            BuildRectangle(position, totalRequiredWidth, totalRequireHeight);
        }

       
        /// <summary>
        /// Create a dynamic UI rectangle to support given text.
        /// </summary>
        internal NineSlice(
            Vector2 position, Texture2D? texture,
            float layer, Text text, Color color, Vector2 scale)
        {

            SharedConstructor(position, texture, layer, color, scale);

            int totalRequiredWidth = (int)text.TotalStringWidth + 32;
            int totalRequireHeight = (int)text.TotalStringHeight + 32;
            Width = totalRequiredWidth;
            Height = totalRequireHeight;

            BuildRectangle(position, totalRequiredWidth, totalRequireHeight);

        }
        private void SharedConstructor(Vector2 position, Texture2D? texture,
           float layer, Color color, Vector2 scale)
        {
            _combinedRectangle = new List<Rectangle>();
            _rectanglePositions = new List<Vector2>();

            Color = color;

            _texture = texture;
            _uiLayer = layer;
            _scale = scale;
        }
        private void BuildRectangle(Vector2 position, int totalRequiredWidth, int totalRequireHeight)
        {
            int currentWidth;
            int currentHeight = 0;
            currentWidth = AddRow(totalRequiredWidth, position, _topLeftCorner, _topEdge, _topRightCorner);
            currentHeight += (int)(_unit * this._scale.Y);
            position = new Vector2(position.X, position.Y + _unit * this._scale.Y);

            while (currentHeight < totalRequireHeight - _unit)
            {
                AddRow(totalRequiredWidth, position, _leftEdge, _center, _rightEdge);
                currentHeight += (int)(_unit * this._scale.Y);
                position = new Vector2(position.X, position.Y + _unit * this._scale.Y);
            }

            AddRow(totalRequiredWidth, position, _bottomLeftCorner, _bottomEdge, _bottomRightCorner);
            currentHeight += (int)(_unit * this._scale.Y);
            _position = new Vector2((int)_rectanglePositions[0].X, (int)_rectanglePositions[0].Y);

        }

        private void AddRectangle(Rectangle rectangle, Vector2 position)
        {
            _combinedRectangle.Add(rectangle);
            _rectanglePositions.Add(position);
        }

        /// <summary>
        /// Returns the width of a single row. To be used once in the constructor set our total width!
        /// </summary>
        private int AddRow(int length, Vector2 position, Rectangle left, Rectangle middle, Rectangle right)
        {
            int totalWidth = 0;
            int startingPositionX = (int)position.X;
            int numberNeeded = (int)(length / this._scale.Y / _unit);
            AddRectangle(left, position);
            totalWidth += left.Width;
            startingPositionX += (int)(_unit * this._scale.X);

            numberNeeded--;

            while (numberNeeded > 1)
            {

                Vector2 newPosition = new Vector2(startingPositionX, position.Y);
                AddRectangle(middle, newPosition);
                totalWidth += middle.Width;
                numberNeeded--;
                startingPositionX += (int)(_unit * this._scale.X);
            }
            AddRectangle(right, new Vector2(startingPositionX, position.Y));
            totalWidth += right.Width;

            return totalWidth;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _combinedRectangle.Count; i++)
            {
               spriteBatch.Draw(_texture, _rectanglePositions[i], _combinedRectangle[i], Color, 0f, Vector2.One, _scale, SpriteEffects.None, _uiLayer);
            }
        }

        private Vector2 CenterTextHorizontal(int width, Text text)
        {
            float textWidth = text.GetTextLength();
            float newWidth = (float)width / 2f - textWidth / 2;
            return new Vector2(_position.X + newWidth, _position.Y);
        }

    }
}
