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

        private Vector2 _microStepsOffset = new Vector2(16, 128);

        private Vector2 _microStepsPosition;
        private Vector2 _scale = new Vector2(2f, 2f);

        private List<RecipeInfo> _parentRecipes;

        private static Rectangle _stepSelectorOutlineSourceRectangle = new Rectangle(368, 96, 25, 48);
        private Vector2 _stepSelectorPosition;
        private static Vector2 _stepSelectorPositionOffSet = new Vector2(-4, -28);

        private Sprite _stepSelectorSprite;
        private bool _stepSelectorActive;
        public RecipeGuideBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }
        public override void LoadContent()
        {
            _parentRecipes = ItemFactory.RecipeHelper.GetAllSubRecipes(RecipeInfo);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, 144 * (int)_scale.X, 112 * (int)_scale.Y);
            _selectedStepText = TextFactory.CreateUIText("", GetLayeringDepth(UILayeringDepths.Medium), .5f);

            _stepSelectorSprite = SpriteFactory.CreateUISprite(_stepSelectorPosition, _stepSelectorOutlineSourceRectangle, UI.ButtonTexture,
                GetLayeringDepth(UILayeringDepths.Low), scale: Scale);
            MovePosition(Position);
            base.LoadContent();



        }

        

        public override void MovePosition(Vector2 newPos)
        {
            base.MovePosition(newPos);
            ChildSections.Clear();


            _selectedStepTextPosition = Position + _selectedTextOffset;

            _microStepsPosition = Position + _microStepsOffset;
            _stackPanelMicroSteps = new StackPanel(this, graphics, content, _microStepsPosition,
                GetLayeringDepth(UILayeringDepths.Medium));
            StackRow stackRow = new StackRow(Width, 16 * (int)_scale.X);

            for(int i =0;  i< _parentRecipes.Count; i++)
            {
                MicroStep step = new MicroStep(this,i ==0, _stackPanelMicroSteps, graphics, content, null,
                   GetLayeringDepth(UILayeringDepths.Medium));
                step.LoadRecipe(_parentRecipes[i]);
                step.LoadContent();
                stackRow.AddItem(step, StackOrientation.Left);
            }
    
            _stackPanelMicroSteps.Add(stackRow);
            _selectedStepTextPosition = Position + _selectedTextOffset;
            _microStepsPosition = Position + _microStepsOffset;



        }

        public void SetStepInstructionsText(MicroStep microStep, string instructions)
        {
            _selectedStepText.SetFullString(instructions);
            _stepSelectorPosition = microStep.Position + _stepSelectorPositionOffSet * Scale;
            _stepSelectorActive = true;
        }




        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive)
            {
                _selectedStepText.Update(gameTime, _selectedStepTextPosition);
                if (_stepSelectorActive)
                    _stepSelectorSprite.Update(gameTime, _stepSelectorPosition);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _selectedStepText.Draw(spriteBatch, true);
                if(_stepSelectorActive)
                _stepSelectorSprite.Draw(spriteBatch);
            }
        }
    }
}
