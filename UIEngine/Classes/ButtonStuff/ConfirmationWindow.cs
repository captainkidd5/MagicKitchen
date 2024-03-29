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
    internal class ConfirmationWindow : MenuSection
    {

        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 512, 256);
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
            Action confirmAction, Action? cancelAction = null, string confirmationText = null) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _confirmAction = confirmAction;
            _cancelAction = cancelAction ?? new Action(() => Deactivate());
            _confirmationText = confirmationText ?? "Are you sure?";
            Selectables = new InterfaceSection[2,2];
            
        }
        public override void LoadContent()
        {
            Position = RectangleHelper.CenterRectangleOnScreen(_backGroundSpriteDimensions);
            _backGroundSprite = SpriteFactory.CreateNineSliceSprite(Position, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height,
                UI.ButtonTexture,.9f);

            Vector2 confirmButtonPos = RectangleHelper.CenterRectangleInRectangle(ButtonFactory.s_greenCheckRectangle, _backGroundSprite.HitBox);
            confirmButtonPos = new Vector2(confirmButtonPos.X + 64, confirmButtonPos.Y);
            _confirmButton = UI.ButtonFactory.CreateButton(this,confirmButtonPos,.91f, ButtonFactory.s_greenCheckRectangle,
                new Action(() =>
                {
                    _confirmAction();
                    UI.RemoveCriticalSection(this);
                }));
            AddSectionToGrid(_confirmButton,0, 1);
            Vector2 cancelButtonPos = RectangleHelper.CenterRectangleInRectangle(ButtonFactory.s_redExRectangle, _backGroundSprite.HitBox);
            cancelButtonPos = new Vector2(cancelButtonPos.X - 64, cancelButtonPos.Y);
            _cancelButton = UI.ButtonFactory.CreateButton(this, cancelButtonPos,.91f, ButtonFactory.s_redExRectangle, _cancelAction);
            AddSectionToGrid(_cancelButton, 0, 0);


            _text = TextFactory.CreateUIText(_confirmationText, _textPosition, null, _backGroundSpriteDimensions.Width, .91f, scale: 2f);
            _textPosition = _text.CenterInRectangle(TotalBounds, 1f);

            TotalBounds = _backGroundSprite.HitBox;


            Deactivate();
            base.LoadContent();

        }

        public override void Deactivate()
        {
            if(IsActive)
            UI.RemoveCriticalSection(this);
            
            base.Deactivate();
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
                Vector2 textPos = _text.CenterInRectangle(_backGroundSprite.HitBox, 1f);
                _text.Update(new Vector2(textPos.X, textPos.Y - _backGroundSprite.Height / 3));

            }


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
                _text.Draw(spriteBatch);

            }
        }

       
    }
}
