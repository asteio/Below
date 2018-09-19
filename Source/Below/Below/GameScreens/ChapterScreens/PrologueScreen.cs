//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// prologue title screen
	/// </summary>
	public class PrologueScreen : GameScreen
	{
		private Texture2D image;
		private float time = 0f;

		/// <summary>
		/// load image
		/// </summary>
		public override void LoadContent()
		{
			ScreenOrder.SaveScreen(this);

			SoundManager.PlaySong("Police", true);

			image = ImageTools.LoadTexture2D("Graphics/Backgrounds/prologue");

			base.LoadContent();
		}

		/// <summary>
		/// unload content
		/// </summary>
		public override void UnloadContent()
		{
			base.UnloadContent();
		}

		/// <summary>
		/// update timer and draw title
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (time > 3f)
				ScreenManager.ChangeScreens(this, new PoliceReportScreen());

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(image, Vector2.Zero, Color.White);

			ScreenManager.SpriteBatch.End();

			base.DrawOverLighting(gameTime);
		}

	}
}
