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
                null, forcedWidth: (int)(_backGroundSprite.Width/2 * _scale.X), forcedHeight: 60, centerText: true, hoverTransparency: false);
            row1.AddItem(titleButton, StackOrientation.Center);
            _stackPanel.Add(row1);






            StackRow stackRow = new StackRow((int)(_backGroundSprite.Width * _scale.X));
            List<Text> text = new List<Text>() { TextFactory.CreateUIText(ActiveQuest.Steps[ActiveQuest.CurrentStep].StartText, GetLayeringDepth(UILayeringDepths.High)) };

            NineSliceTextButton btn = new NineSliceTextButton(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium), text,
                null, forcedWidth: (int)(_backGroundSprite.Width / 2 * _scale.X), forcedHeight: (int)(48), centerText: true)
            { Displaybackground = false };
            stackRow.AddItem(btn, StackOrientation.Left);
            _stackPanel.Add(stackRow);

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
