//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// displays the date of the hiking incident
	/// </summary>
	public class DateScreen : GameScreen
	{
		private float time = 0;

		private string text = "A week earlier...";

		/// <summary>
		/// load the content
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;
			ScreenOrder.SaveScreen(this);

			BackgroundColor = Color.Black;

			base.LoadContent();
		}

		/// <summary>
		/// stop music and unload
		/// </summary>
		public override void UnloadContent()
		{
			SoundManager.StopAll();
			base.UnloadContent();
		}

		/// <summary>
		/// update timer and draw text
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (time > 3)
				ScreenManager.ChangeScreens(this, new ForestScreen());

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.DrawString(ScreenManager.LargeFont, text, TextTools.CenterText(ScreenManager.LargeFont, text), Color.White);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
