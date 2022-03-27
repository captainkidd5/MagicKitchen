﻿using Globals.Classes.Helpers;
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

namespace UIEngine.Classes.ButtonStuff
{
    internal class ConfirmationWindow : InterfaceSection
    {

        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 240, 128);
        private NineSliceSprite _backGroundSprite;

        private Action _confirmAction;
        private Action _cancelAction;
        private Button _confirmButton;
        private Button _cancelButton;

        private string _confirmationText;
        private Text _text;
        private Vector2 _textPosition;
        /// <summary>
        /// Default cancel action is to just close this window
        /// </summary>
        public ConfirmationWindow(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth, 
            Action confirmAction, Action? cancelAction = null, bool suppressParentSection = true, string confirmationText = null) : base(interfaceSection, graphicsDevice, content, position, layerDepth, suppressParentSection)
        {
            _confirmAction = confirmAction;
            _cancelAction = cancelAction ?? new Action(() => Deactivate());
            _confirmationText = confirmationText ?? "Are you sure?";
            
        }
        public override void LoadContent()
        {
            Position = RectangleHelper.CenterRectangleOnScreen(_backGroundSpriteDimensions);
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(Position, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low));

            Vector2 confirmButtonPos = RectangleHelper.CenterRectangleInRectangle(ButtonFactory.s_greenCheckRectangle, _backGroundSprite.HitBox);
            confirmButtonPos = new Vector2(confirmButtonPos.X + 64, confirmButtonPos.Y);
            _confirmButton = new Button(this, graphics, content, confirmButtonPos, GetLayeringDepth(UILayeringDepths.Medium), ButtonFactory.s_greenCheckRectangle, null, UI.ButtonTexture, null,
                new Action(() =>
                {
                    _confirmAction();
                    UI.RemoveCriticalSection(this);
                }),
                
                scale: 1f); ;

            Vector2 cancelButtonPos = RectangleHelper.CenterRectangleInRectangle(ButtonFactory.s_redExRectangle, _backGroundSprite.HitBox);
            cancelButtonPos = new Vector2(cancelButtonPos.X - 64, cancelButtonPos.Y);
            _cancelButton = new Button(this, graphics, content, cancelButtonPos, GetLayeringDepth(UILayeringDepths.Medium), ButtonFactory.s_redExRectangle, null, UI.ButtonTexture, null, _cancelAction, scale: 1f);

            _text = TextFactory.CreateUIText(_confirmationText, GetLayeringDepth(UILayeringDepths.High));
            _textPosition = Text.CenterInRectangle(TotalBounds, _text);

            IsActive = false;
            TotalBounds = _backGroundSprite.HitBox;
            base.LoadContent();

        }

        public override void Deactivate()
        {
            base.Deactivate();
            UI.RemoveCriticalSection(this);
        }
        public override void Activate()
        {
            base.Activate();
            UI.AddCriticalSection(this);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                Vector2 textPos = Text.CenterInRectangle(_backGroundSprite.HitBox, _text);
                _text.Update(gameTime, new Vector2(textPos.X, textPos.Y - _backGroundSprite.HitBox.Height / 3));

            }


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
                _text.Draw(spriteBatch, true);

            }
        }

       
    }
}