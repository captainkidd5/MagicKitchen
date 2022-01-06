using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.TileAddons
{
    /// <summary>
    /// A tile addon which, when attached to a tile, gives it the ability to show the player if the player is ever behind the tile.
    /// This should only be used on tiles which are ever in front of the player (e.a foreground or front layers)
    /// </summary>
    public class TileTransparency : Collidable, ITileAddon
    {

        private readonly float MinOpacity = .2f;
        private readonly float MaxOpcacity = 1f;
        private readonly float Speed = .005f;
        private readonly Rectangle destinationRectangle;


        public Tile Tile { get; set; }
        private float Opacity { get; set; }

        private bool IsTurningTransparent { get; set; }
        private bool IsReturningToOpaque { get; set; }

        public TileTransparency(Tile tile, Vector2 position, Rectangle destinationRectangle)
        {
            Tile = tile;
            Move(new Vector2(position.X + destinationRectangle.Width / 2, position.Y + destinationRectangle.Height / 2));
            this.destinationRectangle = destinationRectangle;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsTurningTransparent)
                TurnTransparent(gameTime);
            
            else if (IsReturningToOpaque)
                ReturnToOpaque(gameTime);
            
        }
        /// <summary>
        /// When player moves behind a tile, turn the tile transparent at rate SPEED until MIN-OPACITY
        /// </summary>
        private void TurnTransparent(GameTime gameTime)
        {
            Opacity -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * Speed;
            if (Opacity < MinOpacity)
            {
                IsTurningTransparent = false;
                Opacity = MinOpacity;
            }


            Tile.ColorMultiplier = Opacity;
        }

        /// <summary>
        /// When player is no longer behind the tile, return it back to its normal opacity at rate SPEED.
        /// </summary>
        private void ReturnToOpaque(GameTime gameTime)
        {
            Opacity += (float)gameTime.ElapsedGameTime.TotalMilliseconds * Speed;
            if (Opacity > MaxOpcacity)
            {
                IsReturningToOpaque = false;
                Opacity = MaxOpcacity;
            }


            Tile.ColorMultiplier = Opacity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Load()
        {
            CreateBody(Position);
            Opacity = 1f;
        }

        public void Destroy()
        {
            Tile.Addons.Remove(this);
        }

        protected override void CreateBody(Vector2 position )
        {
            base.CreateBody(position);
            AddSecondaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, Position, destinationRectangle.Width, destinationRectangle.Height, new List<Category>() { Category.TransparencySensor }, null,
                OnCollides, OnSeparates, isSensor: true, blocksLight: false));
        }

        public void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            IsTurningTransparent = true;
            IsReturningToOpaque = false;

        }

        public void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            IsReturningToOpaque = true;
            IsTurningTransparent = false;
        }

        public void Interact()
        {
         //   throw new NotImplementedException();
        }
    }
}
