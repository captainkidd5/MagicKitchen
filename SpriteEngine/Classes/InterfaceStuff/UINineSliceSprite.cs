using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.InterfaceStuff
{
    public class UINineSliceSprite : BaseSprite
    {
        internal NineSlice NineSlice { get; set; }
        public Color Color { get { return NineSlice.Color; } set { NineSlice.Color = value; } }
        internal UINineSliceSprite(GraphicsDevice graphics, ContentManager content, Vector2 position, NineSlice nineSlice, Texture2D texture, Color primaryColor,
             Vector2 origin, float scale, Layers layer) : base(graphics, content, position, texture, primaryColor, origin, scale, layer)
        {
            NineSlice = nineSlice;
            HitBox = NineSlice.HitBox;
        }


        public override void Update(GameTime gameTime,Vector2 position)
        {
            base.Update(gameTime, position);
            NineSlice.Color = Color.White;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            NineSlice.Draw(spriteBatch);
        }
    }
}
