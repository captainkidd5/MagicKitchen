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
    public class HungerBar : HealthBar
    {
        private readonly Color HungerColor = new Color(233, 164, 142);

        public HungerBar(BarOrientation barOrientation, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth)
            : base(barOrientation,interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            ProgressColor = HungerColor;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
