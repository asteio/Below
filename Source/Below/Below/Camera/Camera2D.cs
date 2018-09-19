//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// a custom camera that can be scaled and moved
	/// </summary>
	public class Camera2D
	{
		/// <summary>
		/// scaling factor
		/// </summary>
		protected float zoom;
		public float Zoom
		{
			get { return zoom; }
			set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }//dont want zoom to be too small
		}

		/// <summary>
		/// Camera's position
		/// </summary>
		private Vector2 position;
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }
		}
		
		/// <summary>
		/// angle rotation of camera
		/// </summary>
		protected float rotation;
		public float Rotation
		{
			get { return rotation; }
			set { rotation = value; }
		}

		/// <summary>
		/// this is what will be passed to the spritebatch to actually manipulate the camera
		/// </summary>
		public Matrix transform;
		public Matrix GetTransformation(GraphicsDevice graphicsDevice)
		{
			transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
				Matrix.CreateRotationZ(Rotation) *
				Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
				Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0, graphicsDevice.Viewport.Height * 0, 0));

			return transform;

		}

		/// <summary>
		/// move the camera by a set vector
		/// </summary>
		/// <param name="amount"></param>
		public void Move(Vector2 amount)
		{
			position += amount;
		}

		/// <summary>
		/// set the camera's position so it is centered on an object's rectangle position
		/// </summary>
		/// <param name="thing"></param>
		public void CenterOn(Rectangle thing)
		{
			position = new Vector2(thing.X - ((ScreenManager.SCREEN_WIDTH - thing.Width) / 2), thing.Y - ((ScreenManager.SCREEN_HEIGHT - thing.Height) / 2));
		}

		/// <summary>
		/// center camera only on the x axis
		/// </summary>
		/// <param name="thing"></param>
		public void CenterX(Rectangle thing)
		{
			position.X = thing.X - ((ScreenManager.SCREEN_WIDTH - thing.Width) / 2);
		}

		/// <summary>
		/// constructor
		/// </summary>
		public Camera2D()
		{
			zoom = 1.0f;
			rotation = 0.0f;
			position = Vector2.Zero;
		}
	}
}
