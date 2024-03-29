﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff
{
    public enum ParticleTextureType : byte
    {
        None = 0,
        Soft = 1,
        Hard = 2
    }
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

        public bool EnableGravity = false;
        public int YVelocityMin = 50;
        public int YVelocityMax = 100;
        public int HeightCutOffMin = -25;
        public int HeightCutOffMax = 25;
        public float Gravity = -500f;

        public ParticleTextureType ParticleTextureType = ParticleTextureType.Hard;
        public Rectangle SourceRectangle;
        public ParticleData()
        {
            SourceRectangle = SourceRectangleFromParticleTextureType(ParticleTextureType);
        }

        public void TextPreset()
        {
            lifespan = .5f;
            colorStart = Color.Yellow;
            colorEnd = Color.Orange;
            opacityStart = 1f;
            opacityEnd = 0f;
            sizeStart = 4f;
            sizeEnd = 12;
            speed = 50f;
            EnableGravity = false;
            
            angle = 0f;
        }
        public void FirePreset()
        {
            lifespan = .25f;
            colorStart = Color.Black;
            colorEnd = Color.Black;
            opacityStart = 1f;
            opacityEnd = 0f;
            sizeStart = 4f;
            sizeEnd = 12;
            speed = 200f;
            angle = 0f;
            EnableGravity = true;
        }
        public void SmokePreset()
        {
            lifespan = .25f;
            colorStart = Color.White;
            colorEnd = Color.Gray;
            opacityStart = 1f;
            opacityEnd = 0f;
            sizeStart = 4f;
            sizeEnd = 12;
            speed = 200f;
            angle = 0f;
            EnableGravity = false;
        }
        private static Rectangle SourceRectangleFromParticleTextureType(ParticleTextureType p)
        {
            switch (p)
            {
                case ParticleTextureType.None:
                    break;
                case ParticleTextureType.Soft:
                    return new Rectangle(0, 0, 32, 32);
                case ParticleTextureType.Hard:
                    return new Rectangle(32, 0, 32, 32);
            }
            throw new Exception($"Invalid particle type");
        }
    }
}