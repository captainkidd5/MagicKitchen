using DataModels.DialogueStuff;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;

namespace UIEngine.Classes.TextStuff
{
    internal class TalkingOptionPanel : InterfaceSection
    {
        public DialogueOption DialogueOption { get; set; }

        private Rectangle _sourceRectangle = new Rectangle(64, 496, 272, 64);
        private Text _titleText;

        private Text _dialogueText;
        private Vector2 _dialogueTextPosition;

        private Sprite BackgroundSprite;

        private Vector2 _scale = new Vector2(2f, 2f);
        public TalkingOptionPanel(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }


        public void LoadNewOption( DialogueOption newOption)
        {
            DialogueOption = newOption;
            _titleText = TextFactory.CreateUIText(DialogueOption.Title, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Medium), 1.2f);
            _dialogueText = TextFactory.CreateUIText(DialogueOption.DialogueText,GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Medium));
            LoadContent();
        }
        public override void LoadContent()
        {
            BackgroundSprite = SpriteFactory.CreateUISprite(Position, _sourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),scale: _scale);
            TotalBounds = RectangleHelper.RectFromPosition(Position, _sourceRectangle.Width * (int)_scale.X, _sourceRectangle.Height * (int)_scale.Y);

            base.LoadContent();
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _dialogueTextPosition = new Vector2(Position.X, Position.Y + _titleText.TotalStringHeight);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            BackgroundSprite.Update(gameTime, Position);
            _titleText.Update(gameTime, Position);

            _dialogueText.Update(gameTime, _dialogueTextPosition);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            BackgroundSprite.Draw(spriteBatch);
            _titleText.Draw(spriteBatch, true);
            _dialogueText.Draw(spriteBatch, true);
        }
    }
}
