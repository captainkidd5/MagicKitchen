using DataModels.ItemStuff;
using Globals.Classes;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes.Storage.ItemAlerts
{
    internal class ItemAlertManager : InterfaceSection
    {
        private Dictionary<int, ItemAlert> _alerts;
        private Queue<RecipeUnlockAlert> _recipeAlerts;
        private byte _queueCountLastFrame = 0;

        private Vector2 _recipeUnlockPosition;
        private RecipeUnlockAlert _activeUnlockAlert;

        private NineSliceTextButton _newItemUnlockedBox;
        private static readonly string s_itemUnlockedString = "New Recipe(s) Unlocked!";
        public ItemAlertManager(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }


        public void RegisterCommands()
        {
            CommandConsole.RegisterCommand("ial", "Adds test item alert", AddItemAlertCommand);
        }
        private void AddItemAlertCommand(string[] args)
        {
            string name = args[0];
            int count = int.Parse(args[1]);
            AddNormalItemAlert(ItemFactory.GetItem(name), count);
        }

        public void Initialize()
        {
            _alerts = new Dictionary<int, ItemAlert>();
            _recipeAlerts = new Queue<RecipeUnlockAlert>();
            Position = new Vector2(Settings.CenterScreen.X + Settings.NativeWidth / 4, Settings.CenterScreen.Y + 80);
            _recipeUnlockPosition = new Vector2(Settings.CenterScreen.X - 80, 80);
            RegisterCommands();
        }
        public override void LoadContent()
        {
           
        }
        public void AddNormalItemAlert(Item item, int count)
        {
            if (_alerts.ContainsKey(item.Id))
            {
                _alerts[item.Id].Increment(count);

            }
            else
            {
                _alerts.Clear();
                ChildSections.RemoveAll(x => x.GetType() == typeof(ItemAlert));
                ItemAlert alert = new ItemAlert(ItemFactory.GetItemData(item.Id), this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
                alert.LoadContent();
                alert.Increment(count);
                _alerts.Add(item.Id, alert);

            }
        }
        public void AddRecipeUnlockAlert(ItemData itemData)
        {
            RecipeUnlockAlert recipeAlert = new RecipeUnlockAlert(itemData, this, graphics, content, _recipeUnlockPosition, GetLayeringDepth(UILayeringDepths.Low));
            recipeAlert.LoadContent();

            ChildSections.Remove(recipeAlert);
            _recipeAlerts.Enqueue(recipeAlert);

            if (!ChildSections.Contains(_newItemUnlockedBox))
            {
                SoundFactory.PlayEffectPackage("RecipeUnlockAlert");
                Text newItemUnlockedText = TextFactory.CreateUIText(s_itemUnlockedString, GetLayeringDepth(UILayeringDepths.Medium));

                _newItemUnlockedBox = new NineSliceTextButton(this, graphics, content, new Vector2(Settings.CenterScreen.X, 32),
                GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low), new List<Text>() { newItemUnlockedText }, null);
                _newItemUnlockedBox.MovePosition(new Vector2(_newItemUnlockedBox.Position.X - _newItemUnlockedBox.Width / 2, 32));
            }

        }
        public override void Update(GameTime gameTime)
        {
            _queueCountLastFrame = (byte)_recipeAlerts.Count;
            _activeUnlockAlert = _recipeAlerts.FirstOrDefault();

            if (_activeUnlockAlert != null)
            {
                _activeUnlockAlert.Update(gameTime);
                if (_activeUnlockAlert.FlaggedForRemoval)
                    _recipeAlerts.Dequeue();
            }
            //Want to remove new recipe unlocked message only when last item unlock alert fades out. It should be constant through all alerts until
            //that point
            if(_queueCountLastFrame > 0 && _recipeAlerts.Count == 0)
            {
                _newItemUnlockedBox.FadeOut();
            }
            if (_newItemUnlockedBox!= null && _newItemUnlockedBox.IsTransparent)
                ChildSections.Remove(_newItemUnlockedBox);

            KeyValuePair<int, ItemAlert>? item = _alerts.FirstOrDefault(kvp => kvp.Value.FlaggedForRemoval == true);

            if (item != null)
                _alerts.Remove(item.Value.Key);
            base.Update(gameTime);


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_activeUnlockAlert != null)
            {
                _activeUnlockAlert.Draw(spriteBatch);

            }
            base.Draw(spriteBatch);
        }

    }
}
