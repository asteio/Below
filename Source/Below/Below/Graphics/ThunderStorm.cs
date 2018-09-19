//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// creates the thunderstorm effect
	/// </summary>
	public class ThunderStorm
	{
		private RainGenerator rain;
		private Texture2D lightning;
		private Random random;

		private float time = 0;
		
		private Color color = Color.Transparent;//the color that will be manipulated to create the lightning effect

		//load sound effects

		/// <summary>
		/// create instance and load content
		/// </summary>
		public ThunderStorm()
		{
			rain = new RainGenerator(ImageTools.LoadTexture2D("Graphics/Particles/raindrop"), 500, ScreenManager.SCREEN_WIDTH + 150);
			lightning = ImageTools.LoadTexture2D("Graphics/Overlays/lightning");
			random = new Random();
		}

		/// <summary>
		/// draw graphical content and randomize lightning strikes
		/// </summary>
		/// <param name="gameTime"></param>
		public void DrawRain(GameTime gameTime)
		{
			rain.Update(gameTime);

			ScreenManager.SpriteBatch.Begin();
			rain.Draw();
			ScreenManager.SpriteBatch.End();
		}

		/// <summary>
		/// draw lighting in seperate function, so it can be drawn over the lighting engine
		/// </summary>
		/// <param name="gameTime"></param>
		public void DrawLightning(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			int ran = random.Next(1, 8000);

			if (time >= ran)
			{
				color = Color.White;//triggers the image to fade out
				time = 0;
			}

			if (color.A >= 0)//if the image is not transparent, fade out to transparency
				color *= 0.95f;

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(lightning, new Rectangle(0, 0, 1280, 960), color);

			ScreenManager.SpriteBatch.End();
		}
	}
}
