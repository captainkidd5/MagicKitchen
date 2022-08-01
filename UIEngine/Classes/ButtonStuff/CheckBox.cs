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
using static UIEngine.Classes.UI;

namespace UIEngine.Classes.ButtonStuff
{
    internal class CheckBox : Button
    {
        private static readonly Rectangle s_emptyBoxSourceRectangle = new Rectangle(144,48, 32,32);
        private static readonly Rectangle s_checkMarkSourceRectangle = new Rectangle(176,48,32,32);
        

        public bool ValueLastFrame { get; set; }
        public bool Value { get; set; }
        public CheckBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2 position,
            float layerDepth, Rectangle? sourceRectangle, Action buttonAction = null, Sprite foregroundSprite = null,
            Point? samplePoint = null, bool hoverTransparency = true, float scale = 2) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth, sourceRectangle,
                buttonAction, foregroundSprite, samplePoint, hoverTransparency, scale)
        {
            BackGroundSprite = SpriteFactory.CreateUISprite(position, s_emptyBoxSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low), null, null, new Vector2(scale,scale));
            Color sampleCol = TextureHelper.SampleAt(ButtonTextureDat, samplePoint ?? _samplePoint, ButtonTexture.Width);
            ForegroundSprite = SpriteFactory.CreateUISprite(Position, s_checkMarkSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium), null, null, new Vector2(scale, scale));
            BackGroundSprite.AddSaturateEffect(sampleCol, false);
            ForeGroundSpriteOffSet = Vector2.Zero;

        }
        public void SetToggleValue(bool value)
        {
            Value = value;
            if (!Value)
                ForegroundSprite.SwapSourceRectangle(new Rectangle(0,0,1,1));
            else
                ForegroundSprite.SwapSourceRectangle(s_checkMarkSourceRectangle);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Clicked)
            {
                Value = !Value;
            }
            if(ValueLastFrame != Value)
            {
                SetToggleValue(Value);

            }
            ValueLastFrame = Value;
        }

        public Action ActionOnSave { get; set; }
        public void SaveSettings()
        {
            ActionOnSave();
        }
    }
}
