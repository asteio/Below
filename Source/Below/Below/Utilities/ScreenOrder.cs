//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework


namespace Below
{
	/// <summary>
	/// the order of gameplay screens and their respective id numbers
	/// needed to save teh current screen to disk
	/// </summary>
	public class ScreenOrder
	{

		/// <summary>
		/// convert integer id to a gamescreen and vice versa
		/// </summary>
		/// <param name="numId"></param>
		/// <returns></returns>
		public static GameScreen IdToScreen(int numId)
		{
			switch(numId)
			{
				case 0:
					return new PrologueScreen();
				case 1:
					return new PoliceReportScreen();
				case 2:
					return new DateScreen();
				case 3:
					return new ForestScreen();
				case 4:
					return new CaveScreen();
				case 5:
					return new WakeUpScreen();
				case 6:
					return new LevelScreen();
				case 7:
					return new Boss1Screen();
				case 8:
					return new Boss2Screen();
				case 9:
					return new EndBossScreen();
				case 10:
					return new ChoiceScreen();
				case 11:
					return new EpilogueScreen();
				case 12:
					return new Ending1Screen();
				case 13:
					return new Ending2Screen();
				default:
					return new MainMenuScreen();
			}

		}
		
		/// <summary>
		/// convert gamescreen to integer id
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public static int ScreenToId(GameScreen screen)
		{
			switch(screen.GetType().Name)
			{
				case "PrologueScreen":
					return 0;
				case "PoliceReportScreen":
					return 1;
				case "DateScreen":
					return 2;
				case "ForestScreen":
					return 3;
				case "CaveScreen":
					return 4;
				case "WakeUpScreen":
					return 5;
				case "LevelScreen":
					return 6;
				case "Boss1Screen":
					return 7;
				case "Boss2Screen":
					return 8;
				case "EndBossScreen":
					return 9;
				case "ChoiceScreen":
					return 10;
				case "EpilogueScreen":
					return 11;
				case "Ending1Screen":
					return 12;
				case "Ending2Screen":
					return 13;
				default:
					return -1;
			}
		}
		
		/// <summary>
		/// easy way to save a gamescreen to LevelData
		/// </summary>
		/// <param name="gameScreen"></param>
		public static void SaveScreen(GameScreen gameScreen)
		{
			LevelData levelData = LevelData.Load();
			levelData.LastScreen = ScreenToId(gameScreen);
			levelData.Save();
		}

	}
}
