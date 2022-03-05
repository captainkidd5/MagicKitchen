using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.EscMenuStuff
{
    internal class EscMenu : InterfaceSection
    {

        private NineSliceButton _returnToMainMenuButton;
        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 148, 240);
        private NineSliceSprite _backGroundSprite;
        public EscMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth, bool suppressParentSection = true) : base(interfaceSection, graphicsDevice, content, position, layerDepth, suppressParentSection)
        {

        }
        public override void LoadContent()
        {
            base.LoadContent();
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(RectangleHelper.CenterRectangleOnScreen(_backGroundSpriteDimensions), _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back));
            //_returnToMainMenuButton = new NineSliceButton(this, graphics, content, )
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

       

        
    }
}
