//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// Tile types
	/// </summary>
	public enum TileCollision
	{
		Passable = 0,//no collisions
		Impassable = 1,//collisions on all sides
		Platform = 2,//collision on top only
	}

	/// <summary>
	/// load tile
	/// </summary>
	struct Tile
	{
		//source rectangle within the texture atlas
		public Rectangle Source;

		//collsion type
		public TileCollision Collision;
		
		public const int Size = 32;
		
		public Tile(Rectangle source, TileCollision collision)
		{
			//Texture = texture;
			Source = source;
			Collision = collision;
		}
	}
}
