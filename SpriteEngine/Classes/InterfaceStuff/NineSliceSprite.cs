using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.InterfaceStuff
{
    public class NineSliceSprite : BaseSprite
    {
        internal NineSlice NineSlice { get; set; }

        public override Rectangle HitBox => NineSlice.Rectangle;

        internal NineSliceSprite(GraphicsDevice graphics, ContentManager content, Vector2 position, NineSlice nineSlice, Texture2D texture, Color primaryColor,
             Vector2 origin, float scale, float customLayer) : base(graphics, content, position, texture, primaryColor, origin, scale,customLayer)
        {
            NineSlice = nineSlice;
        }


        public override void Update(GameTime gameTime,Vector2 position)
        {
            base.Update(gameTime, position);
            NineSlice.Color = PrimaryColor;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            NineSlice.Draw(spriteBatch);
        }
    }
}
