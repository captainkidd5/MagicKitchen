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
        private readonly float _speed = .005f;
        private HueState _hueState = HueState.Normal;
        private Color _originalColor;
        private Color _targetColor;


        public HueShifter(Color originalColor, Color targetColor)
        {
            _originalColor = originalColor;
            _targetColor = targetColor;
        }
        public void Update(GameTime gameTime, BaseSprite sprite)
        {
            throw new NotImplementedException();
        }

        private void Intensify(GameTime gameTime, BaseSprite sprite)
        {
            if (sprite.PrimaryColor.R < _targetColor.R)
                sprite.PrimaryColor.R++;
            if (sprite.PrimaryColor.G < _targetColor.G)
                sprite.PrimaryColor.G++;
            if (sprite.PrimaryColor.B < _targetColor.B)
                sprite.PrimaryColor.B++;

            if (sprite.PrimaryColor == _targetColor)
                _hueState = HueState.Saturated;
        }

        private void REduce(GameTime gameTime, BaseSprite sprite)
        {
            if (_originalColor.R > _targetColor.R)
                _originalColor.R--;
            if (_originalColor.G > _targetColor.G)
                _originalColor.G--;
            if (_originalColor.B > _targetColor.B)
                _originalColor.B--;

            if (_originalColor == _targetColor)
                _hueState = HueState.Saturated;
        }
    }
}
