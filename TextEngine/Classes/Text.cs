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

        internal Text(string sentence, Vector2 position, float? lineXStart, float? lineLimit, float layerDepth,  ImageFont imageFont, FontType fontType, Color color, Vector2 scale)
        {
            _scale = scale;
            _layerDepth = layerDepth;
            _imageFont = imageFont;
            _fontType = fontType ;
            Color = color;
            _words = new List<Word>();
            if(!string.IsNullOrEmpty(sentence))
             AddSentence(sentence);

            CalculateWidthAndHeight(position,lineXStart, lineLimit);

        }

        public void Clear() => _words.Clear();
        public override string ToString()
        {
            string stringToReturn = string.Empty;
            foreach(Word word in _words)
            {
                stringToReturn += word.Str;
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
            foreach(string str in strings)
            {
                AddWord(str);
            }
        }
        public void AddWord(string str)
        {
            Word word = new Word(str, _fontType, _imageFont,Color, _scale);
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

            if(_words[_words.Count - 1].BackSpace())
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
        public Vector2 Update(Vector2 position, float? lineXStart, float? lineLimit)
        {
            return CalculateWidthAndHeight(position, lineXStart, lineLimit);
        }
        public Vector2 CalculateWidthAndHeight(Vector2 position, float? lineXStart, float? lineLimit)
        {
            Height = _imageFont.FontDimension * _scale.Y;
            Width = 0;
            lineLimit = lineLimit ?? 1000;
            lineXStart = lineXStart ?? position.X;
            if (_words.Count > 5)
                Console.WriteLine("test");
            for (int i = 0; i < _words.Count; i++)
            {
                if (_words.Count > 5)
                    Console.WriteLine("test");
                //Reached line limit, wrap around
                if (position.X + _words[i].Width > lineXStart + lineLimit || (i > 0 && _words[i - 1].Str.Contains("\n")))

                {
                    if (_words.Count > 5)
                        Console.WriteLine("test");
                    position = new Vector2(lineXStart.Value, position.Y + _words[i].Height);
                    Width = lineLimit.Value;
                    Height += _words[i].Height;
                    _words[i].Update(position);

                }

                else
                {
                    _words[i].Update(position);

                    position = new Vector2(position.X + _words[i].Width, position.Y);
                }
               
                if (Width < lineLimit)
                    Width += _words[i].Width;
            }
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

        public static Vector2 CenterInRectangle(Rectangle rectangle, float rectangleScale)
        {
            return new Vector2(rectangle.X, rectangle.Y + rectangleScale);
        }
    }
}
