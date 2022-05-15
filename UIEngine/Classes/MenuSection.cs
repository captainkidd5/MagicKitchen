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

        protected Point CurrentSelectedPoint { get; set; }
        protected InterfaceSection[,] Selectables { get; set; }
        public MenuSection(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content,Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Selectables = new InterfaceSection[3,3];
            CurrentSelectedPoint = new Point(1, 0);
        }

       
        /// <summary>
        /// Selects next selectable based on 2d array
        /// </summary>
        /// <param name="direction"></param>
        protected void SelectNext(Direction direction)
        {
            Point newIndex = CurrentSelectedPoint;
            switch (direction)
            {
                case Direction.Up:
                    newIndex = new Point(newIndex.X, newIndex.Y - 1); 
                    break;
                case Direction.Down:
                    newIndex = new Point(newIndex.X, newIndex.Y + 1);
                    break;
                case Direction.Left:
                    newIndex = new Point(newIndex.X -1, newIndex.Y );

                    break;
                case Direction.Right:
                    newIndex = new Point(newIndex.X + 1, newIndex.Y);

                    break;
                default:
                    break;
            }
            if (!ScrollHelper.InBounds(newIndex, Selectables.GetLength(0), Selectables.GetLength(1)))
                return;

            if(Selectables[newIndex.X, newIndex.Y] != null)
            {
                CurrentSelectedPoint = newIndex;
                CurrentSelected = Selectables[CurrentSelectedPoint.X, CurrentSelectedPoint.Y];
            }
        }

        /// <summary>
        /// Adds new interface section to given index, only if it is in bounds and no section exists there yet
        /// </summary>
        protected void AddSectionToGrid(InterfaceSection section, int x, int y)
        {
            if (!ScrollHelper.InBounds(new Point(x, y), Selectables.GetLength(0), Selectables.GetLength(1)))
                throw new Exception($"Tried to add interface section to invalid 2d array index {x},{y}");

            if (Selectables[x, y] != null)
                throw new Exception($"Overwriting interface section at {x},{y}");

            Selectables[x, y] = section;
        }
        protected void ClearGrid()
        {
            Selectables = new InterfaceSection[Selectables.GetLength(0), Selectables.GetLength(1)];
        }
        public override void Update(GameTime gameTime)
        {
            if (CurrentSelected != null)
                CurrentSelected.IsSelected = true;
            base.Update(gameTime);
           

            if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadUp))
            {
                SelectNext(Direction.Up);
            }
            else if(Controls.WasGamePadButtonTapped(GamePadActionType.DPadDown))
            {
                SelectNext(Direction.Down);

            }
            else if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadLeft))
            {
                SelectNext(Direction.Left);

            }
            else if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadRight))
            {
                SelectNext(Direction.Right);

            }
        }
    }
}
