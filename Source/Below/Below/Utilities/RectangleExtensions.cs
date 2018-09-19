//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// extensions for the Monogame's rectangle class
	/// </summary>
	public static class RectangleExtensions
	{
		/// <summary>
		/// Calculates depth of intersection between two rectangles.
		/// </summary>
		/// <returns>
		/// overlap between rectangles
		/// </returns>
		public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
		{
			// Calculate half sizes.
			float halfWidthA = rectA.Width / 2.0f;
			float halfHeightA = rectA.Height / 2.0f;
			float halfWidthB = rectB.Width / 2.0f;
			float halfHeightB = rectB.Height / 2.0f;

			// Calculate centers.
			Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
			Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

			// Calculate current and minimum-non-intersecting distances between centers.
			float distanceX = centerA.X - centerB.X;
			float distanceY = centerA.Y - centerB.Y;
			float minDistanceX = halfWidthA + halfWidthB;
			float minDistanceY = halfHeightA + halfHeightB;

			// If we are not intersecting at all, return (0, 0).
			if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
				return Vector2.Zero;

			// Calculate and return intersection depths.
			float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
			float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
			return new Vector2(depthX, depthY);
		}

		/// <summary>
		/// Gets the position of the center of the bottom edge of the rectangle.
		/// </summary>
		public static Vector2 GetBottomCenter(this Rectangle rect)
		{
			return new Vector2(rect.X + rect.Width / 2.0f, rect.Bottom);
		}

		/// <summary>
		/// returns true if rectangle is within another rectangle by a given radius
		/// </summary>
		/// <param name="r"></param>
		/// <param name="target"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static bool IsWithin(this Rectangle r, Rectangle target, int radius)
		{
			int newX = target.X - radius;
			int newY = target.Y - radius;
			int newWidth = target.Width + (2 * radius);
			int newHeight = target.Height + (2 * radius);

			Rectangle newTarget = new Rectangle(newX, newY, newWidth, newHeight);

			if (r.Intersects(newTarget))
				return true;
			else
				return false;
		}
		
	}
}
