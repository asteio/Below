//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using Microsoft.Xna.Framework;

namespace Below
{
	public class SpotLight : Light
	{
		private float spotRotation;

		/// <summary>
		/// narrowness of lgiht
		/// </summary>
		/// <value>The spot beamwidth exponent.</value>
		public float SpotBeamWidthExponent { get; set; }

		/// <summary>
		/// angle of the beam
		/// </summary>
		public float SpotRotation
		{
			get { return spotRotation; }
			set
			{
				spotRotation = value;
				Direction = new Vector3(
					(float)Math.Cos(spotRotation),
					(float)Math.Sin(spotRotation),
					Direction.Z);
			}
		}

		/// <summary>
		/// direction it is shining
		/// </summary>
		public Vector3 Direction { get; private set; }
	
		/// <summary>
		/// depth of light
		/// </summary>
		public float DirectionZ
		{
			get { return Direction.Z; }
			set
			{
				Direction = new Vector3(Direction.X, Direction.Y, value);
			}
		}

		/// <summary>
		/// lights in a specific direction
		/// </summary>
		public SpotLight()
			: base(LightType.Spot)
		{
		}

	}
}