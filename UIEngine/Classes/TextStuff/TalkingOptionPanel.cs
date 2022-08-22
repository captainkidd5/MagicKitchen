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
        private NewTextBuilder _titleTextBuilder;

        private NewTextBuilder _dialogueTextBuilder;

        private Vector2 _dialogueTextPosition;

        private Sprite BackgroundSprite;

        private Vector2 _scale = new Vector2(.5f, .5f);
        public TalkingOptionPanel(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }


        public void LoadNewOption( DialogueOption newOption)
        {
            DialogueOption = newOption;
            LoadContent();
        }
        public override void LoadContent()
        {
            BackgroundSprite = SpriteFactory.CreateUISprite(Position, _sourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),scale: _scale);
            TotalBounds = RectangleHelper.RectFromPosition(Position, (int)(_sourceRectangle.Width * _scale.X), (int)(_sourceRectangle.Height * _scale.Y));

            base.LoadContent();
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);

            _titleTextBuilder = new NewTextBuilder(DialogueOption.Title,Position, null,null, GetLayeringDepth(UILayeringDepths.Medium), scale: 1.2f);
            _dialogueTextPosition = new Vector2(Position.X, Position.Y + _titleTextBuilder.Height * 2);

            _dialogueTextBuilder = new NewTextBuilder(DialogueOption.DialogueText, Position, null, null, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Medium));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            BackgroundSprite.Update(gameTime, Position);
            _titleTextBuilder.Update(gameTime, Position, TotalBounds.Width);
            _dialogueTextBuilder.Update(gameTime, _dialogueTextPosition, TotalBounds.Width);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            BackgroundSprite.Draw(spriteBatch);
            _titleTextBuilder.Draw(spriteBatch);
            _dialogueTextBuilder.Draw(spriteBatch);

        }
    }
}
