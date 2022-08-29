using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    public class TextBuilder
    {
        private readonly Text _currentText;
        private readonly string desiredString;
        public float Height => _currentText.Height;

        public bool IsComplete => _currentText.ToString() == desiredString;
        public void ClearCurrent()
        {
        _currentText.Clear();
            _currentIndex = 0;
        }

        private SimpleTimer _textTimer;
        private float _typeTime = .15f;
        private int _currentIndex;
        public TextBuilder(String value, Vector2 position, float? lineXStart, float? lineLimit, float layerDepth, ImageFont imageFont = null, FontType? fontType = null, Color? color = null, float? scale = null)
        {
            desiredString = value;
            _currentText = TextFactory.CreateUIText(string.Empty, position, lineXStart, lineLimit, layerDepth,imageFont, fontType,color,scale);
            _textTimer = new SimpleTimer(_typeTime);
        }

        public bool Update(GameTime gameTime, Vector2 position, float lineLimit)
        {
            if (!IsComplete && _textTimer.Run(gameTime) )
            {
                _currentText.Append(desiredString.ElementAt(_currentIndex));
                _currentIndex++;

            }
            _currentText.Update(position);
            return IsComplete;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentText.Draw(spriteBatch,.99f);
        }

        public void ForceComplete()
        {
            _currentText.ClearAndSet(desiredString);
        }
    }
}
