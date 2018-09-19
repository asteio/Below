//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// first ending
	/// ted dies and is found in the mountains
	/// </summary>
	public class Ending1Screen : GameScreen
	{
		private Texture2D overlay;
		private Color col = Color.Black;

		private Texture2D report;

		/// <summary>
		/// load content
		/// </summary>
		public override void LoadContent()
		{
			SoundManager.PlaySong("Ambient", true);

			report = ImageTools.LoadTexture2D("Graphics/Backgrounds/epilogue_blue");

			overlay = ImageTools.LoadTexture2D("Graphics/Overlays/lightning");

			base.LoadContent();
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
		}

		public override void Update(GameTime gameTime)
		{
			if (ScreenManager.AnyKeyPressed())
				ScreenManager.ChangeScreens(this, new CreditsScreen());

			base.Update(gameTime);
		}

		/// <summary>
		/// draw text and update timer
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			//fade out
			if (col.A > 0)
				col *= .99f;

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(report, Vector2.Zero, Color.White);

			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, "Press any key to continue...", TextTools.CenterTextWithinRect("Press any key to continue...", ScreenManager.SmallFont, new Rectangle(0, 0, 1280, 170)), Color.White);

			ScreenManager.SpriteBatch.Draw(overlay, new Rectangle(0, 0, 1280, 960), col);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}

	}
}
