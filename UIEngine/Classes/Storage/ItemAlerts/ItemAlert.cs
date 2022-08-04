using DataModels.ItemStuff;
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
    /// <summary>
    /// That little box that appears which shows which items have just been added to the player inventory.
    /// Alerts come from <see cref="PlayerInventoryDisplay.OnItemAddedToInventory(Item, int)"/>
    /// This also works nicely with crafted items being shown on craft
    /// </summary>
    internal class ItemAlert : InterfaceSection
    {
        private static Rectangle _backgroundSourceRectangle = new Rectangle(176, 0, 96, 32);
        protected virtual float TTl { get; set; } = 2f;
        public ItemData ItemData { get; set; }
        public byte Count { get; set; }
        private NineSliceTextButton _background;
        private Text _text;
        private SimpleTimer _simpleTimer;

        protected Vector2 BackgroundOffSet { get; private set; } = new Vector2(64, 16);
        private Vector2 _textOffSet = new Vector2(16, 16);


        private NineSliceButton _button;

        public ItemAlert(ItemData itemData, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            ItemData = itemData;
        }

        public virtual void Increment(int amt)
        {
            _simpleTimer.ResetToZero();
            _simpleTimer.SetNewTargetTime(TTl);
            Count += (byte)amt;
            _text.SetFullString($"{ItemData.Name} +({Count})");
            _background.Color = Color.White;
            ChildSections.Remove(_background);
            _background = new NineSliceTextButton(this, graphics, content, Position + BackgroundOffSet,
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low), new List<Text>() { _text }, null);
        }
        protected virtual Text SetInitialText()
        {
            return TextFactory.CreateUIText($"", GetLayeringDepth(UILayeringDepths.Medium));

        }
        public override void LoadContent()
        {
           // base.LoadContent();
            _simpleTimer = new SimpleTimer(TTl, false);
            _text = SetInitialText();
            _background = new NineSliceTextButton(this, graphics, content, Position + BackgroundOffSet,
               GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low),new List<Text>() { _text }, null);

            //SpriteFactory.CreateUISprite(Position + _backgroundOffSet, _backgroundSourceRectangle, UI.ButtonTexture,
            //    GetLayeringDepth(UILayeringDepths.Low), scale:new Vector2(2f,2f));
            _button = new NineSliceButton(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), null, null, null, null, hoverTransparency: true);

            _button.SwapForeGroundSprite(SpriteFactory.CreateUISprite(Position,
            Item.GetItemSourceRectangle(ItemData.Id), ItemFactory.ItemSpriteSheet,
            UI.IncrementLD(GetLayeringDepth(UILayeringDepths.High), true), Color.White, Vector2.Zero, new Vector2(3f,3f)));
            _button.SetForegroundSpriteOffSet(new Vector2(8, 8));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           // _text.Update(gameTime, Position + _backgroundOffSet + _textOffSet);
          //  _button.Update(gameTime);
            if (_simpleTimer.Run(gameTime))
            {
                _button.FadeOut();
                _background.FadeOut();
            }

            if (_button.IsTransparent || _background.IsTransparent)
                FlaggedForRemoval = true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
           // _background.Draw(spriteBatch);
           // _text.Draw(spriteBatch, true);
          //  _button.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
