using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.Components;

namespace UIEngine.Classes.StatusStuff
{
    public class ManaBar : HealthBar
    {
        private readonly Color ManaColor = new Color(255, 255, 255);

        public ManaBar(BarOrientation barOrientation, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : base(barOrientation,interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            ProgressColor = ManaColor;
        }
    }
}
