using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    internal class Sentence
    {
        private readonly Vector2 _scale;
        private readonly ImageFont _imageFont;
        private readonly FontType _fontType;
        private readonly Color _color;
        private List<Word> _words;

        public float Width { get; private set; }
        public float Height { get; private set; }

        public Sentence(Vector2 scale, ImageFont imageFont, FontType? fontType = null, Color? color = null)
        {
            _scale = scale;
            _imageFont = imageFont;
            _fontType = fontType ?? FontType.Standard;
            _color = color ?? Color.White;
            _words = new List<Word>();
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
            Word word = new Word(str, _fontType, _imageFont,_color, _scale);
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
        public Vector2 Update(Vector2 position, float lineXStart, float lineLimit)
        {
            Height = ImageFont.FontDimension;
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

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            for (int i = _words.Count - 1; i >= 0; i--)
            {
                _words[i].Draw(spriteBatch,layerDepth);
            }
        }

        public Vector2 CenterInRectangle(Rectangle rectangle, float rectangleScale)
        {
            return new Vector2(rectangle.X, rectangle.Y + rectangleScale);
        }
    }
}
