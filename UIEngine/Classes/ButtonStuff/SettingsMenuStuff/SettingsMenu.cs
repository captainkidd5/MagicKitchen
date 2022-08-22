using Globals.Classes.Helpers;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes.SongStuff;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.ButtonStuff.SettingsMenuStuff
{
    internal class SettingsMenu : MenuSection
    {
        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 416, 448);
        private NineSliceTextButton _saveSettingsButton;
        private StackPanel _stackPanel;

        private CheckBox _muteMusicCheckBox;
        private CheckBox _enableFullScrenCheckBox;

        private bool _muteMusic;
        private bool _enableFullScren;

        private Button _backButton;

        public SettingsMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Selectables = new InterfaceSection[3, 3];
            CurrentSelectedPoint = new Point(0, 0);
        }

        public void ReadjustBasedOnParent(Rectangle newRectangle, Vector2 parentPos)
        {
            ChildSections.Clear();
            Unload();
            _backGroundSpriteDimensions = newRectangle;
            Position = parentPos;
            LoadContent();
            
        }
        public override void LoadContent()
        {

            ClearGrid();
            GetSettingsValues();
            Position = RectangleHelper.CenterRectangleInRectangle(_backGroundSpriteDimensions,
                new Rectangle((int)Position.X, (int)Position.Y, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height));
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            StackRow stackRow1 = new StackRow(_backGroundSpriteDimensions.Width);
            _saveSettingsButton = UI.ButtonFactory.CreateNSliceTxtBtn(_stackPanel, Position,
                GetLayeringDepth(UILayeringDepths.Low),  new List<string>()
                { "Save Settings!" },
                SetSettingsValues,true);
            stackRow1.AddItem(_saveSettingsButton, StackOrientation.Center);
            _stackPanel.Add(stackRow1);

            AddSectionToGrid(_saveSettingsButton, 0, 0);


            StackRow stackRow2 = new StackRow(_backGroundSpriteDimensions.Width);

            NineSliceTextButton _muteMusicText = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                new List<Text>() { TextFactory.CreateUIText("Mute Music",Position, null, null, GetLayeringDepth(UILayeringDepths.Medium))}, null);
            _muteMusicText.Displaybackground = false;

            stackRow2.AddItem(_muteMusicText, StackOrientation.Left);
            _muteMusicCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
                _muteMusicCheckBox.SetToggleValue(SettingsManager.Mute);
            stackRow2.AddItem(_muteMusicCheckBox, StackOrientation.Right);
            _stackPanel.Add(stackRow2);

            AddSectionToGrid(_muteMusicCheckBox, 1, 0);

            StackRow stackRow3 = new StackRow(_backGroundSpriteDimensions.Width);
            NineSliceTextButton _enableFullScreenText = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
               new List<Text>() { TextFactory.CreateUIText("Enable FullScreen",Position,null,null, GetLayeringDepth(UILayeringDepths.Medium)) }, null);
            _enableFullScreenText.Displaybackground = false;

            stackRow3.AddItem(_enableFullScreenText, StackOrientation.Left);
            _enableFullScrenCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            _enableFullScrenCheckBox.SetToggleValue(SettingsManager.FullScreen);
            stackRow3.AddItem(_enableFullScrenCheckBox, StackOrientation.Right);
            AddSectionToGrid(_enableFullScrenCheckBox,2, 0);

            _stackPanel.Add(stackRow3);


            Vector2 backButtonPosition = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(RectangleHelper.RectFromPosition(Position, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height),
               UISourceRectangles._backButtonRectangle);
            _backButton = UI.ButtonFactory.CreateButton(this, backButtonPosition,
                GetLayeringDepth(UILayeringDepths.Medium), UISourceRectangles._backButtonRectangle,
             UI.MainMenu._outerMenu.ChangeToPlayOrExitState, scale: 2f);
            _backButton.CustomClickSoundName = "BackButton1";

            Deactivate();

            NormallyActivated = false;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _enableFullScren = _enableFullScrenCheckBox.Value;
                _muteMusic = _muteMusicCheckBox.Value;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
        }

        private void GetSettingsValues()
        {
            _muteMusic = SettingsManager.Mute;
            _enableFullScren = SettingsManager.FullScreen;
        }

        private void SetSettingsValues()
        {
            SettingsManager.Mute = _muteMusic;
            SongManager.Muted = SettingsManager.Mute;

            SettingsManager.FullScreen = _enableFullScren;
            SettingsManager.SaveSettings();
        }
    }
}
