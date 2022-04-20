using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static UIEngine.Classes.UI;
using static Globals.Classes.Settings;
using Globals.Classes.Helpers;

namespace UIEngine.Classes.ButtonStuff
{
    internal class NineSliceButton : ButtonBase
    {


        public NineSliceButton(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2 position,float layerDepth, Rectangle? sourceRectangle,
           Action buttonAction = null, Sprite foregroundSprite = null, Point? samplePoint = null,  bool hoverTransparency = false)
            : base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, buttonAction, foregroundSprite, samplePoint, hoverTransparency)
        {
            BackGroundSprite = SpriteFactory.CreateNineSliceSprite(position, sourceRectangle == null ? DefaultButtonWidth : sourceRectangle.Value.Width,
            sourceRectangle == null ? DefaultButtonHeight : sourceRectangle.Value.Height, UI.ButtonTexture, LayerDepth, null, null);
            Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat, samplePoint ?? _samplePoint, ButtonTexture.Width);
            BackGroundSprite.AddSaturateEffect(sampleCol, false);
        }
        public override void MovePosition(Vector2 newPos)
        {
            BackGroundSprite = SpriteFactory.CreateNineSliceSprite(Position,  SourceRectangle.Width,
            SourceRectangle.Height, UI.ButtonTexture, LayerDepth, null, null);
            Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat,_samplePoint, ButtonTexture.Width);
            BackGroundSprite.AddSaturateEffect(sampleCol, false);
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
