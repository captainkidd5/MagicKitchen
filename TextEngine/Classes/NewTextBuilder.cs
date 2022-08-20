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
        private readonly Text _text;

        public NewTextBuilder(String value, ImageFont? imageFont = null, FontType? fontType = null, Color? color = null, Vector2? scale = null)
        {
            _text = TextFactory.CreateUIText(value,imageFont,fontType,color,scale);
        }

        public void Update(GameTime gameTime, Vector2 position, float lineLimit)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
