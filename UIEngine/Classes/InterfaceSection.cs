﻿using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
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

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            private set
            {
                _isActive = value;
                Hovered = false;
            }
        }
        public bool WasJustActivated => _framesActive < 2;
        private byte _framesActive = 0;
        /// <summary>
        /// UI elements such as escape window should not be re-activated when something like the talking window ends, even though its part of the same UI group. Default is true
        /// </summary>
        public bool NormallyActivated { get; protected set; } = true;

        //Some Interface sections can contain other interface sections
        internal protected List<InterfaceSection> ChildSections { get; protected set; }

        internal float LayerDepth { get; private set; }
        internal protected Vector2 Position { get; set; }
        private Vector2 _positionLastFrame;
        protected bool DidPositionChange => (Position != _positionLastFrame);
        internal virtual Rectangle TotalBounds { get; set; }

        internal int Width => TotalBounds.Width;
        internal int Height => TotalBounds.Height;

        private bool _hovered;
        public virtual bool Hovered { get { return _hovered; } protected set { _hovered = value; if (_hovered) { UI.IsHovered = true; } } }
        private bool _hoveredLastFrame;

        protected bool JustHovered => !_hoveredLastFrame && Hovered;
        protected bool WasHovered => (_hoveredLastFrame && !Hovered);
        internal virtual protected bool Clicked { get; set; }
        internal virtual protected bool RightClicked { get; set; }

        public bool BlockInteractions { get; set; }
        internal Button CloseButton { get; set; }

        public bool FlaggedForRemoval { get; set; }
        public bool FlaggedForCriticalRemoval { get; set; }

        public bool IsSelected { get; set; }
        private bool _wasSelectedLastFrame;

        public bool WasJustSelected => IsSelected && !_wasSelectedLastFrame;
        public bool WasJustUnSelected => !IsSelected && _wasSelectedLastFrame;


        //protected Rectangle BackgroundSourceRectangle { get; set; }

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
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, TotalBounds.Width, TotalBounds.Height);
        }
        public virtual void Activate()
        {
            IsActive = true;
            _framesActive = 0;
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
        /// <summary>
        /// We know this interface section was just activated if frames active is less than 2, increases by 1 every update cycle
        /// </summary>
        protected void CheckFramesActive()
        {
            if (_framesActive < 2)
                _framesActive++;
        }

        protected virtual void DetectControllerSelection()
        {
            if (Controls.ControllerConnected)
                Hovered = IsSelected;
            else
                Hovered = false;
        }
        public virtual void Update(GameTime gameTime)
        {
            _hoveredLastFrame = Hovered;

            DetectControllerSelection();

            if (IsActive)
            {

                _positionLastFrame = Position;
                Clicked = false;
                RightClicked = false;
                BlockInteractions = false;


                if (Controls.IsHovering(ElementType.UI, TotalBounds) ||
                    Hovered ||
                    Controls.TouchControlConnected)
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
                        if (ChildSections[i].Hovered)
                            Hovered = true;

                    }
                    if (ChildSections[i].FlaggedForRemoval)
                        ChildSections.RemoveAt(i);
                }
                CheckFramesActive();
                CheckOveriddenLogic(gameTime);
            }
            _wasSelectedLastFrame = IsSelected;
            //Set to false here. If this is part of another MenuSection,
            //and is selected from there, this will be set to true again
            IsSelected = false;
        }

        /// <summary>
        /// Override this to perform custom logic that depends on things such as IsSelected
        /// </summary>
        protected virtual void CheckOveriddenLogic(GameTime gameTime)
        {

        }
        private void CheckChildHovers()
        {
            if (Controls.TouchControlConnected)
            {
                Hovered = false;

                if (Controls.DidTouchOccurHere(TotalBounds))
                {
                    Hovered = true;
                    Clicked = true;
                    return;
                }
                
            }
 
            Hovered = true;
            if (Controls.IsClicked || Controls.WasGamePadButtonTapped(GamePadActionType.Select))
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
                //if(!ChildSections[i].NormallyActivated)
                ChildSections[i].CleanUp();

            }
        }
    }
}
