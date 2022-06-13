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
    internal class MenuSection : InterfaceSection
    {
        protected InterfaceSection CurrentSelected { get; set; }

        protected Point CurrentSelectedPoint { get; set; }
        protected InterfaceSection[,] Selectables { get; set; }

        //Default selected slot index in 2d array. Some slots may be null, such as the 0,0 in the dining table (which starts at 1,1)
        protected Point RestingIndex { get; set; } = new Point(0, 0);

        //If has control, navigating selections with control stick will work
        protected bool HasControl { get; set; } = true;

        protected Dictionary<Direction, MenuSection> TransferSections { get; set; }

        public MenuSection(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Selectables = new InterfaceSection[3, 3];
            CurrentSelectedPoint = new Point(0, 1);
            TransferSections = new Dictionary<Direction, MenuSection>();
        }

        protected void AssignControlSectionAtEdge(Direction direction, MenuSection newSection)
        {
            //if (TransferSections.ContainsKey(direction))
            //    throw new Exception($"Overwriting existing edge direction");
            TransferSections[direction] = newSection;
        }
        internal void ReceiveControl(Direction direction)
        {
            HasControl = true;
            //Todo: make this dependant on direction
            CurrentSelected = Selectables[0, 0];

        }
        private void GiveSectionControl(Direction direction)
        {
            HasControl = false;
            TransferSections[direction].ReceiveControl(Vector2Helper.GetOppositeDirection(direction));
        }
        /// <summary>
        /// Selects next selectable based on 2d array
        /// </summary>
        /// <param name="direction"></param>
        protected virtual void SelectNext(Direction direction)
        {
            Point newIndex = CurrentSelectedPoint;
            switch (direction)
            {
                case Direction.Up:
                    newIndex = new Point(newIndex.X - 1, newIndex.Y);
                    if (newIndex.X >= Selectables.GetLength(0))
                        newIndex = new Point(0, newIndex.Y);
                    if (newIndex.X < 0)
                    {
                        if(TransferSections.ContainsKey(direction))
                        {
                            GiveSectionControl(direction);
                            return;
                        }
                         newIndex = new Point(Selectables.GetLength(0) - 1, newIndex.Y);

                    }
                    break;
                case Direction.Down:
                    newIndex = new Point(newIndex.X + 1, newIndex.Y);

                    if (newIndex.X >= Selectables.GetLength(0))
                    {
                        if (TransferSections.ContainsKey(direction))
                        {
                            GiveSectionControl(direction);

                            return;
                        }
                        newIndex = new Point(0, newIndex.Y);

                    }
                   
                    break;
                case Direction.Left:
                    newIndex = new Point(newIndex.X, newIndex.Y - 1);

                    if(newIndex.Y < 0)
                    {
                        if (TransferSections.ContainsKey(direction))
                        {
                            GiveSectionControl(direction);

                            return;
                        }
                    }
                    break;
                case Direction.Right:
                    newIndex = new Point(newIndex.X, newIndex.Y + 1);
                    if(newIndex.Y >= Selectables.GetLength(1))
                    {
                        if (TransferSections.ContainsKey(direction))
                        {
                            GiveSectionControl(direction);

                            return;
                        }
                    }
                    break;
                default:
                    break;
            }
            if (!ScrollHelper.InBounds(newIndex, Selectables.GetLength(0), Selectables.GetLength(1)))
                return;

            DoSelection(newIndex);
        }
        protected virtual void DoSelection(Point newIndex)
        {

            if (Selectables[newIndex.X, newIndex.Y] != null)
            {
                CurrentSelectedPoint = newIndex;
                CurrentSelected = Selectables[CurrentSelectedPoint.X, CurrentSelectedPoint.Y];
            }
        }
        /// <summary>
        /// Adds new interface section to given index, only if it is in bounds and no section exists there yet
        /// </summary>
        protected void AddSectionToGrid(InterfaceSection section, int row, int column)
        {
            if (!ScrollHelper.InBounds(new Point(row, column), Selectables.GetLength(0), Selectables.GetLength(1)))
                throw new Exception($"Tried to add interface section to invalid 2d array index {row},{column}");

            if (Selectables[row, column] != null)
                throw new Exception($"Overwriting interface section at {row},{column}");

            Selectables[row, column] = section;
        }
        protected void ClearGrid()
        {
            Selectables = new InterfaceSection[Selectables.GetLength(0), Selectables.GetLength(1)];
        }
        protected virtual void CheckButtonTaps()
        {
            if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadUp))
            {
                SelectNext(Direction.Up);
            }
            else if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadDown))
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
        public override void Update(GameTime gameTime)
        {
            if (CurrentSelected != null)
                CurrentSelected.IsSelected = true;
            base.Update(gameTime);
            if (HasControl)
                CheckButtonTaps();
            else
                CurrentSelected = null;


        }
    }
}
