using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace UIEngine.Classes
{
    public abstract class InterfaceElement : Component
    {

        
        internal protected bool IsActive { get; set; }
        internal protected Vector2 Position { get; protected set; }
        public virtual Rectangle HitBox { get; protected set; }
        internal protected virtual bool Hovered { get; protected set; }

        public virtual bool Clicked { get; private set; }

        internal protected float Layer { get; set; }

        internal protected float Scale { get; set; }

        internal protected readonly InterfaceSection interfaceSection;


        public InterfaceElement(GraphicsDevice graphicsDevice, ContentManager content,
            InterfaceSection interfaceSection, Vector2 position) :
            base(graphicsDevice, content)
        {
            IsActive = true;
            this.interfaceSection = interfaceSection;
            Position = position;
            Layer = SpriteUtility.GetUILayer(SetUILayer());
        }

        protected virtual Layers SetUILayer()
        {
            return Layers.background;
        }
        internal virtual void Remove()
        {
            interfaceSection.Elements.Remove(this);
        }
        public virtual void Update(GameTime gameTime)
        {
            if (IsActive)
            {

            Hovered = false;
            Clicked = false;
            if(Controls.IsHovering(ElementType.UI, HitBox))
            {
                Hovered = true;
                if (Controls.IsClicked)
                    Clicked = true;
            }
            }

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        internal virtual void Toggle()
        {
            IsActive = !IsActive;
        }
    }
}
