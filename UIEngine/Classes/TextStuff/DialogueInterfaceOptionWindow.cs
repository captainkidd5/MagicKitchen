using DataModels.DialogueStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.Classes.TextStuff
{
    internal class DialogueInterfaceOptionWindow : InterfaceSection
    {
        public DialogueOption DialogueOption { get; set; }
        public DialogueInterfaceOptionWindow(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
        }


        public void LoadNewOption(Vector2 position, DialogueOption newOption)
        {
            Position = position;
            DialogueOption = newOption;
            LoadContent();
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
