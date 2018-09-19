//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// the screen where all main levels take place
	/// </summary>
	public class LevelScreen : GameScreen
	{
		private LevelGroup levelGroup;

		/// <summary>
		/// load content
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;

			ScreenManager.LightingEnabled = true;
			

			LevelData levelData = LevelData.Load();
			levelGroup = new LevelGroup(levelData.GroupIndex, levelData.LevelIndex);
			levelGroup.LoadLevel();
			levelData.LastScreen = ScreenOrder.ScreenToId(this);
			levelData.Save();
			
			//load music depending on the group
			switch (levelGroup.GroupIndex)
			{
				case 0:
					SoundManager.PlaySong("Group_1", true);
					break;
				case 1:
					SoundManager.PlaySong("Group_2", true);
					break;
				case 2:
					SoundManager.PlaySong("Group_3", true);
					break;
			}


			base.LoadContent();
		}

		/// <summary>
		/// stop sounds
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
			levelGroup.Update(gameTime);

			//choose what boss level to load once the level group is done
			if (levelGroup.GroupDone)
			{
				switch (levelGroup.GroupIndex)
				{
					case 0:
						ScreenManager.ChangeScreens(this, new Boss1Screen());
						break;
					case 1:
						ScreenManager.ChangeScreens(this, new Boss2Screen());
						break;
					case 2:
						ScreenManager.ChangeScreens(this, new EndBossScreen());
						break;
				}
			}

			

			//if the player has died
			if (levelGroup.Player.IsDead || levelGroup.Player.Position.Y > 1000)
			{
				//save all current level data
				LevelData levelData = LevelData.Load();
				levelData.GroupIndex = levelGroup.GroupIndex;
				levelData.LevelIndex = levelGroup.LevelIndex;
				levelData.LastScreen = ScreenOrder.ScreenToId(this);
				
				//load death screen
				ScreenManager.ChangeScreens(this, new DeathScreen());

			}

			base.Update(gameTime);
		}

		/// <summary>
		/// draw graphics
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			levelGroup.Draw(gameTime);

			base.Draw(gameTime);
		}

		/// <summary>
		/// draw over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			levelGroup.DrawOverLighting(gameTime);

			base.DrawOverLighting(gameTime);
		}

	}
}
