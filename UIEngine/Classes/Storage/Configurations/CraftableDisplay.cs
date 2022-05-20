using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.Storage.Configurations
{
    internal class CraftableDisplay : InventoryDisplay
    {
        protected InventorySlotDisplay OutputSlot;
        protected int? OutputSlotRow;
        protected int? OutputSlotColumn;

        protected InventorySlotDisplay FuelSlot;
        protected int? FuelSlotRow;
        protected int? FuelSlotColumn;


        protected NineSliceTextButton CraftingActionButton;
        public CraftableDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        

        protected bool IsOutputSlot(int row, int column)
        {
            return OutputSlotRow == row && OutputSlotColumn == column;
        }

        protected bool IsFuelSlot(int row, int column)
        {
            return FuelSlotRow == row && FuelSlotColumn == column;
        }
        protected void AssignOutputSlot()
        {
            if (OutputSlotRow == null || OutputSlotColumn == null)
                throw new Exception($"Forgot to assign crafting row or column");
            OutputSlot = InventorySlots[OutputSlotRow.Value, OutputSlotColumn.Value];    
        }

        protected void AssignFuelSlot()
        {
            if (FuelSlotRow == null || FuelSlotColumn == null)
                throw new Exception($"Forgot to assign fuel row or column");
            FuelSlot = InventorySlots[FuelSlotRow.Value, FuelSlotColumn.Value];
        }

        protected void CraftItem()
        {

        }
    }
}
