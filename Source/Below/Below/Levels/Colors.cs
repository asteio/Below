//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// the colors that will load tiles
	/// </summary>
	public static class Colors
	{
		public static Color Start = Color.Lime;//passable, no texture
		public static Color End = Color.Blue;//passable, no texture
		public static Color Water = Color.Aqua;//water tile that refills 
		public static Color Grabber = Color.Red;//enemy
		public static Color Portal = Color.Orange;//(255, 165, 0) orange - portal

		public static Color Transparent = new Color(165, 0, 255);
		public static Color Platform = new Color(0, 64, 0);//dark green
		public static Color T1 = Color.Black;//impassable
		public static Color T2 = new Color(64, 64, 64);//impasable
		public static Color T3 = new Color(128, 128, 128);//impassable
		public static Color T4 = new Color(72, 0, 255);//purple pure yellow doesn't seem to work
		public static Color T5 = new Color(128, 128, 0);//darker yellow
		public static Color T6 = new Color(64, 64, 0);//darkest yellow
		public static Color T7 = new Color(0, 128, 128);//dark aqua
		public static Color T8 = new Color(0, 64, 64);//darkest aqua
		public static Color T9 = new Color(255, 0, 255);//pink
		public static Color T10 = new Color(128, 0, 128);//darker pink
		public static Color T11 = new Color(64, 0, 64);//darkest pink
	}
}
