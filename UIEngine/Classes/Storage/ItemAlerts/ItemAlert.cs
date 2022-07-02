using Globals.Classes;
using ItemEngine.Classes;
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
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.Storage.ItemAlerts
{
    internal class ItemAlert : InterfaceSection
    {
        private static Rectangle _backgroundSourceRectangle = new Rectangle(176, 0, 96, 32);
        private static float _TTL = 2f;
        public Item Item { get; set; }
        public byte Count { get; set; }
        private Sprite _background;
        private Text _text;
        private SimpleTimer _simpleTimer;

        private Vector2 _backgroundOffSet = new Vector2(60, 0);
        private Vector2 _textOffSet = new Vector2(16, 16);


        private NineSliceButton _button;

        public ItemAlert(Item item, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Item = item;
        }

        public void Increment(int amt)
        {
            _simpleTimer.SetNewTargetTime(_TTL);
            Count += (byte)amt;
            _text.SetFullString($"{Item.Name} +{Count}");
            _background.ResetColors();
           
        }
        public override void LoadContent()
        {
           // base.LoadContent();
            _simpleTimer = new SimpleTimer(_TTL, false);
            _text = TextFactory.CreateUIText($"", GetLayeringDepth(UILayeringDepths.Medium));
            _background = SpriteFactory.CreateUISprite(Position + _backgroundOffSet, _backgroundSourceRectangle, UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Low), scale:new Vector2(2f,2f));
            _button = new NineSliceButton(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), null, null, null, null, hoverTransparency: true);

            _button.SwapForeGroundSprite(SpriteFactory.CreateUISprite(Position,
            Item.GetItemSourceRectangle(Item.Id), ItemFactory.ItemSpriteSheet,
            UI.IncrementLD(GetLayeringDepth(UILayeringDepths.High), true), Color.White, Vector2.Zero, new Vector2(3f,3f)));
            _button.SetForegroundSpriteOffSet(new Vector2(8, 8));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _background.Update(gameTime, Position + _backgroundOffSet);
            _text.Update(gameTime, Position + _backgroundOffSet + _textOffSet);
            if (_simpleTimer.Run(gameTime))
            {
                _background.AddFaderEffect(null, null, true);
                _button.FadeOut();
            }

            if (_background.IsTransparent)
                FlaggedForRemoval = true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _background.Draw(spriteBatch);
            _text.Draw(spriteBatch, true);

            base.Draw(spriteBatch);
        }
    }
}
