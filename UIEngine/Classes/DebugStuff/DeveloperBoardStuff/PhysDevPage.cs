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
        public override void LoadContent()
        {
            base.LoadContent();
            List<PhysCat> physCats = Enum.GetValues(typeof(PhysCat))
                       .Cast<PhysCat>()
                       .ToList();

            StackRow currentStackRow = new StackRow(BackgroundSpriteDimensions.Width);
            for (int i = 0; i < physCats.Count -1; i++)
            {

                if (i % 3 == 0)
                {
                    StackPanel.Add(currentStackRow);
                    currentStackRow = new StackRow(BackgroundSpriteDimensions.Width);
                }
                CheckBox checkBox = new CheckBox(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low), null);
                checkBox.ActionOnSave = new Action(() =>
                {

                    PhysCat cat = physCats[i];
                    if (checkBox.Value)
                    {
                        PhysicsManager.PhysicsDebugger.AddDebugCategory((Category)cat);
                    }
                    else
                    {
                        PhysicsManager.PhysicsDebugger.RemoveDebugCategory((Category)cat);
                    }


                }
                );
                AddCheckBox(checkBox, currentStackRow, physCats[i].ToString(),
                    PhysicsManager.PhysicsDebugger.DebuggableCategories.HasFlag((Category)physCats[i]));


            }

           





            //Deactivate();

            // NormallyActivated = false;
        }

    }
}
