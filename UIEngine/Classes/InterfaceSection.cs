using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
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
    public abstract class InterfaceSection : Component
    {
        internal protected readonly InterfaceSection parentSection;

        internal SectionState State;
        public bool IsActive { get; set; }
        private bool _activeLastFrame { get; set; }

        public bool WasJustActived => IsActive != _activeLastFrame;
        //Some Interface sections can contain other interface sections
        internal protected List<InterfaceSection> ChildSections { get; protected set; }
        internal float LayerDepth { get; private set; }
        internal protected Vector2 Position { get;  set; }
        internal virtual  Rectangle HitBox { get; set; }
        public virtual bool Hovered { get; protected set; }
        private bool _hoveredLastFrame;

        protected bool WasHovered => (_hoveredLastFrame && !Hovered);
        internal virtual protected bool Clicked { get; set; }
        internal virtual protected bool RightClicked { get; set; }


        public InterfaceSection(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(graphicsDevice, content)
        {
            parentSection = interfaceSection;
            Position = position ?? Vector2.Zero;

            ChildSections = new List<InterfaceSection>();
            IsActive = true;
            State = SectionState.None;
            LayerDepth = layerDepth;
            if(interfaceSection != null && interfaceSection.ChildSections != null && !interfaceSection.ChildSections.Contains(this))
            {
                interfaceSection.ChildSections.Add(this);
                LayerDepth = UI.GetChildUILayerDepth(layerDepth);
            }

        }

        public override void Load()
        {

        }

        public override void Unload()
        {
            foreach (InterfaceSection interfaceSection in ChildSections)
                interfaceSection.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            _hoveredLastFrame = Hovered;
            Hovered = false;
            Clicked = false;
            RightClicked = false;

            //baseline check
            if (Controls.IsHovering(ElementType.UI, HitBox))
            {
                Hovered = true;
                if (Controls.IsClicked)
                    Clicked = true;
                if(Controls.IsRightClicked)
                    RightClicked = true;
                return;
            }
            foreach (InterfaceSection section in ChildSections)
            {
                section.Update(gameTime);
                if (section.Hovered)
                {
                    Hovered = true;
                    if (section.Clicked)
                        Clicked = true;
                    if(section.RightClicked)
                        RightClicked= true;
                }
            }
            _activeLastFrame = IsActive;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (InterfaceSection section in ChildSections)
                section.Draw(spriteBatch);
            
        }
        public virtual void Toggle()
        {
            IsActive = !IsActive;
        }

    }
}
