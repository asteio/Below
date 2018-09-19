//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// screen where player is presented choice
	/// </summary>
	public class ChoiceScreen : GameScreen
	{
		private Level level;

		private Animation bossAnim;
		private Texture2D eyeD;

		private DialogueBox dbox;

		private Texture2D leftChoice;
		private Rectangle leftBound;
		private Texture2D rightChoice;
		private Rectangle rightBound;

		private ParallaxBackground background;

		/// <summary>
		/// load game assets
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;
			ScreenOrder.SaveScreen(this);

			dbox = new DialogueBox("Scripts/choiceScript.txt");
			dbox.LoadDBox();
			dbox.IsActive = true;

			level = new Level("Levels/endBoss.bmp");
			level.TileSetRow = 2;
			level.LoadLevel();
			level.Player.MovementEnabled = false;


			bossAnim = new Animation(ImageTools.LoadTexture2D("Characters/Bosses/wraith_animation"), 6, true);
			eyeD = ImageTools.LoadTexture2D("Characters/Bosses/eyeball_D");

			leftChoice = ImageTools.LoadTexture2D("Graphics/Misc/crystal_ball_red");
			leftBound = new Rectangle((int)level.Player.Position.X - 64, (int)level.Player.Position.Y, 32, 64);

			rightChoice = ImageTools.LoadTexture2D("Graphics/Misc/crystal_ball_blue");
			rightBound = new Rectangle((int)level.Player.Position.X + 64, (int)level.Player.Position.Y, 32, 64);

			//dbox = new DialogueBox("Scripts/choiceScript.txt");
			//dbox.LoadDBox();
			background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_background"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_bottom_parallax"),
				ImageTools.LoadTexture2D("Graphics/Backgrounds/cave_level_3_top_parallax"));

			base.LoadContent();
		}

		/// <summary>
		/// unload assets
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

		

			if (level.Player.BoundingRectangle.Intersects(leftBound))
				ScreenManager.ChangeScreens(this, new Ending2Screen());
			else if (level.Player.BoundingRectangle.Intersects(rightBound))
				ScreenManager.ChangeScreens(this, new Ending1Screen());

			base.Update(gameTime);
		}

		/// <summary>
		/// draw graphics
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			background.Draw();
			level.Draw(gameTime);

			ScreenManager.SpriteBatch.Begin();

			Vector2 position = new Vector2(100, 300);
			bossAnim.DrawFrame(0, position, SpriteEffects.None);
			ScreenManager.SpriteBatch.Draw(eyeD, position + new Vector2(51, 27), null, Color.White, ImageTools.Rotation(position + new Vector2(51, 27), level.Player.Position), new Vector2(eyeD.Width / 2, eyeD.Height / 2), 1, SpriteEffects.None, 1);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// draw over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			level.DrawOverLighting(gameTime);

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(leftChoice, leftBound, Color.White);
			ScreenManager.SpriteBatch.Draw(rightChoice, rightBound, Color.White);

			ScreenManager.SpriteBatch.End();

			dbox.Draw();

			base.DrawOverLighting(gameTime);
		}
	}
}
