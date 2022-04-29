using DataModels.ItemStuff;
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
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.RecipeStuff.PanelStuff
{
    internal class RecipeGuideBox : RecipeSection
    {
        private StackPanel _stackPanelMicroSteps;

        private Vector2 _selectedTextOffset = new Vector2(16, 16);
        private Vector2 _selectedStepTextPosition;
        private Text _selectedStepText;

        private Vector2 _microStepsOffset = new Vector2(16, 64);

        private Vector2 _microStepsPosition;
        private List<MicroStep> _microSteps;
        private Vector2 _scale = new Vector2(2f, 2f);
        public RecipeGuideBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _microSteps = new List<MicroStep>();
        }
        public override void LoadContent()
        {

            List<RecipeInfo> childRecipes = ItemFactory.RecipeHelper.GetAllSubRecipes(RecipeInfo);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, 144 * (int)_scale.X, 112 * (int)_scale.Y);


            _selectedStepText = TextFactory.CreateUIText("test", GetLayeringDepth(UILayeringDepths.Medium), .5f);
            _selectedStepTextPosition = Position + _selectedTextOffset;

            _microStepsPosition = Position + _microStepsOffset;
            _stackPanelMicroSteps = new StackPanel(this, graphics, content, _microStepsPosition,
                GetLayeringDepth(UILayeringDepths.Medium));
            StackRow stackRow = new StackRow(Width);
            foreach (var childRec in childRecipes)
            {
                MicroStep step = new MicroStep(this, _stackPanelMicroSteps, graphics, content, null,
                    GetLayeringDepth(UILayeringDepths.Medium));
                step.LoadRecipe(childRec);
                step.LoadContent();
                stackRow.AddItem(step, StackOrientation.Left);

            }
            _stackPanelMicroSteps.Add(stackRow);
            base.LoadContent();



        }

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            _selectedStepTextPosition = Position + _selectedTextOffset;
            _microStepsPosition = Position + _microStepsOffset;


        }

        public void SetStepInstructionsText(string instructions)
        {
            _selectedStepText.SetFullString(instructions);
        }




        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive)
            {
                _selectedStepText.Update(gameTime, _selectedStepTextPosition);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _selectedStepText.Draw(spriteBatch, true);
            }
        }
    }
}
