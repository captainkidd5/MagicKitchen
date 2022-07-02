
using DataModels;
using Globals.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using tainicom.Aether.Physics2D.Dynamics.Joints;
using TiledEngine.Classes.Helpers;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{
    public class GrassTuft : Collidable, ITileAddon
    {


        public TileObject Tile { get; set; }

        public HullBody Tuft { get; set; }

        private readonly Vector2 _tuftOffSet = new Vector2(8, 32);
        public GrassTuft(TileObject tile, Texture2D texture)
        {
           Tile = tile;
            
        }

        public void Load()
        {
            Tile.Sprite.Origin = _tuftOffSet;
            Tile.Sprite.CustomLayer = SpriteUtility.GetYAxisLayerDepth(Tile.Position, Tile.SourceRectangle);
            Tile.Sprite.OffSet = _tuftOffSet;
            Move(Tile.Position + _tuftOffSet);
            CreateBody(Position);


        }

        protected override void CreateBody(Vector2 position)
        {

            
            HullBody body = PhysicsManager.CreateCircularHullBody(BodyType.Static, Position, 4f, new List<Category>() { (Category)PhysCat.Grass }, new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.NPC },
                OnCollides, OnSeparates, isSensor:true);
            Tuft =  PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, new Vector2(Position.X, Position.Y), 8f, 6f, new List<Category>() { (Category)PhysCat.Grass },
                new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.NPC, (Category)PhysCat.Item },
                OnCollides, OnSeparates, isSensor: false);
            Tuft.Body.SetRestitution(6f);
            Tuft.Body.SetFriction(.4f);
            Tuft.Body.Mass = .2f;
            Tuft.Body.AngularDamping = .25f;
            WeldJoint joint = PhysicsManager.Weld(body.Body, Tuft.Body, new Vector2(0f, 1f), new Vector2(0f, 4f), null, null);
            AddPrimaryBody(body);
            AddSecondaryBody(Tuft);
            

        }



        public override void Update(GameTime gameTime)
        {
            Tile.Sprite.Rotation = Tuft.Body.Rotation;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            //Tile.Sprite.Draw(spriteBatch);
        }


        /// <summary>
        /// TODO: Use the entity's direction as the initial shuff direction. If direction is up or down, then do a random direction.
        /// </summary>
        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);

        }

        public void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
      
        }

        public ActionType? Interact(bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)
        {
            return null;
        }



    }
}
