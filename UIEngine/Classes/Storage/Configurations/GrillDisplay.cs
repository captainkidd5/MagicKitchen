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
    internal class GrillDisplay : CraftableDisplay
    {


        //XXX
        //X--
        public GrillDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                UIProgressBar.SetProgressRatio((StorageContainer as CraftingStorageContainer).CraftedItemMetre.Ratio);
                FuelBar.SetProgressRatio((StorageContainer as CraftingStorageContainer).FuelMetre.Ratio);
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
            Position = new Vector2(Position.X, Position.Y - 256);
            BackgroundSourceRectangle = new Rectangle(816, 0, 80, 96);

            BackgroundSpritePositionOffset = new Vector2(0, 0);
            Vector2 backdropScale = new Vector2(2f, 2f);
            BackdropSprite = SpriteFactory.CreateUISprite(new Vector2(Position.X, Position.Y), BackgroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),
                Color.White, scale: backdropScale);

            Vector2 offSet = new Vector2(0, BackgroundSourceRectangle.Height / 2 * backdropScale.Y);


            int requiredSlots = 3;
            if (StorageContainer.Slots.Count != requiredSlots)
                throw new Exception($"Storage container passed into display must have exactly {requiredSlots} slots");

            StackPanel = new StackPanel(this, graphics, content, Position + offSet, GetLayeringDepth(UILayeringDepths.Low));


            ClearGrid();
            int slotIndex = 0;
            //This is total rows, not just slots (includes progress bars etc)
            Rows = 5;
            Columns = 1;

            Selectables = new InterfaceSection[Rows , Columns ];

            InventorySlots = new InventorySlotDisplay[Rows, Columns];
            DrawCutOff = Rows;
            OutputSlotRow = 0;
            OutputSlotColumn = 0;
            FuelSlotRow = 4;
            FuelSlotColumn = 0;


            for (int row = 0; row < Rows; row++)
            {
                StackRow stackRow = new StackRow(BackgroundSourceRectangle.Width * 2);

                //add extra for spacing
                for (int column = 0; column < Columns; column++)
                {
                    if (row == 0)
                    {


                        //stackRow.AddSpacer(new Rectangle(0,0,_buttonWidth,_buttonWidth), StackOrientation.Left);


                        InventorySlotDisplay display = new InventorySlotDisplay(row, column,
                            this, graphics, content, (StorageContainer as CraftingStorageContainer).OutputSlot,
             Position, GetLayeringDepth(UILayeringDepths.Medium), SlotVisualVariant.Output);
                        InventorySlots[row, column] = display;
                        AddSectionToGrid(display, row, column);
                        display.LoadContent();

                        stackRow.AddItem(display, StackOrientation.Center, true);
                        slotIndex++;

                    }
                    if (row == 1)
                    {
                        UIProgressBar = new UIProgressBar(BarOrientation.Vertical, StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
                        UIProgressBar.LoadContent();
                        stackRow.AddItem(UIProgressBar, StackOrientation.Center, true);
                        UIProgressBar.LoadContent();
                    }
                    if (row == 2)
                    {
                        InventorySlotDisplay display = new InventorySlotDisplay(row - 1, column, this, graphics, content, StorageContainer.Slots[slotIndex],
                   Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots[row - 1, column] = display;
                        AddSectionToGrid(display, row - 1, column);
                        display.LoadContent();

                        stackRow.AddItem(display, StackOrientation.Center, true);
                        slotIndex++;
                    }
                    if (row == 3)
                    {
                        FuelBar = new UIProgressBar(BarOrientation.Vertical, StackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
                        FuelBar.LoadContent();
                        stackRow.AddItem(FuelBar, StackOrientation.Center, true);
                        //FuelBar.LoadContent();
                    }
                   
                   
               
                    if (row ==4)
                    {




                        InventorySlotDisplay display = new InventorySlotDisplay(row, column, this, graphics, content,
                           (StorageContainer as CraftingStorageContainer).FuelSlot,
                 Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots[row, column] = display;
                        AddSectionToGrid(display, row-1, column);
                        display.LoadContent();
                        stackRow.AddItem(display, StackOrientation.Center, true);
                        slotIndex++;


                    }
                    StackPanel.Add(stackRow);

                }

            }
            UIProgressBar.ProgressColor = Color.White;
            FuelBar.ProgressColor = Color.Orange;
            // UIProgressBar.MovePosition(new Vector2(UIProgressBar.Position.X, UIProgressBar.Position.Y + UIProgressBar.Height / 2));
            //FuelBar.MovePosition(new Vector2(FuelBar.Position.X, FuelBar.Position.Y + FuelBar.Height / 2));

            AssignOutputSlot();
            AssignFuelSlot();
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);



        }
    }
}
