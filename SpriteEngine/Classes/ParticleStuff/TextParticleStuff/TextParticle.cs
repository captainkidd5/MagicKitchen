using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;

namespace SpriteEngine.Classes.ParticleStuff.TextParticleStuff
{
    public class TextParticle : Particle
    {
        private Text _text;
        public TextParticle(string text, Vector2 pos, ParticleData data) : base(pos, data)
        {
            _text = TextFactory.CreateUIText(text, SpriteUtility.GetYAxisLayerDepth(Position, data.SourceRectangle),.5f);
            data.colorStart = Color.Red;
            data.colorEnd = Color.Orange;
        }
        public override void Update(GameTime gameTime, Vector2? position = null)
        {
            base.Update(gameTime, position);
            _text.Color = Color;

            _text.Update(Position);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
           // base.Draw(spriteBatch);
            _text.Draw(spriteBatch, SpriteUtility.GetYAxisLayerDepth(Position, _text.Rectangle));
        }
    }
}
