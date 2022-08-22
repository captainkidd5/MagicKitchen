using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.TextStuff
{
    internal class TypingEntryPointMarker
    {
        private SimpleTimer _simpleTimer;
        private float _blinkRate = .5f;
        private bool _isDrawn = true;

        private Sprite _sprite;
        private Rectangle _sourceRectangle = new Rectangle(720,16,16,16);
        public TypingEntryPointMarker()
        {
            _simpleTimer = new SimpleTimer(_blinkRate);
            
        }
        public void Load(float layer)
        {
            _sprite = SpriteFactory.CreateUISprite(Vector2.Zero, _sourceRectangle, UI.ButtonTexture, layer, scale: new Vector2(2f,2f));
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            if (_simpleTimer.Run(gameTime))
                _isDrawn = !_isDrawn;

            if (_isDrawn)
                _sprite.Update(gameTime, position);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isDrawn)
                _sprite.Draw(spriteBatch);
        }
    }
}
