using InputEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.Storage.Configurations;

namespace UIEngine.Classes.EquipmentMenuStuff
{
    internal class EquipmentMenu : MenuSection
    {

        private Vector2 _scale = new Vector2(2f, 2f);

        public EquipmentDisplay EquipmentDisplay;
        public EquipmentMenu(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
           // Deactivate();
        }
        public override void LoadContent()
        {

            TotalBounds = parentSection.TotalBounds;
            Position = new Vector2(TotalBounds.X, TotalBounds.Y);
            EquipmentDisplay = new EquipmentDisplay(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            EquipmentDisplay.LoadContent();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Controls.WasKeyTapped(Keys.C))
                Toggle();
            base.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }

       
    }
}
