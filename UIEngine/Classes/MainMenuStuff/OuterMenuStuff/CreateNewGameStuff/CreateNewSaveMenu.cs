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
using UIEngine.Classes.TextStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class CreateNewSaveMenu : InterfaceSection
    {
        private Rectangle _nameWindowRectangle = new Rectangle(0, 0, 128, 32);
        private TypingBox _nameTypingBox;
        public CreateNewSaveMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void Load()
        {
            base.Load();

            Vector2 typingBoxPos = RectangleHelper.CenterRectangleOnScreen(_nameWindowRectangle);
            _nameTypingBox = new TypingBox(this,graphics, content, typingBoxPos, GetLayeringDepth(UILayeringDepths.Low), _nameWindowRectangle.Width, _nameWindowRectangle.Height, null);
        }

        public override void Unload()
        {
            base.Unload();
        }
       
        

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            _nameTypingBox.Update(gameTime);

            // _backGroundSprite.Update(gameTime, Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            _nameTypingBox.Draw(spriteBatch);
           // _backGroundSprite.Draw(spriteBatch);  
        }

    }
}
