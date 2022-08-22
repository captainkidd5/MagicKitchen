using Globals.Classes;
using Globals.Classes.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;

namespace UIEngine.Classes.TextStuff
{
    internal class CentralAlertQueue : InterfaceSection
    {

        private Queue<Text> _textQueue;
        private readonly float _textDisplayTime = 5f;
        private readonly float _fadeTime = .5f;
        private SimpleTimer _textDisplayTimer;


        bool fadingIn;
        bool fadingOut;
        public CentralAlertQueue(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            Position = Settings.CenterScreen;
        }
        public override void LoadContent()
        {
            //base.LoadContent();
            _textDisplayTimer = new SimpleTimer(_fadeTime);
            _textQueue = new Queue<Text>();
            CommandConsole.RegisterCommand("cen_alert", "adds central alert to screen", AddCentralAlert);
            AddTextToQueue("clear_buffer", 2f);
        }
        private void AddCentralAlert(string[] args)
        {
            AddTextToQueue(args[0], float.Parse(args[1]));
        }
        public void AddTextToQueue(string str, float scale)
        {
            Text text = TextFactory.CreateUIText(str, GetLayeringDepth(SpriteEngine.Classes.UILayeringDepths.Medium), scale:scale);
            text.Color = Color.Transparent;
            _textQueue.Enqueue(text);
            _textDisplayTimer.SetNewTargetTime(_fadeTime);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (_textQueue.Count > 0)
            {
                if (_textDisplayTimer.Run(gameTime) && !fadingIn)
                {
                    fadingOut = true;
                }

                Text text = _textQueue.ElementAt(0);
                text.Update(new Vector2(Position.X - text.Width /2, Position.Y - text.Height));
                if (fadingIn)
                {
                    text.Color = Color.Lerp(text.Color, Color.White, .1f);

                }
                else if (fadingOut)
                {
                    text.Color = Color.Lerp(text.Color, Color.Transparent, .1f);

                }
                if (text.Color.A >= 245)
                {
                    _textDisplayTimer.SetNewTargetTime(_textDisplayTime);
                    fadingIn = false;

                }
                else if (text.Color == Color.Transparent)
                {
                    MoveToNextText();

                }
            }
        }

        private void MoveToNextText()
        {
            _textQueue.Dequeue();
            fadingOut = false;
            fadingIn = true;
            _textDisplayTimer.SetNewTargetTime(_fadeTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (_textQueue.Count > 0)
                _textQueue.ElementAt(0).Draw(spriteBatch);

        }


    }
}
