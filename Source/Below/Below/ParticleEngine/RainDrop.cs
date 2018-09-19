//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// a simpler particle to be used as rain
	/// </summary>
	public class RainDrop
	{
		private Texture2D texture;

		public Vector2 Position { get; private set; }
		private Vector2 velocity;

		private Color color = Color.White;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="Texture"></param>
		/// <param name="Position_"></param>
		/// <param name="Velocity"></param>
		public RainDrop(Texture2D Texture, Vector2 Position_, Vector2 Velocity)
		{
			texture = Texture;
			Position = Position_;
			velocity = Velocity;

			color *= .5f;
		}

		/// <summary>
		/// update position
		/// </summary>
		public void Update()
		{
			Position += velocity;
		}

		/// <summary>
		/// draw raindrop
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, Position, color);
		}
	}
}
