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
        public bool IsActive { get; set; }
        private bool _activeLastFrame { get; set; }

        public bool WasJustActived => IsActive != _activeLastFrame;
        //Some Interface sections can contain other interface sections
        internal protected List<InterfaceSection> ChildSections { get; protected set; }
        internal float LayerDepth { get; private set; }
        internal protected Vector2 Position { get; set; }
        private Vector2 _positionLastFrame;
        protected bool DidPositionChange => (Position != _positionLastFrame);
        internal virtual Rectangle HitBox { get; set; }
        public virtual bool Hovered { get; protected set; }
        private bool _hoveredLastFrame;

        protected bool WasHovered => (_hoveredLastFrame && !Hovered);
        internal virtual protected bool Clicked { get; set; }
        internal virtual protected bool RightClicked { get; set; }
        internal virtual void Close() => IsActive = false;

        internal protected bool SupressParentSection { get; set; }

        internal Button CloseButton { get; set; }

        public bool FlaggedForRemoval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suppressParentSection">Set to true if clicking on this section should not trigger parent click</param>
        public InterfaceSection(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth, bool suppressParentSection = true) :
            base(graphicsDevice, content)
        {
            parentSection = interfaceSection;
            Position = position ?? Vector2.Zero;

            ChildSections = new List<InterfaceSection>();
            IsActive = true;
            State = SectionState.None;
            LayerDepth = layerDepth;
            AssignLayeringDepths();

            if (interfaceSection != null && interfaceSection.ChildSections != null && !interfaceSection.ChildSections.Contains(this))
            {
                interfaceSection.ChildSections.Add(this);
                //LayerDepth = UI.IncrementLD(layerDepth);
            }
            SupressParentSection = suppressParentSection;
        }

        private void AssignLayeringDepths()
        {
            LayeringDepths = new float[5];
            float tempDepth = LayerDepth;
            for (int i = 0; i < 5; i++)
            {
                tempDepth = UI.IncrementLD(tempDepth);
                LayeringDepths[i] = tempDepth;
            }
        }
        protected float GetLayeringDepth(UILayeringDepths depth)
        {
            return LayeringDepths[(int)depth];
        }

        public override void LoadContent()
        {

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

                //baseline check
                if (CloseButton != null)
                {
                    CloseButton.Update(gameTime);
                    if (CloseButton.Hovered)
                    {
                        return;
                    }
                }
                if (Controls.IsHovering(ElementType.UI, HitBox))
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

                for (int i = ChildSections.Count - 1; i >= 0; i--)
                {
                    ChildSections[i].Update(gameTime);
                    if (ChildSections[i].FlaggedForRemoval)
                        ChildSections.RemoveAt(i);
                }

            }
            _activeLastFrame = IsActive;
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
            IsActive = !IsActive;
        }

        internal virtual void Reset()
        {
            for (int i = ChildSections.Count - 1; i >= 0; i--)
            {
                ChildSections[i].FlaggedForRemoval = true;
            }
        }
    }
}
