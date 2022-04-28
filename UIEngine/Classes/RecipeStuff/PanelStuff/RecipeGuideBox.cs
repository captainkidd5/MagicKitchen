using DataModels.ItemStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Classes;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.RecipeStuff.PanelStuff
{
    internal class RecipeGuideBox : RecipeSection
    {
        private StackPanel _stackPanel;

        private Vector2 _selectedTextOffset = new Vector2(16, 16);
        private Vector2 _selectedStepTextPosition;
        private Text _selectedStepText;


        private List<MicroStep> _microSteps;

        public RecipeGuideBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _microSteps = new List<MicroStep>();
        }
        public override void LoadContent()
        {
            base.LoadContent();


            
        }


        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
        }

        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
