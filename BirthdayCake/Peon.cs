using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace BirthdayCake
{
	enum PeonState
	{
		Wandering, Following, Worshipping
	}

    public class Peon : DrawableGameComponent
    {
        SoundEffect sfxWorshipping;
		private Engine game;
		private Vector2 spriteOrigin;

        float internalTimer;

		private Vector2 velocity;

		private float angle;

		private SpriteSheet tex;
        private SpriteSheet glow;

		private PeonState state;

		private Player activePlayer;
        public Player ActivePlayer
        {
            get { return activePlayer; }
            //set { followingPlayer = value; } // shouldn't need this externally
        }
        float opacity;

        const float MAX_FAVOR = 3.0f;
        public float Max_Favor
        {
            get { return MAX_FAVOR; }
        }
        
        static Texture2D favorTex;
        static Texture2D favorBorder;
        float favor;
        public float Favor
        {
            get { return favor; }
            set 
            { 
                favor = value;
                favor = MathHelper.Clamp(favor, 0, 1);
            }
        }

        BoundingCircle boundingCircle;

		/// <summary>
		/// A wider area which other peons will try to avoid entering.
		/// </summary>
		BoundingCircle comfortZone;

        public Vector2 worshipPosition;

		private Vehicle vehicle;

		private PeonManager peonManager;

		private Vector2 lastGoodPositionHack;

        public BoundingCircle BoundingCircle
        {
            get { return boundingCircle; }
            set { boundingCircle = value; }
        }

		public Peon(Engine game, PeonManager manager, Texture2D gfxPlaceholder, Vector2 position)
			: base(game)
		{
			this.game = game;
			this.tex = new SpriteSheet(gfxPlaceholder,new Vector2(21,20),3.0f);
            this.glow = new SpriteSheet(Engine.ContentManager.Load<Texture2D>("glowPeon"), new Vector2(26, 30), 3);

            sfxWorshipping = Engine.ContentManager.Load<SoundEffect>("sound/bleep");

            boundingCircle = new BoundingCircle(position, 10);
			comfortZone = new BoundingCircle(position, 20);
            this.tex.Position = position;
            //opacity = 1.0f;
            //this.gfxPlaceholder.FrameCount = 3;

            favorTex = Engine.ContentManager.Load<Texture2D>("favorBar");
            favorBorder = Engine.ContentManager.Load<Texture2D>("favorBorder");
            this.tex.Origin = new Vector2(10, 10);
            glow.Origin = new Vector2(13, 15);
			// add our peon to the manager which contains all peons
			manager.addPeon(this);
			vehicle = new Vehicle();
			vehicle.Position = position;
            opacity = 0.1f;
			//startFollowingPlayer(Engine.playerOne);
			startWandering();

			this.peonManager = manager;
		}

		public float Angle
		{
			get
			{
				return angle;
			}
		}

		public Vector2 Position
		{
			get
			{
				return vehicle.Position;
			}
			set
			{
				vehicle.Position = value;
			}
		}

		public Vehicle Vehicle
		{
			get
			{
				return vehicle;
			}
		}

		public override void Update(GameTime gt)
		{
			float delta = gt.ElapsedGameTime.Milliseconds / 1000f;

			switch (state)
			{
				case PeonState.Wandering:
					wander(delta);
					break;

				case PeonState.Following:
					follow(delta);
					break;

                case PeonState.Worshipping:
                    goToWorship(delta, worshipPosition);
                    break;
			}
            
			boundingCircle.Position = vehicle.Position;
			comfortZone.Position = vehicle.Position;

			tex.Update(gt);
            glow.Update(gt);
			tex.Position = vehicle.Position;
            glow.Position = tex.Position;
			tex.Rotation = vehicle.Angle - (float)(Math.PI / 2);
            glow.Rotation = tex.Rotation;
			if (Engine.terrain.CheckCollision(vehicle.Position))
			{
				// This is not a great way of doing this at all tbfh
				vehicle.Velocity = -vehicle.Velocity;

				if (state == PeonState.Wandering)
				{
					vehicle.Position = lastGoodPositionHack;
					boundingCircle.Position = lastGoodPositionHack;
					comfortZone.Position = lastGoodPositionHack;
				}
			}
			else
			{
				lastGoodPositionHack = vehicle.Position;
			}

			tex.Update(gt);
			tex.Position = vehicle.Position;
			tex.Rotation = vehicle.Angle - (float)(Math.PI / 2);

            //favor -= 0.01f;

            //favor = (float)(Math.Sin(gt.TotalGameTime.Milliseconds / 100) + 1) / 2;
            

		}

		public void startFollowingPlayer(Player player)
        {
            if (Engine.playerOne.attachedPeons.Contains(this))
            {
                Engine.playerOne.attachedPeons.Remove(this);
                Engine.playerTwo.attachedPeons.Add(this);
            }
            else if (Engine.playerTwo.attachedPeons.Contains(this))
            {
                Engine.playerTwo.attachedPeons.Remove(this);
                Engine.playerOne.attachedPeons.Add(this);
            }

			player.attachPeon(this);

            //    throw new NullReferenceException("player was null");
            vehicle.MaxSpeed = 100.0f;
			state = PeonState.Following;
			activePlayer = player;
		}

		private static Random RND = new Random();

        public void Worship(Vector2 position)
        {
            sfxWorshipping.Play();
            worshipPosition = position;
            state = PeonState.Worshipping;
        }

		public void startWandering()
		{
			state = PeonState.Wandering;
            if(activePlayer != null)
			    activePlayer.deattachPeon(this);
			activePlayer = null; // no active player when we start wandering
			vehicle.MaxSpeed = 80;
			vehicle.Velocity = new PolarVector2(1, (float)(RND.NextDouble() * Math.PI * 2)).Cartesian;
		}

		private void wander(float time)
		{
			Debug.Assert(state == PeonState.Wandering);

			Vector2 accel = vehicle.wander(20, 100, (float)(Math.PI / 10));
			//accel += vehicle.avoidTerrain(Engine.terrain, 3f);

			foreach(Peon p in peonManager.Peons)
			{
				if(!p.Equals(this))
					accel += vehicle.avoidCircle(comfortZone, p.comfortZone);
			}
			// Try to avoid players
			foreach (Player p in Engine.players.Values)
				accel += vehicle.avoidCircle(comfortZone, p.BoundingCircle);

			Vector2 left = vehicle.Position + new PolarVector2(20, (float)(vehicle.Angle - Math.PI / 2)).Cartesian;
			Vector2 right = vehicle.Position + new PolarVector2(20, (float)(vehicle.Angle + Math.PI / 2)).Cartesian;

			bool leftIntersects = Engine.terrain.CheckCollision(left);
			bool rightIntersects = Engine.terrain.CheckCollision(right);

			if (leftIntersects && !rightIntersects)
			{
				accel += vehicle.seek(right);
			}
			else if (rightIntersects && !leftIntersects)
			{
				accel += vehicle.seek(left);
			}

			vehicle.apply(accel, time);

		}
        public void startWorshipping()
        {
            sfxWorshipping.Play();
            state = PeonState.Worshipping;
        }

		private void follow(float time)
		{
			Debug.Assert(state == PeonState.Following);

			Vehicle target = activePlayer.getFollowTarget(this);
			Vector2 accel = vehicle.seek(target);
			// try to avoid other peons
			foreach (Peon p in peonManager.Peons)
			{
				if (!p.Equals(this))
					accel += vehicle.avoidCircle(comfortZone, p.comfortZone);
			}

			// Try to avoid players
			foreach (Player p in Engine.players.Values)
				accel += vehicle.avoidCircle(comfortZone, p.BoundingCircle);

			vehicle.apply(accel, time);
		}

        private void goToWorship(float time, Vector2 position)
        {
            Vector2 diff = position - this.Position;
            angle = (float)Math.Atan2(diff.Y, diff.X);
            Vector2 accel = vehicle.seek(position);
            vehicle.apply(accel, time);
        }

		public override void Draw(GameTime gt)
		{
            if (internalTimer > 10 && opacity != 1.0f)
            {
                opacity += 0.1f;
                internalTimer = 0;
            }
            if (opacity >= 1.0f)
            {
                opacity = 1.0f;
            }
            tex.Opacity = opacity;
            internalTimer++;
            float favorPercent = favor / MAX_FAVOR;
            Vector2 barPos = this.vehicle.Position - new Vector2(0, 8);
            barPos.X = (int)barPos.X;
            barPos.Y = (int)barPos.Y;
            tex.Draw();
            if (state != PeonState.Worshipping)
            {
                if (activePlayer == null)
                {
                    //Engine.spriteBatch.Draw(favorTex, barPos, Color.White);
                }
                else
                {
                    Engine.spriteBatch.Draw(favorTex, barPos - tex.Origin, null, activePlayer.PlayerColor, 0, Vector2.Zero, new Vector2(21.0f * favor, 1), SpriteEffects.None, 0);
                }

                Engine.spriteBatch.Draw(favorBorder, barPos - tex.Origin, Color.White);
            }
            else
            {
                glow.Opacity = (float)(Math.Sin(gt.TotalGameTime.TotalMilliseconds / 350) + 1) / 2;
                glow.Scale = Vector2.One + ((Vector2.One / 4) * glow.Opacity);
                glow.Draw();
            }
		}

		private const float FAVOR_SUB_FOLLOWING = 1 / 4f;
		private const float FAVOR_ADD = 1 / 4f;
		private const float FAVOR_SUB_WANDERING = 1 / 4f;

		/// <summary>
		/// Called when the specified player has performed a combo by the peon
		/// </summary>
		/// <param name="player"></param>
        public void onNearbyCombo(Player player)
        {
			if (state == PeonState.Following)
			{
				if(activePlayer.Equals(player))
				{
					favor = Math.Min(1, favor + FAVOR_ADD);
				}
				else
				{
					favor = Math.Max(0, favor - FAVOR_SUB_FOLLOWING);
					
					// check if the player has become neutral
					if (favor == 0)
						startWandering();
				}
			}
			else if (state == PeonState.Wandering)
			{
				if (activePlayer == null)
					activePlayer = player;

				if (activePlayer.Equals(player))
				{
					favor = Math.Min(1, favor + FAVOR_ADD);

					// if the peon hits max favor then it starts following us
					if (favor >= 1)
						startFollowingPlayer(player);
				}
				else
				{
					// stealing the player from someone else
					favor = Math.Max(0, favor - FAVOR_SUB_WANDERING);
					if (favor == 0)
						activePlayer = null; // remove the active player, peon is neutral
				}
			}
			else if (state == PeonState.Worshipping)
			{
				// TODO: let ppl steal peons when worshiping?
			}

			// favor must always be in range 0..1
			Debug.Assert(favor <= 1 && favor >= 0);
        }
    }
}
