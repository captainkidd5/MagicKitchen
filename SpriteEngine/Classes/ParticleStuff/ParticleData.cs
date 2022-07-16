using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff
{
    public struct ParticleData
    {
        public Texture2D texture = ParticleManager.ParticleAtlas;
        public float lifespan = 2f;
        public Color colorStart = Color.Yellow;
        public Color colorEnd = Color.Red;
        public float opacityStart = 1f;
        public float opacityEnd = 0f;
        public float sizeStart = 32f;
        public float sizeEnd = 4f;
        public float speed = 100f;
        public float angle = 0f;

        public int YVelocityMin = 25;
        public int YVelocityMax = 50;
        public int HeightCutOff = 2;
        public float Gravity = -500f;
        public ParticleData()
        {
        }

        public void FirePreset()
        {
            lifespan = 2f;
            colorStart = Color.Black;
            colorEnd = Color.Black;
            opacityStart = 1f;
            opacityEnd = 0f;
            sizeStart = 4f;
            sizeEnd = 32f;
            speed = 200f;
            angle = 0f;
        }
    }
}
