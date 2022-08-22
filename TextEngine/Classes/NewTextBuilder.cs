using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    public class NewTextBuilder
    {
        private readonly Text _currentText;
        private readonly string desiredString;
        public float Height => _currentText.Height;

        public bool IsComplete() => _currentText.ToString() == desiredString;
        public void ClearCurrent()=> _currentText.Clear();
        public NewTextBuilder(String value, Vector2 position, float? lineXStart, float? lineLimit, float layerDepth, ImageFont imageFont = null, FontType? fontType = null, Color? color = null, float? scale = null)
        {
            desiredString = value;
            _currentText = TextFactory.CreateUIText(string.Empty, position, lineXStart, lineLimit, layerDepth,imageFont, fontType,color,scale);
        }

        public bool Update(GameTime gameTime, Vector2 position, float lineLimit)
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void ForceComplete()
        {
            _currentText.ClearAndSet(desiredString);
        }
    }
}
