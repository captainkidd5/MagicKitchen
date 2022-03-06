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

namespace UIEngine.Classes.ButtonStuff
{
    internal class ConfirmationWindow : InterfaceSection
    {

        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 80, 128);
        private NineSliceSprite _backGroundSprite;

        private Action _confirmAction;
        private Action _cancelAction;
        private Button _confirmButton;
        private Button _cancelButton;
        /// <summary>
        /// Default cancel action is to just close this window
        /// </summary>
        public ConfirmationWindow(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth, 
            Action confirmAction, Action? cancelAction = null, bool suppressParentSection = true) : base(interfaceSection, graphicsDevice, content, position, layerDepth, suppressParentSection)
        {
            _confirmAction = confirmAction;
            _cancelAction = cancelAction ?? new Action(() => Close());
            
        }
        public override void LoadContent()
        {
            base.LoadContent();
            Position = RectangleHelper.CenterRectangleOnScreen(_backGroundSpriteDimensions);
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(Position, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back));

            Vector2 confirmButtonPos = RectangleHelper.CenterRectangleInRectangle(ButtonFactory.s_greenCheckRectangle, HitBox);
            _confirmButton = new Button(this, graphics, content, confirmButtonPos, GetLayeringDepth(UILayeringDepths.Low), ButtonFactory.s_greenCheckRectangle, null, UI.ButtonTexture, null, _confirmAction);

            Vector2 cancelButtonPos = RectangleHelper.CenterRectangleInRectangle(ButtonFactory.s_redExRectangle, HitBox);
            _cancelButton = new Button(this, graphics, content, confirmButtonPos, GetLayeringDepth(UILayeringDepths.Low), ButtonFactory.s_redExRectangle, null, UI.ButtonTexture, null, _cancelAction);
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
