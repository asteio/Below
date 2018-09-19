//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Below
{
	/// <summary>
	/// basic  GUI button 
	/// </summary>
	public class Button
	{
		private Texture2D buttonTexture;
		private Rectangle position;

		private SpriteFont font = ScreenManager.SmallFont;

		private MouseState oldMouse;
		private MouseState mouse;

		private Vector2 textPos;

		private string text = "";
		public string Text
		{
			get { return text; }
			set
			{
				text = value;

				Vector2 size = font.MeasureString(text);

				textPos = TextTools.CenterTextWithinRect(text, font, new Rectangle(position.X, position.Y, buttonTexture.Width, buttonTexture.Height));
			}
		}

		Color textColor = Color.White;
		public Color TextColor
		{
			set { textColor = value; }
		}
		Color hoverTextColor = Color.DarkGray;
		public Color HoverTextColor
		{
			set { hoverTextColor = value; }
		}

		private Color mainColor = Color.White;
		public Color MainColor
		{
			set { mainColor = value; }
		}

		private Color hoverColor = Color.DarkGray;
		public Color HoverColor
		{
			set { hoverColor = value; }
		}

		private bool clicked = false;
		public bool Clicked
		{
			get { return clicked; }
			set { clicked = value; }
		}

		/// <summary>
		/// constructor with default texture
		/// </summary>
		public Button()
		{
			buttonTexture = ImageTools.LoadTexture2D("Graphics/GUI/button");

			//set pos to -1000, 1000 to fix graphical but that causes the button to pop in the corner for a split second
			position = new Rectangle(-1000, -1000, buttonTexture.Width, buttonTexture.Height);

			Text = "Button";
		}

		/// <summary>
		/// constructor with custom texture
		/// </summary>
		/// <param name="ButtonTexture"></param>
		public Button(Texture2D ButtonTexture)
		{
			buttonTexture = ButtonTexture;

			//set pos to -1000, 1000 to fix graphical but that causes the button to pop in the corner for a split second
			position = new Rectangle(-1000, -1000, buttonTexture.Width, buttonTexture.Height);

			Text = "Button";
		}

		/// <summary>
		/// set location
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Location(int x, int y)
		{
			position.X = x;
			position.Y = y;

			Vector2 size = font.MeasureString(text);
			textPos = TextTools.CenterTextWithinRect(text, font, new Rectangle(position.X, position.Y, buttonTexture.Width, buttonTexture.Height));
		}

		/// <summary>
		/// update logic
		/// </summary>
		public void Update()
		{
			mouse = Mouse.GetState();

			//if clicked, Button.Clicked will return true
			if (mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
			{
				if (position.Contains(new Point(mouse.X, mouse.Y)))
				{
					clicked = true;
					SoundManager.PlaySound("button", 1f);
				}
			}

			oldMouse = mouse;
		}

		/// <summary>
		/// draw button
		/// </summary>
		public void Draw()
		{
			ScreenManager.SpriteBatch.Begin();

			if (position.Contains(new Point(mouse.X, mouse.Y)))
			{
				ScreenManager.SpriteBatch.Draw(buttonTexture, position, hoverColor);

				ScreenManager.SpriteBatch.DrawString(font, text, textPos, hoverTextColor);
			}
			else
			{
				ScreenManager.SpriteBatch.Draw(buttonTexture, position, mainColor);

				ScreenManager.SpriteBatch.DrawString(font, text, textPos, textColor);
			}

			ScreenManager.SpriteBatch.End();
		}

	}
}
