using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
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

        Category category;

        private void SetCheckBox(StackRow stackRow, PhysCat physCat)
        {
            CheckBox checkBox = new CheckBox(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
            checkBox.ActionOnSave = new Action(() => { TogglePhysCat(checkBox.Value, physCat); });
            AddCheckBox(checkBox, stackRow, physCat.ToString(),
                PhysicsManager.PhysicsDebugger.DebuggableCategories.HasFlag((Category)physCat));
        }
        public override void LoadContent()
        {
            base.LoadContent();
            List<PhysCat> physCats = Enum.GetValues(typeof(PhysCat))
                       .Cast<PhysCat>()
                       .ToList();

            StackRow stackRow1 = new StackRow(BackgroundSpriteDimensions.Width);

            SetCheckBox(stackRow1, PhysCat.SolidLow);
            SetCheckBox(stackRow1, PhysCat.SolidHigh);
            SetCheckBox(stackRow1, PhysCat.TransparencySensor);



            StackPanel.Add(stackRow1);

            StackRow stackRow2= new StackRow(BackgroundSpriteDimensions.Width);
            SetCheckBox(stackRow2, PhysCat.Player);
            SetCheckBox(stackRow2, PhysCat.PlayerBigSensor);
            SetCheckBox(stackRow2, PhysCat.Item);

            StackPanel.Add(stackRow2);


            StackRow stackRow3 = new StackRow(BackgroundSpriteDimensions.Width);
            SetCheckBox(stackRow3, PhysCat.Tool);
            SetCheckBox(stackRow3, PhysCat.NPC);
            SetCheckBox(stackRow3, PhysCat.NPCBigSensor);

            StackPanel.Add(stackRow3);


            StackRow stackRow4 = new StackRow(BackgroundSpriteDimensions.Width);
            SetCheckBox(stackRow4, PhysCat.ArraySensor);
            SetCheckBox(stackRow4, PhysCat.ClickBox);



            StackPanel.Add(stackRow4);
            //Deactivate();

            // NormallyActivated = false;
        }


        private void TogglePhysCat(bool val, PhysCat cat)
        {
            if (val)
            {
                PhysicsManager.PhysicsDebugger.AddDebugCategory((Category)cat);
            }
            else
            {
                PhysicsManager.PhysicsDebugger.RemoveDebugCategory((Category)cat);
            }


        }
    }
}
