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

        private Text _dialogueText;

        private Sprite BackgroundSprite;

        private Vector2 _scale = new Vector2(2f, 2f);
        public TalkingOptionPanel(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }


        public void LoadNewOption( DialogueOption newOption)
        {
            DialogueOption = newOption;
            _dialogueText = TextFactory.CreateUIText(DialogueOption.DialogueText,GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Medium));
            LoadContent();
        }
        public override void LoadContent()
        {
            BackgroundSprite = SpriteFactory.CreateUISprite(Position, _sourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),scale: _scale);
            TotalBounds = RectangleHelper.RectFromPosition(Position, _sourceRectangle.Width * (int)_scale.X, _sourceRectangle.Height * (int)_scale.Y);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            BackgroundSprite.Update(gameTime, Position);
            _dialogueText.Update(gameTime, Position);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            BackgroundSprite.Draw(spriteBatch);
            _dialogueText.Draw(spriteBatch, true);
        }
    }
}
