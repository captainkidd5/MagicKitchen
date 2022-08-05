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
    internal class PhysDevPage : DevPage
    {




        public PhysDevPage(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = false;
        }



        public override void MovePosition(Vector2 newPos)
        {
            //base.MovePosition(newPos);
        }
        public override void LoadContent()
        {
            base.LoadContent();


            StackRow stackRow1 = new StackRow(BackgroundSpriteDimensions.Width);

            CheckBox playeerHurtSoundsCheckBox = new CheckBox(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            playeerHurtSoundsCheckBox.ActionOnSave = new Action(() => { SettingsManager.EnablePlayerHurtSounds = playeerHurtSoundsCheckBox.Value; });

            AddCheckBox(playeerHurtSoundsCheckBox, stackRow1, "Player Hurt Sound", SettingsManager.EnablePlayerHurtSounds);

            CheckBox enablePlayerDeathSoundsCheckBox = new CheckBox(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            enablePlayerDeathSoundsCheckBox.ActionOnSave = new Action(() => { SettingsManager.EnablePlayerDeath = enablePlayerDeathSoundsCheckBox.Value; });

            AddCheckBox(enablePlayerDeathSoundsCheckBox, stackRow1, "Enable Death", SettingsManager.EnablePlayerDeath);


            CheckBox nightTimeCheckBox = new CheckBox(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            nightTimeCheckBox.ActionOnSave = new Action(() => { SettingsManager.IsNightTime = nightTimeCheckBox.Value; });

            AddCheckBox(nightTimeCheckBox, stackRow1, "Toggle Night", SettingsManager.IsNightTime);

            StackPanel.Add(stackRow1);


            StackRow stackRow2 = new StackRow(BackgroundSpriteDimensions.Width);

            CheckBox debugVelcroCheckBox = new CheckBox(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            debugVelcroCheckBox.ActionOnSave = new Action(() => { SettingsManager.DebugVelcro = debugVelcroCheckBox.Value; });

            AddCheckBox(debugVelcroCheckBox, stackRow2, "Toggle velcro", SettingsManager.DebugVelcro);

            CheckBox debugGridCheckBox = new CheckBox(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            debugGridCheckBox.ActionOnSave = new Action(() => { SettingsManager.DebugGrid = debugGridCheckBox.Value; });

            AddCheckBox(debugGridCheckBox, stackRow2, "Toggle grid", SettingsManager.DebugGrid);

            CheckBox ePathCheckBox = new CheckBox(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            ePathCheckBox.ActionOnSave = new Action(() => { SettingsManager.ShowEntityPaths = ePathCheckBox.Value; });

            AddCheckBox(ePathCheckBox, stackRow2, "Show Entity Paths", SettingsManager.ShowEntityPaths);


            StackPanel.Add(stackRow2);






            Deactivate();

            // NormallyActivated = false;
        }

    }
}
