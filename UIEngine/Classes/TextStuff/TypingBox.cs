using UIEngine.Classes.ButtonStuff;
using UIEngine.Classes.DebugStuff;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine.Classes;
using static UIEngine.Classes.UI;
using static Globals.Classes.Settings;
using TextEngine;

namespace UIEngine.Classes.TextStuff
{

    public delegate void ExecuteCommand(string command);


    internal class TypingBox : InterfaceSection
    {
        public static readonly int DefaultWidth = 80;

        public static readonly int DefaultHeight = 32;


        public event ExecuteCommand ExecuteCommand;
        private NineSliceSprite NineSliceSprite { get; set; }
        private NineSliceButton SendButton { get; set; }
        private TextBuilder TextBuilder { get; set; }
        private Text Text { get; set; }
        private List<Keys> AcceptableKeys { get; set; }


        public TypingBox(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2 position, float layerDepth, int? width, int? height, Color? color)
            : base(interfaceSection,graphicsDevice, content, position, layerDepth)
        {
            NineSliceSprite = SpriteFactory.CreateNineSliceSprite(position, width ?? DefaultWidth, height ?? DefaultHeight, UI.ButtonTexture,LayerDepth, color, null, null);
           // SendButton = new NineSliceButton(interfaceSection, graphicsDevice, content,
           //      position,LayerDepth, null, null,null,null);
            TextBuilder = new TextBuilder(TextFactory.CreateUIText(string.Empty, LayerDepth));
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           // SendButton.Update(gameTime);
            AcceptableKeys = Controls.AcceptableKeysForTyping;
            if (AcceptableKeys.Count > 0)
                ProcessKey(gameTime, AcceptableKeys);
            TextBuilder.Update(gameTime,Position);
        }


        protected virtual void OnCommandExecuted(string command)
        {
            ExecuteCommand?.Invoke(command);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //SendButton.Draw(spriteBatch);
            TextBuilder.Draw(spriteBatch);
            NineSliceSprite.Draw(spriteBatch);
        }
        /// <summary>
        /// Determines what value should be given to the Text Builder, if any.
        /// </summary>
        /// <param name="recentlyPressedKeys">Latest key tapped.</param>
        private void ProcessKey(GameTime gameTime, List<Keys> recentlyPressedKeys)
        {

            string keyValue = string.Empty;

            bool wasAnyKeyPressed = false;


            foreach (Keys key in recentlyPressedKeys)
            {

                if ((int)key > 64 && (int)key < 91)
                {
                    keyValue += key.ToString();
                }
                else if ((int)key > 47 && (int)key < 58)
                {
                    keyValue += key.ToString();
                    keyValue = keyValue.TrimStart('D');

                }
                else
                {
                    switch (key)
                    {
                        case Keys.Space:
                            keyValue += " ";
                            break;
                        //TODO: allow backspace to move back to the previous line.
                        case Keys.Back:
                            TextBuilder.BackSpace();
                            break;
                        case Keys.OemPeriod:
                            keyValue += ".";
                            break;
                        case Keys.Subtract:
                            keyValue += "-";
                            break;
                        case Keys.OemMinus:
                            keyValue += "-";
                            break;
                        case Keys.Enter:
                            OnCommandExecuted(TextBuilder.GetText());
                            TextBuilder.ClearText();
                            break;
                        //Up and Down used to scroll through previous commands.
                        //case Keys.Up:
                        //    TextBuilder.SetText(TextFactory.CreateUIText((interfaceSection as CustomConsole).GetPreviousCommand(Direction.Up)));
                        //    break;
                        //case Keys.Down:
                        //    TextBuilder.SetText(TextFactory.CreateUIText((interfaceSection as CustomConsole).GetPreviousCommand(Direction.Down)));
                        //    break;
                        default:
                            break;
                    }
                }

                wasAnyKeyPressed = true;
            }

            if (wasAnyKeyPressed)
                Controls.ClearUseableKeys();

            TextBuilder.Update(gameTime,Position, keyValue, NineSliceSprite.Width);

        }
    }
}
