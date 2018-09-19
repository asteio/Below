//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// portal that counts as an exit in a level
	/// </summary>
	public class Portal
	{
		private Texture2D portal;

		private Level level;
		private Rectangle boundingRect;

		//inactive by default
		public bool IsActive { get; set; } = false;
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="Player"></param>
		/// <param name="Position"></param>
		/// <param name="CurrentScreen"></param>
		/// <param name="TargetScreen"></param>
		public Portal(Level Level, Vector2 Position)
		{
			portal = ImageTools.LoadTexture2D("Graphics/Misc/finished_portal");

			level = Level;

			boundingRect = new Rectangle((int)Position.X, (int)Position.Y, portal.Width, portal.Height);

		}

		/// <summary>
		/// update portal logic and draw it
		/// </summary>
		public void UpdateAndDraw()
		{
			if (IsActive)
			{
				ScreenManager.StartCameraSpriteBatch();
				ScreenManager.SpriteBatch.Draw(portal, boundingRect, Color.White);
				ScreenManager.EndCameraSpriteBatch();

				if (level.Player.BoundingRectangle.Intersects(boundingRect))// && ScreenManager.IsKeyPressed(Keybindings.Interact))
				{
					SoundManager.PlaySound("portal", 1f);
					level.ExitReached = true;
				}
			}
		}
	}
}
