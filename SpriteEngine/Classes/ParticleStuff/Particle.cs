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
        protected Vector2 Position;
        protected float LifeSpanLeft;
        private float _lifespanAmount;
        protected Color Color { get; private set; }
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
            LifeSpanLeft = data.lifespan;
            _lifespanAmount = 1f;
            Position = pos;
            Color = data.colorStart;
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
            LifeSpanLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (LifeSpanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            _lifespanAmount = MathHelper.Clamp(LifeSpanLeft / _data.lifespan, 0, 1);
            Color = Color.Lerp(_data.colorEnd, _data.colorStart, _lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
            _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;

            _velocity += _data.Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _height -= _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;


            float newHeight = Position.Y;
            Position += _direction * _data.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_data.EnableGravity)
            {

                if (Position.Y < _heightCutOff)
                    newHeight = Position.Y + _height;

                else
                    isFinished = true;
                Position = new Vector2(Position.X, newHeight);

            }


        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_data.texture, Position, _data.SourceRectangle, Color * _opacity, 0f, _origin, _scale,
                SpriteEffects.None, SpriteUtility.GetYAxisLayerDepth(Position, _data.SourceRectangle));
        }
    }

}
