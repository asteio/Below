//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// the first scene in which the players wander into the cave
	/// </summary>
	public class ForestScreen : GameScreen
	{
		private Level level;
		private ThunderStorm storm;
		private ParallaxBackground background;

		private DialogueBox dbox;
		private DialogueBox dbox2;

		private Animation friend1;
		private Vector2 f1Pos;
		private Animation friend2;
		private Vector2 f2Pos;
		private Rectangle friendBoundingBox;

		private Texture2D cave;
		private Rectangle caveBounds;

		private List<string> controls;
		private List<Vector2> controlsPos;

		/// <summary>
		/// load game content
		/// </summary>
		public override void LoadContent()
		{
			Pausable = true;
			ScreenOrder.SaveScreen(this);

			SoundManager.PlaySong("Forest", true);
			SoundManager.AddLoopingSound("rainlong", .05f);

			level = new Level("Levels/forest.bmp");
			level.TileSetRow = 0;
			level.LoadLevel();
			level.CamType = "center";
			

			cave = ImageTools.LoadTexture2D("Graphics/Misc/cave");
			caveBounds = new Rectangle(4541, 512 - 64, 32 * 4, 64 * 2);

			storm = new ThunderStorm();

			background = new ParallaxBackground(ImageTools.LoadTexture2D("Graphics/Backgrounds/sun_background"), 
				ImageTools.LoadTexture2D("Graphics/Backgrounds/mountains_parallax"), ImageTools.LoadTexture2D("Graphics/Backgrounds/clouds_parallax"));

			dbox = new DialogueBox("Scripts/forestScript.txt");
			dbox.LoadDBox();
			dbox.IsActive = true;

			dbox2 = new DialogueBox("Scripts/forestScript2.txt");
			dbox2.LoadDBox();

			controls = new List<string>();
			controls.Add("Use <A> and <D> to move left and right and <W> or <Space> to jump.");
			controls.Add("This is your canteen.\nPress <Q> to take  a drink  and heal for 1 health.");
			controls.Add("This is your current health.");
			controls.Add("You can refill your canteen by interacting, <E>, with ponds like this.");
			controls.Add("Use the mouse to move your flashlight around.");
			controls.Add("You can pause the game by pressing <Escape>");

			controlsPos = new List<Vector2>();
			controlsPos.Add(level.Player.Position - new Vector2(210, 200));
			controlsPos.Add(new Vector2(1280 - 500, 960 - 75));
			controlsPos.Add(new Vector2(1280 - 245, 115));
			controlsPos.Add(new Vector2(2688 - 200, 416 + 70));
			controlsPos.Add(level.Player.Position - new Vector2(130, 150));
			controlsPos.Add(new Vector2(TextTools.CenterText(ScreenManager.ExtraSmallFont, controls[5]).X, 50));


			friend1 = new Animation(ImageTools.LoadTexture2D("Characters/Friends/pat"), new int[] { 0, 0, 0, 0, 0, 1 }, 32, true);
			friend1.FrameTime = .75f;
			f1Pos = level.Player.Position + new Vector2(64, 0);

			friend2 = new Animation(ImageTools.LoadTexture2D("Characters/Friends/naomi"), new int[] { 0, 0, 1, 0, 0, 0 }, 32, true);
			friend2.FrameTime = .75f;
			f2Pos = f1Pos + new Vector2(50, 0);

			friendBoundingBox = new Rectangle((int)f1Pos.X, (int)f1Pos.Y, 64 + 18, 64);

			ScreenManager.LightingEngine.SetLightPower(3);

			base.LoadContent();
		}

		/// <summary>
		/// unload
		/// </summary>
		public override void UnloadContent()
		{
			SoundManager.StopAll();

			base.UnloadContent();
		}

		/// <summary>
		/// update logic
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			level.Update(gameTime);

			dbox.Update(gameTime);
			dbox2.Update(gameTime);
			if (dbox.IsActive || dbox2.IsActive)
				level.Player.MovementEnabled = false;
			else
				level.Player.MovementEnabled = true;

			background.Update(level.Player.Direction, level.CamType);

			//start 2nd dbox when at the cave
			if (level.Player.BoundingRectangle.Intersects(caveBounds))
				dbox2.IsActive = true;

			//when dbox2 is done
			if (dbox2.Finished)
				ScreenManager.ChangeScreens(this, new CaveScreen());

			base.Update(gameTime);
		}

		/// <summary>
		/// draw graphics under lighting engine
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			background.Draw();

			ScreenManager.StartCameraSpriteBatch();

			ScreenManager.SpriteBatch.Draw(cave, new Vector2(4488, 596 - 340), Color.White);

			ScreenManager.EndCameraSpriteBatch();

			level.Draw(gameTime);

			ScreenManager.StartCameraSpriteBatch();

			friend1.PlayAnimation(gameTime, f1Pos, Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally);
			friend2.PlayAnimation(gameTime, f2Pos, Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally);


			ScreenManager.EndCameraSpriteBatch();

			storm.DrawRain(gameTime);
			
			base.Draw(gameTime);
		}

		/// <summary>
		/// draw graphics under lighting engine
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			level.DrawOverLighting(gameTime);

			storm.DrawLightning(gameTime);
			 
			ScreenManager.StartCameraSpriteBatch();

			ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, controls[0], controlsPos[0], Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, controls[3], controlsPos[3], Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, controls[4], controlsPos[4], Color.White);

			ScreenManager.EndCameraSpriteBatch();


			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, controls[2], controlsPos[2], Color.Red);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, controls[1], controlsPos[1], Color.Red);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, controls[5], controlsPos[5], Color.White);


			ScreenManager.SpriteBatch.End();
			
			dbox.Draw();
			dbox2.Draw();

			base.DrawOverLighting(gameTime);
		}
	}
}