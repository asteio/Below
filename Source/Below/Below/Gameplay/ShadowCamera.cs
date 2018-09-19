//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// mysterious entity that will push the character through the main levels
	/// </summary>
	public class ShadowCamera
	{
		private Level level;
		private Texture2D shadow;

		public Vector2 shadowLocation;

		private float camSpeed = 2;
		public float CamSpeed
		{
			set { camSpeed = value; }
		}

		public bool Enabled { get; set; } = false;
		private bool stopped = false;

		public Rectangle BoundingRect { get; private set; }
		public int Direction { get; private set; } = 0;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="Level"></param>
		public ShadowCamera(Level Level)
		{
			level = Level;
			shadowLocation = ScreenManager.Camera.Position + new Vector2(20, 20);
			shadow = ImageTools.LoadTexture2D("Characters/Enemies/shadow_wall");
		}
		
		/// <summary>
		/// update shadow cam logic
		/// </summary>
		public void Update()
		{
			if (camSpeed < 0)
				shadowLocation = new Vector2(ScreenManager.Camera.Position.X + (1280 - 130), 0);

			//movement logic
			if (Enabled && !stopped)
			{
				//set position of shadow wall based on direction
				if (camSpeed < 0)
					shadowLocation = new Vector2(ScreenManager.Camera.Position.X + (1280 - 130), 0);
				else
					shadowLocation = ScreenManager.Camera.Position;

				//actually move the camera
				ScreenManager.Camera.Move(Vector2.UnitX * camSpeed);
			}

			//set the bounding rect based on the vector location
			BoundingRect = new Rectangle((int)shadowLocation.X, (int)shadowLocation.Y, 80, 960);//shadow.Height);

			//damage and stop the shadow cam if it is touching the player
			if (level.Player.BoundingRectangle.Intersects(BoundingRect))
			{
				stopped = true;
				level.Player.Health.TakeDamage(1);
			}
			else
				stopped = false;

			//set the direction data
			if (stopped || !Enabled)
				Direction = 0;
			else
			{
				if (camSpeed > 0)
					Direction = 1;
				else
					Direction = -1;
			}

			//reset player position if it walks through the shadow wall
			if (camSpeed > 0)
			{
				if (level.Player.Position.X < shadowLocation.X)
					level.Player.Position = new Vector2(shadowLocation.X + BoundingRect.Width, level.Player.Position.Y);
			}
			else if (camSpeed < 0)
			{
				if (level.Player.Position.X > shadowLocation.X + BoundingRect.Width)
					level.Player.Position = new Vector2(shadowLocation.X - level.Player.BoundingRectangle.Width, level.Player.Position.Y);
			}
				
		}
		
		/// <summary>
		/// draw the shadow cam
		/// </summary>
		public void Draw()
		{
			if (Enabled)
			{
				ScreenManager.SpriteBatch.Draw(shadow, shadowLocation, Color.White);
			}
		}
		
	}
}
