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

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class CreateNewSaveMenu : InterfaceSection
    {
      //  private readonly Rectangle _backGroundRectangleDimensions = new Rectangle(0,0, 360,480);
     //   private NineSliceSprite _backGroundSprite;
        public CreateNewSaveMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void Load()
        {
            base.Load();
            //Vector2 backGroundPos = RectangleHelper.CenterRectangleOnScreen(_backGroundRectangleDimensions);
            //_backGroundSprite = SpriteFactory.CreateNineSliceSprite(backGroundPos, _backGroundRectangleDimensions.Width, _backGroundRectangleDimensions.Height,
            //    UI.ButtonTexture, LayerDepth);
        }

        public override void Unload()
        {
            base.Unload();
        }
       
        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           // _backGroundSprite.Update(gameTime, Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
           // _backGroundSprite.Draw(spriteBatch);  
        }

    }
}
