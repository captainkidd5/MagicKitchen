using DataModels;
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

namespace UIEngine.Classes.RecipeStuff
{
    internal class RecipePage : InterfaceSection
    {
        private StackPanel _stackPanel;

        private int _width;

        private List<RecipePanel> _panels;
        public RecipePage(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _panels = new List<RecipePanel>();
        }

        public override void LoadContent()
        {
            _width = parentSection.Width;
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));

            foreach(RecipePanel info in _panels)
            {
                StackRow stackRow = new StackRow(_width);
                stackRow.AddItem(info, StackOrientation.Left);
                _stackPanel.Add(stackRow);

            }


            base.LoadContent();

        }

        public void LoadRecipes(List<RecipeInfo> recipeInfo)
        {
            foreach(RecipeInfo recipe in recipeInfo)
            {
                _panels.Add(new RecipePanel(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low)));
            }
        }
    }
}
