//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// properties of a single particle
	/// </summary>
	public class Particle
	{
		public Texture2D Texture { get; set; }        // The texture that will be drawn to represent the particle
		public Vector2 Position { get; set; }        // The current position of the particle        
		public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
		public float Angle { get; set; }            // The current angle of rotation of the particle
		public float AngularVelocity { get; set; }    // The speed that the angle is changing
		public float Size { get; set; }                // The size of the particle
		public int  LifeTime { get; set; }                // The 'time to live' of the particle

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="angle"></param>
		/// <param name="angularVelocity"></param>
		/// <param name="color"></param>
		/// <param name="size"></param>
		/// <param name="ttl"></param>
		public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, float size, int lifeTime)
		{
			Texture = texture;
			Position = position;
			Velocity = velocity;
			Angle = angle;
			AngularVelocity = angularVelocity;
			Size = size;
			LifeTime = lifeTime;
		}

		/// <summary>
		/// update logic
		/// </summary>
		public void Update()
		{
			LifeTime--;
			Position += Velocity;
			Angle += AngularVelocity;
		}

		/// <summary>
		/// draw particle
		/// </summary>
		public void Draw()
		{
			Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
			Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

			//Color of texture not being modified
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.Draw(Texture, Position, sourceRectangle, Color.White,
				Angle, origin, Size, SpriteEffects.None, 0f);
			ScreenManager.SpriteBatch.End();
		}

	}


}
