//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// plays right before main menu
	/// </summary>
	public class SoundSplashScreen : GameScreen
	{
		private string text = "This game is best experienced with the sound on.";

		private float time = 0f;

		/// <summary>
		/// update timer and draw text
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (time > 3)
				ScreenManager.ChangeScreens(this, new MainMenuScreen());

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, text, TextTools.CenterText(ScreenManager.SmallFont, text), Color.White);

			ScreenManager.SpriteBatch.End();

			base.DrawOverLighting(gameTime);
		}
	}
}
