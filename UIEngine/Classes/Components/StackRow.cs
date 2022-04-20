using Globals.Classes.Helpers;
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
    public enum StackOrientation
    {
        None = 0,
        Left = 1,
        Center = 2,
        Right = 3,

    }
    internal class StackRow 
    {
        public int Height { get; private set; }
        private int _maxWidth;
        private int _currentContentWidth = 0;
        private int _currentX;
        private List<InterfaceSection> _rowSections;


        public StackRow(int stackPanelTotalWidth )
        {
            _rowSections = new List<InterfaceSection>();
            _maxWidth = stackPanelTotalWidth;
        }

        public void AddItem(InterfaceSection section, StackOrientation orientation)
        {
            if (section.Width + _currentContentWidth > _maxWidth)
                throw new Exception($"Stack row max width exceeded");

            if(section.Height > Height)
                Height = section.Height;

            Vector2 newPos = Vector2.Zero;
            _currentContentWidth += section.Width;

            switch (orientation)
            {
                case StackOrientation.None:
                    throw new Exception($"Invalid orientation");
                case StackOrientation.Left:
                    newPos = new Vector2(_currentX, 0);
                    _currentX = _currentContentWidth;
                    break;
                case StackOrientation.Center:
                    newPos = new Vector2((_maxWidth / 2) - section.Width /2, 0);
                    break;
                case StackOrientation.Right:
                    newPos = new Vector2(_maxWidth - section.Width , 0);

                    break;
            }

            section.MovePosition(newPos);
            _rowSections.Add(section);
        }

        public void AdjustPosition(Vector2 pos)
        {
            foreach(InterfaceSection section in _rowSections)
                section.MovePosition(new Vector2(section.Position.X + pos.X, pos.Y));
        }
    }
}
