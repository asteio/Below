//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// base class for a boss that has health
	/// </summary>
	public class Boss : Entity
	{
		public Vector2 Position { get; set; }
		public Rectangle BoundingRect { get; set; }

		public Health Health { get; private set; }

		//a boss can have 3 different groups of projectiles max
		public List<ShadowBall> Projectiles { get; set; } = new List<ShadowBall>();
		public List<ShadowBall> Projectiles2 { get; set; } = new List<ShadowBall>();
		public List<ShadowBall> Projectiles3 { get; set; } = new List<ShadowBall>();

		/// <summary>
		/// constructor
		/// </summary>
		public Boss(int MaxHealth)
		{
			Health = new Health(MaxHealth, false);
			
		}
	
		/// <summary>
		/// launch each list of projectiles
		/// </summary>
		public void LaunchAllProjectiles()
		{
			if (!Health.IsDead)
			{
				foreach (ShadowBall ball in Projectiles)
				{
					ball.IsActive = true;
				}
				foreach (ShadowBall ball in Projectiles2)
				{
					ball.IsActive = true;
				}
				foreach (ShadowBall ball in Projectiles3)
				{
					ball.IsActive = true;
				}
			}
		}

		/// <summary>
		/// launch a specific list of projectiles
		/// </summary>
		/// <param name="projectiles"></param>
		public void LaunchProjectiles(List<ShadowBall> projectiles)
		{
			if (!Health.IsDead)
			{
				foreach (ShadowBall ball in projectiles)
				{
					ball.IsActive = true;
				}
			}
		}

		/// <summary>
		/// check for and apply damage to boss when it collides with a shadowball
		/// </summary>
		public void CheckProjectileDamage()
		{
			foreach (ShadowBall ball in Projectiles)
			{
				if (ball.Deflected)
				{
					if (ball.BoundingRectangle.Intersects(BoundingRect))
						Health.TakeDamage(1);
				}
			}
			foreach (ShadowBall ball in Projectiles2)
			{
				if (ball.Deflected)
				{
					if (ball.BoundingRectangle.Intersects(BoundingRect))
						Health.TakeDamage(1);
				}
			}
			foreach (ShadowBall ball in Projectiles3)
			{
				if (ball.Deflected)
				{
					if (ball.BoundingRectangle.Intersects(BoundingRect))
						Health.TakeDamage(1);
				}
			}
		}

		/// <summary>
		/// draw the shadowballs
		/// </summary>
		public void DrawProjectiles()
		{
			foreach (ShadowBall ball in Projectiles)
			{
				ball.Draw();
			}
			foreach (ShadowBall ball in Projectiles2)
			{
				ball.Draw();
			}
			foreach (ShadowBall ball in Projectiles3)
			{
				ball.Draw();
			}
		}
		
	}
}
