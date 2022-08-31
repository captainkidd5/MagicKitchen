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

        //gap between elements, default is no gap
        private int Gap;


        public StackRow(int stackPanelTotalWidth, int gap = 0)
        {
            _rowSections = new List<InterfaceSection>();
            _maxWidth = stackPanelTotalWidth;
            Gap = gap;
        }
        public void AddSpacer(Rectangle rectangle, StackOrientation orientation)
        {
            Place(rectangle, orientation ,false);
        }

        public void AddItem(InterfaceSection section, StackOrientation orientation, bool centerVertically = false)
        {
            Vector2 newPosition = Place(section.TotalBounds, orientation, centerVertically);


            section.MovePosition(newPosition);
            _rowSections.Add(section);
        }

        private Vector2 Place(Rectangle rectangle, StackOrientation stackOrientation, bool centerVertically)
        {
            if (rectangle.Width + _currentContentWidth + Gap > _maxWidth)
                throw new Exception($"Stack row max width exceeded");

            if (rectangle.Height > Height)
                Height = rectangle.Height;

            Vector2 newPos = Vector2.Zero;
            _currentContentWidth += rectangle.Width + Gap;

            switch (stackOrientation)
            {
                case StackOrientation.None:
                    throw new Exception($"Invalid orientation");
                case StackOrientation.Left:
                    if (centerVertically)
                        newPos = new Vector2(_currentX, rectangle.Height / 2 * -1);
                    else
                        newPos = new Vector2(_currentX, 0);

                    _currentX = _currentContentWidth;
                    break;
                case StackOrientation.Center:
                    newPos = new Vector2((_maxWidth / 2) - rectangle.Width / 2, 0);
                    break;
                case StackOrientation.Right:
                    if (_currentX > _currentContentWidth)
                    {
                        //Means we've previously added either a center or right side orientation
                        _currentX = _currentX - rectangle.Width;

                    }
                    else
                    {
                        _currentX = _maxWidth - rectangle.Width;

                    }
                    newPos = new Vector2(_currentX, 0);

                    break;
            }
            return newPos;
        }
       

        public void AdjustPosition(Vector2 pos)
        {
            foreach(InterfaceSection section in _rowSections)
                section.MovePosition(new Vector2(section.Position.X + pos.X,section.Position.Y +  pos.Y));
        }
    }
}
