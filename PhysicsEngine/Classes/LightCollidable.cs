using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace PhysicsEngine.Classes
{
    public class LightCollidable : Collidable
    {
        private LightSprite _lightSprite;

        public bool RestoresLumens { get; private set; }
        public byte CurrentLumens { get; private set; }
        public LightCollidable(Vector2 position, Vector2 offSet, LightType lightType,bool restoresLumens, float scale)
        {
            RestoresLumens = restoresLumens;
            CurrentLumens = (byte)(scale * 100);
            _lightSprite = SpriteFactory.CreateLight(Position, offSet, lightType, scale);
            ResizeLight(new Vector2(CurrentLumens * .01f, CurrentLumens * .01f));

        }

        public void ResizeLight(Vector2 newScale)
        {
            _lightSprite.Sprite.SwapScale(newScale);
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);

            if (MainHullBody == null)
                MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, _lightSprite.Sprite.Width * _lightSprite.Sprite.Scale.X /4, new List<Category>() { (Category)PhysCat.LightSource },
                    new List<Category>() { (Category)PhysCat.PlayerBigSensor }, OnCollides, OnSeparates, ignoreGravity: true, blocksLight: true, userData: this);
        }

        public int SiphonLumens(byte amt)
        {
            if ((int)CurrentLumens - (int)amt < 0)
                return 0;
            CurrentLumens -= amt;
            ResizeLight(new Vector2(CurrentLumens * .1f, CurrentLumens * .1f));
            return amt;

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _lightSprite.Update(gameTime, Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _lightSprite.Draw(spriteBatch);
        }
       
    }
}
