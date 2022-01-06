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

        private Vector2 Position { get; set; }

        private float Scale { get; set; }
        private List<Rectangle> CombinedRectangle { get; set; }
        private List<Vector2> RectanglePositions { get; set; }
        private Texture2D Texture { get; set; }
        internal Color Color { get; set; }

        private readonly int Unit = 16;

        private readonly Rectangle TopLeftCorner = new Rectangle(0, 0, 16, 16);
        private readonly Rectangle TopEdge = new Rectangle(16, 0, 16, 16);
        private readonly Rectangle TopRightCorner = new Rectangle(32, 0, 16, 16);

        private readonly Rectangle LeftEdge = new Rectangle(0, 16, 16, 16);
        private readonly Rectangle Center = new Rectangle(16, 16, 16, 16);
        private readonly Rectangle RightEdge = new Rectangle(32, 16, 16, 16);

        private readonly Rectangle BottomLeftCorner = new Rectangle(0, 32, 16, 16);
        private readonly Rectangle BottomEdge = new Rectangle(16, 32, 16, 16);
        private readonly Rectangle BottomRightCorner = new Rectangle(32, 32, 16, 16);
        private readonly float uiLayer;

        internal Rectangle HitBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }
        }
        /// <summary>
        /// Create a dynamic UI rectangle of any size.
        /// </summary>
        internal NineSlice(Vector2 position, Texture2D texture, Layers uiLayer,
            int width, int height, Color? color)
        {
            Width = width;
            Height = height;
            CombinedRectangle = new List<Rectangle>();
            RectanglePositions = new List<Vector2>();
            Color = color ?? Color.White;
            Texture = texture;
            Scale = Globals.Classes.Settings.GameScale;
            this.uiLayer = Settings.GetLayerDepth(uiLayer);
            int totalRequiredWidth = width;
            int totalRequireHeight = height;
            BuildRectangle(position, totalRequiredWidth, totalRequireHeight);
        }

        /// <summary>
        /// Create a dynamic UI rectangle to support given text.
        /// </summary>
        internal NineSlice(
            Vector2 position, Texture2D? texture,
            Layers uiLayer, Text text, Color? color)
        {
            CombinedRectangle = new List<Rectangle>();
            RectanglePositions = new List<Vector2>();

            Color = color ?? Color.White;

            Texture = texture;
            this.uiLayer = Settings.GetLayerDepth(uiLayer);
            Scale = Globals.Classes.Settings.GameScale;

            int totalRequiredWidth = (int)text.GetTextLength() + 48;
            int totalRequireHeight = (int)text.CurrentStringHeight + 32;
            Width = totalRequiredWidth;
            Height = totalRequireHeight;

            BuildRectangle(position, totalRequiredWidth, totalRequireHeight);

        }

        private void BuildRectangle(Vector2 position, int totalRequiredWidth, int totalRequireHeight)
        {
            int currentWidth;
            int currentHeight = 0;
            currentWidth = AddRow(totalRequiredWidth, position, TopLeftCorner, TopEdge, TopRightCorner);
            currentHeight += (int)(Unit * this.Scale);
            position = new Vector2(position.X, position.Y + Unit * this.Scale);

            while (currentHeight < totalRequireHeight - Unit)
            {
                AddRow(totalRequiredWidth, position, LeftEdge, Center, RightEdge);
                currentHeight += (int)(16 * this.Scale);
                position = new Vector2(position.X, position.Y + Unit * this.Scale);
            }

            AddRow(totalRequiredWidth, position, BottomLeftCorner, BottomEdge, BottomRightCorner);
            currentHeight += (int)(Unit * this.Scale);
            Position = new Vector2((int)RectanglePositions[0].X, (int)RectanglePositions[0].Y);

        }

        private void AddRectangle(Rectangle rectangle, Vector2 position)
        {
            CombinedRectangle.Add(rectangle);
            RectanglePositions.Add(position);
        }

        /// <summary>
        /// Returns the width of a single row. To be used once in the constructor set our total width!
        /// </summary>
        private int AddRow(int length, Vector2 position, Rectangle left, Rectangle middle, Rectangle right)
        {
            int totalWidth = 0;
            int startingPositionX = (int)position.X;
            int numberNeeded = (int)(length / this.Scale / Unit);
            AddRectangle(left, position);
            totalWidth += left.Width;
            startingPositionX += (int)(Unit * this.Scale);

            numberNeeded--;

            while (numberNeeded > 1)
            {

                Vector2 newPosition = new Vector2(startingPositionX, position.Y);
                AddRectangle(middle, newPosition);
                totalWidth += middle.Width;
                numberNeeded--;
                startingPositionX += (int)(Unit * this.Scale);
            }
            AddRectangle(right, new Vector2(startingPositionX, position.Y));
            totalWidth += right.Width;

            return totalWidth;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < CombinedRectangle.Count; i++)
            {
               spriteBatch.Draw(Texture, RectanglePositions[i], CombinedRectangle[i], Color, 0f, Vector2.One, Scale, SpriteEffects.None, uiLayer);
            }
        }

        private Vector2 CenterTextHorizontal(int width, Text text)
        {
            float textWidth = text.GetTextLength();
            float newWidth = (float)width / 2f - textWidth / 2;
            return new Vector2(Position.X + newWidth, Position.Y);
        }

    }
}
