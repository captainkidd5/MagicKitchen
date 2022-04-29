using Globals.Classes.Helpers;
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
        private Vector2 _scale;

        public Button(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2 position,float layerDepth,  Rectangle? sourceRectangle,  Action buttonAction = null, Sprite foregroundSprite = null,
            Point? samplePoint = null,  bool hoverTransparency = true, float scale = 2f) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle, buttonAction, foregroundSprite, samplePoint, hoverTransparency)
        {
            _scale = new Vector2(scale, scale);
            BackGroundSprite = SpriteFactory.CreateUISprite(position, sourceRectangle.Value,ButtonTexture, LayerDepth, null, null, _scale);
            Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat, samplePoint ?? _samplePoint, ButtonTexture.Width);
            BackGroundSprite.AddSaturateEffect(sampleCol, false);


        }
        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            
            BackGroundSprite = SpriteFactory.CreateUISprite(Position, SourceRectangle, ButtonTexture, LayerDepth, null, null, _scale);
            Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat, _samplePoint, ButtonTexture.Width);
            BackGroundSprite.AddSaturateEffect(sampleCol, false);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (ForegroundSprite != null)
                ForegroundSprite.Update(gameTime, Position + ForeGroundSpriteOffSet.Value);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
