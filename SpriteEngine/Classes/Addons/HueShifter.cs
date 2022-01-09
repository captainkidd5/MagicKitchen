using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.Addons
{
    internal enum HueState
    {
        Normal = 1,
        Intensifying = 2,
        Saturated = 3,
        Reducing = 4,
    }
    internal class HueShifter : ISpriteAddon
    {
        private const float ColorMaxVal = 255;
        private const float ColorMinVal = 0;
        private readonly float _speed = .005f;
        private HueState _hueState = HueState.Normal;
        private Color _originalColor;
        private Color _targetColor;

        internal bool IsNormal => _hueState == HueState.Normal;
        internal bool IsSaturated => _hueState == HueState.Saturated;


        public HueShifter(Color originalColor, Color targetColor)
        {
            _originalColor = originalColor;
            _targetColor = targetColor;
        }
        public void Update(GameTime gameTime, BaseSprite sprite)
        {
            if(_hueState == HueState.Intensifying)
                Intensify(gameTime, sprite);
            else if(_hueState == HueState.Reducing)
                Reduce(gameTime, sprite);
        }

        private void Intensify(GameTime gameTime, BaseSprite sprite)
        {
            Color sColor = sprite.PrimaryColor;

            if (sColor.R < _targetColor.R)
                sprite.UpdateColor(IncreaseColorValue(gameTime, sColor.R), null,null,null);

            if (sColor.G < _targetColor.G)
                sprite.UpdateColor( null, IncreaseColorValue(gameTime, sColor.G) , null, null);

            if (sColor.B < _targetColor.B)
                sprite.UpdateColor(null, null, IncreaseColorValue(gameTime, sColor.B), null);

            if (sprite.PrimaryColor == _targetColor)
                _hueState = HueState.Saturated;
        }

        private float IncreaseColorValue(GameTime gameTime, float val, float maxVal = ColorMaxVal)
        {
            val += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _speed;
            if(val > maxVal)
                val = maxVal;
            return val;
        }

        private float DecreaseColorValue(GameTime gameTime, float val, float minVal = ColorMinVal)
        {
            val -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _speed;
            if (val < minVal)
                val = minVal;
            return val;
        }

        private void Reduce(GameTime gameTime, BaseSprite sprite)
        {
            Color sColor = sprite.PrimaryColor;

            if (sColor.R > _originalColor.R)
                sprite.UpdateColor(DecreaseColorValue(gameTime, sColor.R), null, null, null);

            if (sColor.G > _originalColor.G)
                sprite.UpdateColor(null, DecreaseColorValue(gameTime, sColor.G), null, null);

            if (sColor.B > _originalColor.B)
                sprite.UpdateColor(null, null, DecreaseColorValue(gameTime, sColor.B), null);

            if (sprite.PrimaryColor == _originalColor)
                _hueState = HueState.Normal;
        }
        internal void TriggerIntensify()
        {
            _hueState = HueState.Intensifying;
        }

        internal void TriggerReduce()
        {
            _hueState = HueState.Reducing;


        }
    }
}
