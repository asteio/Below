//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// splash screen for our "studio"
	/// first splash screen
	/// </summary>
	public class SharkBoysSplashScreen : GameScreen
	{
		private Texture2D logo;

		private float time = 0f;

		/// <summary>
		/// load logo
		/// </summary>
		public override void LoadContent()
		{
			logo = ImageTools.LoadTexture2D("Graphics/Logos/sharkboylogo");

			base.LoadContent();
		}
		
		/// <summary>
		/// update timer and draw logo
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{

			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (time > 3)
				ScreenManager.ChangeScreens(this, new SoundSplashScreen());

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(logo, ImageTools.CenterImage(logo), Color.White);

			ScreenManager.SpriteBatch.End();

			base.DrawOverLighting(gameTime);
		}
	}
}
