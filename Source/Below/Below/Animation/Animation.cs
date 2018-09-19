//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	public class Animation
	{
		private Texture2D spriteSheet;

		private int[] order;//order of sprites if a custom order is wanted
		private int orderIndex = 0;//index for the custom order

		// the elapsed amount of time the frame has been shown for
		private float time;
		// duration of time to show each frame
		public float FrameTime = 0.1f;
		// an index of the current frame being shown
		private int frameIndex;

		// total number of frames in our spritesheet
		private int totalFrames;
		//size of animation frame
		private int frameHeight;
		public int Height { get { return frameHeight; } }
		private int frameWidth;
		public int Width { get { return frameWidth; } }
		//determines whether timer is runnign
		private bool isCounting = true;
		//determines whether animation will loop or not
		private bool isLooping = true;

		private bool customOrder;

		//returns the amount of time it takes to play the animation
		public float TotalAnimationTime
		{
			get
			{
				return FrameTime * totalFrames;
			}
		}

		//the index of the current frame that is being drawn
		public int CurrenFrame
		{
			get
			{
				if (customOrder)
					return order[orderIndex];
				else
					return frameIndex;
			}

		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="SpriteSheet"></param>
		/// <param name="Order"></param>
		/// <param name="FrameWidth"></param>
		/// <param name="IsLoopping"></param>
		public Animation(Texture2D SpriteSheet, int[] Order, int FrameWidth, bool IsLoopping)
		{
			spriteSheet = SpriteSheet;
			order = Order;
			frameWidth = FrameWidth;
			frameHeight = spriteSheet.Height;
			isLooping = IsLoopping;

			totalFrames = order.Length;

			frameIndex = order[0];

			customOrder = true;
		}

		/// <summary>
		/// construction to play animation in the order it is in the spritesheet
		/// </summary>
		/// <param name="SpriteSheet"></param>
		/// <param name="TotalFrames"></param>
		/// <param name="IsLooping"></param>
		public Animation(Texture2D SpriteSheet, int TotalFrames, bool IsLooping)
		{
			spriteSheet = SpriteSheet;

			List<int> orderList = new List<int>();
			for (int i = 0; i < TotalFrames; i++)
			{
				orderList.Add(i);
			}
			order = orderList.ToArray();

			totalFrames = TotalFrames;
			frameWidth = spriteSheet.Width / totalFrames;
			frameHeight = spriteSheet.Height;
			isLooping = IsLooping;

			customOrder = false;
		}

		/// <summary>
		/// play animation
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="position"></param>
		/// <param name="spriteEffect"></param>
		public void PlayAnimation(GameTime gameTime, Rectangle position, SpriteEffects spriteEffect)
		{
			if (isCounting)
				time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			while (time > FrameTime)
			{
				orderIndex++;
				time = 0f;
			}

			if (orderIndex > order.Length - 1)
			{
				if (isLooping)
					orderIndex = 0;
				else
				{
					orderIndex = order.Length - 1;
					isCounting = false;
				}
			}

			frameIndex = order[orderIndex];

			Rectangle source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
			//ScreenManager.SpriteBatch.Draw(spriteSheet, position, source, Color.White);
			ScreenManager.SpriteBatch.Draw(spriteSheet, position, source, Color.White, 0, Vector2.Zero, spriteEffect, 0);
		}

		/// <summary>
		/// overload that uses vector as position
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="position"></param>
		/// <param name="spriteEffect"></param>
		public void PlayAnimation(GameTime gameTime, Vector2 position, SpriteEffects spriteEffect)
		{
			if (isCounting)
				time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			while (time > FrameTime)
			{
				orderIndex++;
				time = 0f;
			}

			if (orderIndex > order.Length - 1)
			{
				if (isLooping)
					orderIndex = 0;
				else
				{
					orderIndex = order.Length - 1;
					isCounting = false;
				}
			}

			frameIndex = order[orderIndex];

			Rectangle source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
			//ScreenManager.SpriteBatch.Draw(spriteSheet, position, source, Color.White);
			Rectangle rectPosition = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

			ScreenManager.SpriteBatch.Draw(spriteSheet, rectPosition, source, Color.White, 0, Vector2.Zero, spriteEffect, 0);
		}

		/// <summary>
		/// draw a single frame
		/// </summary>
		/// <param name="index"></param>
		/// <param name="position"></param>
		/// <param name="spriteEffect"></param>
		public void DrawFrame(int index, Rectangle position, SpriteEffects spriteEffect)
		{
			Rectangle source = new Rectangle(index * frameWidth, 0, frameWidth, frameHeight);
			ScreenManager.SpriteBatch.Draw(spriteSheet, position, source, Color.White, 0, Vector2.Zero, spriteEffect, 0);

			Reset();
		}
		/// <summary>
		/// overload that uses vector for position
		/// </summary>
		/// <param name="index"></param>
		/// <param name="position"></param>
		/// <param name="spriteEffect"></param>
		public void DrawFrame(int index, Vector2 position, SpriteEffects spriteEffect)
		{

			Rectangle source = new Rectangle(index * frameWidth, 0, frameWidth, frameHeight);
			Rectangle rectPos = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
			ScreenManager.SpriteBatch.Draw(spriteSheet, rectPos, source, Color.White, 0, Vector2.Zero, spriteEffect, 0);

			Reset();
		}


		/// <summary>
		/// reset frame index and timer
		/// </summary>
		private void Reset()
		{
			orderIndex = 0;
			isCounting = true;
		}

	}
}
