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
            Sprite foregroundSprite, Texture2D? texture, Point? samplePoint, Action buttonAction = null, bool hoverTransparency = false, bool requireConfirmation = false)
            : base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, foregroundSprite, texture, samplePoint, buttonAction, hoverTransparency, requireConfirmation)
        {
            texture = texture ?? UI.ButtonTexture;
                BackGroundSprite = SpriteFactory.CreateNineSliceSprite(position, sourceRectangle == null ? DefaultButtonWidth : sourceRectangle.Value.Width,
                    sourceRectangle == null ? DefaultButtonHeight : sourceRectangle.Value.Height, UI.ButtonTexture, LayerDepth, null, null);
                Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat, samplePoint ?? _samplePoint, ButtonTexture.Width);
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
