//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// screen where the player can control the settings
	/// </summary>
	public class SettingsScreen : GameScreen
	{
		private Button musicButton;
		private Button soundButton;

		private Button controlsButton;

		private Button backButton;
		

		private int x = 300;
		private int my = 250;
		private int sy = 450;
		private int cy = 650;

		/// <summary>
		/// load buttons
		/// </summary>
		public override void LoadContent()
		{

			backButton = new Button();
			backButton.Location(20, 20);
			backButton.Text = "Back";

			musicButton = new Button();
			musicButton.Location(x + (int)ScreenManager.MediumFont.MeasureString("Music").X + 100, my + ((int)ScreenManager.MediumFont.MeasureString("Music").Y - 50)/2);

			soundButton = new Button();
			soundButton.Location(x + (int)ScreenManager.MediumFont.MeasureString("Music").X + 100, sy + ((int)ScreenManager.MediumFont.MeasureString("Sound").Y - 50) / 2);

			controlsButton = new Button();
			controlsButton.Location(x + (int)ScreenManager.MediumFont.MeasureString("Music").X + 100, cy + ((int)ScreenManager.MediumFont.MeasureString("Sound").Y - 50) / 2);
			controlsButton.Text = "Controls";

			SetButtonData(Settings.Load());

			base.LoadContent();
		}

		/// <summary>
		/// update buttons
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			backButton.Update();
			musicButton.Update();
			soundButton.Update();
			controlsButton.Update();

			if (backButton.Clicked)
				ScreenManager.RemoveScreen(this);
			
			//set colors on toggle buttons
			if (musicButton.Clicked)
			{
				Settings settings = Settings.Load();
				settings.MusicOn = !settings.MusicOn;
				SetButtonData(settings);
				settings.Save();
				musicButton.Clicked = false;
			}

			if (soundButton.Clicked)
			{
				Settings settings = Settings.Load();
				settings.SoundOn = !settings.SoundOn;
				SetButtonData(settings);
				settings.Save();
				soundButton.Clicked = false;
			}
			if (controlsButton.Clicked)
			{
				controlsButton.Clicked = false;
				ScreenManager.AddScreen(new ControlsScreen());
			}
			
			base.Update(gameTime);
		}

		/// <summary>
		/// set the button's color and text based on the state of the settings
		/// </summary>
		/// <param name="settings"></param>
		private void SetButtonData(Settings settings)
		{
			//Settings settings = Settings.Load();
			if (!settings.MusicOn)
			{
				musicButton.MainColor = Color.Green;
				musicButton.TextColor = Color.Green;
				musicButton.Text = "On";
			}
			else
			{
				musicButton.MainColor = Color.Red;
				musicButton.TextColor = Color.Red;
				musicButton.Text = "Off";
			}

			if (!settings.SoundOn)
			{
				soundButton.MainColor = Color.Green;
				soundButton.TextColor = Color.Green;
				soundButton.Text = "On";
			}
			else
			{
				soundButton.MainColor = Color.Red;
				soundButton.TextColor = Color.Red;
				soundButton.Text = "Off";
			}
			
		}
		
		/// <summary>
		/// draw buttons over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{

			backButton.Draw();
			musicButton.Draw();
			soundButton.Draw();
			controlsButton.Draw();

			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.DrawString(ScreenManager.MediumFont, "Music", new Vector2(x, my), Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.MediumFont, "Sound", new Vector2(x, sy), Color.White);
			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
