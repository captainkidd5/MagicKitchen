using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIEngine.Classes.ButtonStuff;
using static Globals.Classes.Settings;

namespace UIEngine.Classes
{
    internal enum SectionState
    {
        None = 1,
        Opening = 2,
        Open = 3,
        Closing = 4,
        Closed = 5,

    }

    public abstract class InterfaceSection : Component, IDirtyFlaggable
    {
        internal protected readonly InterfaceSection parentSection;
        protected float[] LayeringDepths;
        internal SectionState State;
        public bool IsActive { get; protected set; }

        /// <summary>
        /// UI elements such as escape window should not be re-activated when something like the talking window ends, even though its part of the same UI group. Default is true
        /// </summary>
        public bool NormallyActivated { get; protected set; } = true;
        private bool _activeLastFrame { get; set; }

        public bool WasJustActived => IsActive != _activeLastFrame;
        //Some Interface sections can contain other interface sections
        internal protected List<InterfaceSection> ChildSections { get; protected set; }
        internal float LayerDepth { get; private set; }
        internal protected Vector2 Position { get; set; }
        private Vector2 _positionLastFrame;
        protected bool DidPositionChange => (Position != _positionLastFrame);
        internal virtual Rectangle TotalBounds { get; set; }
        public virtual bool Hovered { get; protected set; }
        private bool _hoveredLastFrame;

        protected bool WasHovered => (_hoveredLastFrame && !Hovered);
        internal virtual protected bool Clicked { get; set; }
        internal virtual protected bool RightClicked { get; set; }

        public bool BlockInteractions { get; set; }
        internal Button CloseButton { get; set; }

        public bool FlaggedForRemoval { get; set; }
        public bool FlaggedForCriticalRemoval { get; set; }


        public InterfaceSection(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(graphicsDevice, content)
        {
            parentSection = interfaceSection;
            Position = position ?? Vector2.Zero;

            ChildSections = new List<InterfaceSection>();
            IsActive = true;
            State = SectionState.None;
            LayerDepth = layerDepth;
            UI.AssignLayeringDepths(ref LayeringDepths, layerDepth);

            if (interfaceSection != null && interfaceSection.ChildSections != null && !interfaceSection.ChildSections.Contains(this))
            {
                interfaceSection.ChildSections.Add(this);
            }

         
        }
        public virtual void MovePosition(Vector2 newPos)
        {
            Position = newPos;
        }
        public virtual void Activate()
        {
            IsActive = true;
        }
        public virtual void Deactivate()
        {
            CleanUp();
            IsActive = false;
        }
        
        protected float GetLayeringDepth(UILayeringDepths depth)
        {
            return LayeringDepths[(int)depth];
        }

        public override void LoadContent()
        {
            if (TotalBounds.Width == 0 || TotalBounds.Height == 0)
                throw new Exception("Hitbox must be greater than 0");
        }

        public override void Unload()
        {
            foreach (InterfaceSection interfaceSection in ChildSections)
                interfaceSection.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsActive)
            {

                _hoveredLastFrame = Hovered;
                _positionLastFrame = Position;
                Hovered = false;
                Clicked = false;
                RightClicked = false;
                BlockInteractions = false;


                if (Controls.IsHovering(ElementType.UI, TotalBounds))
                {
                  
                 
                        CheckChildHovers();

                    
                    if (CloseButton != null && CloseButton.Hovered)
                    {
                        BlockInteractions = true;

                    }

                }

                for (int i = ChildSections.Count - 1; i >= 0; i--)
                {
                    if (BlockInteractions)
                    {
                        if (ChildSections[i] == CloseButton)
                            ChildSections[i].Update(gameTime);
                        
                    }
                    else
                    {
                        ChildSections[i].Update(gameTime);

                    }
                    if (ChildSections[i].FlaggedForRemoval)
                        ChildSections.RemoveAt(i);
                }

            }
            _activeLastFrame = IsActive;
        }

        private void CheckChildHovers()
        {

                Hovered = true;
                if (Controls.IsClicked)
                {
                    Clicked = true;
                }
                if (Controls.IsRightClicked)
                {
                    RightClicked = true;

                }
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {

                for (int i = ChildSections.Count - 1; i >= 0; i--)
                {
                    ChildSections[i].Draw(spriteBatch);
                }

                if (CloseButton != null)
                    CloseButton.Draw(spriteBatch);
            }

        }
        public virtual void Toggle()
        {
            if (IsActive)
                Deactivate();
            else
                Activate();
        }

        internal virtual void Reset()
        {
            for (int i = ChildSections.Count - 1; i >= 0; i--)
            {
                ChildSections[i].FlaggedForRemoval = true;
            }
        }
        internal virtual void CleanUp()
        {
            for (int i = ChildSections.Count - 1; i >= 0; i--)
            {
                ChildSections[i].CleanUp();
            }
        }
    }
}
