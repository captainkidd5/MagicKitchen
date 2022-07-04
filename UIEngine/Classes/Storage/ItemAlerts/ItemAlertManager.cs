using Globals.Classes;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.Storage.ItemAlerts
{
    internal class ItemAlertManager : InterfaceSection
    {
        private Dictionary<int, ItemAlert> _alerts;
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
            AddAlert(ItemFactory.GetItem(name), count);
        }
        public override void LoadContent()
        {
            _alerts = new Dictionary<int, ItemAlert>();
            Position = new Vector2(Settings.CenterScreen.X + Settings.NativeWidth / 4 , Settings.CenterScreen.Y + 80);
            RegisterCommands();
        }
        public void AddAlert(Item item, int count)
        {
            if (_alerts.ContainsKey(item.Id))
            {
                _alerts[item.Id].Increment(count);

            }
            else
            {
                _alerts.Clear();
                ChildSections.Clear();
                ItemAlert alert = new ItemAlert(item, this, graphics, content, Position, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
                alert.LoadContent();
                alert.Increment(count);
                _alerts.Add(item.Id, alert);

            }
        }
        public override void Update(GameTime gameTime)
        {
            KeyValuePair<int, ItemAlert>? item = _alerts.FirstOrDefault(kvp => kvp.Value.FlaggedForRemoval == true);

            if(item != null)
               _alerts.Remove(item.Value.Key);
            base.Update(gameTime);

         
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
