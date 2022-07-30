using Globals.Classes;
using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.ParticleStuff.HomingParticleStuff;
using SpriteEngine.Classes.Presets;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace PhysicsEngine.Classes
{
    public class LightCollidable : Collidable
    {
        public static readonly float S_RechargeRate = 1f;
        public float TimeSinceFull { get; set; }
        private float _restPeriod = 1f;
        private SimpleTimer _restPeriodTimer;
        private bool _mayStartRechargWaitTimer = true;
        private bool _recharging;

        private LightSprite _lightSprite;
        public bool RestoresLumens { get; private set; }
        public bool ImmuneToDrain { get; }
        public byte CurrentLumens { get; private set; }
        public byte MaxLumens { get; set; }

        public void SetCurrentLumens(Byte val) => CurrentLumens = val;

        public bool HasCharge => CurrentLumens > 0;

        private PersonalEmitter _personaEmitter;
        public event ParticleReachedDestination PReached;

        public LightCollidable(Vector2 position, Vector2 offSet, LightType lightType, bool restoresLumens, float scale, bool immuneToDrain)
        {
            RestoresLumens = restoresLumens;
            ImmuneToDrain = immuneToDrain;
            MaxLumens = (byte)(scale * 100);
            CurrentLumens = MaxLumens;
            _lightSprite = SpriteFactory.CreateLight(Position, offSet, lightType, scale);
            Vector2 newScale = new Vector2(CurrentLumens * .1f, CurrentLumens * .1f);
            ResizeLight(newScale);

        }

        public void ResizeLight(Vector2 newScale)
        {
            _lightSprite.Sprite.SwapScale(newScale);
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);

            if (MainHullBody == null)
                MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, _lightSprite.Sprite.Width * _lightSprite.Sprite.Scale.X / 4, new List<Category>() { (Category)PhysCat.LightSource },
                    new List<Category>() { (Category)PhysCat.PlayerBigSensor }, OnCollides, OnSeparates, ignoreGravity: true, blocksLight: true, userData: this);
            _restPeriodTimer = new SimpleTimer(_restPeriod);

            _personaEmitter = new PersonalEmitter();
            _personaEmitter.PReached += OnParticleReachedDestination;
        }

        public int SiphonLumens(byte amt)
        {

            if ((int)CurrentLumens - (int)amt < 0)
                return 0;
            if (!ImmuneToDrain)
            {
                CurrentLumens -= amt;
                ResizeLight(new Vector2(CurrentLumens * .1f, CurrentLumens * .1f));
            }

            _personaEmitter.AddParticle(CenteredPosition);

            return amt;

        }


        public void OnParticleReachedDestination()
        {
            
            PReached?.Invoke();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _lightSprite.Update(gameTime, Position);
            if (CurrentLumens == MaxLumens)
            {
                TimeSinceFull = -1;
            }
            if (CurrentLumens != MaxLumens)
            {
                if (_mayStartRechargWaitTimer)
                {
                    if (_restPeriodTimer.Run(gameTime))
                    {
                        if (TimeSinceFull == -1)
                            TimeSinceFull = Clock.TotalTime;
                        _recharging = true;

                    }
                }

            }

            if (_recharging)
            {
                if (_restPeriodTimer.Run(gameTime))
                {
                    if (CurrentLumens < MaxLumens)
                        CurrentLumens++;
                       else
                        _recharging = false;
                    ResizeLight(new Vector2(CurrentLumens * .1f, CurrentLumens * .1f));


                }

            }
            _personaEmitter.Update(gameTime, Shared.PlayerPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _lightSprite.Draw(spriteBatch);
        }

        public void DrawEmitter(SpriteBatch spriteBatch)
        {
            _personaEmitter.Draw(spriteBatch);

        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayerBigSensor))
            {
                _mayStartRechargWaitTimer = false;
                _recharging = false;

            }
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayerBigSensor))
                _mayStartRechargWaitTimer = true;

            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
