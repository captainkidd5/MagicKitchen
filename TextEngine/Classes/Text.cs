﻿using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace TextEngine.Classes
{
    public class Text
    {
        public float CurrentStringWidth => GetWidth(CurrentString);

        public float CurrentStringHeight => GetTextHeight(CurrentString);


        public float TotalStringWidth => GetWidth(FullString);

        public float TotalStringHeight => GetTextHeight(FullString);
        public string CurrentString { get; set; }
        public string FullString { get; private set; }
        private float _scale;
        private Color _color;
        private Vector2 _position;
        private SpriteFont _spriteFont;

        private float LayerDepth { get; set; }


        internal Text(String value, float scale, SpriteFont spriteFont, Layers layers)
        {
            CurrentString = string.Empty;
            FullString = value;
            _scale = scale;
            _color = Color.White;
            _spriteFont = spriteFont;

            LayerDepth = Settings.GetLayerDepth(layers);

        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            _position = position;
        }
        //TODO There is currently key delay because a key is only recorded if it was pressed AND released. This needs to be changed so if a key
        //is pressed and not released, it is recorded. If held longer than a certain interval, it should be repeated.
        /// <param name="fullString">Set to true if you want to ignore the current string and draw the entire string at once.</param>
        public void Draw(SpriteBatch spriteBatch, bool fullString = false)
        {
            if (fullString)
                spriteBatch.DrawString(_spriteFont, FullString.ToLower(),
                    _position, _color, 0f, Vector2.Zero, _scale, SpriteEffects.None, LayerDepth);
            else
                spriteBatch.DrawString(_spriteFont, CurrentString.ToLower(),
                    _position, _color, 0f, Vector2.Zero, _scale, SpriteEffects.None, LayerDepth);

        }


        /// <summary>
        /// Append key presses
        /// </summary>
        internal void Append(string letter, int textBoxWidth)
        {
            CurrentString += letter;

            if (GetLengthOfCurrentLine() >= textBoxWidth)
                WrapInputText(textBoxWidth);

        }
       
        /// <summary>
        /// Appends the next letter of the target string.
        /// </summary>
        /// <returns>Returns true if the string matches the target string (we're done).</returns>
        internal bool Append()
        {
            if (CurrentString.Length < FullString.Length)
            {
                CurrentString += FullString[CurrentString.Length];
                return false;
            }
            return true;
        }

        /// <summary>
        /// Removes the last character in the string.
        /// </summary>
        internal void Backspace()
        {
            if (CurrentString.Length > 0)
                CurrentString = CurrentString.Remove(CurrentString.Length - 1);
        }
        public void SetFullString(string newString)
        {
            FullString = newString;
        }

        /// <summary>
        /// Called once when dialogue should be parsed, NOT for player input.
        /// </summary>
        private String WrapAutoText(float lineLimit)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = CurrentString.Split(' ');

            foreach (String word in wordArray)
            {
                if (_spriteFont.MeasureString(line + word).Length() * _scale > lineLimit)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }

        /// <summary>
        /// Gets the width of the current text. ONLY SUPPORTS SINGLE LINE.
        /// </summary>
        private float GetWidth(string value)
        {
            return _spriteFont.MeasureString(value).Length() * _scale;
        }
        private void WrapInputText(float lineLimit)
        {
            CurrentString += "\n";
        }

        /// <summary>
        /// Returns the width of the longest text line, separated at the new line.
        /// </summary>
        public float GetTextLength()
        {
            String[] lineArray = CurrentString.Split('\n');
            float lengthToReturn = 0f;
            for (int i = 0; i < lineArray.Length; i++)
            {
                float length = _spriteFont.MeasureString(lineArray[i]).X * _scale;
                if (length > lengthToReturn)
                {
                    lengthToReturn = length;
                }
            }

            return lengthToReturn;
        }

        internal float GetTextHeight(string value)
        {
            String[] lineArray = value.Split('\n');
            float totalHeight = 0;

            foreach (string line in lineArray)
            {
                totalHeight += _spriteFont.MeasureString(line).Y * _scale;
            }

            return totalHeight;
        }

        /// <summary>
        /// Gets length of the last line in a paragraph separated at the \n
        /// </summary>
        /// <returns></returns>
        private float GetLengthOfCurrentLine()
        {
            string[] splitString = CurrentString.Split('\n');
            if (splitString.Length > 0)
                return _spriteFont.MeasureString(splitString[splitString.Length - 1]).X * _scale;
            else
                return _spriteFont.MeasureString(CurrentString).X * _scale;

        }

        internal void Clear()
        {
            CurrentString = string.Empty;
        }

        /// <summary>
        /// Completely replaces text
        /// </summary>
        public void ReplaceCurrentText(string value)
        {
            CurrentString = value;
        }

        /// <summary>
        /// Sets the current string equal to the full string!
        /// </summary>
        internal void ForceComplete()
        {
            CurrentString = FullString;
        }

        /// <summary>
        /// String width and height already have their scale applied to them, scale parameter is for unscaled rectangle.
        /// </summary>
        internal Vector2 CenterInRectangle(Vector2 rectanglePosition, Rectangle rectangleToCenterOn, float scale = 1f)
        {
            return new Vector2(rectanglePosition.X + rectangleToCenterOn.Width / 2f * scale - TotalStringWidth / 2f,
                rectanglePosition.Y + rectangleToCenterOn.Height / 2f * scale - TotalStringHeight / 2f);
        }

    }
}
