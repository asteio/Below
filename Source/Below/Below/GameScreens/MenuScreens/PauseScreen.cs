//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// the screen that will appear when paused
	/// </summary>
	public class PauseScreen : GameScreen
	{
		private Button resumeButton;
		private Button settingsButton;
		private Button exitButton;
		private Button exitToDesktopButton;

		/// <summary>
		/// load buttons
		/// </summary>
		public override void LoadContent()
		{
			resumeButton = new Button();
			resumeButton.Text = "Resume";
			resumeButton.Location(540, 300);

			settingsButton = new Button();
			settingsButton.Text = "Options";
			settingsButton.Location(540, 400);

			exitButton = new Button();
			exitButton.Text = "Exit to Menu";
			exitButton.Location(540, 500);

			exitToDesktopButton = new Button();
			exitToDesktopButton.Text = "Exit Game";
			exitToDesktopButton.Location(540, 600);


			base.LoadContent();
		}
		
		/// <summary>
		/// update buttons
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			resumeButton.Update();
			settingsButton.Update();
			exitButton.Update();
			exitToDesktopButton.Update();

			//if (ScreenManager.IsKeyPressed(Keys.Escape))
			//	ScreenManager.RemoveScreen(this);

			if (resumeButton.Clicked)
				ScreenManager.RemoveScreen(this);

			if (settingsButton.Clicked)
			{
				settingsButton.Clicked = false;
				ScreenManager.AddScreen(new SettingsScreen());
			}
			if (exitButton.Clicked)
			{
				if (MessageBox.Show("Are you sure you want to exit to the main menu?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
					ScreenManager.RemoveAllScreens(new MainMenuScreen());

				exitButton.Clicked = false;
			}
			if (exitToDesktopButton.Clicked)
			{
				ScreenManager.ExitGame = true;

				exitButton.Clicked = false;
			}
			base.Update(gameTime);
		}

		/// <summary>
		/// draw buttons over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{

			ScreenManager.SpriteBatch.Begin();

			Vector2 pos = new Vector2(TextTools.CenterText(ScreenManager.ExtraSmallFont, "Below auto-saves the game whenever a new scene is loaded").X, 50);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, "Below auto-saves the game whenever a new scene is loaded", pos, Color.White);

			ScreenManager.SpriteBatch.End();

			resumeButton.Draw();
			settingsButton.Draw();
			exitButton.Draw();
			exitToDesktopButton.Draw();

			base.Draw(gameTime);
		}
	}
}
