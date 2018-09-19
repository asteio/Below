//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// the group of levels that the shadow cam will be in
	/// </summary>
	public class LevelGroup : Level
	{
		public int GroupIndex { get; private set; }
		public int LevelIndex { get; private set; }

		private ShadowCamera shadowCam;
		private Enemy enemy;

		private ParallaxBackground background;

		private Texture2D groupBackground;
		private float titleTime = 0f;
		private bool showTitle;

		public bool GroupDone = false;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="GroupIndex">difficulty group</param>
		/// <param name="LevelIndex">level within difficulty group</param>
		public LevelGroup(int GroupIndex, int LevelIndex) : base("Levels/Groups/" + GroupIndex.ToString() + "-" + LevelIndex.ToString() + ".bmp")
		{
			this.GroupIndex = GroupIndex;
			this.LevelIndex = LevelIndex;

			CamType = "shadow";
			//CamType = "center"; 

			//determine whether or not to show a group title
			if (LevelIndex != 0)
				showTitle = false;
			else
				showTitle = true;
		}

		/// <summary>
		/// convert the group index and level index to a bitmap path
		/// </summary>
		/// <param name="group"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		private string indexToLevel(int groupIndex, int levelIndex)
		{
			return "Levels/Groups/" + groupIndex.ToString() + "-" + levelIndex.ToString() + ".bmp";
		}
		
		/// <summary>
		///	TODO - add music
		///	TODO - change tilesetrow
		/// </summary>
		/// <param name="groupIndex"></param>
		private void loadGroupInfo(int groupIndex)
		{
			
			switch (groupIndex)
			{
				case 0:
					ScreenManager.LightingEngine.SetLightPower(3);
					shadowCam.CamSpeed = 2.5f;
					enemy = new Enemy(Player, 0);
					background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_background"),
						ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_bottom_parallax"),
						ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_top_parallax"));
					groupBackground = ImageTools.LoadTexture2D("Graphics/Backgrounds/chapter 1");
					break;
				case 1:
					ScreenManager.LightingEngine.SetLightPower(3);
					shadowCam.CamSpeed = 2.5f;
					enemy = new Enemy(Player, 9);
					background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_2_background"),
						ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_2_bottom_parallax"),
						ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_2_top_parallax"));
					groupBackground = ImageTools.LoadTexture2D("Graphics/Backgrounds/chapter 2");
					break;
				case 2:
					ScreenManager.LightingEngine.SetLightPower(3);
					shadowCam.CamSpeed = 2.5f;
					enemy = new Enemy(Player, 9);
					background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_background"),
						ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_bottom_parallax"),
						ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_top_parallax"));
					groupBackground = ImageTools.LoadTexture2D("Graphics/Backgrounds/chapter 3");
					break;
			}
			
		}

		/// <summary>
		/// load tile map data
		/// </summary>
		public override void LoadLevel()
		{
			shadowCam = new ShadowCamera(this);
			shadowCam.Enabled = true;

			//set tile set row before the level base.loadlevel
			switch(GroupIndex)
			{
				case 0:
					TileSetRow = 0;
					break;
				case 1:
					TileSetRow = 1;
					break;
				case 2:
					TileSetRow = 2;
					break;
			}


			base.LoadLevel();

			loadGroupInfo(GroupIndex);

			SetPortalState(true);

		}

		/// <summary>
		/// update game logic
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			if (!showTitle)
				enemy.Update(gameTime);

			background.Update(Player.Direction, CamType);

			if (ExitReached && LevelIndex < 2)
			{
				LevelIndex++;
				Reset(indexToLevel(GroupIndex, LevelIndex));
				ExitReached = false;
				
				LevelData levelData = LevelData.Load();
				levelData.GroupIndex = GroupIndex;
				levelData.LevelIndex = LevelIndex;
				levelData.Save();
				
			}
			else if (ExitReached && LevelIndex >=2 )
				GroupDone = true;

			shadowCam.Update();

			if (showTitle)
				shadowCam.Enabled = false;
			else
				shadowCam.Enabled = true;


			base.Update(gameTime);
		}
		
		/// <summary>
		/// draw the level
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			background.Draw();

			base.Draw(gameTime);

			ScreenManager.StartCameraSpriteBatch();
			shadowCam.Draw();
			ScreenManager.EndCameraSpriteBatch();
			
			//enemy.Draw();
		}

		/// <summary>
		/// draw things over lighting
		/// </summary>
		public override void DrawOverLighting(GameTime gameTime)
		{

			enemy.Draw();

			if (showTitle)
			{

				titleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

				ScreenManager.SpriteBatch.Begin();
				ScreenManager.SpriteBatch.Draw(groupBackground, Vector2.Zero, Color.White);
				ScreenManager.SpriteBatch.End();
			}

			if (titleTime >= 2f)
			{
				titleTime = 0f;
				showTitle = false;
			}

			if (!showTitle)
				base.DrawOverLighting(gameTime);
		}

	}
}
