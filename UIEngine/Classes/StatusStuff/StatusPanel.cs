using Globals.Classes.Helpers;
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
    public class StatusPanel : InterfaceSection
    {
        public HealthBar HealthBar { get; set; }

        public ManaBar ManaBar { get; set; }

        public HungerBar HungerBar { get; set; }


        private StackPanel _stackPanel;
        public StatusPanel(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,
            Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Position = new Vector2(32,32);
        }

        public override void LoadContent()
        {
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            StackRow _stackRow1 = new StackRow(192);
            HealthBar = new HealthBar(BarOrientation.Horizontal, _stackPanel, graphics, content, Position,
               GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            HealthBar.LoadContent();
            _stackRow1.AddItem(HealthBar,StackOrientation.Left);
            _stackPanel.Add(_stackRow1);

            StackRow _stackRow2 = new StackRow(192);
            _stackRow2.AddSpacer(new Rectangle(0, 0, 64, 16),StackOrientation.Left);
            _stackPanel.Add(_stackRow2);

            StackRow _stackRow3 = new StackRow(192);
            ManaBar = new ManaBar(BarOrientation.Horizontal, _stackPanel, graphics, content, Position,
               GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            ManaBar.LoadContent();

            _stackRow3.AddItem(ManaBar, StackOrientation.Left);
            _stackPanel.Add(_stackRow3);



            StackRow _stackRow4 = new StackRow(192);
            _stackRow4.AddSpacer(new Rectangle(0, 0, 64, 16), StackOrientation.Left);
            _stackPanel.Add(_stackRow4);

            StackRow _stackRow5 = new StackRow(192);
            HungerBar = new HungerBar(BarOrientation.Horizontal, _stackPanel, graphics, content, Position,
               GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Low));
            HungerBar.LoadContent();

            _stackRow5.AddItem(HungerBar, StackOrientation.Left);
            _stackPanel.Add(_stackRow5);

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
