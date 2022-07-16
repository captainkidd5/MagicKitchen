using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff
{
    public class Particle
    {

        private readonly ParticleData _data;
        protected Vector2 _position;
        private float _lifespanLeft;
        private float _lifespanAmount;
        private Color _color;
        private float _opacity;
        public bool isFinished = false;
        private float _scale;
        private Vector2 _origin;
        private Vector2 _direction;

        private float _velocity;
        private float _height;
        private int _heightCutOff;
        public Particle(Vector2 pos, ParticleData data)
        {
            _data = data;
            _lifespanLeft = data.lifespan;
            _lifespanAmount = 1f;
            _position = pos;
            _color = data.colorStart;
            _opacity = data.opacityStart;
            _origin = new(_data.SourceRectangle.Width / 2, _data.SourceRectangle.Height / 2);

            if (data.speed != 0)
            {
                _data.angle = MathHelper.ToRadians(_data.angle);
                _direction = new Vector2((float)Math.Sin(_data.angle), -(float)Math.Cos(_data.angle));
            }
            else
            {
                _direction = Vector2.Zero;
            }

            _velocity = Settings.Random.Next(_data.YVelocityMin, _data.YVelocityMax);

           _heightCutOff = Settings.Random.Next(_data.HeightCutOffMin, _data.HeightCutOffMax) + (int)pos.Y;

        }

        public virtual void Update(GameTime gameTime)
        {
            _lifespanLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_lifespanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.lifespan, 0, 1);
            _color = Color.Lerp(_data.colorEnd, _data.colorStart, _lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
            _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;

            _velocity += _data.Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _height -= _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;


            float newHeight = _position.Y;
            _position += _direction * _data.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_position.Y < _heightCutOff)
                newHeight = _position.Y  + _height;
           
            else
                isFinished = true;

            _position = new Vector2(_position.X, newHeight);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_data.texture, _position, _data.SourceRectangle, _color * _opacity, 0f, _origin, _scale, SpriteEffects.None, 1f);
        }
    }

}
