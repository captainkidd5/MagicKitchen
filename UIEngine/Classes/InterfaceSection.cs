using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIEngine.Classes
{
    public abstract class InterfaceSection : Component
    {

        public bool IsActive { get; set; }

        internal protected List<InterfaceElement> Elements { get; protected set; }

        //Some Interface sections can contain other interface sections
        internal protected List<InterfaceSection> ChildSections { get; protected set; }
        internal protected float LayerDepth { get; set; }
        protected Vector2 Position { get; set; }
        internal Rectangle HitBox { get; set; }
        public bool Hovered { get; protected set; }
        internal protected bool Clicked { get; set; }

        public InterfaceSection( GraphicsDevice graphicsDevice, ContentManager content , Vector2? position) :
            base(graphicsDevice, content)
        {
            Elements = new List<InterfaceElement>();
            Position = position ?? Vector2.Zero;

            ChildSections = new List<InterfaceSection>();
            IsActive = true;
        }

        public virtual void Load()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            Hovered = false;
            Clicked = false;

            foreach (InterfaceSection element in ChildSections)
            {
                element.Update(gameTime);
                if (element.Hovered)
                {
                    Hovered = true;
                    if (element.Clicked)
                        Clicked = true;
                }
            }
            foreach (InterfaceElement element in Elements)
            {
                element.Update(gameTime);
                if (element.Hovered)
                {
                    Hovered = true;
                    if (element.Clicked)
                        Clicked = true;
                }
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (InterfaceElement element in Elements)
                element.Draw(spriteBatch);
            
        }

        public virtual void AddElement(InterfaceElement interfaceElement)
        {
            Elements.Add(interfaceElement);
        }

        public virtual void RemoveElement(InterfaceElement interfaceElement)
        {
            Elements.Remove(interfaceElement);
        }

        public virtual void Toggle()
        {
            IsActive = !IsActive;
        }

    }
}
