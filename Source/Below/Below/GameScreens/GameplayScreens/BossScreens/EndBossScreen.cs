//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// the last boss fight
	/// the player, now knowing the boss's weakness
	/// to light, tries to use his flashlight to hurt the last boss
	/// the boss is to strong and overcomes the player
	/// cue ending cutscene
	/// </summary>
	public class EndBossScreen : GameScreen
	{
		private Level level;
		
		private Texture2D eyeD;
		private Animation bossAnim;
		private Boss boss;
		private Vector2 velocity;

		private Rectangle leftBound;
		private Rectangle rightBound;

		private ParallaxBackground background;

		private float timer = 0f;
		
		/// <summary>
		/// load game content
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;
			ScreenOrder.SaveScreen(this);

			SoundManager.PlaySong("Boss_3", true);

			level = new Level("Levels/endBoss.bmp");
			level.TileSetRow = 2;
			level.LoadLevel();
			level.CamType = "static";
			

			bossAnim = new Animation(ImageTools.LoadTexture2D("Characters/Bosses/wraith_animation"), new int[] { 0, 1, 2, 3, 4, 5, 0 }, 384, false);
			bossAnim.FrameTime = .1f;
			boss = new Boss(10);

			eyeD = ImageTools.LoadTexture2D("Characters/Bosses/eyeball_D");
			boss.Position = new Vector2(500, 20);
			velocity = new Vector2(2, 0);

			leftBound = new Rectangle(0, 0, 10, 300);
			rightBound = new Rectangle(1270, 0, 10, 300);

			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(0, 5), new Vector2(150, 0)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(0, 5), new Vector2(350, 0)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(0, 5), new Vector2(550, 0)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(0, 5), new Vector2(750, 0)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(0, 5), new Vector2(950, 0)));
			boss.Projectiles.Add(new ShadowBall(level.Player, new Vector2(0, 5), new Vector2(1150, 0)));

			background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_background"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_bottom_parallax"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_top_parallax"));
			

			base.LoadContent();
		}
		
		/// <summary>
		/// stop all sounds and unload the screen
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

			level.Update(gameTime);

			boss.Position += velocity;

			boss.BoundingRect = new Rectangle((int)boss.Position.X, (int)boss.Position.Y, 140, 220);
			boss.Health.UpdateCooldownTimer(gameTime);

			boss.CheckProjectileDamage();

			if (boss.BoundingRect.Intersects(leftBound) || boss.BoundingRect.Intersects(rightBound))
				velocity *= -1;
			
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (timer >= 6f)
			{
				boss.LaunchAllProjectiles();
				//timer = 0f;
			}

			if (timer >= 8)
			{
				SoundManager.PlaySound("skeletonlaugh", 1f);
				timer = 0;
			}
			//load next screen upon boss death
			if (boss.Health.IsDead)
				ScreenManager.ChangeScreens(this, new EpilogueScreen());

			//if player dies
			if (level.Player.Health.IsDead)
				ScreenManager.ChangeScreens(this, new DeathScreen());

			base.Update(gameTime);
		}

		/// <summary>
		/// draw stuff under lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			background.Draw();

			level.Draw(gameTime);

			ScreenManager.SpriteBatch.Begin();

			if (timer >= 6)
				bossAnim.PlayAnimation(gameTime, boss.Position, SpriteEffects.None);
			else
				bossAnim.DrawFrame(0, boss.Position, SpriteEffects.None);

			ScreenManager.SpriteBatch.Draw(eyeD, boss.Position + new Vector2(51, 27), null, Color.White, ImageTools.Rotation(boss.Position + new Vector2(51, 27), level.Player.Position), new Vector2(eyeD.Width / 2, eyeD.Height / 2), 1, SpriteEffects.None, 1);

			ScreenManager.SpriteBatch.End();

			boss.DrawProjectiles();
			


			base.Draw(gameTime);
		}

		/// <summary>
		/// draw stuff over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			level.DrawOverLighting(gameTime);
			
			boss.Health.DrawHealthBar(new Vector2(10, 10), .75f);

			base.DrawOverLighting(gameTime);
		}

		
	}
}
