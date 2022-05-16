using Globals.Classes.Helpers;
using InputEngine.Classes;
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
using static DataModels.Enums;

namespace UIEngine.Classes.Storage.Configurations
{
    internal class DiningTableDisplay : InventoryDisplay
    {
        private static readonly Rectangle s_backgroundSourceRectangle = new Rectangle(464, 0, 80, 64);
        private static readonly Vector2 s_backgroundOffset= new Vector2(-32, 0);

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
            ClearGrid();
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
                        stackRow.AddSpacer(new Rectangle(0, 0, 64, 64), StackOrientation.Left);
                    }
                    else
                    {



                    InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[slotIndex],
                    Position, GetLayeringDepth(UILayeringDepths.Medium));
                        InventorySlots.Add(display);
                        AddSectionToGrid(display, i, j);
                    display.LoadContent();

                    stackRow.AddItem(display, StackOrientation.Left);
                    slotIndex++;
                    }


                }
                StackPanel.Add(stackRow);
            }

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);
            BackgroundSpritePositionOffset = new Vector2(-64, 0);
            BackdropSprite = SpriteFactory.CreateUISprite(new Vector2(Position.X - 64, Position.Y), s_backgroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Low),
                Color.White , scale: new Vector2(4f,4f));
        }

        protected override void CheckGamePadInput()
        {
            if (Controls.WasGamePadButtonTapped(GamePadActionType.BumperLeft) ||
                                     Controls.WasGamePadButtonTapped(GamePadActionType.DPadLeft))
            {
                if (CurrentSelectedIndex > 1 && CurrentSelectedIndex < 4)
                    CurrentSelectedIndex--;
                SelectSlotAndMoveCursorIcon();
            }
            else if (Controls.WasGamePadButtonTapped(GamePadActionType.BumperRight) ||
                Controls.WasGamePadButtonTapped(GamePadActionType.DPadRight))
            {
                if (CurrentSelectedIndex >= 1 && CurrentSelectedIndex < 3)
                    CurrentSelectedIndex++;

                SelectSlotAndMoveCursorIcon();

            }
            else if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadUp))
            {
                if (CurrentSelectedIndex == 4 || CurrentSelectedIndex == 2)
                    CurrentSelectedIndex -= 2;

                SelectSlotAndMoveCursorIcon();

            }
            else if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadDown))
            {
                if (CurrentSelectedIndex == 0 || CurrentSelectedIndex == 2)
                    CurrentSelectedIndex +=2;


                SelectSlotAndMoveCursorIcon();

            }
        }


    }
}
