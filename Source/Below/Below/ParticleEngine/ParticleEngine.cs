//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// emits many particles out
	/// </summary>
	public class ParticleEngine
	{
		private Random random;
		public Vector2 EmitterLocation { get; set; }
		private List<Particle> particles;
		private List<Texture2D> textures;
		

		private bool customVelocity;
		public Vector2 Velocity { private get; set; }

		public bool IsActive { get; set; } = true;
		

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="textures"></param>
		/// <param name="location"></param>
		public ParticleEngine(List<Texture2D> textures, Vector2 location, bool CustomVelocity)
		{
			EmitterLocation = location;
			this.textures = textures;
			this.particles = new List<Particle>();
			random = new Random();
			customVelocity = CustomVelocity;
		}
		

		/// <summary>
		///	update particle velocities
		/// </summary>
		public void Update()
		{
			int total = 10;
			//add the total amount of particles
			for (int i = 0; i < total; i++)
			{
				particles.Add(GenerateNewParticle());
			}

			//update movement of each particle and remove 
			for (int particle = 0; particle < particles.Count; particle++)
			{
				particles[particle].Update();
				if (particles[particle].LifeTime <= 0)
				{
					particles.RemoveAt(particle);
					particle--;
				}
			}
		}

		/// <summary>
		/// create a new particle 
		/// </summary>
		/// <returns></returns>
		private Particle GenerateNewParticle()
		{
			Texture2D texture = textures[random.Next(textures.Count)];
			Vector2 position = EmitterLocation;


			if (!customVelocity)
				Velocity = new Vector2(
									1f * (float)(random.NextDouble() * 2 - 1),
									1f * (float)(random.NextDouble() * 2 - 1));
			
			float angle = 1f;
			float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
			Color color = new Color(
						(float)random.NextDouble(),
						(float)random.NextDouble(),
						(float)random.NextDouble());
			float size = (float)random.NextDouble();
			int ltime = 10 + random.Next(25);

			return new Particle(texture, position, Velocity, angle, angularVelocity, size, ltime);
		}

		/// <summary>
		/// draw the particles
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void Draw()
		{

			if (IsActive)
			{
				for (int index = 0; index < particles.Count; index++)
				{
					particles[index].Draw();
				}
			}
		}

		/// <summary>
		/// clear all particles
		/// </summary>
		public void ClearParticles()
		{
			particles.Clear();
		}
	}
}
