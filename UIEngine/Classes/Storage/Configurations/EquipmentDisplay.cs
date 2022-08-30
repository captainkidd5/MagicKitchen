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
using UIEngine.Classes.Components;
using static DataModels.Enums;

namespace UIEngine.Classes.Storage.Configurations
{
    internal class EquipmentDisplay : InventoryDisplay
    {
        public EquipmentDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void LoadNewEntityInventory(StorageContainer storageContainer, bool displayWallet)
        {
            base.LoadNewEntityInventory(storageContainer, displayWallet);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            GenerateUI(false);
        }
        protected override void GenerateUI(bool displayWallet)
        {
            ChildSections.Clear();
            Vector2 offSet = new Vector2(32,64);

            Position = parentSection.Position;

            Selectables = new InterfaceSection[4, 2];


            int requiredSlots = 8;
            if (StorageContainer.Slots.Count != requiredSlots)
                throw new Exception($"Storage container passed into display must have exactly {requiredSlots} slots");

            StackPanel = new StackPanel(this, graphics, content, Position + offSet, GetLayeringDepth(UILayeringDepths.Low));


            ClearGrid();
            int slotIndex = 0;
            Rows = 4;
            Columns = 2;

            Selectables = new InterfaceSection[Rows, Columns];

            InventorySlots = new InventorySlotDisplay[Rows, Columns ];
            DrawCutOff = Rows;



            for (int row = 0; row < Rows; row++)
            {
                StackRow stackRow = new StackRow(128);

                //add extra for spacing
                for (int column = 0; column < Columns; column++)
                {
                    SlotVisualVariant slotVariant = SlotVisualVariant.None;
                    if (column == 0)
                    {
                         slotVariant = GetVisualVariantFromRow(row);
                    }
                    else
                    {
                        slotVariant = SlotVisualVariant.Trinket;
                    }
                    InventorySlotDisplay display = new InventorySlotDisplay(row, column , this, graphics, content, StorageContainer.Slots[slotIndex],
                  Position, GetLayeringDepth(UILayeringDepths.Medium), slotVariant);
                    InventorySlots[row, column ] = display;
                    AddSectionToGrid(display, row, column);
                    display.LoadContent();

                    stackRow.AddItem(display, StackOrientation.Left, true);
                    slotIndex++;

                }
                StackPanel.Add(stackRow);

            }

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);

            parentSection.TotalBounds = TotalBounds;

        }

        private SlotVisualVariant GetVisualVariantFromRow(int row)
        {
            switch (row)
            {
                case 0:
                    return SlotVisualVariant.Helmet;
                case 1:
                    return SlotVisualVariant.Torso;
                case 2:
                    return SlotVisualVariant.Legs;
                case 3:
                    return SlotVisualVariant.Boots;
                default:
                    throw new Exception("Invalid row");
            }
        }

        internal override void ReceiveControl(MenuSection sender, Direction direction)
        {
            base.ReceiveControl(sender, direction);

            //if (sender.GetType() == typeof(TabsColumnMenu))
            //{
            //    DoSelection(new Point(0, 0));
            //    return;
            //}
            //if (_lastSelectedPoint != null)
            //    DoSelection(_lastSelectedPoint.Value);
        }
    }
}
