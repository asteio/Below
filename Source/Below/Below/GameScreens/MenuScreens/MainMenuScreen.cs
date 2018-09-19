//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// main menu 
	/// </summary>
	public class MainMenuScreen : GameScreen
	{
		private Texture2D background;
		private Texture2D belowLogo;

		private Button startButton;
		private Button optionsButton;
		private Button creditsButton;
		private Button exitButton;
		private Button resetButton;

		/// <summary>
		/// load assets
		/// </summary>
		public override void LoadContent()
		{

			SoundManager.PlaySong("Loop-1", true);

			ScreenManager.LightingEnabled = true;
			ScreenManager.LightingEngine.AmbientLightPower = 0.3f;
			
			belowLogo = ImageTools.LoadTexture2D("Graphics/Logos/below_logo");

			startButton = new Button();
			startButton.Text = "Play";
			startButton.Location(540, 500);

			optionsButton = new Button();
			optionsButton.Text = "Options";
			optionsButton.Location(540, 580);

			creditsButton = new Button();
			creditsButton.Text = "Credits";
			creditsButton.Location(540, 660);

			exitButton = new Button();
			exitButton.Text = "Exit";
			exitButton.Location(540, 740);

			resetButton = new Button();
			resetButton.Text = "Reset Game";
			resetButton.Location(20, 890);
			resetButton.HoverColor = Color.Red;
			resetButton.HoverTextColor = Color.Red;

			background = ImageTools.LoadTexture2D("Graphics/Backgrounds/mountain");

		

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
			startButton.Update();
			optionsButton.Update();
			creditsButton.Update();
			exitButton.Update();
			resetButton.Update();

			if (startButton.Clicked)
			{
				ScreenManager.ChangeScreens(this, ScreenOrder.IdToScreen(LevelData.Load().LastScreen));
			}
			if (optionsButton.Clicked)
			{
				optionsButton.Clicked = false;
				ScreenManager.AddScreen(new SettingsScreen());
			}
			if (creditsButton.Clicked)
			{
				ScreenManager.ChangeScreens(this, new CreditsScreen());
			}
			if (exitButton.Clicked)
			{
				ScreenManager.ExitGame = true;
			}

			if (resetButton.Clicked)
			{
				LevelData levelData = LevelData.Load();
				levelData.LastScreen = ScreenOrder.ScreenToId(new PrologueScreen());
				levelData.GroupIndex = 0;
				levelData.LevelIndex = 0;
				levelData.Save();

				PlayerData playerData = PlayerData.Load();
				playerData.Health = 5;
				playerData.FlaskUses = 3;
				playerData.Save();

				resetButton.Clicked = false;
				
			}

			base.Update(gameTime);
		}
		
		/// <summary>
		/// draw over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			startButton.Draw();
			optionsButton.Draw();
			creditsButton.Draw();
			exitButton.Draw();
			resetButton.Draw();

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(belowLogo, new Vector2(ImageTools.CenterImage_vect(belowLogo).X, 100), Color.White);

			ScreenManager.SpriteBatch.End();

			base.DrawOverLighting(gameTime);
		}

	}
}
