using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TextEngine.Classes;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.InterfaceStuff
{
    public class NineSliceSprite : BaseSprite
    {
        internal NineSlice NineSlice { get; set; }



        public override Rectangle HitBox => NineSlice.Rectangle;

        public override int Width => NineSlice.Rectangle.Width;
        public override int Height => NineSlice.Rectangle.Height;


        /// <summary>
        /// Note if using Text, the text is expecting to fill up the nineslice. Draw text separately over the nineslice if planning using additional graphics.
        /// </summary>
        internal NineSliceSprite(GraphicsDevice graphics, ContentManager content, Vector2 position, NineSlice nineSlice, Texture2D texture, Color primaryColor,
             Vector2 origin, Vector2 scale, float customLayer) : base(graphics, content, position, texture, primaryColor, origin, scale,customLayer)
        {
            NineSlice = nineSlice;

        }
        public override void ResetColors()
        {
            base.ResetColors();
            NineSlice.Color = PrimaryColor;
        }
        public override void Update(GameTime gameTime,Vector2 position, bool updatePeripheralActoins = true)
        {
            base.Update(gameTime, position, updatePeripheralActoins);
            NineSlice.Color = PrimaryColor;

        }

        /// <summary>
        /// Only fullstring text supported at the moment
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            NineSlice.Draw(spriteBatch);

        }
    }
}
