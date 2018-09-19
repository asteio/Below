//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// a collection of helper methods for text
	/// </summary>
	public static class TextTools
	{

		/// <summary>
		/// returns true centered location of text
		/// </summary>
		/// <param name="font"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static Vector2 CenterText(SpriteFont font, string text)
		{
			Vector2 textDim = font.MeasureString(text);

			int x = (ScreenManager.SCREEN_WIDTH - (int)textDim.X) / 2;
			int y = (ScreenManager.SCREEN_HEIGHT - (int)textDim.Y) / 2;

			return new Vector2(x, y);
		}

		/// <summary>
		/// adds newline in a string to make it fit into a specified width
		/// </summary>
		/// <param name="font"></param>
		/// <param name="text"></param>
		/// <param name="maxLineWidth"></param>
		/// <returns></returns>
		public static string WrapText(SpriteFont font, string text, float maxLineWidth)
		{
			string[] words = text.Split(' ');
			StringBuilder sb = new StringBuilder();
			float lineWidth = 0f;
			float spaceWidth = font.MeasureString(" ").X;

			foreach (string word in words)
			{
				Vector2 size = font.MeasureString(word);
				
				if (lineWidth + size.X < maxLineWidth)
				{
					sb.Append(word + " ");
					lineWidth += size.X + spaceWidth;
				}
				else
				{
					sb.Append("\n" + word + " ");
					lineWidth = size.X + spaceWidth;
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// returns vector that centers text in an a rectangle
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static Vector2 CenterTextWithinRect(string text, SpriteFont font, Rectangle rect)
		{
			Vector2 size = font.MeasureString(text);
			return new Vector2(rect.X + (rect.Width - size.X) / 2, rect.Y + (rect.Height - size.Y) / 2);
		}
	}
}
