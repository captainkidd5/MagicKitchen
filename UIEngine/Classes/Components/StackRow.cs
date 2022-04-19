using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.Components
{
    internal class StackRow 
    {
        public int Height { get; private set; }
        private int _maxWidth;
        private int _currentContentWidth = 0;
        public List<InterfaceSection> RowSections { get; set; }


        public StackRow(int stackPanelTotalWidth)
        {
            RowSections = new List<InterfaceSection>();
            _maxWidth = stackPanelTotalWidth;
        }

        public void AddItem(InterfaceSection section)
        {
            if (section.Width + _currentContentWidth > _maxWidth)
                throw new Exception($"Stack row max width exceeded");

            if(section.Height > Height)
                Height = section.Height;
            section.MovePosition(new Vector2(_currentContentWidth, 0));
            RowSections.Add(section);
            _currentContentWidth += section.Width;
        }

        public void AdjustPosition(Vector2 pos)
        {
            foreach(InterfaceSection section in RowSections)
                section.MovePosition(new Vector2(section.Position.X + pos.X, pos.Y));
        }
    }
}
