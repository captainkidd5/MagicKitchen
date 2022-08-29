using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    public class Text
    {
        private readonly Vector2 _scale;
        private readonly float _layerDepth;
        private readonly ImageFont _imageFont;
        private readonly FontType _fontType;
        public Color Color { get; set; }
        private List<Word> _words;

        public float Width { get; private set; }
        public float Height { get; private set; }

        //For now, used for y axis layer depth in world text
        public Rectangle Rectangle => new Rectangle(0, 0, (int)Width, (int)Height);

        public bool IsEmpty => _words.Count < 1;

        public float SingleCharacterWidth => _scale.X * _imageFont.FontSpaceWidth;

        internal Text(string sentence, Vector2 position, float? lineXStart, float? lineLimit, float layerDepth, ImageFont imageFont, FontType fontType, Color color, Vector2 scale)
        {
            _scale = scale;
            _layerDepth = layerDepth;
            _imageFont = imageFont;
            _fontType = fontType;
            Color = color;
            _words = new List<Word>();
            if (!string.IsNullOrEmpty(sentence))
                AddSentence(sentence);

            CalculateWidthAndHeight(position, lineXStart, lineLimit);

        }

        public void Clear() => _words.Clear();
        public override string ToString()
        {
            string stringToReturn = string.Empty;
            for (int i = 0; i < _words.Count; i++)
            {
                stringToReturn += _words[i].Str;
                if (i < _words.Count - 1)
                    stringToReturn += " ";
            }

            return stringToReturn;
        }
        public void ClearAndSet(string sentence)
        {
            Clear();
            AddSentence(sentence);
        }
        public void AddSentence(string fullSentence)
        {
            string[] strings = fullSentence.Split(' ');
            foreach (string str in strings)
            {
                AddWord(str);
            }
        }
        public void AddWord(string str)
        {
            Word word = new Word(str, _fontType, _imageFont, Color, _scale);
            _words.Add(word);
        }


        public void Append(char c)
        {
            if (c == ' ')
            {
                //Dissallow adding double spaces
                if (_words[_words.Count - 1].Empty)
                    return;
                AddWord("");
                return;
            }
            //appending character to empty sentence
            if (_words.Count < 1)
            {
                AddWord(c.ToString());
                return;
            }


            _words[_words.Count - 1].Append(c);
        }
        public void Append(string s)
        {
            if (s == " ")
            {
                //Dissallow adding double spaces
                if (_words[_words.Count - 1].Empty)
                    return;
                AddWord("");
                return;
            }
            //appending character to empty sentence
            if (_words.Count < 1)
            {
                AddWord(s);
                return;
            }


            _words[_words.Count - 1].Append(s);
        }
        public void BackSpace()
        {
            //Todo
            if (_words.Count < 1)
                return;

            if (_words[_words.Count - 1].BackSpace())
            {
                _words.RemoveAt(_words.Count - 1);
            }
        }

        public void Update(Vector2 position)
        {
            CalculateWidthAndHeight(position, null, null);
            //foreach (Word word in _words)
            //  word.Update(position);
        }
        public Vector2 Update(Vector2 position, float? lineXStart, float? lineLimit, string entireSentence = null)
        {
            if (entireSentence == null)
                return CalculateWidthAndHeight(position, lineXStart, lineLimit);
            else
                return CalculateIncrementedWidthAndHeight(position, lineXStart, lineLimit, entireSentence);

        }

        private bool WordExceedsLimit(Word word, Vector2 pos, float lineXStart, float lineLimit)
        {
            return pos.X +  _imageFont.FontDimension * 2 +word.Width >= lineXStart + lineLimit;
        }

        /// <summary>
        /// Only for use with text builder. All other scenarios should use the other one
        /// </summary>

        private Vector2 CalculateIncrementedWidthAndHeight(Vector2 position, float? lineXStart, float? lineLimit, string entireSentence)
        {
            Height = _imageFont.FontDimension * _scale.Y;
            Width = 0;
            lineLimit = lineLimit ?? 1000;
            lineXStart = lineXStart ?? position.X;
            int totalCharacters = 0;
            for (int i = 0; i < _words.Count; i++)
            {
                if (entireSentence == null)
                    throw new Exception($"Method should only be used with text builder");

                string _wordWorkingOn = entireSentence.Substring(totalCharacters , entireSentence.Length - totalCharacters );
                _wordWorkingOn = _wordWorkingOn.Split(' ')[0];
                Word word = TextFactory.CreatePlaceHolderWord(_wordWorkingOn);
                if (WordExceedsLimit(word, position, lineXStart.Value, lineLimit.Value) || (i > 0 && word.Str.Contains("\n")))
                {
                    position = MoveToNextLineDown(position, lineXStart, lineLimit, i);
                    if (Width < lineLimit)
                        Width += _words[i].Width + SingleCharacterWidth;
                    totalCharacters += _words[i].Str.Length + 1;

                    continue;

                }


                //Reached line limit, wrap around
                if (WordExceedsLimit(_words[i], position, lineXStart.Value, lineLimit.Value) || (i > 0 && _words[i - 1].Str.Contains("\n")))

                {
                    position = MoveToNextLineDown(position, lineXStart, lineLimit, i);

                }

                else
                {
                    _words[i].Update(position);

                    position = new Vector2(position.X + _words[i].Width + SingleCharacterWidth, position.Y);
                }

                if (Width < lineLimit)
                    Width += _words[i].Width + SingleCharacterWidth;
                int len = _words[i].Str.Length + 1;// plus 1 to account for space
                totalCharacters += len;

            }
            return position;
        }


        public Vector2 CalculateWidthAndHeight(Vector2 position, float? lineXStart, float? lineLimit)
        {
            Height = _imageFont.FontDimension * _scale.Y;
            Width = 0;
            lineLimit = lineLimit ?? 1000;
            lineXStart = lineXStart ?? position.X;

            for (int i = 0; i < _words.Count; i++)
            {

                //Reached line limit, wrap around
                if (WordExceedsLimit(_words[i], position, lineXStart.Value, lineLimit.Value) || (i > 0 && _words[i - 1].Str.Contains("\n")))

                {
                    position = MoveToNextLineDown(position, lineXStart, lineLimit, i);

                }

                else
                {
                    _words[i].Update(position);

                    position = new Vector2(position.X + _words[i].Width + SingleCharacterWidth, position.Y);
                }

                if (Width < lineLimit)
                    Width += _words[i].Width + SingleCharacterWidth;
            }
            return position;
        }

        private Vector2 MoveToNextLineDown(Vector2 position, float? lineXStart, float? lineLimit, int i)
        {
            position = new Vector2(lineXStart.Value, position.Y + _words[i].Height);
            Width = lineLimit.Value;
            Height += _words[i].Height;
            _words[i].Update(position);
            position = new Vector2(lineXStart.Value + _words[i].Width + SingleCharacterWidth, position.Y);
            return position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            for (int i = _words.Count - 1; i >= 0; i--)
            {
                _words[i].Draw(spriteBatch, _layerDepth);
            }
        }
        public void Draw(SpriteBatch spriteBatch, float customLayer)
        {
            for (int i = _words.Count - 1; i >= 0; i--)
            {
                _words[i].Draw(spriteBatch, customLayer);
            }
        }

        public Vector2 CenterInRectangle(Rectangle rectangle, float rectangleScale)
        {
            return new Vector2(rectangle.X + rectangle.Width * rectangleScale / 2 - Width / 2, rectangle.Y + rectangle.Height * rectangleScale / 2 - Height / 2);
        }
    }
}
