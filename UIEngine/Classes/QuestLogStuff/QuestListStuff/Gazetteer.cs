using DataModels.QuestStuff;
using IOEngine.Classes;
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
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.QuestLogStuff.QuestListStuff
{
    internal class Gazetteer : MenuSection
    {
        private StackPanel _stackPanel;

        public Quest ActiveQuest { get; set; }
        private Dictionary<string, Quest> _quests => SaveLoadManager.CurrentSave.GameProgressData.QuestProgress;
        private Rectangle _backGroundSourceRectangle = new Rectangle(336, 304, 240, 256);
        private Sprite _backGroundSprite;
        private Vector2 _scale = new Vector2(2f, 2f);

        private float _paragraphTextSize = .7f;
        public Gazetteer(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }

        public override void LoadContent()
        {
            _backGroundSprite = SpriteFactory.CreateUISprite(Position, _backGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back), scale: _scale);

            if (ActiveQuest == null)
                return;

            ChildSections.Clear();
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));


            StackRow row1 = new StackRow((int)(_backGroundSprite.Width * _scale.X));
            List<Text> titleText = new List<Text>() { TextFactory.CreateUIText(ActiveQuest.Name, GetLayeringDepth(UILayeringDepths.High)) };

            NineSliceTextButton titleButton = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), titleText,
                null, centerText: false, hoverTransparency: false)
            {IgnoreDefaultHoverSoundEffect = true};
            row1.AddItem(titleButton, StackOrientation.Center);
            _stackPanel.Add(row1);






            StackRow stackRow = new StackRow((int)(_backGroundSprite.Width * _scale.X));
            List<Text> text = new List<Text>() { TextFactory.CreateUIText(ActiveQuest.Steps[ActiveQuest.CurrentStep].StartText, GetLayeringDepth(UILayeringDepths.High), _paragraphTextSize) };
            text[0].SetFullString(text[0].WrapAutoText((int)(_backGroundSprite.Width * _scale.X - 16)));

            NineSliceTextButton btn = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), text,
                null, forcedWidth: (int)(_backGroundSprite.Width * _scale.X), forcedHeight: (int)(text[0].TotalStringHeight), centerText: false)
            { Displaybackground = false,IgnoreDefaultHoverSoundEffect = true };
            stackRow.AddItem(btn, StackOrientation.Left);
            _stackPanel.Add(stackRow);





            GenerateRequirementTextRows();

        }

        private void GenerateRequirementTextRows()
        {

            foreach(PreRequisite requirement in ActiveQuest.Steps[ActiveQuest.CurrentStep].PreRequisites)
            {
                StackRow stackRowDescription = new StackRow((int)(_backGroundSprite.Width * _scale.X));
                List<Text> requirementDescriptionText = new List<Text>() { TextFactory.CreateUIText($"Return to {ActiveQuest.Steps[ActiveQuest.CurrentStep].TurnInto} with", GetLayeringDepth(UILayeringDepths.High), _paragraphTextSize) };
                requirementDescriptionText[0].SetFullString(requirementDescriptionText[0].WrapAutoText((int)(_backGroundSprite.Width * _scale.X - 16)));

                NineSliceTextButton requirementDescriptionButton = new NineSliceTextButton(_stackPanel, graphics, content, Position,
                    GetLayeringDepth(UILayeringDepths.Medium), requirementDescriptionText,
                    null, forcedWidth: (int)(_backGroundSprite.Width * _scale.X), forcedHeight: (int)(requirementDescriptionText[0].TotalStringHeight + 16), centerText: true)
                { Displaybackground = false, IgnoreDefaultHoverSoundEffect = true };
                stackRowDescription.AddItem(requirementDescriptionButton, StackOrientation.Left);
                _stackPanel.Add(stackRowDescription);
                foreach (QuestItemRequirement itemReq in requirement.ItemRequirements)
                {
                    StackRow stackRow = new StackRow((int)(_backGroundSprite.Width * _scale.X));
                    List<Text> requirementText = new List<Text>() { TextFactory.CreateUIText($"{itemReq.ItemName} : x{itemReq.Count}", GetLayeringDepth(UILayeringDepths.High), _paragraphTextSize) };
                    requirementText[0].SetFullString(requirementText[0].WrapAutoText((int)(_backGroundSprite.Width * _scale.X - 16)));

                    NineSliceTextButton requirementButton = new NineSliceTextButton(_stackPanel, graphics, content, Position,
                        GetLayeringDepth(UILayeringDepths.Medium), requirementText,
                        null, forcedWidth: (int)(_backGroundSprite.Width * _scale.X), forcedHeight: (int)(requirementText[0].TotalStringHeight + 16), centerText: true)
                    { Displaybackground = false, IgnoreDefaultHoverSoundEffect = true };
                    stackRow.AddItem(requirementButton, StackOrientation.Left);
                    _stackPanel.Add(stackRow);


                }
            }

            
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                _backGroundSprite.Update(gameTime, Position);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsActive)
            {
                _backGroundSprite.Draw(spriteBatch);
            }
        }

       
    }
}
