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
        private string _desiredString;
        public float Height => _currentText.Height;

        public bool IsComplete => _currentText.ToString() == _desiredString;
        public void ClearCurrent()
        {
            _currentText.Clear();
            _currentIndex = 0;
        }

        private SimpleTimer _textTimer;
        private float _typeTime = .05f;
        private float _pauseTime = .2f;
        private int _currentIndex;
        public TextBuilder(String value, Vector2 position, float? lineXStart, float? lineLimit, float layerDepth, ImageFont imageFont = null, FontType? fontType = null, Color? color = null, float? scale = null)
        {
            _desiredString = value;
            _currentText = TextFactory.CreateUIText(string.Empty, position, lineXStart, lineLimit, layerDepth, imageFont, fontType, color, scale);
            _textTimer = new SimpleTimer(_typeTime);
        }


        public void SetDesiredText(string desiredString)
        {
            _desiredString = desiredString;
            ClearCurrent();

        }

        public bool Update(GameTime gameTime, Vector2 position, float lineLimit)
        {

            if (!IsComplete && _textTimer.Run(gameTime))
            {
                float pauseDuration = GetCharacterPauseDuration();

                _textTimer.SetNewTargetTime(pauseDuration);
                //char c = _desiredString.ElementAt(_currentIndex);
                //if(c == ' ')
                //{
                //    _wordWorkingOn = _desiredString.Substring(_currentIndex + 1, _desiredString.Length - _currentIndex - 1);
                //    _wordWorkingOn = _wordWorkingOn.Split(' ')[0];
                //    _word = TextFactory.CreatePlaceHolderWord(_wordWorkingOn);
                //}
                _currentText.Append(_desiredString.ElementAt(_currentIndex));
                _currentIndex++;


            }
            _currentText.Update(position, null, lineLimit, _desiredString);
            return IsComplete;
        }

        
        /// <summary>
        /// Each character may specify the duration to be paused for. Defaults to field if none given
        /// </summary>
        /// <returns></returns>
        private float GetCharacterPauseDuration()
        {

            char c = _desiredString.ElementAt(_currentIndex);
            if (c == ' ')
                return _typeTime;
            float pauseDuration = LanguageManager.CurrentLanguage.CharPoints[c].PauseDuration == 0 ?
                _typeTime : LanguageManager.CurrentLanguage.CharPoints[c].PauseDuration / 100;

            return pauseDuration;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentText.Draw(spriteBatch, .99f);
        }

        public void ForceComplete()
        {
            _currentText.ClearAndSet(_desiredString);
        }
    }
}
