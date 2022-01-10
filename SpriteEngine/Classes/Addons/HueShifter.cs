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
        private readonly float _speed = .5f;
        private HueState _hueState = HueState.Normal;
        private Color _originalColor;
        private Color _targetColor;

        internal bool IsNormal => _hueState == HueState.Normal;
        internal bool IsSaturated => _hueState == HueState.Saturated;

        internal bool FlaggedForRemovalUponFinish;

        public HueShifter(Color originalColor, Color targetColor)
        {
            _originalColor = originalColor;
            _targetColor = targetColor;
        }

        /// <summary>
        /// Constructor for fader (automatically decreasing opacity only)
        /// </summary>
        /// <param name="originalColor"></param>
        /// <param name="transparentColor"></param>
        public HueShifter(Color originalColor,  float? maxOpac)
        {
            float opac = maxOpac ?? 51;
            maxOpac = maxOpac ?? 51;
            _originalColor = originalColor;
            _targetColor = new Color((int)opac, (int)opac, (int)opac, (int)opac);
        }
        public void Update(GameTime gameTime, BaseSprite sprite)
        {
            if(_hueState == HueState.Intensifying)
                IntensifyEffect(gameTime, sprite);
            else if(_hueState == HueState.Reducing)
                ReduceEffect(gameTime, sprite);
        }

        private void IntensifyEffect(GameTime gameTime, BaseSprite sprite)
        {
            UpdateColors(gameTime, sprite, _targetColor);

            if (sprite.PrimaryColor == _targetColor)
                _hueState = HueState.Saturated;
        }

        private void UpdateColors(GameTime gameTime, BaseSprite sprite, Color target)
        {
            Color sColor = sprite.PrimaryColor;

            if (sColor.R != target.R)
            {
                sColor.R = CheckColorValue(gameTime, sColor.R, target.R);
                sprite.UpdateColor(sColor.R, null, null, null);

            }

            if (sColor.G != target.G)
            {
                sColor.G = CheckColorValue(gameTime, sColor.G, target.G);
                sprite.UpdateColor(null, sColor.G, null, null);

            }

            if (sColor.B != target.B)
            {
                sColor.B = CheckColorValue(gameTime, sColor.B, target.B);
                sprite.UpdateColor(null, null, sColor.B, null);

            }

            if (sColor.A != target.A)
            {
                sColor.A = CheckColorValue(gameTime, sColor.A, target.A);
                sprite.UpdateColor(null, null, null, sColor.A);

            }
        }

        private byte CheckColorValue(GameTime gameTime, byte col, byte target)
        {

                if (col < target)
                    return IncreaseColorValue(gameTime, col, target);
                else if (col > target)
                    return DecreaseColorValue(gameTime, col, target);
            return col;
        }
        private byte IncreaseColorValue(GameTime gameTime, float val, float maxVal = ColorMaxVal)
        {
            val += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _speed;
            if(val > maxVal)
                val = maxVal;
            return (byte)val;
        }

        private byte DecreaseColorValue(GameTime gameTime, float val, float minVal = ColorMinVal)
        {
            val -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _speed;
            if (val < minVal)
                val = minVal;
            return (byte)val;
        }

        private void ReduceEffect(GameTime gameTime, BaseSprite sprite)
        {
            UpdateColors(gameTime, sprite, _originalColor);

            if (sprite.PrimaryColor == _originalColor)
                _hueState = HueState.Normal;
        }
        internal void TriggerIntensifyEffect()
        {
            _hueState = HueState.Intensifying;
        }

        internal void TriggerReduceEffect()
        {
            _hueState = HueState.Reducing;


        }
    }
}
