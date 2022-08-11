using DataModels;
using DataModels.DialogueStuff;
using Globals.Classes;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
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
        private Rectangle _backgroundSourceRectangle = new Rectangle(64, 496, 272, 64);
        private Sprite BackdropSprite { get; set; }
        private TextBuilder TextBuilder { get; set; }

        public Direction DirectionPlayerShouldFace { get; set; }

        private Vector2 _textOffSet = new Vector2(16, 16);

        private Vector2 _scale = new Vector2(3f,3f);

        private StackPanel _stackPanel;

        private Dialogue _curerentDialogue;

        private int _curerentDialogueIndex;

        private Rectangle _goToNextDialogueButtonSourceRectangle = new Rectangle(144, 0, 32, 16);
        private Button _goToNextDialogueButton;
        private bool _displayNextButton;
        internal List<TalkingOptionPanel> OptionWindows { get; set; }
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
            TextBuilder = new TextBuilder(TextFactory.CreateUIText("Dialogue Test", GetLayeringDepth(UILayeringDepths.Front), .5f),
                .05f);
            Deactivate();


            TotalBounds = BackdropSprite.HitBox;

            _curerentDialogueIndex = 0;
            _goToNextDialogueButton = new Button(null, graphics, content,
                RectangleHelper.PlaceRectangleAtBottomRightOfParentRectangle(TotalBounds,_goToNextDialogueButtonSourceRectangle),
                GetLayerDepth(Layers.midground), _goToNextDialogueButtonSourceRectangle, GoToNext );
            base.LoadContent();
        }
        public void RegisterCommands()
        {
            CommandConsole.RegisterCommand("talk", "displays given text as speech", AddSpeechCommand);
        }
        private void AddSpeechCommand(string[] args)
        {
            DirectionPlayerShouldFace = Direction.Down;
            string totalString = string.Empty;
            foreach(string arg in args)
            {
                totalString += " ";
                totalString += arg;
            }
          //  LoadNewConversation(totalString);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive)
            {
                _displayNextButton = TextBuilder.IsComplete();

                if (_displayNextButton)
                    _goToNextDialogueButton.Update(gameTime);
                Hovered = Controls.IsHovering(ElementType.UI, BackdropSprite.HitBox);


                if (TextBuilder.Update(gameTime, Position + _textOffSet, BackdropSprite.HitBox.Width))
                {
                    //end of text reached
                    if (Controls.IsClickedWorld || Controls.WasGamePadButtonTapped(GamePadActionType.Cancel))
                    {
                        UI.ReactiveSections();
                        Deactivate();

                    }
                }
                if (Hovered && Controls.IsClicked || Controls.WasGamePadButtonTapped(GamePadActionType.Select))
                {
                    TextBuilder.ForceComplete(BackdropSprite.HitBox.Width);
                }
            }





        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
            if (IsActive)
            {
                if (_displayNextButton)
                    _goToNextDialogueButton.Draw(spriteBatch);
                BackdropSprite.Draw(spriteBatch);
                TextBuilder.Draw(spriteBatch);
            }


        }

        private void GoToNext()
        {
            _curerentDialogueIndex++;
            LoadNewConversation(_curerentDialogue);
        }
        public void LoadNewConversation(Dialogue dialogue)
        {

            _curerentDialogue = dialogue;
            TextBuilder.ClearText();
            Text text = TextFactory.CreateUIText(dialogue.DialogueText[_curerentDialogueIndex], GetLayeringDepth(UILayeringDepths.Front), scale: 1f);
            text.SetFullString(text.WrapAutoText(BackdropSprite.HitBox.Width));



            TextBuilder.SetText(text, BackdropSprite.HitBox.Width,false);
            float totalTextHeight = TextBuilder.GetWidthOfTotalWrappedText(BackdropSprite.HitBox.Width);
            Activate();

            if (ChildSections.Contains(_stackPanel))
                ChildSections.Remove(_stackPanel);
            
            _stackPanel = new StackPanel(this, graphics, content, new Vector2(Position.X, Position.Y + totalTextHeight), GetLayeringDepth(UILayeringDepths.Medium));

            if(dialogue.Options != null)
            {

            foreach(var option in dialogue.Options)
            {
                StackRow stackRow = new StackRow((int)((float)_backgroundSourceRectangle.Width /4 * _scale.X));
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
