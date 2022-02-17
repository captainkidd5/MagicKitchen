
using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes.Helpers;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Dynamics.Joints;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{
    public class GrassTuft : Collidable, ITileAddon
    {


        public Tile Tile { get; set; }

        public HullBody Tuft { get; set; }

        public GrassTuft(Tile tile, Texture2D texture)
        {
           Tile = tile;
            
        }

        public void Load()
        {
            Tile.Sprite.Origin = new Vector2(8, 32);
            Tile.Sprite.CustomLayer = SpriteUtility.GetYAxisLayerDepth(Tile.Position - Tile.Sprite.Origin, Tile.SourceRectangle);
            Move(new Vector2(Tile.Position.X , Tile.Position.Y ));
            CreateBody(Position);


        }

        protected override void CreateBody(Vector2 position)
        {

            
            HullBody body = PhysicsManager.CreateCircularHullBody(BodyType.Static, Position, 4f, new List<Category>() { Category.Grass }, new List<Category>() { Category.Player, Category.NPC },
                OnCollides, OnSeparates, isSensor:true);
            Tuft =  PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, new Vector2(Position.X, Position.Y), 2f, 4f, new List<Category>() { Category.Grass }, new List<Category>() { Category.Player, Category.NPC,Category.Item },
                OnCollides, OnSeparates, isSensor: false);
            Tuft.Body.Restitution = .1f;
            Tuft.Body.Friction = .4f;
            Tuft.Body.Mass = .61f;
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
        public void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
          

        }

        public void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
      
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }



        ///// <summary>
        ///// Grass has a chance to trigger a jekan battle!
        ///// </summary>
        //private void CheckForBattle()
        //{
        //    if (GlobalUtility.Random.Next(0, 100) < ChanceToTriggerBattle)
        //        Game1.UserInterface.LoadNewBattle(BattleEncounterType.WildEncounter, Game1.JekanHandler.GetNewCreature(JekanStuff.CreatureType.Wavrunt),
        //                Game1.JekanHandler.GetRandomEncounterCreature());

        //}
    }
}
