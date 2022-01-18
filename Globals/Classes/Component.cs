
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes
{
    /// <summary>
    /// Baseline class to reduce clutter in child classes
    /// </summary>
    public abstract class Component
    {
        protected readonly GraphicsDevice graphics;
        protected readonly ContentManager content;

        public Component(GraphicsDevice graphics, ContentManager content)
        {
            this.graphics = graphics;
            this.content = content;
        }

        public virtual void Load()
        {

        }
        public virtual void Unload()
        {

        }

       
        //public Template(GraphicsDevice graphics, ContentManager content) : base(graphics,content)
        //{ }
    }
}
