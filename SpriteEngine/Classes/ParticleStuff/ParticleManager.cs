﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.ParticleStuff.TextParticleStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff
{
    public enum EmitterType
    {
        None = 0,
        Fire = 1,
        Text = 2,
        Smoke =3,
    }
    public static class ParticleManager
    {
        private static readonly List<Particle> _particles = new();
        private static readonly List<ParticleEmitter> _particleEmitters = new();

        internal static Texture2D ParticleAtlas;

        public static void Load(ContentManager content)
        {
            ParticleAtlas = content.Load<Texture2D>("Effects/Particles/ParticleAtlas");
        }
        public static void AddParticle(Particle p)
        {
            _particles.Add(p);
        }

        public static void AddParticleEmitter(IEmitter iEmitter, EmitterType emitterType, string text = null,
            Color? colorstart = null, Color? colorEnd = null, float? customLayer = null)
        {
            ParticleData data = new ParticleData();
            ParticleEmitterData emitterData = new ParticleEmitterData();
            ParticleEmitter emitter;
            switch (emitterType)
            {
                case EmitterType.None:
                    break;
                case EmitterType.Fire:
                    data.FirePreset();
                    break;
                case EmitterType.Text:
                    data.TextPreset();
                    data.colorStart = colorstart ?? data.colorStart;
                    data.colorEnd = colorEnd ?? data.colorEnd;

                    break;
                case EmitterType.Smoke:
                    data.SmokePreset();
                    emitterData.AngleVariance = 4f;
                    emitterData.EmitCount = 20;
                    emitterData.TotalLifeSpan = 2f;
                    break;
            }
            emitterData.ParticleData = data;
            if(text == null)
            emitter = new ParticleEmitter(iEmitter, emitterData, customLayer);
            else
                emitter = new TextParticleEmitter(text,iEmitter, emitterData);


            _particleEmitters.Add(emitter);

        }
        public static void AddParticleEmitter(ParticleEmitter e)
        {
            _particleEmitters.Add(e);
        }

        private static void UpdateParticles(GameTime gameTime)
        {
            foreach (var particle in _particles)
            {
                particle.Update(gameTime);
            }

            _particles.RemoveAll(p => p.isFinished);
        }

        private static void UpdateEmitters(GameTime gameTime)
        {
            foreach (var emitter in _particleEmitters)
            {
                emitter.Update(gameTime);
            }
            _particleEmitters.RemoveAll(e => e.IsFinished);

        }

        public static void Update(GameTime gameTime)
        {
            UpdateParticles(gameTime);
            UpdateEmitters(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in _particles)
            {
                particle.Draw(spriteBatch);
            }
        }

    }
}