using ItemEngine.Classes.StorageStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Presets;
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
                UIProgressBar.GetProgressRatio((StorageContainer as CraftingStorageContainer).CraftedItemMetre.Ratio);
                //CraftingActionButton.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
             //   CraftingActionButton.Draw(spriteBatch);
            }
        }

      

        protected override void GenerateUI(bool displayWallet)
        {

            BackgroundSourceRectangle = new Rectangle(48, 640, 240, 64);

            BackgroundSpritePositionOffset = new Vector2(0, 0);
            BackdropSprite = SpriteFactory.CreateUISprite(new Vector2(Position.X, Position.Y), BackgroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),
                Color.White, scale: new Vector2(2f, 2f));

            Vector2 offSet = new Vector2(0, BackgroundSourceRectangle.Height / 2);


            int requiredSlots = 3;
            if (StorageContainer.Slots.Count != requiredSlots)
                throw new Exception($"Storage container passed into display must have exactly {requiredSlots} slots");

            StackPanel = new StackPanel(this, graphics, content, Position + offSet, GetLayeringDepth(UILayeringDepths.Low));


            ClearGrid();
            int slotIndex = 0;
            Rows = 1;
            Columns = 5;

            Selectables = new InterfaceSection[Rows, Columns];

            InventorySlots = new InventorySlotDisplay[Rows, Columns];
            DrawCutOff = Rows;
            OutputSlotRow = 0;
            OutputSlotColumn = 4;
            FuelSlotRow = 0;
            FuelSlotColumn = 0;

            StackRow stackRow = new StackRow((Columns + 1) * _buttonWidth);

            for (int row = 0; row < Rows; row++)
            {
                //add extra for spacing
                for (int column = 0; column < Columns; column++)
                {
                    if(column == 1)
                    {
                        FuelBar = new UIProgressBar(StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
                        FuelBar.LoadContent();
                        stackRow.AddItem(FuelBar, StackOrientation.Left);
                        FuelBar.LoadContent();
                    }
                    if (column ==2 || column == 3)
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
                        UIProgressBar = new UIProgressBar(StackPanel, graphics, content,Position, GetLayeringDepth(UILayeringDepths.Medium));
                        UIProgressBar.LoadContent();
                        stackRow.AddItem(UIProgressBar, StackOrientation.Left);
                        UIProgressBar.LoadContent();

                        //stackRow.AddSpacer(new Rectangle(0,0,_buttonWidth,_buttonWidth), StackOrientation.Left);


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


                

                        InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content,
                           (StorageContainer as CraftingStorageContainer).FuelSlot,
                 Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots[row, column] = display;
                        AddSectionToGrid(display, row, column);
                        display.LoadContent();
                        stackRow.AddItem(display, StackOrientation.Left);
                        slotIndex++;
                

                    }
                }

            }
            StackPanel.Add(stackRow);
            UIProgressBar.MovePosition(new Vector2(UIProgressBar.Position.X, UIProgressBar.Position.Y + UIProgressBar.Height / 2));
            FuelBar.MovePosition(new Vector2(FuelBar.Position.X, FuelBar.Position.Y + FuelBar.Height / 2));

            AssignOutputSlot();
            AssignFuelSlot();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);

          

        }
    }
}
