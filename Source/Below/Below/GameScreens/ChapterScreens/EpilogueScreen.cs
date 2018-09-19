//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// epilogue title screen
	/// </summary>
	public class EpilogueScreen : GameScreen
	{

		private Texture2D image;
		private float time = 0f;

		/// <summary>
		/// load image
		/// </summary>
		public override void LoadContent()
		{
			ScreenOrder.SaveScreen(this);

			SoundManager.PlaySong("Epilogue", true);

			image = ImageTools.LoadTexture2D("Graphics/Backgrounds/epilogue");

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
		/// update timer and draw the title
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (time > 3f)
				ScreenManager.ChangeScreens(this, new ChoiceScreen());

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(image, Vector2.Zero, Color.White);

			ScreenManager.SpriteBatch.End();

			base.DrawOverLighting(gameTime);
		}
	}
}
