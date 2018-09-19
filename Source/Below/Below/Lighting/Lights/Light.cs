//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// The light type. 
	/// A pointlight shines equally bright into all directions.
	/// A spotlight shines into a specified direction.
	/// </summary>
	public enum LightType
	{
		Point,
		Spot
	}

	/// <summary>
	/// base properties of a light
	/// </summary>
	public abstract class Light
	{
		protected Color color;
		protected float actualPower;


		/// <summary>
		/// The position of the light in the scene.
		/// </summary>
		public Vector3 Position { get; set; }

		

		/// <summary>
		/// initial power of light
		/// </summary>
		public float Power
		{
			get { return actualPower; }
			set
			{
				actualPower = value;
			}
		}

		/// <summary>
		/// color of light
		/// </summary>
		public Color Color
		{
			get { return color; }
			set
			{
				color = value;
				Color4 = color.ToVector4();
			}
		}

		/// <summary>
		/// color as vector4
		/// </summary>
		public Vector4 Color4 { get; private set; }


		/// <summary>
		/// The distance over which the light decays to zero. 
		/// </summary>
		public float LightDecay { get; set; }

		/// <summary>
		/// type of light (point or spot)
		/// </summary>
		public LightType LightType { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="lightType"></param>
		protected Light(LightType lightType)
		{
			LightType = lightType;
		}

		/// <summary>
		/// whether light is enabled or not
		/// </summary>
		public bool IsEnabled { get; set; }
	}
}