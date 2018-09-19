//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// shows the police report of the missing hikers
	/// </summary>
	public class PoliceReportScreen : GameScreen
	{
		private Texture2D report;

		/// <summary>
		/// load assets
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;
			ScreenOrder.SaveScreen(this);

			report = ImageTools.LoadTexture2D("Graphics/Backgrounds/police report");

			base.LoadContent();
		}

		/// <summary>
		/// unload assets
		/// </summary>
		public override void UnloadContent()
		{
			base.UnloadContent();
		}

		/// <summary>
		/// check for input
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			if (ScreenManager.AnyKeyPressed())
				ScreenManager.ChangeScreens(this, new DateScreen());

			base.Update(gameTime);
		}

		/// <summary>
		/// draw report and text
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{ 

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(report, Vector2.Zero, Color.White);

			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, "Press any key to continue...", TextTools.CenterTextWithinRect("Press any key to continue...", ScreenManager.SmallFont, new Rectangle(0, 0, 1280, 170)), Color.White);

			ScreenManager.SpriteBatch.End();

			base.DrawOverLighting(gameTime);
		}
	}
}
