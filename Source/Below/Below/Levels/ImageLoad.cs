//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.IO;
using System.Drawing;

namespace Below
{
	/// <summary>
	/// load a color array from a bitmap
	/// </summary>
	public class ImageLoad
	{

		/// <summary>
		/// load a bmp from a path
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private static Bitmap LoadBMP(string filename)
		{
			Bitmap bmp;

			using (Stream BitmapStream = System.IO.File.Open(filename, System.IO.FileMode.Open))
			{
				Image img = Image.FromStream(BitmapStream);

				bmp = new Bitmap(img);
			}

			return bmp;
		}


		/// <summary>
		/// load a 2d color array from the pixels in a bitmap
		/// </summary>
		/// <param name="bmpPath"></param>
		/// <returns></returns>
		public static Microsoft.Xna.Framework.Color[,] LoadArray(string bmpPath)
		{
			Bitmap bmp = LoadBMP(bmpPath);

			int width = bmp.Width;
			int height = bmp.Height;
			//Color level = Color.White;
			Microsoft.Xna.Framework.Color[,] grid = new Microsoft.Xna.Framework.Color[height, width];

			for (int h = 0; h < height; h++)
			{
				for (int w = 0; w < width; w++)
				{
					System.Drawing.Color sysColor = bmp.GetPixel(w, h);//GetPixel only works with System.Drawing.Color
					Microsoft.Xna.Framework.Color xnaColor = new Microsoft.Xna.Framework.Color(sysColor.R, sysColor.G, sysColor.B, sysColor.A);//convert system color to xna color

					grid[h, w] = xnaColor;
				}
			}

			return grid;
		}


	}
}
