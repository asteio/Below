//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// the boss chases the player back through the last level
	/// while shooting shadow balls the player can try and deflect
	/// there will be a massive hole for the player to jump through at the end
	/// of the fight
	/// </summary>
	public class Boss2Screen : GameScreen
	{
		private LevelData levelData;

		private Level level;
		
		private Boss boss;
		private ShadowCamera bossCam;
		private Texture2D wallBoss;

		private float timer = 0f;

		private DialogueBox dbox;

		private List<Vector2> eyePos = new List<Vector2>();
		private Texture2D eyeA;
		private Texture2D eyeB;
		private Texture2D eyeC;
		private Texture2D eyeD;
		private List<Texture2D> eyes = new List<Texture2D>();

		private ParallaxBackground background;
		

		/// <summary>
		/// load info for each eye
		/// </summary>
		private void LoadEyes()
		{
			eyePos.Add(bossCam.shadowLocation + new Vector2(71, 53));
			eyePos.Add(bossCam.shadowLocation + new Vector2(93, 142));
			eyePos.Add(bossCam.shadowLocation + new Vector2(69, 264));
			eyePos.Add(bossCam.shadowLocation + new Vector2(91, 367));
			eyePos.Add(bossCam.shadowLocation + new Vector2(56, 467));
			eyePos.Add(bossCam.shadowLocation + new Vector2(73, 543));
			eyePos.Add(bossCam.shadowLocation + new Vector2(79, 665));
			eyePos.Add(bossCam.shadowLocation + new Vector2(101, 791));
			eyePos.Add(bossCam.shadowLocation + new Vector2(96, 906));

			eyeA = ImageTools.LoadTexture2D("Characters/Bosses/eyeball_A");
			eyeB = ImageTools.LoadTexture2D("Characters/Bosses/eyeball_B");
			eyeC = ImageTools.LoadTexture2D("Characters/Bosses/eyeball_C");
			eyeD = ImageTools.LoadTexture2D("Characters/Bosses/eyeball_D");

			eyes.Add(eyeC);
			eyes.Add(eyeA);
			eyes.Add(eyeD);
			eyes.Add(eyeB);
			eyes.Add(eyeA);
			eyes.Add(eyeD);
			eyes.Add(eyeC);
			eyes.Add(eyeA);
			eyes.Add(eyeB);
		}
		/// <summary>
		/// set eye positions
		/// </summary>
		private void SetEyePositions()
		{
			eyePos[0] = bossCam.shadowLocation + new Vector2(71, 53);
			eyePos[1] = bossCam.shadowLocation + new Vector2(93, 142);
			eyePos[2] = bossCam.shadowLocation + new Vector2(69, 264);
			eyePos[3] = bossCam.shadowLocation + new Vector2(91, 367);
			eyePos[4] = bossCam.shadowLocation + new Vector2(56, 467);
			eyePos[5] = bossCam.shadowLocation + new Vector2(73, 543);
			eyePos[6] = bossCam.shadowLocation + new Vector2(79, 665);
			eyePos[7] = bossCam.shadowLocation + new Vector2(101, 791);
			eyePos[8] = bossCam.shadowLocation + new Vector2(96, 906);
		}

		/// <summary>
		/// load screen content
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;

			SoundManager.PlaySong("Group_2_boss", true);

			//save current screen and data for next level
			levelData = LevelData.Load();
			levelData.LastScreen = ScreenOrder.ScreenToId(this);
			levelData.GroupIndex = 2;
			levelData.LevelIndex = 0;
			levelData.Save();

			

			level = new Level("Levels/boss2.bmp");
			level.TileSetRow = 1;
			level.LoadLevel();
			level.CamType = "shadow";

			boss = new Boss(1);
			bossCam = new ShadowCamera(level);
			bossCam.CamSpeed = -2.5f;
			bossCam.Enabled = true;
			wallBoss = ImageTools.LoadTexture2D("Characters/Bosses/shadow_wall_boss");

			LoadEyes();

			//load projectiles
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-8, 0), new Vector2(1280, 192)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-8, 0), new Vector2(1280, 192 * 2)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-8, 0), new Vector2(1280, 192 * 3)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-8, 0), new Vector2(1280, 192 * 4)));

			background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_2_background"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_2_bottom_parallax"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_2_top_parallax"));

			dbox = new DialogueBox("Scripts/boss2Script.txt");
			dbox.LoadDBox();
			dbox.IsActive = true;

			base.LoadContent();
		}
		
		/// <summary>
		/// stop sound and music
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
			background.Update(level.Player.Direction, level.CamType);

			dbox.Update(gameTime);
			if (dbox.IsActive)
				bossCam.Enabled = false;
			else
				bossCam.Enabled = true;

			level.Update(gameTime);
			bossCam.Update();

			SetEyePositions();

			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			//launch the projectiles ever 8 seconds
			if (timer >= 8f)
			{
				SoundManager.PlaySound("loudwall", 1f);
				boss.LaunchAllProjectiles();
				timer = 0f;
			}

			//trigger to the next screen when player falls down hole
			if (level.Player.Position.Y > 1000)
				ScreenManager.ChangeScreens(this, new LevelScreen());

			//check if player has died
			if (level.Player.IsDead)
				ScreenManager.ChangeScreens(this, new DeathScreen());


			base.Update(gameTime);
		}
		/// <summary>
		/// draw screen content under lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			background.Draw();

			level.Draw(gameTime);

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(wallBoss, new Vector2(1280 - wallBoss.Width, 0), Color.White);

			ScreenManager.SpriteBatch.End();

			ScreenManager.StartCameraSpriteBatch();
			//draw each eye with the custom rotation
			for (int i = 0; i < eyes.Count; i++)
			{
				ScreenManager.SpriteBatch.Draw(eyes[i], eyePos[i], null, Color.White, ImageTools.Rotation(eyePos[i], level.Player.Position), new Vector2(eyes[i].Width / 2, eyes[i].Height / 2), 1, SpriteEffects.None, 1);
			}
			ScreenManager.EndCameraSpriteBatch();

			//draw the shadow balls
			boss.DrawProjectiles();

			base.Draw(gameTime);
		}
		/// <summary>
		/// draw screen content over lighting
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
