using Globals.Classes;
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



        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 224, 320);
        private NineSliceSprite _backGroundSprite;

        private Action _returnToMainMenuAction;
        public EscMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth, bool suppressParentSection = true) : base(interfaceSection, graphicsDevice, content, position, layerDepth, suppressParentSection)
        {
            IsActive = false;
            NormallyActivated = false;
        }
        public override void LoadContent()
        {
            _returnToMainMenuAction = new Action(ReturnToMainMenu);
            Vector2 escMenuPos = RectangleHelper.CenterRectangleOnScreen(_backGroundSpriteDimensions);
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(escMenuPos, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back));

            _backGroundSprite.LoadContent();
            _returnToMainMenuButton = new NineSliceTextButton(this, graphics, content, RectangleHelper.CenterRectangleInRectangle(_returnToMainMenuButtonBackgroundDimensions, _backGroundSprite.HitBox),
                GetLayeringDepth(UILayeringDepths.Low), _returnToMainMenuButtonBackgroundDimensions, null, UI.ButtonTexture,
                new List<Text>() {TextFactory.CreateUIText("Return to main menu", GetLayeringDepth(UILayeringDepths.Medium)) }, null, _returnToMainMenuAction);
            _returnToMainMenuButton.AddConfirmationWindow($"Return to main menu?");

            _returnToMainMenuButton.LoadContent();
            CloseButton = UI.ButtonFactory.CreateCloseButton(this, new Rectangle((int)escMenuPos.X, (int)escMenuPos.Y, _backGroundSprite.Width, _backGroundSprite.Height), GetLayeringDepth(UILayeringDepths.Medium),
                new Action(() =>
                {
                    Close();
                    Flags.Pause = false;
                }));
            CloseButton.LoadContent();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height);
            base.LoadContent();

        }

        private void ReturnToMainMenu()
        {
            UI.ReturnToMainMenu();
        }
        public override void Update(GameTime gameTime)
        {
            if (Controls.WasKeyTapped(Keys.Escape))
            {
                Toggle();

                Flags.Pause = IsActive;

            }
            base.Update(gameTime);
          
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
           
        }

       

        
    }
}
