using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;
using MonoGame.Extended.BitmapFonts;

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
        private BitmapFont _spriteFont;

        private float LayerDepth { get; set; }


        internal Text(String value, float scale, BitmapFont spriteFont, float layerDepth)
        {
            CurrentString = string.Empty;
            FullString = value;
            _scale = scale;
            _color = Color.Black;
            _spriteFont = spriteFont;

            LayerDepth = layerDepth;

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

        public void ChangeColor(Color newColor)
        {
            _color = newColor;
        }

        /// <summary>
        /// Append key presses
        /// </summary>
        internal void Append(string letter, int textBoxWidth)
        {
            CurrentString += letter;

            if (ExceedsWidth(textBoxWidth))
                WrapInputText(textBoxWidth);

        }

        internal bool ExceedsWidth(int width, int? line = null)
        {
            if (line == null)
                return GetLengthOfCurrentLine() >= width;
            else
                return GetLengthOfLine(line.Value) >= width;
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
                if (_spriteFont.MeasureString(line + word).Width * _scale > lineLimit)
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
            string[] separated = value.Split("\n");
            string longest = separated[0];

            foreach(string word in separated)
                if(word.Length > longest.Length)
                    longest = word;
            return _spriteFont.MeasureString(longest).Width * _scale;
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
            String[] lineArray = FullString.Split('\n');
            float lengthToReturn = 0f;
            for (int i = 0; i < lineArray.Length; i++)
            {
                float length = _spriteFont.MeasureString(lineArray[i]).Width * _scale;
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
            float totalHeight =  16;

            if(lineArray.Length > 1)
            {

            foreach (string line in lineArray)
            {
                totalHeight += _spriteFont.MeasureString(line).Height * _scale;
            }
            }

            return totalHeight;
        }

        /// <summary>
        /// Gets length of the last line in a paragraph separated at the \n
        /// </summary>
        /// <returns></returns>
        public float GetLengthOfCurrentLine()
        {
            string[] splitString = CurrentString.Split('\n');
            return GetLengthOfLine(splitString.Length - 1);

          
        }

        public float GetLengthOfLine(int line)
        {
            string[] splitString = CurrentString.Split('\n');
            float returnedLength;
            
            if (splitString.Length > 0)
                returnedLength = _spriteFont.MeasureString(splitString[line]).Width* _scale;
            else
                returnedLength = _spriteFont.MeasureString(CurrentString).Height * _scale;
            return returnedLength;
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
        public static Vector2 CenterInRectangle(Rectangle rectangleToCenterOn,Text text, float scale = 1f)
        {
            return new Vector2(rectangleToCenterOn.X + (rectangleToCenterOn.Width / 2) - text.TotalStringWidth,
                rectangleToCenterOn.Y + (rectangleToCenterOn.Height / 2) - text.TotalStringHeight );
        }

    }
}
