using Globals.Classes;
using Globals.Classes.Helpers;
using IOEngine.Classes;
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
using UIEngine.Classes.TextStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class CreateNewSaveMenu : InterfaceSection
    {
        private Rectangle _backGroundRectangle;

        private Text _createNewText;
        private Vector2 _createNewTextPosition;

        private Rectangle _nameWindowRectangle = new Rectangle(0, 0, 128, 32);
        private TypingBox _nameTypingBox;

        private Rectangle _createNewGameButtonRectangle = new Rectangle(0, 0, 32, 32);
        private NineSliceTextButton _createNewGameButton;
        private Action _createNewGameAction;
        public CreateNewSaveMenu(InterfaceSection interfaceSection,Rectangle backGroundRectangle, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _backGroundRectangle = backGroundRectangle;
        }

        public override void Load()
        {
            base.Load();


            _createNewText = TextFactory.CreateUIText("Create New Game", GetLayeringDepth(UILayeringDepths.High));
            _createNewTextPosition = Text.CenterInRectangle(_backGroundRectangle, _createNewText);
            _createNewTextPosition = new Vector2(_createNewTextPosition.X, _backGroundRectangle.Y + 4);


            Vector2 typingBoxPos = RectangleHelper.CenterRectangleInRectangle(_nameWindowRectangle,_backGroundRectangle);
            typingBoxPos = new Vector2(typingBoxPos.X, _createNewTextPosition.Y + _createNewText.TotalStringHeight * 2);
            _nameTypingBox = new TypingBox(this,graphics, content, typingBoxPos, GetLayeringDepth(UILayeringDepths.Low), _nameWindowRectangle.Width, _nameWindowRectangle.Height, null);

            _createNewGameAction = CreateNewSaveAction;
            _createNewGameButton = new NineSliceTextButton(this, graphics, content,
                RectangleHelper.PlaceBottomRightQuadrant(_backGroundRectangle, _createNewGameButtonRectangle),
                GetLayeringDepth(UILayeringDepths.Low), null, null, UI.ButtonTexture,new List<Text>()
                { TextFactory.CreateUIText("Go!", GetLayeringDepth(UILayeringDepths.Medium))  }, null, _createNewGameAction, true);
            _createNewGameButton.SetLock(true);


        }

        public override void Unload()
        {
            base.Unload();
        }
       
        

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            CheckButtonLock();

            _nameTypingBox.Update(gameTime);
            _createNewText.Update(gameTime, _createNewTextPosition);
            _createNewGameButton.Update(gameTime);
            // _backGroundSprite.Update(gameTime, Position);
        }

        /// <summary>
        /// Disallow create new game if name is empty 
        /// </summary>
        private void CheckButtonLock()
        {
            if (_nameTypingBox.IsEmpty)
                _createNewGameButton.SetLock(true);
            else
                _createNewGameButton.SetLock(false);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            _nameTypingBox.Draw(spriteBatch);
            _createNewText.Draw(spriteBatch, true);
            _createNewGameButton.Draw(spriteBatch);
           // _backGroundSprite.Draw(spriteBatch);  
        }


        private void CreateNewSaveAction()
        {
            SaveLoadManager.CreateNewSave(_nameTypingBox.CurrentString);
            SaveLoadManager.SetCurrentSave(_nameTypingBox.CurrentString);

            UI.LoadGame(SaveLoadManager.CurrentSave);
        }
    }
}
