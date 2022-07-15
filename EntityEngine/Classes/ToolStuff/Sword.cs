using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.ToolStuff
{
    internal class Sword : Tool
    {
        private float _swingSpeed = 10f;
        public Sword(Item item) : base(item)
        {
            RequiresCharge = false;
            SourceRectangle = Item.GetItemSourceRectangle(item.Id);
        }
        protected override void LoadSprite()
        {
            base.LoadSprite();
            Sprite.Origin = new Vector2(15, 15);
        }

        private void Swing()
        {
        }
        protected override void AlterSpriteRotation(GameTime gameTime)
        {
            Sprite.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * _swingSpeed;

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(Holder.CenteredPosition);
        }
    }
}
