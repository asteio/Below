//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// multi layer background to give the perception of depth
	/// </summary>
	public class ParallaxBackground
	{
		/// <summary>
		/// Character/camera moves 5 units to the right
		/// background right behind character moves 1 unit
		/// background behind that moves 2 units
		/// background 3 moves 3 units
		/// background 4 moves 4 units
		/// furthest away background moves with player/camera
		/// 
		/// Move backgrounds with camera, where the actual movement
		/// is a function based on teh camera's movement and the distance
		/// to/behind the screen. The further away something is, the more
		/// you move it so it appears static.
		/// 
		/// have to draw each background twice with no offsets so it
		/// always fills the screen
		/// </summary>


		//closest background
		private Texture2D background1;
		private Vector2 b1Pos;
		private Vector2 b1ClonePos;
		private Vector2 b1Vel;

		//furthest background
		private Texture2D background2;
		private Vector2 b2Pos;
		private Vector2 b2ClonePos;
		private Vector2 b2Vel;

		private Texture2D staticBackground;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="StaticBackground"></param>
		/// <param name="Background1"></param>
		/// <param name="Background2"></param>
		public ParallaxBackground(Texture2D StaticBackground, Texture2D Background1, Texture2D Background2 = null)
		{
			staticBackground = StaticBackground;

			background1 = Background1;
			background2 = Background2;

			if (background2 == null)
				background2 = ImageTools.LoadTexture2D("Graphics/Overlays/transparent");

			b1Pos = new Vector2(0, 0);
			b1ClonePos = new Vector2(background1.Width, 0);
			b1Vel = new Vector2(.5f, 0);

			b2Pos = new Vector2(0, 0);
			b2ClonePos = new Vector2(background2.Width, 0);
			b2Vel = new Vector2(2, 0);
		}
		
		/// <summary>
		/// update background movement
		/// </summary>
		/// <param name="direction"></param>
		public void Update(int direction, string camType)
		{
			if (camType.ToLower() != "static")
			{
				if (direction != 0)
				{
					b1Pos += b1Vel * direction;
					b1ClonePos += b1Vel * direction;

					b2Pos += b2Vel * direction;
					b2ClonePos += b2Vel * direction;
				}
			}
			
			if (direction < 0)
			{
				if (b1Pos.X <= -background1.Width)
					b1Pos.X = b1ClonePos.X + background1.Width;

				if (b1ClonePos.X <= -background1.Width)
					b1ClonePos.X = b1Pos.X + background1.Width;


				if (b2Pos.X <= -background2.Width)
					b2Pos.X = b2ClonePos.X + background2.Width;

				if (b2ClonePos.X <= -background2.Width)
					b2ClonePos.X = b2Pos.X + background2.Width;
			}
			else if (direction > 0)
			{

				if (b1Pos.X >= ScreenManager.SCREEN_WIDTH)
					b1Pos.X = b1ClonePos.X - background1.Width;

				if (b1ClonePos.X >= ScreenManager.SCREEN_WIDTH)
					b1ClonePos.X = b1Pos.X - background1.Width;

				if (b2Pos.X >= ScreenManager.SCREEN_WIDTH)
					b2Pos.X= b2ClonePos.X - background2.Width;

				if (b2ClonePos.X >= ScreenManager.SCREEN_WIDTH)
					b2ClonePos.X = b2Pos.X - background2.Width;

			}
		}

		/// <summary>
		/// draw the backgrounds
		/// </summary>
		public void Draw()
		{
			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(staticBackground, Vector2.Zero, Color.White);

			//draw pair of closest background
			ScreenManager.SpriteBatch.Draw(background1, b1Pos, Color.White);
			ScreenManager.SpriteBatch.Draw(background1, b1ClonePos, Color.White);

			if (background2 != null)
			{
				ScreenManager.SpriteBatch.Draw(background2, b2Pos, Color.White);
				ScreenManager.SpriteBatch.Draw(background2, b2ClonePos, Color.White);
			}
			ScreenManager.SpriteBatch.End();
		}


	}
}
