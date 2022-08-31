using DataModels.QuestStuff;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
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
    internal class QuestList : MenuSection
    {
        private StackPanel _stackPanel;
        
        private Rectangle _backGroundSourceRectangle = new Rectangle(336, 304, 112, 256);
        private Sprite _backGroundSprite;

        private Vector2 _scale = new Vector2(2f, 2f);
        public QuestList(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        public override void LoadContent()
        {
            ChildSections.Clear();
            _backGroundSprite = SpriteFactory.CreateUISprite(Position, _backGroundSourceRectangle, UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Back), scale:_scale);
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));


            StackRow row1 = new StackRow((int)(_backGroundSprite.Width * _scale.X));
            List<Text> titleText = new List<Text>() { TextFactory.CreateUIText("Active Quests", GetLayeringDepth(UILayeringDepths.High),scale:1.8f) };

            NineSliceTextButton titleButton = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), titleText,
                null, forcedWidth: (int)(_backGroundSprite.Width * _scale.X), forcedHeight: 60, centerTextHorizontally: true, hoverTransparency: false,
                centerTextVertically: true)
            { IgnoreDefaultHoverSoundEffect = true, };
            row1.AddItem(titleButton, StackOrientation.Left);
            _stackPanel.Add(row1);


            foreach (KeyValuePair<string, Quest> questPair in SaveLoadManager.CurrentSave.GameProgressData.QuestProgress)
            {
                Quest quest = questPair.Value;
                
                StackRow stackRow = new StackRow((int)(_backGroundSprite.Width * _scale.X));
                List<Text> text = new List<Text>() { TextFactory.CreateUIText(quest.ProperName, GetLayeringDepth(UILayeringDepths.High), scale: 1.5f) };

                NineSliceTextButton btn = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), text,
                    new Action (()=> { SetActiveQuest(quest); }), forcedWidth:(int)(_backGroundSprite.Width* _scale.X), forcedHeight: 60,centerTextHorizontally: true, centerTextVertically:true);
                stackRow.AddItem(btn, StackOrientation.Left);
                _stackPanel.Add(stackRow);
            }
 
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)(_backGroundSourceRectangle.Width * _scale.X),
                (int)(_backGroundSourceRectangle.Height * _scale.Y));
        }

       

        private void SetActiveQuest(Quest quest)
        {
            (parentSection as QuestLog).SetActiveQuest(quest);
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
