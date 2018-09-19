//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{
	public class CaveScreen : GameScreen
	{
		private Level level;

		private DialogueBox dbox;

		private Animation friend1;
		private Vector2 f1Pos;
		private Animation friend2;
		private Vector2 f2Pos;
		private Rectangle friendBoundingBox;

		private ParallaxBackground background;

		/// <summary>
		/// constructor
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;
			ScreenOrder.SaveScreen(this);

			SoundManager.PlaySong("Cave_One_Portal", true);

			dbox = new DialogueBox("Scripts/caveScript.txt");
			dbox.LoadDBox();

			level = new Level("Levels/cave.bmp");
			level.LoadLevel();
			level.CamType = "center";
			level.SetPortalState(true);

			background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_background"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_bottom_parallax"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_1_top_parallax"));

			friend1 = new Animation(ImageTools.LoadTexture2D("Characters/Friends/pat"), new int[] { 0, 0, 0, 0, 0, 1 }, 32, true);
			friend1.FrameTime = .75f;
			f1Pos = new Vector2(2324, 800);

			friend2 = new Animation(ImageTools.LoadTexture2D("Characters/Friends/naomi"), new int[] { 0, 0, 1, 0, 0, 0 }, 32, true);
			friend2.FrameTime = .75f;
			f2Pos = new Vector2(2370, 800);

			friendBoundingBox = new Rectangle((int)f1Pos.X, (int)f1Pos.Y, 110, 64);

			base.LoadContent();
		}

		/// <summary>
		/// unload screen
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
			level.Update(gameTime);
			background.Update(level.Player.Direction, level.CamType);

			dbox.Update(gameTime);
			if (dbox.IsActive)
				level.Player.MovementEnabled = false;
			else
				level.Player.MovementEnabled = true;

			//open dbox if ted interacts with friends
			if (level.Player.BoundingRectangle.Intersects(friendBoundingBox))
				dbox.IsActive = true;

			//load next screen when done
			if (dbox.Finished)
			{
				SoundManager.PlaySound("portal", 1f);
				ScreenManager.ChangeScreens(this, new WakeUpScreen());
			}
			base.Update(gameTime);
		}

		/// <summary>
		/// draw under lighting engine
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			background.Draw();
			level.Draw(gameTime);

			ScreenManager.StartCameraSpriteBatch();

			friend1.PlayAnimation(gameTime, f1Pos, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
			friend2.PlayAnimation(gameTime, f2Pos, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);

			ScreenManager.EndCameraSpriteBatch();

			base.Draw(gameTime);
		}

		/// <summary>
		/// draw over the lighting engine
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
