using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.EscMenuStuff
{
    internal class EscMenu : InterfaceSection
    {
        private Rectangle _returnToMainMenuButtonBackgroundDimensions = new Rectangle(0, 0, 80, 96);
        private NineSliceButton _returnToMainMenuButton;



        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 148, 240);
        private NineSliceSprite _backGroundSprite;
        public EscMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth, bool suppressParentSection = true) : base(interfaceSection, graphicsDevice, content, position, layerDepth, suppressParentSection)
        {
            IsActive = false;
        }
        public override void LoadContent()
        {
            base.LoadContent();
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(RectangleHelper.CenterRectangleOnScreen(_backGroundSpriteDimensions), _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back));
            _returnToMainMenuButton = new NineSliceTextButton(this, graphics, content, RectangleHelper.CenterRectangleInRectangle(_returnToMainMenuButtonBackgroundDimensions, _backGroundSpriteDimensions),
                GetLayeringDepth(UILayeringDepths.Low), _returnToMainMenuButtonBackgroundDimensions, null, UI.ButtonTexture,
                new List<Text>() {TextFactory.CreateUIText("Return to main menu", GetLayeringDepth(UILayeringDepths.Medium)) }, null);
        }
        public override void Update(GameTime gameTime)
        {
            if (Controls.WasKeyTapped(Keys.Escape))
                Toggle();
            base.Update(gameTime);
          
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

       

        
    }
}
