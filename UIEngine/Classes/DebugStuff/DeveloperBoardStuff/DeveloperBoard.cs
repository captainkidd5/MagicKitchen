using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
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
using UIEngine.Classes.Components;

namespace UIEngine.Classes.DebugStuff.DeveloperBoardStuff
{
    internal class DeveloperBoard : MenuSection
    {
        private Rectangle _backGroundSpriteDimensions = new Rectangle(0, 0, 960, 448);
        private NineSliceTextButton _saveSettingsButton;
        private StackPanel _stackPanel;

        private NineSliceSprite _backgroundSprite;

        private List<CheckBox> _checkBoxes;
        public DeveloperBoard(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = false;
        }
  

        private void AddCheckBox(CheckBox checkBox, StackRow stackRow, string text, bool initialToggleValue)
        {
            checkBox.SetToggleValue(initialToggleValue);
            stackRow.AddItem(checkBox, StackOrientation.Left);


            NineSliceTextButton checkBoxDescription = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                new List<Text>() { TextFactory.CreateUIText(text, GetLayeringDepth(UILayeringDepths.Medium)) }, null);
            checkBoxDescription.Displaybackground = false;

            stackRow.AddItem(checkBoxDescription, StackOrientation.Left);
            _checkBoxes.Add(checkBox);
        }
        public override void MovePosition(Vector2 newPos)
        {
            //base.MovePosition(newPos);
        }
        public override void LoadContent()
        {
            _checkBoxes = new List<CheckBox>();
            ClearGrid();
            // GetSettingsValues();
            Position = RectangleHelper.CenterRectangleInRectangle(_backGroundSpriteDimensions,
                Settings.ScreenRectangle);
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            StackRow stackRow1 = new StackRow(_backGroundSpriteDimensions.Width);

            CheckBox playeerHurtSoundsCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            playeerHurtSoundsCheckBox.ActionOnSave = new Action(() => { SettingsManager.EnablePlayerHurtSounds = playeerHurtSoundsCheckBox.Value; });

            AddCheckBox(playeerHurtSoundsCheckBox,stackRow1, "Player Hurt Sound", SettingsManager.EnablePlayerHurtSounds);

            CheckBox enablePlayerDeathSoundsCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            enablePlayerDeathSoundsCheckBox.ActionOnSave = new Action(() => { SettingsManager.EnablePlayerDeath = enablePlayerDeathSoundsCheckBox.Value; });

            AddCheckBox(enablePlayerDeathSoundsCheckBox, stackRow1, "Enable Death", SettingsManager.EnablePlayerDeath);


            CheckBox nightTimeCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            nightTimeCheckBox.ActionOnSave = new Action(() => { SettingsManager.IsNightTime = nightTimeCheckBox.Value; });

            AddCheckBox(nightTimeCheckBox, stackRow1, "Toggle Night", SettingsManager.IsNightTime);

            _stackPanel.Add(stackRow1);


            StackRow stackRow2 = new StackRow(_backGroundSpriteDimensions.Width);

            CheckBox debugVelcroCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            debugVelcroCheckBox.ActionOnSave = new Action(() => { SettingsManager.DebugVelcro = debugVelcroCheckBox.Value; });

            AddCheckBox(debugVelcroCheckBox, stackRow2, "Toggle velcro", SettingsManager.DebugVelcro);

            CheckBox debugGridCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            debugGridCheckBox.ActionOnSave = new Action(() => { SettingsManager.DebugGrid = debugGridCheckBox.Value; });

            AddCheckBox(debugGridCheckBox, stackRow2, "Toggle grid", SettingsManager.DebugGrid);

            CheckBox ePathCheckBox = new CheckBox(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            ePathCheckBox.ActionOnSave = new Action(() => { SettingsManager.ShowEntityPaths = ePathCheckBox.Value; });

            AddCheckBox(ePathCheckBox, stackRow2, "Show Entity Paths", SettingsManager.ShowEntityPaths);


            _stackPanel.Add(stackRow2);



            StackRow stackRowSave = new StackRow(_backGroundSpriteDimensions.Width);

            _saveSettingsButton = UI.ButtonFactory.CreateNSliceTxtBtn(_stackPanel, Position,
                GetLayeringDepth(UILayeringDepths.Medium), new List<string>()
                { "Save Settings!" },
                SetSettingsValues, true);
            stackRowSave.AddItem(_saveSettingsButton, StackOrientation.Center);
            _stackPanel.Add(stackRowSave);

            _backgroundSprite = SpriteFactory.CreateNineSliceSprite(Position, _backGroundSpriteDimensions.Width, _backGroundSpriteDimensions.Height, UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Low));


            Deactivate();

           // NormallyActivated = false;
        }
        private void SetSettingsValues()
        {
            foreach(CheckBox checkBox in _checkBoxes)
            {
                checkBox.SaveSettings();
            }

            SettingsManager.SaveSettings();
        }
        public override void Update(GameTime gameTime)
        {
#if DEBUG
            if (Controls.WasKeyTapped(Microsoft.Xna.Framework.Input.Keys.L))
                Toggle();
            base.Update(gameTime);
#endif 

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
                _backgroundSprite.Draw(spriteBatch);
            base.Draw(spriteBatch);

        }
    }
}
