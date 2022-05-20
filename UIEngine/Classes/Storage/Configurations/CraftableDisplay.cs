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
        protected InventorySlotDisplay CraftingSlot;
        protected int? CraftingRow;
        protected int? CraftingColumn;

        protected NineSliceTextButton CraftingActionButton;
        public CraftableDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        

        protected bool IsCraftingSlot(int row, int column)
        {
            return CraftingRow == row && CraftingColumn == column;
        }
        protected void AssignCraftingSlot()
        {
            if (CraftingRow == null || CraftingColumn == null)
                throw new Exception($"Forgot to assign crafting row or column");
            CraftingSlot = InventorySlots[CraftingRow.Value, CraftingColumn.Value];    
        }

        protected void CraftItem()
        {

        }
    }
}
