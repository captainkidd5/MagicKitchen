﻿using DataModels;
using DataModels.DialogueStuff;
using DataModels.NPCStuff;
using DataModels.QuestStuff;
using Globals.Classes;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using IOEngine.Classes;
using ItemEngine.Classes;
using ItemEngine.Classes.StorageStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.Components;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace UIEngine.Classes.TextStuff
{
    internal class TalkingWindow : MenuSection
    {
        private static readonly int s_portraitWidth = 96;
        private Rectangle _backgroundSourceRectangle = new Rectangle(64, 496, 272, 64);
        private Sprite BackdropSprite { get; set; }
        private TextBuilder TextBuilder { get; set; }

        public Direction DirectionPlayerShouldFace { get; set; }

        private Vector2 _textOffSet = new Vector2(16, 16);

        private Vector2 _scale = new Vector2(3f, 3f);

        private StackPanel _stackPanel;

        private Dialogue _curerentDialogue;

        private int _curerentDialogueIndex;

        private Rectangle _goToNextDialogueButtonSourceRectangle = new Rectangle(144, 0, 32, 16);
        private Button _goToNextDialogueButton;
        private bool _displayNextButton;
        internal List<TalkingOptionPanel> OptionWindows { get; set; }

        private Vector2 _portraitSpritePosition;
        private Sprite _portraitSprite;
        private Sprite _potraitFrameSprite;

        private StackPanel _tabsStackPanel;
        private NineSliceTextButton _talkTab;
        private NineSliceTextButton _questTab;


        private NineSliceTextButton _nameTab;

        private Quest _activeTalkedQuest;

        public TalkingWindow(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
           base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            RegisterCommands();

        }
        public override void LoadContent()
        {


            Position = RectangleHelper.PlaceBottomCenterScreen(RectangleHelper.RectangleToScale(_backgroundSourceRectangle, _scale));
            Position = new Vector2(Position.X, Position.Y - Settings.Gutter);

            BackdropSprite = SpriteFactory.CreateUISprite(Position, _backgroundSourceRectangle,
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back), scale: _scale);
            TextBuilder = new TextBuilder("Dialogue Test", Position, null, null, GetLayeringDepth(UILayeringDepths.Front), scale:2f);

            Deactivate();


            TotalBounds = BackdropSprite.HitBox;

            _curerentDialogueIndex = 0;
            _goToNextDialogueButton = new Button(null, graphics, content,
                RectangleHelper.PlaceRectangleAtBottomRightOfParentRectangle(TotalBounds, _goToNextDialogueButtonSourceRectangle),
                GetLayerDepth(Layers.midground), _goToNextDialogueButtonSourceRectangle, GoToNext);
            _portraitSpritePosition = new Vector2(Position.X + TotalBounds.Width - s_portraitWidth * 2, Position.Y - s_portraitWidth * 2);

            AddTabs(null);
            base.LoadContent();
        }

        /// <summary>
        /// Talk, Quest, and Shop, if available
        /// </summary>
        private void AddTabs(NPCData npcData)
        {
            ChildSections.Clear();
            Vector2 tabsStackPanelPosition = new Vector2(Position.X, Position.Y - 64);
            _tabsStackPanel = new StackPanel(this, graphics, content, tabsStackPanelPosition, GetLayeringDepth(UILayeringDepths.Low));
            StackRow _tabStackRow = new StackRow(TotalBounds.Width);
            _talkTab = new NineSliceTextButton(_tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                new List<Text>() { TextFactory.CreateUIText("Talk", GetLayeringDepth(UILayeringDepths.Medium),scale:2f) }, SwitchToTalkTab,
                forcedWidth: 128, forcedHeight: 64, centerTextHorizontally: true, centerTextVertically: true);
            _tabStackRow.AddItem(_talkTab, StackOrientation.Left);

            _questTab = new NineSliceTextButton(_tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
                new List<Text>() { TextFactory.CreateUIText("Quest", GetLayeringDepth(UILayeringDepths.Medium), scale: 2f) }, SwitchToQuestTab,
                forcedWidth: 128, forcedHeight: 64, centerTextHorizontally: true, centerTextVertically: true);
            _tabStackRow.AddItem(_questTab, StackOrientation.Left);

            //Width here should be the same width as the scaled portrait
            _tabStackRow.AddSpacer(new Rectangle(0, 0, 192, 64), StackOrientation.Right);
            _nameTab = new NineSliceTextButton(_tabsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
              new List<Text>() { TextFactory.CreateUIText(npcData == null ? "Name" : npcData.Name, GetLayeringDepth(UILayeringDepths.Medium), scale: 2f) }, SwitchToQuestTab,
              forcedWidth: 160, forcedHeight: 64, centerTextHorizontally: true, centerTextVertically: true);
            _tabStackRow.AddItem(_nameTab, StackOrientation.Right);

            _tabsStackPanel.Add(_tabStackRow);
        }

        private StackPanel _questButtonsStackPanel;
        private List<NineSliceTextButton> _availableQuests;

        private void SwitchToTalkTab()
        {
            LoadNewConversation(CurrentNPCTalkingTo, Scheduler.GetScheduleFromCurrentTime(CurrentNPCTalkingTo.Name).Dialogue);
        }
        private void SwitchToQuestTab()
        {
            TextBuilder.ClearCurrent();

            //Find all quests which start with this NPC
            List<Quest> quests = UI.QuestLog.QuestLoader.AllQuests.Values.Where(x =>
            x.Steps.First().Value.AcquiredFrom.ToLower() == CurrentNPCTalkingTo.Name.ToLower()).ToList();

            //And make sure that quest isn't already completed
            quests = quests.Where(x => SaveLoadManager.CurrentSave.GameProgressData.QuestProgress.ContainsKey(x.Name) &&
            !SaveLoadManager.CurrentSave.GameProgressData.QuestProgress[x.Name].Completed).ToList();
            ClearQuestStackPanel();
            _questButtonsStackPanel.Activate();
            _availableQuests = new List<NineSliceTextButton>();
            StackRow explanationRow = new StackRow(TotalBounds.Width);

            NineSliceTextButton explBtn = new NineSliceTextButton(_questButtonsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium),
                  new List<Text>() { TextFactory.CreateUIText(quests.Any(x => !x.Completed) ? "Talk about which quest?" : "No available quests", GetLayeringDepth(UILayeringDepths.High),scale:2f) },
                  null, centerTextHorizontally: true, centerTextVertically:true)
            { Displaybackground = false, IgnoreDefaultHoverSoundEffect = true,IgnoreDefaultClickSoundEffect= true };

            explanationRow.AddItem(explBtn, StackOrientation.Center);
            _questButtonsStackPanel.Add(explanationRow);
            foreach (Quest quest in quests)
            {
                if (quest.Completed)
                    continue;
                StackRow stackRow = new StackRow(TotalBounds.Width);
                NineSliceTextButton btn = new NineSliceTextButton(_questButtonsStackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium),
                    new List<Text>() { TextFactory.CreateUIText(quest.Name, GetLayeringDepth(UILayeringDepths.High)) },
                    new Action(() => { DetermineNextQuestDialogue(quest); }));
                stackRow.AddItem(btn, StackOrientation.Center);
                _questButtonsStackPanel.Add(stackRow);
            }


            //Dialogue dialogue = new Dialogue() { DialogueText = new Dictionary<int, DSnippet>() { };  }

        }

        private void ClearQuestStackPanel()
        {
            if (ChildSections.Contains(_questButtonsStackPanel))
                ChildSections.Remove(_questButtonsStackPanel);

            _questButtonsStackPanel = new StackPanel(this, graphics, content, new Vector2(Position.X, Position.Y + 32), GetLayeringDepth(UILayeringDepths.Low));



        }
        private void DetermineNextQuestDialogue(Quest quest)
        {
            _activeTalkedQuest = quest;
            _questButtonsStackPanel.Deactivate();

            var completedQuests = SaveLoadManager.CurrentSave.GameProgressData.CompletedQuests();
            QuestStep questStep = quest.Steps[quest.CurrentStep];

            StorageContainer playerStorageContainer = UI.StorageDisplayHandler.PlayerInventoryDisplay.StorageContainer;

            Dictionary<string, int> storedItems = new Dictionary<string, int>();



            bool satisfied = true;
            foreach (var preReq in questStep.PreRequisites)
            {
                if (!preReq.Satisfied(playerStorageContainer.GetItemStoredAsDictionary(), completedQuests))
                    satisfied = false;
            }

            if (satisfied)
            {
                foreach (var preReq in questStep.PreRequisites)
                {
                    if (preReq.ItemRequirements != null)
                    {
                        //Remove handed-in items from player inventory
                        foreach (var itemReq in preReq.ItemRequirements)
                        {
                            int countToRemove = itemReq.Count;

                            playerStorageContainer.RemoveItem(itemReq.ItemName, ref countToRemove);
                        }
                    }
                }
                RewardPlayer(questStep.QuestReward, playerStorageContainer);
                UI.CentralAlertQueue.AddTextToQueue($"Quest Complete!:{quest.Steps[quest.CurrentStep].StepName}", 2f);
                quest.IncrementStep();

                MoveToNextQuestStep(quest, true);
            }
            else
            {
                LoadInQuestHelpText(quest);
            }

        }

        private void RewardPlayer(QuestReward questReward, StorageContainer playerStorageContainer)
        {

            foreach (KeyValuePair<string, int> item in questReward.Items)
            {
                int countToAdd = item.Value;
                playerStorageContainer.AddItem(ItemFactory.GetItem(item.Key), ref countToAdd);
            }
        }
        private void MoveToNextQuestStep(Quest quest, bool isEnd = false)
        {
            var snippets = new Dictionary<int, DSnippet>();
            string dialogueText = string.Empty;
            if (isEnd)
                dialogueText = quest.Steps[quest.CurrentStep - 1].EndText;
            else
                dialogueText = quest.Steps[quest.CurrentStep].StartText;

            string goToResult = string.Empty;
            if (quest.Completed)
                goToResult = "End";
            else
                goToResult = "questStep";
            DSnippet snippet = new DSnippet() { DialogueText = dialogueText, GoTo = goToResult };
            snippets.Add(0, snippet);
            Dialogue dialogue = new Dialogue() { DialogueText = snippets };
            _curerentDialogueIndex = 0;
            
            LoadNewConversation(CurrentNPCTalkingTo, dialogue);


        }
        private void LoadInQuestHelpText(Quest quest)
        {

            var snippets = new Dictionary<int, DSnippet>();
            DSnippet snippet = new DSnippet() { DialogueText = quest.Steps[quest.CurrentStep].HelpText };
            snippets.Add(0, snippet);
            Dialogue dialogue = new Dialogue() { DialogueText = snippets };
            _curerentDialogueIndex = 0;
            LoadNewConversation(CurrentNPCTalkingTo, dialogue);
        }
        public void RegisterCommands()
        {
            CommandConsole.RegisterCommand("talk", "displays given text as speech", AddSpeechCommand);
        }
        private void AddSpeechCommand(string[] args)
        {
            DirectionPlayerShouldFace = Direction.Down;
            string totalString = string.Empty;
            foreach (string arg in args)
            {
                totalString += " ";
                totalString += arg;
            }
            //  LoadNewConversation(totalString);
        }

        private bool _selectNextActionJustOccurred = false;
        public override void Update(GameTime gameTime)
        {

            if (IsActive)
            {
                base.Update(gameTime);

                if (_portraitSprite != null)
                {
                    _potraitFrameSprite.Update(gameTime, _portraitSpritePosition);
                    _portraitSprite.Update(gameTime, _portraitSpritePosition);

                }
                _displayNextButton = TextBuilder.IsComplete;

                if (_displayNextButton)
                    _goToNextDialogueButton.Update(gameTime);
                Hovered = Controls.IsHovering(ElementType.UI, BackdropSprite.HitBox);

                if (!_selectNextActionJustOccurred && Hovered && Controls.IsClicked || Controls.WasGamePadButtonTapped(GamePadActionType.Select))
                {
                    TextBuilder.ForceComplete();
                }
                if (TextBuilder.Update(gameTime, Position + _textOffSet, BackdropSprite.HitBox.Width))
                {
                    //end of text reached
                    if (Controls.IsClickedWorld || Controls.WasGamePadButtonTapped(GamePadActionType.Cancel))
                    {
                        UI.ReactiveSections();
                        Deactivate();
                        _curerentDialogueIndex = 0;


                    }
                }
              
            }


            _selectNextActionJustOccurred = false;


        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (IsActive)
            {
                base.Draw(spriteBatch);

                if (_displayNextButton)
                    _goToNextDialogueButton.Draw(spriteBatch);

                if (_portraitSprite != null)
                {
                    _potraitFrameSprite.Draw(spriteBatch);
                    _portraitSprite.Draw(spriteBatch);

                }
                BackdropSprite.Draw(spriteBatch);
                TextBuilder.Draw(spriteBatch);
            }


        }
        public override void Deactivate()
        {
            UI.ReactiveSections();
            _curerentDialogueIndex = 0;
            _portraitSprite = null;
            TextBuilder.ClearCurrent();
            base.Deactivate();

        }
        private void GoToNext()
        {
            string goToResult = _curerentDialogue.DialogueText[_curerentDialogueIndex].GoTo;

            int newIndex = 0;

            if (int.TryParse(goToResult, out newIndex))
            {
                _curerentDialogueIndex = newIndex;
                LoadNewConversation(CurrentNPCTalkingTo, _curerentDialogue);
            }

            else if (goToResult == "End")
            {
                Deactivate();
                return;
            }
            else if (goToResult == "questStep")
            {
                MoveToNextQuestStep(_activeTalkedQuest);
            }


        }
        public NPCData CurrentNPCTalkingTo { get; set; }
        public void LoadNewConversation(NPCData npcData, Dialogue dialogue)
        {
            AddTabs(npcData);

            _selectNextActionJustOccurred = true;

            CurrentNPCTalkingTo = npcData;
            ClearQuestStackPanel();

            _portraitSprite = SpriteFactory.CreateUISprite(_portraitSpritePosition, new Rectangle(dialogue.DialogueText[_curerentDialogueIndex].PortraitIndexX * s_portraitWidth,
                dialogue.DialogueText[_curerentDialogueIndex].PortraitIndexY * s_portraitWidth, s_portraitWidth, s_portraitWidth),
                UI.PortraitsManager.PortraitsTexture, GetLayeringDepth(UILayeringDepths.Medium), scale: new Vector2(2f, 2f));

            _potraitFrameSprite = SpriteFactory.CreateUISprite(_portraitSpritePosition, new Rectangle(0,0, s_portraitWidth, s_portraitWidth),
              UI.PortraitsManager.PortraitsTexture, GetLayeringDepth(UILayeringDepths.High), scale: new Vector2(2f, 2f));

            _curerentDialogue = dialogue;


            TextBuilder.SetDesiredText(dialogue.DialogueText[_curerentDialogueIndex].DialogueText);
            float totalTextHeight = TextBuilder.Height;
            Activate();

            if (ChildSections.Contains(_stackPanel))
                ChildSections.Remove(_stackPanel);

            _stackPanel = new StackPanel(this, graphics, content, new Vector2(Position.X, Position.Y + totalTextHeight), GetLayeringDepth(UILayeringDepths.Medium));

            if (dialogue.Options != null)
            {

                foreach (var option in dialogue.Options)
                {
                    StackRow stackRow = new StackRow((int)((float)_backgroundSourceRectangle.Width / 4 * _scale.X));
                    TalkingOptionPanel window = new TalkingOptionPanel(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));
                    window.LoadNewOption(option.Value);
                    stackRow.AddItem(window, StackOrientation.Left);
                    _stackPanel.Add(stackRow);
                }
            }

            UI.DeactivateAllCurrentSectionsExcept(new List<InterfaceSection>() { this, UI.ClockBar });
        }

    }
}
