using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace Globals.Classes
{
    public class PulserTimer
    {
        private float _speed;
        private float _minVal;
        private float _maxVal;
        private float _value;
        private Direction _direction;
        public PulserTimer(float minVal, float maxVal, float speed)
        {
            _minVal = minVal;
            _maxVal = maxVal;
            _speed = speed;
            _value = _minVal;
        }
        private void Increase(GameTime gameTime)
        {
            _value += (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;
            if(_value >= _maxVal)
            {
                _direction = Direction.Down;
            }
        }
        private void Decrease(GameTime gameTime)
        {
            _value -= (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;
            if (_value <= _minVal)
            {
                _direction = Direction.Up;
            }
        }
        public float Update(GameTime gameTime)
        {
            if(_direction == Direction.Up)
                Increase(gameTime);
            else
                Decrease(gameTime);

            return _value;
        }
    }
}
