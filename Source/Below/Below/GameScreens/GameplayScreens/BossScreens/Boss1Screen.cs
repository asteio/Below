//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// first boss
	/// 
	/// Several platforms around a boss that is in the center of the screen
	/// The boss shoots shadow balls at the player
	/// the player must deflect a certain amount back at the boss to defeat
	/// this stage
	/// </summary>
	public class Boss1Screen : GameScreen
	{
		private LevelData levelData;

		private Level level;

		private DialogueBox dbox;

		private Animation bossAnim;
		private Boss boss;

		private float timer = 0f;
		private int projectileTurn = 0;


		private ParallaxBackground background;

		private Random random = new Random();

		/// <summary>
		///	load all the projectiles
		/// </summary>
		private void LoadProjectiles()
		{
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(0, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-1, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-2, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-3, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-4, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(-5, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(1, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(2, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(3, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(4, -5), new Vector2(624, 880)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(5, -5), new Vector2(624, 880)));

			boss.Projectiles2.Add(new ShadowBall(level.Player, new Vector2(-5, 0), new Vector2(624, 880)));
			boss.Projectiles2.Add(new ShadowBall(level.Player, new Vector2(-5, -1), new Vector2(624, 880)));
			boss.Projectiles2.Add(new ShadowBall(level.Player, new Vector2(-5, -2), new Vector2(624, 880)));
			boss.Projectiles2.Add(new ShadowBall(level.Player, new Vector2(-5, -3), new Vector2(624, 880)));
			boss.Projectiles2.Add(new ShadowBall(level.Player, new Vector2(-5, -4), new Vector2(624, 880)));
			boss.Projectiles2.Add(new ShadowBall(level.Player, new Vector2(-5, -5), new Vector2(624, 880)));

			boss.Projectiles3.Add(new ShadowBall(level.Player, new Vector2(5, 0), new Vector2(624, 880)));
			boss.Projectiles3.Add(new ShadowBall(level.Player, new Vector2(5, -1), new Vector2(624, 880)));
			boss.Projectiles3.Add(new ShadowBall(level.Player, new Vector2(5, -2), new Vector2(624, 880)));
			boss.Projectiles3.Add(new ShadowBall(level.Player, new Vector2(5, -3), new Vector2(624, 880)));
			boss.Projectiles3.Add(new ShadowBall(level.Player, new Vector2(5, -4), new Vector2(624, 880)));
			boss.Projectiles3.Add(new ShadowBall(level.Player, new Vector2(5, -5), new Vector2(624, 880)));
		}

		/// <summary>
		/// launch a specified list of projectiles
		/// </summary>
		/// <param name="projectiles"></param>
		private void LaunchProjectiles(List<ShadowBall> projectiles)
		{
			if (!dbox.IsActive)
				boss.LaunchProjectiles(projectiles);

			projectileTurn++;
			if (projectileTurn > 2)
				projectileTurn = 0;
			
		}

		/// <summary>
		/// load all boss content
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;

			SoundManager.PlaySong("Boss_Song", true);

			levelData = LevelData.Load();
			levelData.LastScreen = ScreenOrder.ScreenToId(this);
			levelData.GroupIndex = 1;
			levelData.LevelIndex = 0;
			levelData.Save();

			dbox = new DialogueBox("Scripts/boss1Script.txt");
			dbox.LoadDBox();
			dbox.IsActive = true;

			level = new Level("Levels/boss1.bmp");
			level.LoadLevel();
			level.CamType = "static";

			bossAnim = new Animation(ImageTools.LoadTexture2D("Characters/Bosses/boss1"), 40, true);
			bossAnim.FrameTime = .15f;
			boss = new Boss(8);
			boss.BoundingRect = new Rectangle(17 * Tile.Size, 960 - (6 * 32), bossAnim.Width, bossAnim.Height);

			LoadProjectiles();

			background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_background"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_bottom_parallax"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_top_parallax"));

			base.LoadContent();
		}

		public override void UnloadContent()
		{
			SoundManager.StopAll();
			base.UnloadContent();
		}


		/// <summary>
		/// update boss logic
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			level.Update(gameTime);
			background.Update(level.Player.Direction, level.CamType);
			
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			//shoot different group at 4 secs
			if (timer >= 4f)
			{
				
				switch(projectileTurn)
				{
					case 0:
						LaunchProjectiles(boss.Projectiles2);
						break;
					case 1:
						LaunchProjectiles(boss.Projectiles);
						break;
					case 2:
						LaunchProjectiles(boss.Projectiles3);
						break;
				}

				timer = -2.0f;//set to -2 to account for the powerdown time of the animation
			}

			boss.CheckProjectileDamage();

			dbox.Update(gameTime);
			if (dbox.IsActive)
				level.Player.MovementEnabled = false;
			else
				level.Player.MovementEnabled = true;



			if (level.Player.Position.Y > 1020 || level.Player.IsDead)
				ScreenManager.ChangeScreens(this, new DeathScreen());

			if (boss.Health.IsDead)
			{
				SoundManager.StopAll();
				level.SetPortalState(true);
			}

			if (level.ExitReached)
				ScreenManager.ChangeScreens(this, new LevelScreen());

			base.Update(gameTime);
		}

		/// <summary>
		/// draw content under lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			background.Draw();

			level.Draw(gameTime);

			boss.Health.UpdateCooldownTimer(gameTime);

			boss.DrawProjectiles();

			base.Draw(gameTime);
		}

		/// <summary>
		/// draw content over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			level.DrawOverLighting(gameTime);

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, "Try pointing your flashlight directly at one of those shadowballs.", Vector2.Zero, Color.White);

			if (!boss.Health.IsDead)
			{
				bossAnim.PlayAnimation(gameTime, boss.BoundingRect, SpriteEffects.None);
			}

			ScreenManager.SpriteBatch.End();


			boss.Health.DrawHealthBar(new Vector2(boss.BoundingRect.X + (7*32), 850), .75f);

			dbox.Draw();

			base.DrawOverLighting(gameTime);
		}


	}
}
