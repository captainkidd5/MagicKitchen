using DataModels;
using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons
{
    /// <summary>
    /// A tile addon which, when attached to a tile, gives it the ability to show the player if the player is ever behind the tile.
    /// This should only be used on tiles which are ever in front of the player (e.a foreground or front layers)
    /// </summary>
    public class TileTransparency : Collidable, ITileAddon
    {

        private readonly Rectangle destinationRectangle;


        public TileObject Tile { get; set; }


        public TileTransparency(TileObject tile, Vector2 position, Rectangle destinationRectangle)
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
            AddSecondaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, Position, destinationRectangle.Width, destinationRectangle.Height,
                new List<Category>() { (Category)PhysCat.TransparencySensor },
                new List<Category>() { (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.Player},
                OnCollides, OnSeparates, isSensor: true, blocksLight: false));
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (Tile.Sprite != null)

                Tile.Sprite.TriggerIntensityEffect();
            
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        public void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(Tile.Sprite != null)
             Tile.Sprite.TriggerReduceEffect();
        }

        public Action Interact(ref ActionType? actionType, bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)
        {
            return null;
        }
    }
}
