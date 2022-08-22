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
    internal class DevPage : MenuSection
    {
        protected Rectangle BackgroundSpriteDimensions { get; set; } = new Rectangle(0, 0, 980, 448);
        private NineSliceTextButton _saveSettingsButton;
        protected StackPanel StackPanel;


        private List<CheckBox> _checkBoxes;
        public DevPage(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = false;
        }


        protected void AddCheckBox(CheckBox checkBox, StackRow stackRow, string text, bool initialToggleValue)
        {
            checkBox.SetToggleValue(initialToggleValue);
            stackRow.AddItem(checkBox, StackOrientation.Left);


            NineSliceTextButton checkBoxDescription = new NineSliceTextButton(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                new List<Text>() { TextFactory.CreateUIText(text,Position, null, null, GetLayeringDepth(UILayeringDepths.Medium)) }, null);
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
            Position = RectangleHelper.CenterRectangleInRectangle(BackgroundSpriteDimensions,
                Settings.ScreenRectangle);
            StackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
           
            StackRow stackRowSave = new StackRow(BackgroundSpriteDimensions.Width);

            _saveSettingsButton = UI.ButtonFactory.CreateNSliceTxtBtn(StackPanel, Position,
                GetLayeringDepth(UILayeringDepths.Medium), new List<string>()
                { "Save Settings!" },
                SetSettingsValues, true);
            stackRowSave.AddItem(_saveSettingsButton, StackOrientation.Center);
            StackPanel.Add(stackRowSave);




         //   Deactivate();

            // NormallyActivated = false;
        }
        private void SetSettingsValues()
        {
            foreach (CheckBox checkBox in _checkBoxes)
            {
                checkBox.SaveSettings();
            }

            SettingsManager.SaveSettings();
        }

    }
}
