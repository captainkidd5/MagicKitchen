using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.Addons
{
    internal class Fader 
    {
        private readonly float _minOpacity = .2f;
        private readonly float _maxOpcacity = 1f;
        private readonly float _speed = .005f;
        private float _opacity = 1f;

        private bool _isTurningTransparent;
        private bool _isReturningToOpaque;

        internal Fader(float? minOpac, float? maxOpac, float? speed)
        {
            _minOpacity = minOpac ?? _minOpacity;
            _maxOpcacity = maxOpac ?? _maxOpcacity;
            _speed = speed ?? _speed;
        }
        internal void Update(GameTime gameTime, BaseSprite sprite)
        {
            if (_isTurningTransparent)
                TurnTransparent(gameTime,sprite);

            else if (_isReturningToOpaque)
                ReturnToOpaque(gameTime,sprite);
        }

        private void TurnTransparent(GameTime gameTime, BaseSprite sprite)
        {
            _opacity -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _speed;
            if (_opacity < _minOpacity)
            {
                _isTurningTransparent = false;
                _opacity = _minOpacity;
            }

            sprite.UpdateColor(Color.White * _opacity);
        }

        /// <summary>
        /// When player is no longer behind the tile, return it back to its normal opacity at rate SPEED.
        /// </summary>
        private void ReturnToOpaque(GameTime gameTime, BaseSprite sprite)
        {
            _opacity += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _speed;
            if (_opacity > _maxOpcacity)
            {
                _isReturningToOpaque = false;
                _opacity = _maxOpcacity;
            }
            sprite.UpdateColor(Color.White * _opacity);


        }

        internal void TriggerTurnTransparent()
        {
            _isTurningTransparent = true;
            _isReturningToOpaque = false;
        }

        internal void TriggerReturnOpaque()
        {
            _isReturningToOpaque = true;
            _isTurningTransparent = false;
        }
    }
}
