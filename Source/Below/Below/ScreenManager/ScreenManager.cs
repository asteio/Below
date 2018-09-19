//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Below
{
	/// <summary>
	/// the main game class
	/// 
	/// loads, unloads, updates, and draws the active screens
	/// </summary>
	public class ScreenManager : Game
	{

		public static Camera2D Camera = new Camera2D();

		private FrameCounter fpsCounter = new FrameCounter();
		private bool fpsCount = false;

		//constants
		public static int SCREEN_WIDTH = 1280;
		public static int SCREEN_HEIGHT = 960;

		public static GraphicsDeviceManager GraphicsDeviceMgr { get; private set; }
		public static SpriteBatch SpriteBatch { get; private set; }
		public static ContentManager ContentMgr { get; private set; }
		//list of screens
		public static List<GameScreen> ScreenList { get; private set; }

		//set to true will exit the game
		public static bool ExitGame = false;

		//input states
		private static KeyboardState oldState;
		private static KeyboardState newState;

		//Global content
		public static SpriteFont ExtraSmallFont;
		public static SpriteFont SmallFont;
		public static SpriteFont MediumFont;
		public static SpriteFont LargeFont;
		public static Settings Settings = Settings.Load();

		#region Lighting
		public static bool LightingEnabled = true;
		public static LightingEngine LightingEngine;
		private Effect lightEffect;
		private Effect deferrectLightEffect;
		private Texture2D normal;
		private void LoadLighting()
		{
			// Shader Effects used by lighting engine
			lightEffect = Content.Load<Effect>("Shaders/LightEffect");
			deferrectLightEffect = Content.Load<Effect>("Shaders/DeferredLightEffect");

			// Initialize lighting engine
			LightingEngine = new LightingEngine(GraphicsDevice, lightEffect, deferrectLightEffect);

			// load the normal pixel 
			normal = Content.Load<Texture2D>("Shaders/normal"); //The normal map.
			LightingEngine.InvertYNormal = false; //this normalmap has the Y normal in the usual direction.
		}
		#endregion

		/// <summary>
		/// MAIN ENTRY POINT
		/// </summary>
		public static void Main()
		{
			using (ScreenManager manager = new ScreenManager())
			{
				manager.Run();
			}
		}

		/// <summary>
		/// create screenmanager
		/// </summary>
		public ScreenManager()
		{
			GraphicsDeviceMgr = new GraphicsDeviceManager(this);

			//window size
			GraphicsDeviceMgr.PreferredBackBufferWidth = SCREEN_WIDTH;
			GraphicsDeviceMgr.PreferredBackBufferHeight = SCREEN_HEIGHT;

			GraphicsDeviceMgr.IsFullScreen = true;

			GraphicsDeviceMgr.GraphicsProfile = GraphicsProfile.HiDef;

			//reduces jitter
			IsFixedTimeStep = true;

			//set the directior for the game content
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// initialize screen manager
		/// </summary>
		protected override void Initialize()
		{
			IsMouseVisible = true;

			base.Initialize();
		}

		
		/// <summary>
		/// sets up content manager and spritebatch to be used in every screen
		/// load all content
		/// </summary>
		protected override void LoadContent()
		{
			GraphicsDeviceMgr.ToggleFullScreen();


			//reset event log
			File.Create("Logs/ScreenEventLog.txt").Close();
			//log that the game has started
			LogScreenEvent("GAME STARTED");

			Camera.Position = new Vector2(0, 0);

			oldState = Keyboard.GetState();
			newState = Keyboard.GetState();

			ContentMgr = Content;
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			
			
			//load any assets to be used in all screens

			ExtraSmallFont = ContentMgr.Load<SpriteFont>("Fonts/ExtraSmallFont");
			SmallFont = ContentMgr.Load<SpriteFont>("Fonts/SmallFont");
			MediumFont = ContentMgr.Load<SpriteFont>("Fonts/MediumFont");
			LargeFont = ContentMgr.Load<SpriteFont>("Fonts/LargeFont");
			
			LoadLighting();
			
			base.LoadContent();

			
			AddScreen(new DefaultScreen());//this empty screen needs to always be on the bottom
			//FIRST LOADED SCREEN
			AddScreen(new CogThoughtSplashScreen());
			//AddScreen(new CogThoughtSpashScreen());
			//FIRST LOADED SCREEN
		}

		/// <summary>
		/// unloads loaded content
		/// </summary>
		protected override void UnloadContent()
		{
			foreach (var screen in ScreenList)
			{
				screen.UnloadContent();
			}

			ScreenList.Clear();
			Content.Unload();
		}

		/// <summary>
		/// updates game logic for each screen in the screen list
		/// </summary>
		/// <param name="gameTime"></param>
		protected override void Update(GameTime gameTime)
		{
			newState = Keyboard.GetState();

			//have the current settings always available
			Settings = Settings.Load();

			//update soundmanager to check any changes with the settings
			SoundManager.Update();

			//check for escape and add PauseScreen if pressed
			CheckPause();

			//load developer keyboard shortcuts
			DevKeys();

			//let any screen be able to close the game
			if (ExitGame)
			{
				//log the game exiting
				LogScreenEvent("GAME EXITED");

				Content.Unload();
				Exit();
			}
			

			try
			{
				var startIndex = ScreenList.Count - 1;
				while (ScreenList[startIndex].IsPopup && ScreenList[startIndex].IsActive)
				{
					startIndex--;
				}
				for (var i = startIndex; i < ScreenList.Count; i++)
				{
					//depending on pause
					ScreenList[i].Update(gameTime);
				}
			} 
			catch (Exception ex)
			{
				ErrorManagement(ex);

				RemoveAllScreens(new MainMenuScreen());
			}
			finally
			{
				base.Update(gameTime);
			}

			oldState = newState;
		}

		/// <summary>
		/// draw graphical content for each screen in screenlist
		/// </summary>
		/// <param name="gameTime"></param>
		protected override void Draw(GameTime gameTime)
		{
			if (LightingEnabled)
			{
				GraphicsDevice.SetRenderTarget(LightingEngine.Colormap);
				GraphicsDevice.Clear(Color.Black);
			}
			
			var startIndex = ScreenList.Count - 1;
			while (ScreenList[startIndex].IsPopup)
			{
				startIndex--;
			}
			
			//to set a custom background color
			GraphicsDevice.Clear(ScreenList[startIndex].BackgroundColor);

			for (var i = startIndex; i < ScreenList.Count; i++)
			{
				ScreenList[i].Draw(gameTime);
			}


			if (LightingEnabled)
			{
				GraphicsDevice.SetRenderTarget(LightingEngine.Normalmap);
				SpriteBatch.Begin();
				SpriteBatch.Draw(normal, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
				SpriteBatch.End();

				LightingEngine.Draw(null, SpriteBatch, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT));
			}
			
			//in order to draw certain HUD content that shouldnt be darkened by the lighting
			for (var i = startIndex; i< ScreenList.Count; i++)
			{
				ScreenList[i].DrawOverLighting(gameTime);
			}

			if (fpsCount)
			{
				float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

				fpsCounter.Update(deltaTime);

				var fps = string.Format("FPS: {0}", fpsCounter.AverageFramesPerSecond);

				SpriteBatch.Begin();

				SpriteBatch.DrawString(ExtraSmallFont, fps, new Vector2(TextTools.CenterText(ExtraSmallFont, fps).X, 0), Color.White);

				SpriteBatch.End();
			}
			
			base.Draw(gameTime);
		}


		#region Screen Tools
		/// <summary>
		/// adds a new screen over previous one
		/// useful for pause sccreens, inventory screens, and store screens
		/// </summary>
		/// <param name="gameScreen"></param>
		public static void AddScreen(GameScreen gameScreen)
		{
			if (ScreenList == null)
			{
				ScreenList = new List<GameScreen>();
			}
			ScreenList.Add(gameScreen);
			gameScreen.LoadContent();

			LogScreenEvent("Added screen: " + gameScreen.GetType().Name);//log that a screen has been added
			
		}

		/// <summary>
		/// removes screen from list
		/// </summary>
		/// <param name="gameScreen"></param>
		public static void RemoveScreen(GameScreen gameScreen)
		{
			LogScreenEvent("Removed screen: " + gameScreen.GetType().Name);//log that a screen has been removed

			gameScreen.UnloadContent();
			ScreenList.Remove(gameScreen);
			if (ScreenList.Count < 1)
			{
				AddScreen(new MainMenuScreen());//screen to add if there are no screens left
			}
		}

		/// <summary>
		/// swap screens - removes current screen, add target screen
		/// </summary>
		/// <param name="currentScreen"></param>
		/// <param name="targetScreen"></param>
		public static void ChangeScreens(GameScreen currentScreen, GameScreen targetScreen)
		{
			RemoveScreen(currentScreen);
			AddScreen(targetScreen);
		}

		/// <summary>
		/// removes all screens and adds specified screen
		/// </summary>
		/// <param name="defaultScreen"></param>
		public static void RemoveAllScreens(GameScreen defaultScreen)
		{
			LightingEngine.ClearLights();

			foreach (GameScreen gs in ScreenList)
			{
				gs.UnloadContent();
			}

			ScreenList.Clear();

			AddScreen(new DefaultScreen());
			AddScreen(defaultScreen);
		}

		/// <summary>
		/// function to easily add pause screen
		/// </summary>
		public static void CheckPause()
		{
			if (IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) && ScreenList[ScreenList.Count - 1].Pausable)
			{
				//set update to stop for the top screen
				AddScreen(new PauseScreen()); 
			}
		}
		#endregion

		#region SpriteBatch and Camera Tools
		/// <summary>
		/// start spritebatch using camera2d matrix
		/// </summary>
		public static void StartCameraSpriteBatch()
		{
			SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.GetTransformation(GraphicsDeviceMgr.GraphicsDevice));
		}
		/// <summary>
		/// end camera spritebatch - probably not needed, but i want it for consistancy's sake
		/// </summary>
		public static void EndCameraSpriteBatch()
		{
			SpriteBatch.End();
		}

		
		/// <summary>
		/// converts global coordinates to fit on 1280x960 screen
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Vector2 GlobalToLocal(float x, float y)
		{
			return new Vector2(x - Camera.Position.X, y - Camera.Position.Y);
		}
		/// <summary>
		/// converts global coordinates to fit on 1280x960 screen but returns as a rectangle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Rectangle GlobalToLocal_rect(int x, int y, int width, int height)
		{
			return new Rectangle(x - (int)Camera.Position.X, y - (int)Camera.Position.Y, width, height);
		}
		#endregion

		#region Input
		/// <summary>
		/// having a global keyboard system is better on system resources
		/// checks if key has been pressed and released
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool IsKeyPressed(Microsoft.Xna.Framework.Input.Keys key)
		{
			return (newState.IsKeyDown(key) && !oldState.IsKeyDown(key));
		}
		/// <summary>
		/// checks if key is currently down
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool IsKeyDown(Microsoft.Xna.Framework.Input.Keys key)
		{
			return newState.IsKeyDown(key);
		}
		/// <summary>
		/// check if any key has been pressed
		/// </summary>
		/// <returns></returns>
		public static bool AnyKeyPressed()
		{
			return newState.GetPressedKeys().Length > 0 && oldState.GetPressedKeys().Length == 0;
		}
		#endregion

		#region Dev and Logging
		/// <summary>
		/// keys for dev mdoes
		/// </summary>
		private void DevKeys()
		{
			//Lighting toggle
			if (IsKeyPressed(Keybindings.DevKey1))
				LightingEnabled = !LightingEnabled;

			if (IsKeyPressed(Keybindings.DevKey3))
				fpsCount = !fpsCount;
		}

		/// <summary>
		/// email error report
		/// </summary>
		/// <param name="data"></param>
		private void EmailErrorReport(string errorReport)
		{
			//set up client
			SmtpClient client = new SmtpClient();
			client.Port = 587;
			client.Host = "smtp.gmail.com";
			client.EnableSsl = true;
			client.Timeout = 3000;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			//hardcode login credentials
			client.Credentials = new System.Net.NetworkCredential("sharkboyserrors@gmail.com", "sboys2018!");

			//create the messaage
			var sendMail = new MailMessage();
			sendMail.IsBodyHtml = true;
			sendMail.From = new MailAddress("sharkboyserrors@gmail.com");
			sendMail.To.Add(new MailAddress("sharkboysstudios@gmail.com"));
			sendMail.Subject = "Below Error Report";
			sendMail.Body = errorReport;

			//try to send the message
			try
			{
				client.Send(sendMail);
				MessageBox.Show("Message Sent");
			}
			catch
			{
				MessageBox.Show("Error sending report...");
			}
		}

		/// <summary>
		/// deal with errors
		/// </summary>
		/// <param name="ex"></param>
		private void ErrorManagement(Exception ex)
		{
			MessageBox.Show("An error occurred. See Errors/ErrorLog.txt for details.");

			//collect log data and write to errorlog.txt
			List<string> logData = new List<string>();
			logData.Add(ex.ToString());
			logData.Add("Active Screens: ");
			foreach (GameScreen gameScreen in ScreenList)
			{
				logData.Add(gameScreen.ToString());
			}
			logData.Add("Occurred at: " + DateTime.Now);
			logData.Add("\n");

			File.AppendAllLines("Logs/ErrorLog.txt", logData);

			//email error log
			if (MessageBox.Show("Would you like to email an error report to sharkboysstudios@gmail.com", "Email Error Report", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				//turn it in to one string
				string emailText = "";
				foreach (string line in logData)
				{
					emailText += line + "\n";
				}
				EmailErrorReport(emailText);
			}
			else { }
			
		}

		/// <summary>
		/// append a message to the screen event text file
		/// </summary>
		/// <param name="message"></param>
		private static void LogScreenEvent(string message)
		{
			//log the message as well as the current time
			File.AppendAllText("Logs/ScreenEventLog.txt", message + " - " + DateTime.Now + "\n");
		}
		#endregion
	}
}
