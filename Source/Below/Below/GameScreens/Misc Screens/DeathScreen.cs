//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{

	/// <summary>
	/// screen that displays after death
	/// intentionally a black screen
	/// </summary>
	public class DeathScreen : GameScreen
	{
		private float time = 0;

		/// <summary>
		/// play song
		/// </summary>
		public override void LoadContent()
		{
			SoundManager.PlaySong("Death", false);

			//reset the player
			PlayerData pd = PlayerData.Load();
			pd.Health = 5;
			pd.FlaskUses = 3;
			pd.Save();


			base.LoadContent();
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
		}
		
		/// <summary>
		/// update timer
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (time >= 5)
				ScreenManager.ChangeScreens(this, ScreenOrder.IdToScreen(LevelData.Load().LastScreen));


			base.Update(gameTime);
		}

		/// <summary>
		/// draw text over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.DrawString(ScreenManager.LargeFont, "You died.", TextTools.CenterText(ScreenManager.LargeFont, "You died."), Color.DarkRed);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
