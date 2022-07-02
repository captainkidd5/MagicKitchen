using Globals.Classes;
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
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.Storage.ItemAlerts
{
    internal class ItemAlert : InterfaceSection
    {
        private static Rectangle _backgroundSourceRectangle = new Rectangle(176, 0, 96, 32);
        private static float _TTL = 2f;
        public int ItemId { get; set; }
        public byte Count { get; set; }
        private Sprite _background;
        private Text _text;
        private SimpleTimer _simpleTimer;

        private Vector2 _textOffSet = new Vector2(16, 16);
        public ItemAlert(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public void Increment(int amt)
        {
            _simpleTimer.SetNewTargetTime(_TTL);
            Count += (byte)amt;
            _text.SetFullString($"+ {Count}");
            _background.ResetColors();
        }
        public override void LoadContent()
        {
           // base.LoadContent();
            _simpleTimer = new SimpleTimer(_TTL, false);
            _text = TextFactory.CreateUIText($"+ {Count}", GetLayeringDepth(UILayeringDepths.Medium));
            _background = SpriteFactory.CreateUISprite(Position, _backgroundSourceRectangle, UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Low), scale:new Vector2(2f,2f));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _background.Update(gameTime, Position);
            _text.Update(gameTime, Position + _textOffSet);
            if (_simpleTimer.Run(gameTime))
                _background.AddFaderEffect(null, null, true);

            if(_background.IsTransparent)
                FlaggedForRemoval = true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _background.Draw(spriteBatch);
            _text.Draw(spriteBatch, true);

            base.Draw(spriteBatch);
        }
    }
}
