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
        public int SkinIndex { get; private set; } = 0;
        public AvatarColorSwapper(string text,BodyPiece bodyPiece, BodyPiece bodyPiece2, InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : base(text, bodyPiece, interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            _bodyPiece2 = bodyPiece2;
        }

        public override void ForwardAction()
        {
            SkinIndex = ScrollHelper.GetIndexFromScroll(Direction.Down, SkinIndex, SpriteFactory.SkinColors.Count);
            BodyPiece1.ChangeColor(SpriteFactory.SkinColors[SkinIndex]);
            _bodyPiece2.ChangeColor(SpriteFactory.SkinColors[SkinIndex]);


        }

        public override void BackwardsAction()
        {
            SkinIndex = ScrollHelper.GetIndexFromScroll(Direction.Up, SkinIndex, SpriteFactory.SkinColors.Count);
            BodyPiece1.ChangeColor(SpriteFactory.SkinColors[SkinIndex]);
            _bodyPiece2.ChangeColor(SpriteFactory.SkinColors[SkinIndex]);

        }
    }
}
