//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// when the player wakes up after going in the portal
	/// </summary>
	public class WakeUpScreen : GameScreen
	{
		private Level level;

		private Texture2D overlay;
		private Color col = Color.Black;//start overlay as pure black

		private DialogueBox dbox;
		private bool showDbox = true;

		private ParallaxBackground background;

		/// <summary>
		/// load assets
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;
			ScreenOrder.SaveScreen(this);

			SoundManager.PlaySong("Hello", true);

			level = new Level("Levels/wakeUp.bmp");
			level.LoadLevel();
			level.CamType = "center";
			level.Player.MovementEnabled = false;

			dbox = new DialogueBox("Scripts/wakeUpScript.txt");
			dbox.LoadDBox();

			overlay = ImageTools.LoadTexture2D("Graphics/Overlays/lightning");

			background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_background"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_bottom_parallax"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_top_parallax"));

			base.LoadContent();
		}

		/// <summary>
		/// stop all sounds
		/// </summary>
		public override void UnloadContent()
		{
			SoundManager.StopAll();
			base.UnloadContent();
		}

		/// <summary>
		/// update game logic
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			level.Update(gameTime);

			background.Update(level.Player.Direction, level.CamType);

			//fade out black overlay
			if (col.A > 0)
				col *= .9999999f;
			
			if (col.A == 0 && showDbox)
			{
				dbox.IsActive = true;
				showDbox = false;
			}

			dbox.Update(gameTime);

			if (dbox.Finished)
				ScreenManager.ChangeScreens(this, new LevelScreen());


			base.Update(gameTime);
		}

		/// <summary>
		/// draw graphics
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			background.Draw();

			level.Draw(gameTime);

			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.Draw(overlay, new Rectangle(0, 0, 1280, 960), col);
			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// draw over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			level.DrawOverLighting(gameTime);

			dbox.Draw();
			base.DrawOverLighting(gameTime);
		}
	}
}
