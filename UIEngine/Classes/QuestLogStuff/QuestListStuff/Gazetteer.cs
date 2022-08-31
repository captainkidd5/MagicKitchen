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
            _stackPanel = new StackPanel(this, graphics, content, new Vector2(Position.X, Position.Y + 16), GetLayeringDepth(UILayeringDepths.Low));


            StackRow row1 = new StackRow((int)(_backGroundSprite.Width * _scale.X));
            List<Text> titleText = new List<Text>() { TextFactory.CreateUIText(ActiveQuest.ProperName, GetLayeringDepth(UILayeringDepths.High), scale: 3f) };

            NineSliceTextButton titleButton = new NineSliceTextButton(_stackPanel, graphics, content, Position,
                GetLayeringDepth(UILayeringDepths.Medium), titleText,
                null, centerTextHorizontally: true, hoverTransparency: false, centerTextVertically: true)
            {IgnoreDefaultHoverSoundEffect = true, Displaybackground = false, IgnoreDefaultClickSoundEffect = true};
            row1.AddItem(titleButton, StackOrientation.Center);
            _stackPanel.Add(row1);

            StackRow stackRowDescription = new StackRow((int)(_backGroundSprite.Width * _scale.X));
            List<Text> stepDescription = new List<Text>() { TextFactory.CreateUIText
                ($"Part {ActiveQuest.CurrentStep + 1}: {ActiveQuest.Steps[ActiveQuest.CurrentStep].StepName}",
                GetLayeringDepth(UILayeringDepths.High)) };
           // stepDescription[0].ClearAndSet(stepDescription[0].WrapAutoText((int)(_backGroundSprite.Width * _scale.X - 16)));

            NineSliceTextButton stepBtn = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), stepDescription,
                null, centerTextHorizontally: false)
            { Displaybackground = false, IgnoreDefaultHoverSoundEffect = true };
            stackRowDescription.AddItem(stepBtn, StackOrientation.Center);

            _stackPanel.Add(stackRowDescription);



            //StackRow stackRow = new StackRow((int)(_backGroundSprite.Width * _scale.X));
            //List<Text> text = new List<Text>() { TextFactory.CreateUIText(ActiveQuest.Steps[ActiveQuest.CurrentStep].StartText, GetLayeringDepth(UILayeringDepths.High), scale:_paragraphTextSize) };
            //text[0].ClearAndSet(text[0].WrapAutoText((int)(_backGroundSprite.Width * _scale.X - 16)));

            //NineSliceTextButton btn = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), text,
            //    null, forcedWidth: (int)(_backGroundSprite.Width * _scale.X), forcedHeight: (int)(text[0].Height), centerText: false)
            //{ Displaybackground = false,IgnoreDefaultHoverSoundEffect = true };
            //stackRow.AddItem(btn, StackOrientation.Left);
            //_stackPanel.Add(stackRow);





            GenerateRequirementTextRows();

            StackRow spacerRow = new StackRow((int)(_backGroundSprite.Width * _scale.X), 24);
            spacerRow.AddSpacer(new Rectangle(0, 0, 16, 32), StackOrientation.Center);
            _stackPanel.Add(spacerRow);
            StackRow rewardsTextStackRow = new StackRow((int)(_backGroundSprite.Width * _scale.X));
            List<Text> requirementDescriptionText = new List<Text>() { TextFactory.CreateUIText($"Rewards", GetLayeringDepth(UILayeringDepths.High),scale: _paragraphTextSize) };

            NineSliceTextButton rewardsDescriptionButton = new NineSliceTextButton(_stackPanel, graphics, content, Position,
                GetLayeringDepth(UILayeringDepths.Medium), requirementDescriptionText,
                null, forcedWidth: (int)(_backGroundSprite.Width * _scale.X), forcedHeight: (int)(requirementDescriptionText[0].Height + 16), centerTextHorizontally: true)
            { Displaybackground = false, IgnoreDefaultHoverSoundEffect = true };
            rewardsTextStackRow.AddItem(rewardsDescriptionButton, StackOrientation.Center);
            _stackPanel.Add(rewardsTextStackRow);
        }

        private void GenerateRequirementTextRows()
        {

            foreach (PreRequisite requirement in ActiveQuest.Steps[ActiveQuest.CurrentStep].PreRequisites)
            {
                StackRow stackRowDescription = new StackRow((int)(_backGroundSprite.Width * _scale.X));
                List<Text> requirementDescriptionText = new List<Text>() { TextFactory.CreateUIText($"Return to {ActiveQuest.Steps[ActiveQuest.CurrentStep].TurnInto} with", GetLayeringDepth(UILayeringDepths.High), scale: _paragraphTextSize) };
                requirementDescriptionText[0].ClearAndSet(requirementDescriptionText[0].WrapAutoText((int)(_backGroundSprite.Width * _scale.X - 16)));

                NineSliceTextButton requirementDescriptionButton = new NineSliceTextButton(_stackPanel, graphics, content, Position,
                    GetLayeringDepth(UILayeringDepths.Medium), requirementDescriptionText,
                    null, forcedWidth: (int)(_backGroundSprite.Width * _scale.X), forcedHeight: (int)(requirementDescriptionText[0].Height + 16), centerText: true)
                { Displaybackground = false, IgnoreDefaultHoverSoundEffect = true };
                stackRowDescription.AddItem(requirementDescriptionButton, StackOrientation.Left);
                _stackPanel.Add(stackRowDescription);
                foreach (QuestItemRequirement itemReq in requirement.ItemRequirements)
                {
                    StackRow stackRow = new StackRow((int)(_backGroundSprite.Width * _scale.X));
                    List<Text> requirementText = new List<Text>() { TextFactory.CreateUIText($"{itemReq.ItemName} : x{itemReq.Count}", GetLayeringDepth(UILayeringDepths.High), scale: _paragraphTextSize) };
                    requirementText[0].ClearAndSet(requirementText[0].WrapAutoText((int)(_backGroundSprite.Width * _scale.X - 16)));

                    NineSliceTextButton requirementButton = new NineSliceTextButton(_stackPanel, graphics, content, Position,
                        GetLayeringDepth(UILayeringDepths.Medium), requirementText,
                        null, forcedWidth: (int)(_backGroundSprite.Width * _scale.X), forcedHeight: (int)(requirementText[0].Height + 16), centerText: true)
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
