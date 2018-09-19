//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// little enemies that will be scattered throughout the main levels
	/// they remain stationary
	/// if the player collides with it, it will "grab" it for about a second
	/// </summary>
	public class GrabberEnemy
	{
		public Animation enemy;
		
		private Player player;
		private Rectangle boundingRectangle;
		private Rectangle trueBounds;
		private Vector2 position;

		private bool trapped = false;
		private float time = 0f;
		private float trapTime = 1f;

		private bool checkingMovement = true;//so that the state of one grabber enemy doesn't mess up other grabber enemies

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="Player"></param>
		/// <param name="Position"></param>
		public GrabberEnemy(Player Player, Vector2 Position)
		{
			player = Player;
			enemy = new Animation(ImageTools.LoadTexture2D("Characters/Enemies/venus_man_trap_animation"), 12, false);
			enemy.FrameTime = .02f;
			position = Position;
			boundingRectangle = new Rectangle((int)Position.X, (int)Position.Y, enemy.Width, enemy.Height);
			trueBounds = new Rectangle((int)position.X + 4, (int) position.Y + 27, enemy.Width - 8, 37);
		}

		/// <summary>
		/// update grabber logic
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			//only start timer if the player is trapped
			if (trapped)
				time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			//damage, set player position, and set trapped to true when player collides 
			if (player.BoundingRectangle.Intersects(trueBounds) && time < trapTime)
			{
				//SoundManager.PlaySound("chomp", 1f);

				player.Health.TakeDamage(1);
				player.Position = new Vector2(boundingRectangle.X, boundingRectangle.Y - 35);

				checkingMovement = true;

				trapped = true;
			}
			else if (time >= trapTime)
				trapped = false;//not trapped when timer exeeds limit

			//reset the time to 0 when the player is not near it
			if (!player.BoundingRectangle.Intersects(trueBounds))
			{
				time = 0f;
				checkingMovement = false;
			}

			if (checkingMovement)
			{
				//set if the player can moved
				if (trapped)
				{
					player.MovementEnabled = false;
					player.Direction = 0;
					//player.Velocity = Vector2.Zero;
				}
				else
					player.MovementEnabled = true;
			}
		}

		/// <summary>
		/// draw grabber enemy
		/// </summary>
		/// <param name="gameTime"></param>
		public void Draw(GameTime gameTime)
		{
			if (trapped)
			{
				enemy.PlayAnimation(gameTime, position, SpriteEffects.None);
			}
			else
			{
				enemy.DrawFrame(0, position, SpriteEffects.None);
			}
		}
	}
}
