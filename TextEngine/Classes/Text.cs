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

        

        internal Text(string sentence, float layerDepth,  ImageFont imageFont, FontType fontType, Color color, Vector2 scale)
        {
            _scale = scale;
            _layerDepth = layerDepth;
            _imageFont = imageFont;
            _fontType = fontType ;
            Color = color;
            _words = new List<Word>();
            if(!string.IsNullOrEmpty(sentence))
             AddSentence(sentence);
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
        public void BackSpace()
        {
            //Todo
        }

        public void Update(Vector2 position)
        {
            foreach (Word word in _words)
                word.Update(position);
        }
        public Vector2 Update(Vector2 position, float lineXStart, float lineLimit)
        {
            return CalculateWidthAndHeight(position, lineXStart, lineLimit);
        }
        public Vector2 CalculateWidthAndHeight(Vector2 position, float lineXStart, float lineLimit)
        {
            Height = _imageFont.FontDimension;
            Width = 0;
            for (int i = _words.Count - 1; i >= 0; i--)
            {
                //Reached line limit, wrap around
                if (position.X + _words[i].Width > lineLimit)
                {
                    position = new Vector2(lineXStart, position.Y + _words[i].Height);
                    Width = lineLimit;
                    Height += _words[i].Height;
                }

                _words[i].Update(position);

                position = new Vector2(position.X + _words[i].Width, position.Y);
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

        public Vector2 CenterInRectangle(Rectangle rectangle, float rectangleScale)
        {
            return new Vector2(rectangle.X, rectangle.Y + rectangleScale);
        }
    }
}
