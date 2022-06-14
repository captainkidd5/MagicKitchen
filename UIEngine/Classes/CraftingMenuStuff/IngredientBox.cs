using DataModels.ItemStuff;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.CraftingMenuStuff
{
    internal class IngredientBox : InterfaceSection
    {
        private readonly CraftingPage _craftingPage;
        CraftingMiniIcon _icon;
        private CraftingIngredient _craftingIngredient;
        
        public IngredientBox(CraftingPage craftingPage, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _craftingPage = craftingPage;
        }


        public void LoadIngredient(CraftingIngredient craftingIngredient)
        {
            _craftingIngredient = craftingIngredient;
            _icon.LoadItemData(ItemFactory.GetItemData(craftingIngredient.Name));
        }
        public override void LoadContent()
        {
            base.LoadContent();
            _icon = new CraftingMiniIcon(_craftingPage, this, graphics, content, Position,
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
        }
    }
}
