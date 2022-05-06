using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.Storage.Configurations
{
    internal class DiningTableDisplay : InventoryDisplay
    {
        //-x-
        //xxx
        //-x-
        public DiningTableDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        protected override void GenerateUI(bool displayWallet)
        {
            if (StorageContainer.Slots.Count != 5)
                throw new Exception($"Storage container passed into dining table display must have exactly 5 slots");

            StackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            Selectables.Clear();
            InventorySlots = new List<InventorySlotDisplay>();
            int slotIndex = 0;
            Rows = 3;
            Columns = 3;
            for (int i = 0; i < Rows; i++)
            {
                StackRow stackRow = new StackRow(Columns * _buttonWidth);
                for (int j = 0; j < Columns; j++)
                {
                    //These indicies should not be drawn
                    //top left
                    if ((i == 0 && j == 0)
                        //bottom left
                        || (i == 0 && j == 2) ||
                        //top right
                        (i == 2 && j == 0) ||
                         //bottom right
                         (i == 2 && j == 2))
                    {

                    }
                    else
                    {



                    InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[slotIndex],
                    Position, GetLayeringDepth(UILayeringDepths.Low));
                    InventorySlots.Add(display);
                    Selectables.Add(display);
                    display.LoadContent();

                    stackRow.AddItem(display, StackOrientation.Left);
                    slotIndex++;
                    }


                }
                StackPanel.Add(stackRow);
            }

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);
        }

       


    }
}
