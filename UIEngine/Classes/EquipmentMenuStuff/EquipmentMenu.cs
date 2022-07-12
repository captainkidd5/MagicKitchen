using DataModels;
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
using static DataModels.Enums;

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
            if(ChildSections.Count == 0)
            {

            TotalBounds = parentSection.TotalBounds;
            Position = new Vector2(TotalBounds.X, TotalBounds.Y);
            EquipmentDisplay = new EquipmentDisplay(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            EquipmentDisplay.LoadContent();

                EquipmentDisplay.AssignControlSectionAtEdge(Direction.Up, parentSection as MenuSection);

                base.LoadContent();
            }

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
        protected override void GiveSectionControl(Direction direction)
        {
            EquipmentDisplay.HasControl = false;
            base.GiveSectionControl(direction);
        }
        internal override void ReceiveControl(MenuSection sender, Enums.Direction direction)
        {
            EquipmentDisplay.ReceiveControl(sender, direction);
            base.ReceiveControl(sender, direction);
        }

    }
}
