using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.ButtonStuff
{
    internal class ConfirmationWindow : InterfaceSection
    {
        private Button _confirmButton;
        private Button _cancelButton;
        public ConfirmationWindow(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth, bool suppressParentSection = true) : base(interfaceSection, graphicsDevice, content, position, layerDepth, suppressParentSection)
        {

        }
    }
}
