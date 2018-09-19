//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// display game controls
	/// </summary>
	public class ControlsScreen : GameScreen
	{
		private Button backButton;

		private List<string> controls;

		/// <summary>
		/// load graphics
		/// </summary>
		public override void LoadContent()
		{

			backButton = new Button();
			backButton.Location(20, 20);
			backButton.Text = "Back";

			controls = new List<string>();
			controls.Add("A - Move Left");
			controls.Add("D - Move Right");
			controls.Add("W/Space - Jump");
			controls.Add("F - Toggle Flashlight");
			controls.Add("E - Interact");
			controls.Add("Q - Heal");
			controls.Add("Escape - Pause");

			base.LoadContent();
		}

		/// <summary>
		/// unload content
		/// </summary>
		public override void UnloadContent()
		{
			base.UnloadContent();
		}

		/// <summary>
		/// update buttons
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			backButton.Update();
			if (backButton.Clicked)
				ScreenManager.RemoveScreen(this);

			base.Update(gameTime);
		}

		/// <summary>
		/// draw stuff over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{
			backButton.Draw();

			for (int i = 0; i < controls.Count; i++)
			{
				ScreenManager.SpriteBatch.Begin();

				ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, controls[i], new Vector2(200, 320+(80 * i)), Color.White);

				ScreenManager.SpriteBatch.End();
			}

			base.DrawOverLighting(gameTime);
		}
	}
}
