using DataModels;
using Globals.Classes;
using Globals.Classes.Helpers;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations.BodyPartStuff;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Components;
using UIEngine.Classes.TextStuff;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class CreateNewSaveMenu : InterfaceSection
    {


        private Text _createNewText;
        private NineSliceTextButton _createNewGameTextButton;

        private TypingBox _nameTypingBox;
        private int _nameWindowWidth = 180;
        private int _nameWindowHeight = 48;

        private Rectangle _createNewGameButtonRectangle = new Rectangle(400, 720, 32, 32);
   
        private Button _createNewGameButton;
        private Action _createNewGameAction;

        private Button _backButton;

        private PlayerAvatarViewer _playerAvatarViewer;

        private StackPanel _stackPanel;

        private AvatarPartSwapper _hairSwapper;

        private AvatarColorSwapper _skinColorSwapper;

        private AvatarColorSwapper _hairColorSwapper;


        public PlayerAvatarData AvatarData { get; set; }
        public CreateNewSaveMenu(InterfaceSection 
            
            
            
            interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void LoadContent()
        {
            BodyPiece[] bodyPieces = new BodyPiece[]
    {
                    new Pants(0),
                    new Shoes(0),
                    new Shirt(0),
                    new Shoulders(0),
                    new Arms(0),
                    new Eyes(0),
                    new Head(0),
                      new Hair(0),

    };

            Position = new Vector2(parentSection.TotalBounds.X, parentSection.TotalBounds.Y);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, parentSection.TotalBounds.Width, parentSection.TotalBounds.Height);

            _createNewText = TextFactory.CreateUIText("Create New Game", GetLayeringDepth(UILayeringDepths.High), scale:2f);


           



            Vector2 stackPanelPos = new Vector2(Position.X, Position.Y);
            _stackPanel = new StackPanel(this, graphics, content, stackPanelPos, GetLayeringDepth(UILayeringDepths.Low));

            StackRow createNewGameTitleRow = new StackRow(Width);

            _createNewGameTextButton = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
               new List<Text>() { _createNewText }, null, centerTextHorizontally: true);

            createNewGameTitleRow.AddItem(_createNewGameTextButton, StackOrientation.Center);
            _stackPanel.Add(createNewGameTitleRow);

            StackRow nameBoxRow = new StackRow(Width);
            
            _nameTypingBox = new TypingBox(_stackPanel,graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), _nameWindowWidth, _nameWindowHeight);
            nameBoxRow.AddItem(_nameTypingBox, StackOrientation.Center);
            _stackPanel.Add(nameBoxRow);
          


            StackRow avatarStackRow = new StackRow(Width);

            _playerAvatarViewer = new PlayerAvatarViewer(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
            _playerAvatarViewer.LoadContent();
            _playerAvatarViewer.SetPieces(bodyPieces);
            avatarStackRow.AddItem(_playerAvatarViewer, StackOrientation.Center);
            _stackPanel.Add(avatarStackRow);



            Rectangle spacer = new Rectangle(0, 0, 32, 16);
            StackRow spacer1 = new StackRow(Width);
            spacer1.AddSpacer(spacer, StackOrientation.Center);
            _stackPanel.Add(spacer1);

            _hairSwapper = new AvatarPartSwapper("hair", bodyPieces[7], _stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            _hairSwapper.LoadContent();
            StackRow hairStackRow = new StackRow(Width);
            hairStackRow.AddItem(_hairSwapper, StackOrientation.Center);
            _stackPanel.Add(hairStackRow);

            _hairColorSwapper = new AvatarColorSwapper("color", bodyPieces[7],null, _stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            _hairColorSwapper.LoadContent();
            StackRow hairColorStackRow = new StackRow(Width);
            hairColorStackRow.AddItem(_hairColorSwapper, StackOrientation.Center);
            _stackPanel.Add(hairColorStackRow);

            StackRow spacer2 = new StackRow(Width);
            spacer2.AddSpacer(spacer, StackOrientation.Center);
            _stackPanel.Add(spacer2);

            _skinColorSwapper = new AvatarColorSwapper("Skin", bodyPieces[4], bodyPieces[6], _stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            _skinColorSwapper.LoadContent();
            StackRow colorStackRow = new StackRow(Width);
            colorStackRow.AddItem(_skinColorSwapper, StackOrientation.Center);
            _stackPanel.Add(colorStackRow);

            _createNewGameAction = CreateNewSaveAction;

            Vector2 bottomRightQuadrant = RectangleHelper.PlaceBottomRightQuadrant(parentSection.TotalBounds, _createNewGameButtonRectangle);
            _createNewGameButton = new Button(this, graphics, content,new Vector2(bottomRightQuadrant.X + 64, bottomRightQuadrant.Y + 32),
                GetLayeringDepth(UILayeringDepths.Medium), _createNewGameButtonRectangle, CreateNewSaveAction);

            _createNewGameButton.SetLock(true);
            _createNewGameButton.AddConfirmationWindow("Create new game?");
            _createNewGameButton.LoadContent();

            Vector2 backButtonPosition = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(
             parentSection.TotalBounds, UISourceRectangles._backButtonRectangle);

            _backButton = UI.ButtonFactory.CreateButton(this, backButtonPosition,
                GetLayeringDepth(UILayeringDepths.Medium), UISourceRectangles._backButtonRectangle,
                new Action(() =>
                {
                    (parentSection as OuterMenu).ChangeState(OuterMenuState.ViewGames);
                })
              , scale: 2f);
            _backButton.CustomClickSoundName = "BackButton1";


            base.LoadContent();

            _skinColorSwapper.BackwardsAction();

        }

        public override void Unload()
        {
            base.Unload();
        }
       
        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CheckButtonLock();

            //_nameTypingBox.Update(gameTime);
          //  _createNewGameButton.Update(gameTime);

           // _backButton.Update(gameTime);

           // _stackPanel.Update(gameTime);
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
            base.Draw(spriteBatch);
         //   _nameTypingBox.Draw(spriteBatch);
          //  _createNewGameButton.Draw(spriteBatch);
           // _backButton.Draw(spriteBatch);
           // _stackPanel.Draw(spriteBatch);
           // _backGroundSprite.Draw(spriteBatch);  
        }


        private void CreateNewSaveAction()
        {
            Dictionary<string, SaveFile> saveFiles = SaveLoadManager.SaveFiles;

           // string regexString = _nameTypingBox.CurrentString.Replace("\n", "");
          //  regexString = regexString.Replace(" ", "");
            if (saveFiles.Keys.Contains(_nameTypingBox.Text.ToString()))
            {

                return;
            }
            PlayerAvatarData avatarData = new PlayerAvatarData();
            Color skinColor = SpriteFactory.SkinColors[_skinColorSwapper.SkinIndex];
            avatarData.SkinRed = skinColor.R;
            avatarData.SkinGreen = skinColor.G;
            avatarData.SkinBlue = skinColor.B;
            avatarData.HairIndex = _hairSwapper.Index;

            Color hairColor = SpriteFactory.SkinColors[_hairColorSwapper.SkinIndex];
            avatarData.HairRed = hairColor.R;
            avatarData.HairGreen = hairColor.G;
            avatarData.HairBlue = hairColor.B;

            SaveLoadManager.CreateNewSave(_nameTypingBox.Text.ToString(), avatarData);
            SaveLoadManager.SetCurrentSave(_nameTypingBox.Text.ToString());
            Flags.IsNewGame = true;
            UI.LoadGame(SaveLoadManager.CurrentSave);
        }
    }
}
