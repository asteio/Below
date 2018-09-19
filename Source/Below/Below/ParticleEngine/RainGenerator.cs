//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// simple particle engine for rain
	/// </summary>
	public class RainGenerator
	{
		private Texture2D texture;

		private float width;
		private float density;

		private List<RainDrop> rainDrops = new List<RainDrop>();

		private Random r1;
		private Random r2;

		private float time;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="RainTexture"></param>
		/// <param name="Density"></param>
		/// <param name="Width"></param>
		public RainGenerator(Texture2D RainTexture, float Density, float Width)
		{
			texture = RainTexture;
			density = Density;
			width = Width;

			r1 = new Random();
			r2 = new Random();
		}

		/// <summary>
		/// add a new particle
		/// </summary>
		public void addRainParicle()
		{
			double fix = r1.Next();//for some reason there is a graphical glitch if I get rid of this

			rainDrops.Add(new RainDrop(texture, new Vector2(-150 + (float)r1.NextDouble() * width, -15), new Vector2(1, r2.Next(5, 8))));
		}

		/// <summary>
		/// update drop positions
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			while(time > 0)
			{
				time -= 1 / density;
				addRainParicle();
			}

			for (int i = 0; i < rainDrops.Count; i++)
			{
				rainDrops[i].Update();

				if (rainDrops[i].Position.Y > ScreenManager.SCREEN_HEIGHT)
				{
					rainDrops.RemoveAt(i);
					i--;
				}
			}
		}

		/// <summary>
		/// draw each raindrop
		/// </summary>
		public void Draw()
		{
			foreach (RainDrop drop in rainDrops)
			{
				drop.Draw(ScreenManager.SpriteBatch);
			}
		}
	}
}
