//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// little particle balls that the bosses will launch at the player
	/// </summary>
	public class ShadowBall
	{
		private Player player;

		private ParticleEngine ball;

		private Vector2 position;
		private Vector2 startPos;

		public Vector2 velocity { get; set; }

		private int speed = 3;

		public Rectangle BoundingRectangle { get; private set; }

		public bool IsActive = false;
		public bool Deflected = false;
		public bool Deflectable = true;//prevent player from "catching" the ball

		public ShadowBall(Player Player, Vector2 Velocity, Vector2 StartPos)
		{
			player = Player;
			velocity = Velocity;
			startPos = StartPos;
			position = startPos;

			BoundingRectangle = new Rectangle((int)position.X, (int)position.Y, 16, 16);

			List<Texture2D> parts = new List<Texture2D>();
			parts.Add(ImageTools.LoadTexture2D("Graphics/Particles/s1"));
			parts.Add(ImageTools.LoadTexture2D("Graphics/Particles/s2"));
			parts.Add(ImageTools.LoadTexture2D("Graphics/Particles/s3"));

			ball = new ParticleEngine(parts, position, false);
		}

		/// <summary>
		/// draw the shadowball and handle logic
		/// </summary>
		public void Draw()
		{
			//damage player on impact
			if (player.LocalBounds.Intersects(BoundingRectangle) && IsActive)
				player.Health.TakeDamage(1);
				
			if (position.X < -50 || position.X > 1280+50 || position.Y < -50 || position.Y > 960+50)//!BoundingRectangle.Intersects(new Rectangle(0, 0, 1280, 960))) doesn't seem to be working for some reason
			{
				//reset the ball
				ball.ClearParticles();
				IsActive = false;
				position = startPos;
				if (Deflected)
				{
					Deflect();
					Deflected = false;
				}
				Deflectable = true;
			}

			//set positioning of ball and draw it
			if (IsActive)
			{
				position += velocity;

				BoundingRectangle = new Rectangle((int)position.X, (int)position.Y, 32, 32);
				ball.Update();
				ball.EmitterLocation = position;
				ball.Draw();

				


				if (player.LineOfSight.HasCollided(BoundingRectangle) && BoundingRectangle.IsWithin(player.BoundingRectangle, 50) && Deflectable)
					Deflect();
				
			}
		}

		/// <summary>
		/// spawn shadow ball with the given velocity
		/// </summary>
		public void Spawn()
		{
			IsActive = true;
		}

		/// <summary>
		/// spawn a homing shadow ball that targets the player
		/// </summary>
		public void SpawnHoming()
		{
			if (!IsActive)
			{
				velocity = GetVelocity(speed);
				position = startPos;
				IsActive = true;
			}
		}

		/// <summary>
		/// calculate velocity to get shadowball to player
		/// BUG - too fast when dx or dy is close to 4
		/// NOT BEING USED RIGHT NOW - too many problems
		/// </summary>
		/// <returns></returns>
		public Vector2 GetVelocity(int speed)
		{
			//need to figure out a better way of doing this 
			//ratios seem to have too many problems

			Vector2 velocity = Vector2.Zero;

			float ratio = 0f;
			float x = player.Position.X - position.X;
			float dx = Math.Abs(x);
			float y = player.Position.Y - position.Y;
			float dy = Math.Abs(y);

			bool respectToY = true;

			if (dy > dx)
			{
				ratio = y / x;
				respectToY = true;
			}
			else if (dx > dy)
			{
				ratio = x / y;
				respectToY = false;
			}

			


			if (respectToY)
			{
				if (x >= 0)
					velocity = new Vector2(1, ratio);
				else
					velocity = new Vector2(-1, ratio);
			}
			else
			{
				if (y >= 0)
					velocity = new Vector2(ratio, 1);
				else
					velocity = new Vector2(-ratio, -1);
			}


			

			return velocity * speed;
		}

		/// <summary>
		/// deflect shadow ball to move in the opposite direction
		/// </summary>
		public void Deflect()
		{
			SoundManager.PlaySound("shadowball", 1f);
			Deflected = true;
			velocity *= -1;
			Deflectable = false;
		}

		/// <summary>
		/// reset just the position of the shadow ball
		/// </summary>
		public void Reset()
		{
			position = startPos;
		}
	}
}
