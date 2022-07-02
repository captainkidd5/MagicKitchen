using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.Storage.ItemAlerts
{
    internal class ItemAlert : InterfaceSection
    {
        private static float _TTL = 2f;
        public int ItemId { get; set; }
        public byte Count { get; set; }
        private NineSliceSprite _nineSlice;
        private SimpleTimer _simpleTimer;
        public ItemAlert(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public void Increment(int amt)
        {
            _simpleTimer.SetNewTargetTime(_TTL);
            Count += (byte)amt;
        }
        public override void LoadContent()
        {
            base.LoadContent();
            _simpleTimer = new SimpleTimer(_TTL, false);
            _nineSlice = SpriteFactory.CreateNineSliceTextSprite(
                Position, TextFactory.CreateUIText("test", GetLayeringDepth(UILayeringDepths.Medium)),UI.ButtonTexture,GetLayeringDepth(UILayeringDepths.Medium));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _nineSlice.Update(gameTime, Position);
            if (_simpleTimer.Run(gameTime))
            {
                FlaggedForRemoval = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _nineSlice.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
