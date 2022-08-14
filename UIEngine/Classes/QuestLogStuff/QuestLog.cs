using DataModels.QuestStuff;
using Globals.Classes;
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
using UIEngine.Classes.QuestLogStuff.QuestListStuff;

namespace UIEngine.Classes.QuestLogStuff
{
    internal class QuestLog : MenuSection
    {
        private QuestList _activeQuestList;
        private Rectangle _totalSourceRectangleBounds;
        private Gazetteer _gazetteer;
        public QuestLoader QuestLoader { get; set; }

        private Dictionary<string, Quest> _quests => SaveLoadManager.CurrentSave.GameProgressData.QuestProgress;
        public QuestLog(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = false;
            Deactivate();
        }
        public override void LoadContent()
        {
            _totalSourceRectangleBounds = new Rectangle(0, 0, Settings.ScreenRectangle.Width/2, Settings.ScreenRectangle.Height  - 64);
            Position = RectangleHelper.CenterRectangleInRectangle(_totalSourceRectangleBounds, Settings.ScreenRectangle);
            Position = new Vector2(Position.X, Position.Y + 48);
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, _totalSourceRectangleBounds.Width, _totalSourceRectangleBounds.Height);
            _activeQuestList = new QuestList(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            _activeQuestList.LoadContent();
            Vector2 gazetteerPos = new Vector2(_activeQuestList.Position.X + _activeQuestList.Width, Position.Y);
            _gazetteer = new Gazetteer(this, graphics, content, gazetteerPos, GetLayeringDepth(UILayeringDepths.Low) );
            _gazetteer.LoadContent();

            // SaveLoadManager.CurrentSave.GameProgressData.StartNewQuest(QuestLoader.AllQuests["A Light in the Dark"]);
            CommandConsole.RegisterCommand("startQ", "Starts quest of given name", StartNewQuest);
            base.LoadContent();

        }
        private void StartNewQuest(string[] args)
        {
            SaveLoadManager.CurrentSave.GameProgressData.StartNewQuest(QuestLoader.AllQuests[args[0]]);
            _activeQuestList.LoadContent();

            UI.CentralAlertQueue.AddTextToQueue($"Quest Started: {QuestLoader.AllQuests[args[0]].Name}", 2f);

        }
        public void SetActiveQuest(Quest quest)
        {
            _gazetteer.ActiveQuest = quest;
            _gazetteer.LoadContent();
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
