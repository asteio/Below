//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// 2D raycasting system using bresenham lines
	/// </summary>
	public struct Ray2D
	{
		private Vector2 startPos;
		private Vector2 endPos;
		private readonly List<Point> result;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="startPos"></param>
		/// <param name="endPos"></param>
		public Ray2D(Vector2 startPos, Vector2 endPos)
		{
			this.startPos = startPos;
			this.endPos = endPos;
			result = new List<Point>();
		}

		/// <summary>  
		/// Determine if the ray intersects the rectangle
		/// return point ray intersects with rectangle
		/// </summary>  
		/// <param name="rectangle">Rectangle to check</param>  
		/// <returns></returns>  
		public Vector2 Intersects(Rectangle rectangle)
		{
			Point p0 = new Point((int)startPos.X, (int)startPos.Y);
			Point p1 = new Point((int)endPos.X, (int)endPos.Y);

			foreach (Point testPoint in BresenhamLine(p0, p1))
			{
				if (rectangle.Contains(testPoint))
					return new Vector2((float)testPoint.X, (float)testPoint.Y);
			}

			return Vector2.Zero;
		}

		/// <summary>
		/// returns true of ray intersects with rect
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public bool HasCollided(Rectangle rectangle)
		{
			Point p0 = new Point((int)startPos.X, (int)startPos.Y);
			Point p1 = new Point((int)endPos.X, (int)endPos.Y);

			Vector2 vector = Vector2.Zero;

			foreach (Point testPoint in BresenhamLine(p0, p1))
			{
				if (rectangle.Contains(testPoint))
				{
					vector = new Vector2((float)testPoint.X, (float)testPoint.Y);
				}
				
			}

			if (vector != Vector2.Zero)
				return true;
			else
				return false;
		}
		

		/// <summary>
		/// swap values of a and b
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <param name="b"></param>
		private void Swap<T>(ref T a, ref T b)
		{
			T c = a;
			a = b;
			b = c;
		}

		/// <summary>
		/// return list of points between two points
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <returns></returns>
		private List<Point> BresenhamLine(Point p0, Point p1)
		{
			return BresenhamLine(p0.X, p0.Y, p1.X, p1.Y);
		}

		/// <summary>
		/// return list of points between two coordinates
		/// </summary>
		/// <param name="x0"></param>
		/// <param name="y0"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <returns></returns>
		private List<Point> BresenhamLine(int x0, int y0, int x1, int y1)
		{
			result.Clear();

			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (steep)
			{
				Swap(ref x0, ref y0);
				Swap(ref x1, ref y1);
			}
			if (x0 > x1)
			{
				Swap(ref x0, ref x1);
				Swap(ref y0, ref y1);
			}

			int deltax = x1 - x0;
			int deltay = Math.Abs(y1 - y0);
			int error = 0;
			int ystep;
			int y = y0;

			if (y0 < y1)
				ystep = 1;
			else
				ystep = -1;

			for (int x = x0; x <= x1; x++)
			{
				if (steep)
					result.Add(new Point(y, x));
				else
					result.Add(new Point(x, y));
				error += deltay;
				if (2 * error >= deltax)
				{
					y += ystep;
					error -= deltax;
				}
			}

			return result;
		}

		/// <summary>
		/// draw the line
		/// </summary>
		public void Draw()
		{
			
			List<Point> points = BresenhamLine(new Point((int)startPos.X, (int)startPos.Y), new Point((int)endPos.X, (int)endPos.Y));
			ScreenManager.SpriteBatch.Begin();

			foreach(Point point in result)
			{
				ScreenManager.SpriteBatch.Draw(ImageTools.LoadTexture2D("Graphics/Overlays/lightning"), new Vector2(point.X, point.Y), Color.Red);
			}

			ScreenManager.SpriteBatch.End();
		}
	}
}
