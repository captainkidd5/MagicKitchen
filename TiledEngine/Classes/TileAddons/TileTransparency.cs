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

        private readonly Rectangle destinationRectangle;


        public Tile Tile { get; set; }


        public TileTransparency(Tile tile, Vector2 position, Rectangle destinationRectangle)
        {
            Tile = tile;
            Move(new Vector2(position.X + destinationRectangle.Width / 2, position.Y + destinationRectangle.Height / 2));
            this.destinationRectangle = destinationRectangle;
        }

        public override void Update(GameTime gameTime)
        {
   
            
        }


        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Load()
        {
            CreateBody(Position);
            Tile.Sprite.AddFaderEffect(null, null);

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
            if (Tile.Sprite != null)

                Tile.Sprite.TriggerIntensityEffect();
            
        }

        public void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(Tile.Sprite != null)
             Tile.Sprite.TriggerReduceEffect();
        }

        public void Interact()
        {
         //   throw new NotImplementedException();
        }
    }
}
