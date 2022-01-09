using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.Addons
{
    internal enum FaderState
    {
        Opaque = 1,
        Turning_Opaque = 2,
        Transparent = 3,
        Turning_Transparent = 4
        
    }
    internal class Fader : ISpriteAddon
    {
        private readonly float _minOpacity = .2f; //51
        private readonly float _maxOpcacity = 1f;
        private readonly float _speed = .005f;
        private float _opacity = 1f;


        internal FaderState FaderState = FaderState.Opaque;
        internal bool IsOpaque => FaderState == FaderState.Opaque;
        internal bool Istransparent => FaderState == FaderState.Transparent;

        internal bool FlaggedForRemovalUponFinish;
        internal Fader(float? minOpac, float? maxOpac, float? speed)
        {
            _minOpacity = minOpac ?? _minOpacity;
            _maxOpcacity = maxOpac ?? _maxOpcacity;
            _speed = speed ?? _speed;
        }
        public void Update(GameTime gameTime, BaseSprite sprite)
        {
            if (FaderState == FaderState.Turning_Transparent)
                TurnTransparent(gameTime,sprite);

            else if (FaderState == FaderState.Turning_Opaque)
                ReturnToOpaque(gameTime,sprite);
        }

        private void TurnTransparent(GameTime gameTime, BaseSprite sprite)
        {
            _opacity -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _speed;
            if (_opacity < _minOpacity)
            {
                FaderState = FaderState.Transparent;
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
                FaderState = FaderState.Opaque;

                _opacity = _maxOpcacity;
            }
            sprite.UpdateColor(Color.White * _opacity);


        }

        internal void TriggerTurnTransparent()
        {
            FaderState = FaderState.Turning_Transparent;
        }

        internal void TriggerReturnOpaque()
        {
            FaderState = FaderState.Turning_Opaque;

        }
    }
}
