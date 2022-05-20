using ItemEngine.Classes.StorageStuff;
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
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.Storage.Configurations
{
    internal class FurnaceTableDisplay : CraftableDisplay
    {


        //XXX
        //X--
        public FurnaceTableDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                CraftingActionButton.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                CraftingActionButton.Draw(spriteBatch);
            }
        }

      

        protected override void GenerateUI(bool displayWallet)
        {


            int requiredSlots = 3;
            if (StorageContainer.Slots.Count != requiredSlots)
                throw new Exception($"Storage container passed into display must have exactly {requiredSlots} slots");

            StackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));


            ClearGrid();
            int slotIndex = 0;
            Rows = 2;
            Columns = 3;

            Selectables = new InterfaceSection[Rows, Columns];

            InventorySlots = new InventorySlotDisplay[Rows, Columns];
            DrawCutOff = Rows;
            OutputSlotRow = 0;
            OutputSlotColumn = 2;
            FuelSlotRow = 1;
            FuelSlotColumn = 0;

            StackRow stackRow = new StackRow((Columns + 1) * _buttonWidth);

            for (int row = 0; row < Rows; row++)
            {
                //add extra for spacing
                for (int column = 0; column < Columns; column++)
                {

                    if (column < 2 && row == 0)
                    {
                        InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[slotIndex],
                   Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots[row, column] = display;
                        AddSectionToGrid(display, row, column);
                        display.LoadContent();

                        stackRow.AddItem(display, StackOrientation.Left);
                        slotIndex++;
                    }
                    else if (IsOutputSlot(row, column))
                    {

                        stackRow.AddSpacer(new Rectangle(0,0,_buttonWidth,_buttonWidth), StackOrientation.Left);


                        InventorySlotDisplay display = new InventorySlotDisplay(
                            this, graphics, content, (StorageContainer as CraftingStorageContainer).OutputSlot,
             Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots[row, column] = display;
                        AddSectionToGrid(display, row, column);
                        display.LoadContent();

                        stackRow.AddItem(display, StackOrientation.Left);
                    }
                    else if (IsFuelSlot(row, column))
                    {
                        StackPanel.Add(stackRow);
                        StackRow stackRow2 = new StackRow((Columns + 1) * _buttonWidth);
                        stackRow2.AddSpacer(new Rectangle(0, 0, _buttonWidth, _buttonWidth), StackOrientation.Left);

                        StackPanel.Add(stackRow2);

                        StackRow stackRow4 = new StackRow((Columns + 1) * _buttonWidth);
                        InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[slotIndex],
                 Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots[row, column] = display;
                        AddSectionToGrid(display, row, column);
                        display.LoadContent();

                        stackRow4.AddItem(display, StackOrientation.Left);
                        slotIndex++;
                        StackPanel.Add(stackRow4);

                    }





                }




            }
            CraftingActionButton = new NineSliceTextButton(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                    new List<TextEngine.Classes.Text>() { TextFactory.CreateUIText("Craft", GetLayeringDepth(UILayeringDepths.Medium)) }, CraftItem);
            StackRow stackRow3 = new StackRow(128);
            stackRow3.AddItem(CraftingActionButton, StackOrientation.Left);
            StackPanel.Add(stackRow3);
            AssignOutputSlot();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);

            //    BackgroundSourceRectangle = new Rectangle(560, 0, 80, 96);

            //    BackgroundSpritePositionOffset = new Vector2(-64, 0);
            //    BackdropSprite = SpriteFactory.CreateUISprite(new Vector2(Position.X - 64, Position.Y), BackgroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),
            //        Color.White, scale: new Vector2(4f, 4f));
            
        }
    }
}
