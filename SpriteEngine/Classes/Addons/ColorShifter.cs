using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.Addons
{
    internal enum ColorState
    {
        Normal = 1,
        Intensifying = 2,
        FullyAltered = 3,
        Reducing = 4,
    }
    internal class ColorShifter : ISpriteAddon
    {
        private const float ColorMaxVal = 255;
        private const float ColorMinVal = 0;
        private readonly float _speed = 2f;
        private ColorState _hueState = ColorState.Normal;
        private Color _originalColor;
        private Color _alternativeColor;

        internal bool IsNormal => _hueState == ColorState.Normal;
        internal bool FullyAltered => _hueState == ColorState.FullyAltered;

        internal bool FlaggedForRemovalUponFinish;

        /// <summary>
        /// Constructor for hue shifting
        /// </summary>
        /// <param name="originalColor">Normal, baseline color</param>
        /// <param name="targetColor">Color to change to</param>
        public ColorShifter(Color originalColor, float? speed, Color? targetColor)
        {
            _originalColor = originalColor;
            _alternativeColor = targetColor ?? new Color(255,255,255,255);
            _speed = speed ?? .5f;

        }

        /// <summary>
        /// Constructor for fader (automatically decreasing opacity only)
        /// </summary>
        /// <param name="originalColor"></param>
        /// <param name="transparentColor"></param>
        public ColorShifter(Color originalColor, float? speed, float? maxOpac)
        {
            float opac = maxOpac ?? 51;
            maxOpac = maxOpac ?? 51;
            _speed = speed ?? .5f;
            _originalColor = originalColor;
            _alternativeColor = new Color((int)opac, (int)opac, (int)opac, (int)opac);
        }
        public void Update(GameTime gameTime, BaseSprite sprite)
        {

            if(_hueState == ColorState.Intensifying)
                IntensifyEffect(gameTime, sprite);
            else if(_hueState == ColorState.Reducing)
                ReduceEffect(gameTime, sprite);
        }

        private void IntensifyEffect(GameTime gameTime, BaseSprite sprite)
        {
            UpdateColors(gameTime, sprite, _alternativeColor);

            if (sprite.PrimaryColor == _alternativeColor)
                _hueState = ColorState.FullyAltered;
        }

        /// <summary>
        /// Checks rgba values to see if current sprite color matches them. If not, it will increase or decrease the values
        /// each frame so they are closer to the target color.
        /// </summary>
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

        /// <summary>
        /// Moves current color rgba val closer to target rgba val
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="col">Current color r, g, b, or a value</param>
        /// <param name="target">Target color r, g, b, or a value</param>
        /// <returns></returns>
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
                _hueState = ColorState.Normal;
        }
        internal void TriggerIntensifyEffect()
        {
            _hueState = ColorState.Intensifying;
        }

        internal void TriggerReduceEffect()
        {
            _hueState = ColorState.Reducing;


        }
    }
}
