using Globals.Classes.Helpers;
using InputEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace UIEngine.Classes
{
    internal class MenuSection :InterfaceSection
    {
        private int _currentSelectedIndex;
        protected InterfaceSection CurrentSelected { get; set; }
        protected List<InterfaceSection> Selectables { get; set; }
        public MenuSection(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content,Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Selectables = new List<InterfaceSection>();
        }

       
        protected void SelectNext(Direction direction)
        {
            _currentSelectedIndex = ScrollHelper.GetIndexFromScroll(direction, _currentSelectedIndex, Selectables.Count);

            if (CurrentSelected != null)
                CurrentSelected.IsSelected = false;
            CurrentSelected = Selectables[_currentSelectedIndex];
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentSelected != null)
                CurrentSelected.IsSelected = true;
            base.Update(gameTime);
           

            if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadUp))
            {
                SelectNext(Direction.Down);
            }
            else if(Controls.WasGamePadButtonTapped(GamePadActionType.DPadDown))
            {
                SelectNext(Direction.Up);

            }
        }
    }
}
