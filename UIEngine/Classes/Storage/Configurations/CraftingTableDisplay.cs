using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.Storage.Configurations
{
    internal class CraftingTableDisplay : CraftableDisplay
    {


        //xxx-
        //xxxx
        //xxx-
        public CraftingTableDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        protected override void GenerateUI(bool displayWallet)
        {
            Selectables = new InterfaceSection[3, 4];

            if (StorageContainer.Slots.Count != 10)
                throw new Exception($"Storage container passed into dining table display must have exactly 10 slots");

            StackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            ClearGrid();
            int slotIndex = 0;
            Rows = 3;
            Columns = 4;
            InventorySlots = new InventorySlotDisplay[Rows, Columns];
            DrawCutOff = Rows;
            CraftingRow = 1;
            CraftingColumn = 3;
            for (int i = 0; i < Rows; i++)
            {
                StackRow stackRow = new StackRow(Columns * _buttonWidth);
                for (int j = 0; j < Columns; j++)
                {
                  
                    if(j < 3)
                    {
                        InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[slotIndex],
                   Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots[i, j] = display;
                        AddSectionToGrid(display, i, j);
                        display.LoadContent();

                        stackRow.AddItem(display, StackOrientation.Left);
                        slotIndex++;
                    }
                    else if (IsCraft
                        ingSlot(i, j))
                    {



                        InventorySlotDisplay display = new InventorySlotDisplay(
                            this, graphics, content, StorageContainer.Slots[slotIndex],
             Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots[i, j] = display;
                        AddSectionToGrid(display, i, j);
                        display.LoadContent();

                        stackRow.AddItem(display, StackOrientation.Left);
                        slotIndex++;
                    }





                }
                StackPanel.Add(stackRow);
            }
            AssignCraftingSlot();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);

            BackgroundSourceRectangle = new Rectangle(560, 0, 80, 96);

            BackgroundSpritePositionOffset = new Vector2(-64, 0);
            BackdropSprite = SpriteFactory.CreateUISprite(new Vector2(Position.X - 64, Position.Y), BackgroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),
                Color.White, scale: new Vector2(4f, 4f));
        }
    }
}
