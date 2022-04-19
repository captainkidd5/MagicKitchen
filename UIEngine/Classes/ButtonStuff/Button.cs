﻿using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Globals.Classes.Settings;
using static UIEngine.Classes.UI;

namespace UIEngine.Classes.ButtonStuff
{
    internal class Button : ButtonBase
    {
        public Button(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2 position,float layerDepth,  Rectangle? sourceRectangle, Sprite foregroundSprite, 
            Point? samplePoint, Action buttonAction = null, bool hoverTransparency = true, float scale = 2f) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, foregroundSprite, samplePoint, buttonAction, hoverTransparency)
        {
            BackGroundSprite = SpriteFactory.CreateUISprite(position, sourceRectangle.Value,ButtonTexture, LayerDepth, null, null, new Vector2(scale, scale));
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
