using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TextEngine.Classes
{
    public class TextBuilder
    {
        public readonly float Slow = .1f;
        public readonly float Normal = .05f;
        public readonly float Fast = .02f;

        private Text _text;

        private float _anchorTextRate;
        private SimpleTimer _simpleTimer;


        public bool ExceedsWidth(int width, int? line) => _text.ExceedsWidth(width, line);
        /// <summary>
        /// Constructor for text which should type itself out over a given rate.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="textRate">How fast the text should be written.</param>
        public TextBuilder(Text text, float textRate)
        {
            _text = text;
            _anchorTextRate = textRate;
            _simpleTimer = new SimpleTimer(textRate);
        }

        /// <summary>
        /// Constructor for
        /// </summary>
        /// <param name="text"></param>
        public TextBuilder(Text text)
        {
           _text = text;
        }

        /// <param name="autoComplete">Set to true if you want the given text to be show, rather than set.</param>
        public void SetText(Text text,
            int textBoxWidth, bool autoComplete = true)
        {
            _text = text;
            if (autoComplete)
                ForceComplete(textBoxWidth);
        }

        /// <param name="textBoxWidth">Provide if you want the text to wrap.</param>
        /// <param name="stringToAppend">Leave null if you are not manually typing (Like command console).</param>
        /// <returns>Returns true if finished with current assignment</returns>
        public bool Update(GameTime gameTime, Vector2 position, int textBoxWidth, string stringToAppend = null)
        {
            _text.Update(gameTime, position);
            if (_simpleTimer != null)
            {
                if (_text.CurrentString == _text.FullString)
                {
                    return true;
                }
                else
                { 
                    //text being appended on a timer.
                    if (_simpleTimer.Run(gameTime))
                        if (_text.Append(textBoxWidth))
                            return true;

                }

            }
            else if (!string.IsNullOrEmpty(stringToAppend))
            {
                _text.Append(stringToAppend, textBoxWidth);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="fullString">Set to true if you just want to print a predefine string as is.</param>
        public void Draw(SpriteBatch spriteBatch, bool fullString = false)
        {
            _text.Draw(spriteBatch, fullString);
        }



        public void BackSpace()
        {
            _text.Backspace();
        }


        public string GetText() => _text.CurrentString;


        public void ClearText() => _text.Clear();

        /// <summary>
        /// Forces text current string to equal total string
        /// </summary>
        public void ForceComplete(int textBoxWidth) => _text.ForceComplete(textBoxWidth);


        public bool IsComplete() => _text.CurrentString == _text.FullString;
        
    }
}
