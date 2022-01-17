using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class CreateNewSaveMenu : InterfaceSection
    {
        private readonly Rectangle _backGroundRectangleDimensions = new Rectangle(0,0, 128,64);
        private Sprite _backGroundSprite;
        public CreateNewSaveMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void Load()
        {
            base.Load();
            _backGroundSprite = SpriteFactory.CreateUISprite(Position, _backGroundRectangleDimensions, UI.ButtonTexture, LayerDepth);
        }

        public override void Unload()
        {
            base.Unload();
        }
       
        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _backGroundSprite.Update(gameTime, Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _backGroundSprite.Draw(spriteBatch);  
        }

    }
}
