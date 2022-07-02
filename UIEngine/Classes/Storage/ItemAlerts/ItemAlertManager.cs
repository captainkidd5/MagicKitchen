using Globals.Classes.Helpers;
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
        private Vector2 _position;
        public ItemAlertManager(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }


       
        public override void LoadContent()
        {
            _alerts = new Dictionary<int, ItemAlert>();
            _position = Vector2Helper.Pla
            base.LoadContent();
        }
        public void AddAlert(int itemId, int count)
        {
            if (_alerts.ContainsKey(itemId))
                _alerts[itemId].Increment(count);
            else
                _alerts.Add(itemId, new ItemAlert(this, graphics, content, Position, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low)));
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
