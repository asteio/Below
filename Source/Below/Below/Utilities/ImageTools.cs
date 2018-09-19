//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// any helper methods for images
	/// </summary>
	public static class ImageTools
	{
		/// <summary>
		/// catch any errors when loading a texture
		/// if textue is missing, load placeholder texture so game doesn't crash
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Texture2D LoadTexture2D(string path)
		{
			try
			{
				return ScreenManager.ContentMgr.Load<Texture2D>(path);
			}
			catch (Exception ex)
			{
				//File.AppendAllText("Errors/ImageErrors.txt", "Placeholder was loaded in place of " + path + "\n");
				return ScreenManager.ContentMgr.Load<Texture2D>("placeholder");
			}
		}

		/// <summary>
		/// return vector position that is center screen
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static Vector2 CenterImage_vect(Texture2D image)
		{
			int x = (ScreenManager.SCREEN_WIDTH - image.Width) / 2;
			int y = (ScreenManager.SCREEN_HEIGHT - image.Height) / 2;

			return new Vector2(x, y);
		}

		/// <summary>
		/// return rect position that is center screen
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static Rectangle CenterImage(Texture2D image)
		{
			int x = (ScreenManager.SCREEN_WIDTH - image.Width) / 2;
			int y = (ScreenManager.SCREEN_HEIGHT - image.Height) / 2;

			return new Rectangle(x, y, image.Width, image.Height);
		}

		/// <summary>
		/// extension to determine if texture is visible on screen
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static bool IsOnScreen(this Texture2D texture, Vector2 position)
		{
			Rectangle boundingRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

			if (boundingRect.Intersects(new Rectangle((int)ScreenManager.Camera.Position.X, (int)ScreenManager.Camera.Position.Y, 1280, 960)))
				return true;
			else
				return false; 
		}

		/// <summary>
		/// get a tile source rect from a row and collumn index
		/// </summary>
		/// <param name="row"></param>
		/// <param name="collumn"></param>
		/// <returns></returns>
		public static Rectangle GetSourceRect(int row, int collumn)
		{
			Rectangle source = new Rectangle(collumn * Tile.Size, row * Tile.Size, Tile.Size, Tile.Size);
			return source;
		}


		/// <summary>
		/// calculate a rotation
		/// </summary>
		/// <param name="position"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static float Rotation(Vector2 position, Vector2 target)
		{
			float dx = position.X - target.X;
			float dy = position.Y - target.Y;

			float rotation = (float)Math.Atan(dy / dx);
			if (dx < 0)
				rotation += MathHelper.Pi;
			return rotation;

		}
	}
}
