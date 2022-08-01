using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
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
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.DebugStuff.DeveloperBoardStuff
{
    internal class DeveloperBoard : MenuSection
    {
        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 416, 448);
        private NineSliceTextButton _saveSettingsButton;
        private StackPanel _stackPanel;


        
        public DeveloperBoard(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }
  

        private void AddCheckBox(StackRow stackRow, string text, bool initialToggleValue)
        {
            CheckBox checkBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            checkBox.SetToggleValue(initialToggleValue);
            stackRow.AddItem(checkBox, StackOrientation.Left);


            NineSliceTextButton checkBoxDescription = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                new List<Text>() { TextFactory.CreateUIText(text, GetLayeringDepth(UILayeringDepths.Medium)) }, null);
            checkBoxDescription.Displaybackground = false;

            stackRow.AddItem(checkBoxDescription, StackOrientation.Left);
        }
        public override void LoadContent()
        {

            ClearGrid();
           // GetSettingsValues();
            Position = RectangleHelper.CenterRectangleInRectangle(_backGroundSpriteDimensions,
                Settings.ScreenRectangle);
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            StackRow stackRow1 = new StackRow(_backGroundSpriteDimensions.Width);

            AddCheckBox(stackRow1, "Player Hurt Sound", Flags.EnablePlayerHurtSounds);

            //_saveSettingsButton = UI.ButtonFactory.CreateNSliceTxtBtn(_stackPanel, Position,
            //    GetLayeringDepth(UILayeringDepths.Low), new List<string>()
            //    { "Save Settings!" },
            //    SetSettingsValues, true);
            //stackRow1.AddItem(_saveSettingsButton, StackOrientation.Center);
            //_stackPanel.Add(stackRow1);

            //AddSectionToGrid(_saveSettingsButton, 0, 0);


            //StackRow stackRow2 = new StackRow(_backGroundSpriteDimensions.Width);

            //NineSliceTextButton _muteMusicText = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
            //    new List<Text>() { TextFactory.CreateUIText("Mute Music", GetLayeringDepth(UILayeringDepths.Medium)) }, null);
            //_muteMusicText.Displaybackground = false;

            //stackRow2.AddItem(_muteMusicText, StackOrientation.Left);
            //_muteMusicCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            //_muteMusicCheckBox.ToggleValue(SettingsManager.Mute);
            //stackRow2.AddItem(_muteMusicCheckBox, StackOrientation.Right);
            //_stackPanel.Add(stackRow2);

            //AddSectionToGrid(_muteMusicCheckBox, 1, 0);

            //StackRow stackRow3 = new StackRow(_backGroundSpriteDimensions.Width);
            //NineSliceTextButton _enableFullScreenText = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
            //   new List<Text>() { TextFactory.CreateUIText("Enable FullScreen", GetLayeringDepth(UILayeringDepths.Medium)) }, null);
            //_enableFullScreenText.Displaybackground = false;

            //stackRow3.AddItem(_enableFullScreenText, StackOrientation.Left);
            //_enableFullScrenCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            //_enableFullScrenCheckBox.ToggleValue(SettingsManager.FullScreen);
            //stackRow3.AddItem(_enableFullScrenCheckBox, StackOrientation.Right);
            //AddSectionToGrid(_enableFullScrenCheckBox, 2, 0);

            //_stackPanel.Add(stackRow3);


            //Vector2 backButtonPosition = RectangleHelper.PlaceRectangleAtBottomLeftOfParentRectangle(RectangleHelper.RectFromPosition(Position, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height),
            //   UISourceRectangles._backButtonRectangle);
            //_backButton = UI.ButtonFactory.CreateButton(this, backButtonPosition,
            //    GetLayeringDepth(UILayeringDepths.Medium), UISourceRectangles._backButtonRectangle,
            // UI.MainMenu._outerMenu.ChangeToPlayOrExitState, scale: 2f);
            //_backButton.CustomClickSoundName = "BackButton1";

            Deactivate();

           // NormallyActivated = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.WasKeyTapped(Microsoft.Xna.Framework.Input.Keys.L))
                Toggle();
            base.Update(gameTime);
        }
    }
}
