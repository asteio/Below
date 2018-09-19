//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// the player's way of healing
	/// A canteen holds 3 drink and each drink heals for 1 health
	/// </summary>
	public class Canteen
	{
		private Texture2D icon;
		private int usesRemaining;

		private Player player;

		private int offset = 40;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="Player"></param>
		public Canteen(Player Player)
		{
			player = Player;

			icon = ImageTools.LoadTexture2D("Graphics/Misc/canteen");

			PlayerData playerData = PlayerData.Load();
			usesRemaining = playerData.FlaskUses;
		}

		/// <summary>
		/// heal with the flask
		/// </summary>
		public void Heal()
		{
			//only heal if there are enough uses and the player is actually damaged
			if (usesRemaining > 0 && player.Health.CurrentHealth < 5)
			{
				SoundManager.PlaySound("water", 1f);
				player.Health.Heal(1);
				usesRemaining--;
			}

		}

		/// <summary>
		/// refill thse flask
		/// </summary>
		public void Refill()
		{
			SoundManager.PlaySound("waterfill", 1f);
			usesRemaining = 3;
		}
		
		/// <summary>
		/// draw the flask icon and the number of uses remaining
		/// </summary>
		public void DrawOverLighting()
		{
			

			Vector2 textSize = ScreenManager.SmallFont.MeasureString(usesRemaining.ToString());
			Vector2 size = new Vector2(icon.Width + 10 + textSize.X, icon.Height);

			Vector2 iconLocation = new Vector2(ScreenManager.SCREEN_WIDTH - size.X - offset, ScreenManager.SCREEN_HEIGHT - size.Y - offset);
			Vector2 textLocation = new Vector2(iconLocation.X + icon.Width + 10, iconLocation.Y + (icon.Height - textSize.Y)/2);

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(icon, iconLocation, Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, usesRemaining.ToString(), textLocation, Color.White);

			ScreenManager.SpriteBatch.End();

			
		}

		/// <summary>
		/// save the number of flasks uses to the xml document
		/// </summary>
		public void Save()
		{
			PlayerData playerData = PlayerData.Load();
			playerData.FlaskUses = usesRemaining;
			playerData.Save();
		}

	}
}
