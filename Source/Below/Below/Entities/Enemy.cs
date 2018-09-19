//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// enemy that will fly across the screen at the players y level
	/// 
	/// this will punish the player from going too far ahead as it will give
	/// them less reaction time to dodge the enemy
	/// </summary>
	public class Enemy
	{
		private Texture2D enemy;

		private Player player;
		private Vector2 position;
		private Vector2 velocity;

		public Rectangle BoundingRectangle { get; private set; }
		private Random random = new Random();

		private bool isMoving = false;

		private float time = 0f;
		private int speed = 20;
		private int limit = 2;

		private int frequency;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="Player"></param>
		public Enemy(Player Player, int Frequency)
		{

			enemy = ImageTools.LoadTexture2D("Characters/Enemies/swiping_enemy");

			player = Player;
			frequency = Frequency;

			velocity = new Vector2(-1, 0);
			position = new Vector2(1300, player.Position.Y);

			BoundingRectangle = new Rectangle((int)position.X, (int)position.Y, enemy.Width, enemy.Height);
		}
		
		/// <summary>
		/// update enemy spawining logic
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			if (frequency != 0)
			{

				time += (float)gameTime.ElapsedGameTime.TotalSeconds;


				//while the enemy isn't moving, have its y match the players y
				if (!isMoving)
				{
					position.Y = player.Position.Y;
				}

				//random spawn moments
				if (time >= limit)
				{
					isMoving = true;
					position += velocity * speed;
				}


				//as soon as the enemy is off screen, set its position back to on the right side of the screen
				if (position.X < 0 - enemy.Width)
				{
					isMoving = false;
					time = 0f;
					position.X = 1280 + enemy.Width;
					limit = random.Next(2, frequency);//reset random time
				}

				//damage the player if the enemy collides with it
				if (BoundingRectangle.Intersects(player.LocalBounds))
				{
					SoundManager.PlaySound("creature", 1f);
					player.Health.TakeDamage(1);
				}

				//set bounding box
				BoundingRectangle = new Rectangle((int)position.X, (int)position.Y, enemy.Width, enemy.Height);
			}
		}

		/// <summary>
		/// draw the enemy
		/// </summary>
		public void Draw()
		{
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.Draw(enemy, position, Color.White);
			ScreenManager.SpriteBatch.End();
		}
	}
}
