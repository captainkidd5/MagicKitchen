using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.TextStuff
{
    internal class PortraitsManager
    {
        public Texture2D PortraitsTexture { get; private set; }
        public PortraitsManager()
        {

        }
        public void LoadContent(ContentManager content)
        {
            PortraitsTexture = content.Load<Texture2D>("Entities/NPC/Portraits");
        }

    }
}
