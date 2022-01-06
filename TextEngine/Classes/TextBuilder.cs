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
        public static float Slow = .1f;
        public static float Normal = .05f;
        public static float Fast = .02f;

        private Text Text { get; set; }

        private float AnchorTextRate { get; set; }
        private SimpleTimer SimpleTimer { get; set; }

        /// <summary>
        /// Constructor for text which should type itself out over a given rate.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="textRate">How fast the text should be written.</param>
        public TextBuilder(Text text, float textRate)
        {
            this.Text = text;
            this.AnchorTextRate = textRate;
            this.SimpleTimer = new SimpleTimer(textRate);
        }

        /// <summary>
        /// Constructor for
        /// </summary>
        /// <param name="text"></param>
        public TextBuilder(Text text)
        {
            this.Text = text;
        }

        /// <param name="autoComplete">Set to true if you want the given text to be show, rather than set.</param>
        public void SetText(Text text, bool autoComplete = true)
        {
            Text = text;
            if (autoComplete)
                ForceComplete();
        }

        /// <param name="textBoxWidth">Provide if you want the text to wrap.</param>
        /// <param name="stringToAppend">Leave null if you are not manually typing (Like command console).</param>
        /// <returns>Returns true if finished with current assignment</returns>
        public bool Update(GameTime gameTime, string stringToAppend = null, int textBoxWidth = 0)
        {

            if (SimpleTimer != null)
            {
                if (Text.CurrentString != Text.FullString)
                {
                    //text being appended on a timer.
                    if (SimpleTimer.Run(gameTime))
                        if (Text.Append())
                            return true;                  
                }
                else
                    return true;

            }
            else
            {
                Text.Append(stringToAppend, textBoxWidth);
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
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool fullString = false)
        {
            Text.Draw(spriteBatch, position, fullString);
        }



        public void BackSpace()
        {
            Text.Backspace();
        }

        public string GetText()
        {
            return Text.CurrentString;
        }

        public void ClearText()
        {
            Text.Clear();
        }

        /// <summary>
        /// Forces text current string to equal total string
        /// </summary>
        public void ForceComplete()
        {
            Text.ForceComplete();
        }

        public bool IsComplete()
        {
            return Text.CurrentString == Text.FullString;
        }
    }
}
