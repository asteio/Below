//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

namespace Below
{
	/// <summary>
	/// contains the basic tools for creating a moving entity
	/// </summary>
	public class Entity
	{

		public const int MaxHP = 5;

		/// <summary>
		/// BUG - player glides across top of blocks
		/// </summary>
		// Constants for controling horizontal movement
		public const float MoveAcceleration = 13000.0f;
		public const float MaxMoveSpeed = 2000f;//1750.0f;
		public const float GroundDragFactor = 0.48f;//0.48f;
		public const float AirDragFactor = 0.48f;//0.58f;

		// Constants for controlling vertical movement
		public const float MaxJumpTime = .35f;
		public const float JumpLaunchVelocity = -3500.0f;
		public const float GravityAcceleration = 3400.0f;
		public const float MaxFallSpeed = 550.0f;
		public const float JumpControlPower = 0.14f;

		
		
	}
}
