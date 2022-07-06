using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations.BodyPartStuff;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace UIEngine.Classes.MainMenuStuff.OuterMenuStuff.CreateNewGameStuff
{
    internal class AvatarColorSwapper : AvatarPartSwapper
    {
        private readonly BodyPiece _bodyPiece2;
        private int _skinIndex = 0;
        public AvatarColorSwapper(string text,BodyPiece bodyPiece, BodyPiece bodyPiece2, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(text, bodyPiece, interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _bodyPiece2 = bodyPiece2;
        }

        public override void ForwardAction()
        {
            _skinIndex = ScrollHelper.GetIndexFromScroll(Direction.Down, _skinIndex, SpriteFactory.SkinColors.Count);
            BodyPiece1.ChangeColor(SpriteFactory.SkinColors[_skinIndex]);
            _bodyPiece2.ChangeColor(SpriteFactory.SkinColors[_skinIndex]);


        }

        public override void BackwardsAction()
        {
            _skinIndex = ScrollHelper.GetIndexFromScroll(Direction.Up, _skinIndex, SpriteFactory.SkinColors.Count);
            BodyPiece1.ChangeColor(SpriteFactory.SkinColors[_skinIndex]);
            _bodyPiece2.ChangeColor(SpriteFactory.SkinColors[_skinIndex]);

        }
    }
}
