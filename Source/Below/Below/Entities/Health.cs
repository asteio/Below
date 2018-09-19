//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// can manage the health of any entity
	/// </summary>
	public class Health
	{
		//HealthOverlay overlay = new HealthOverlay();

		private Texture2D overlay;
		private Color overlayColor = Color.Transparent;
		private Texture2D healthBarOutline;
		private Texture2D healthBar;

		public int MaxHealth { get; }

		public int CurrentHealth { get; private set; }

		bool isDead;
		public bool IsDead
		{
			get { return isDead; }
		}

		float cooldown = 1f;
		float time = 1f;

		public bool GodMode = false;

		private bool isPlayer;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="MaxHealth"></param>
		public Health(int MaxHealth, bool player)
		{
			this.MaxHealth = MaxHealth;

			isPlayer = player;

			if (player)
				CurrentHealth = PlayerData.Load().Health;
			else
				CurrentHealth = MaxHealth;

			overlay = ImageTools.LoadTexture2D("Graphics/Overlays/damage flash");
			healthBar = overlay;
			healthBarOutline = ImageTools.LoadTexture2D("Graphics/Misc/healthbar_outline");
		}

		/// <summary>
		/// take damage
		/// </summary>
		/// <param name="amount"></param>
		public void TakeDamage(int amount)
		{
			if (time >= cooldown && !GodMode)//only take damage if timer has finished cooldown and godmode is not activated
			{
				SoundManager.PlaySound("hurt", 1f);
				CurrentHealth -= amount;
				overlayColor = Color.White;//reset overlay to fade out
										   //SoundManager.PlaySound("hurt", 0.5f);
				time = 0f;//reset cooldown timer

			}

			if (CurrentHealth <= 0)
			{
				CurrentHealth = 0;
				isDead = true;
			}
		}

		/// <summary>
		/// heal player
		/// </summary>
		/// <param name="amount"></param>
		public void Heal(int amount)
		{
			if (CurrentHealth < MaxHealth)
			{
				CurrentHealth += amount;
				//SoundManager.PlaySound("heal", 0.5f);
			}

			//so the player cant heal past max health
			if (CurrentHealth > MaxHealth)
			{
				CurrentHealth = MaxHealth;
			}
		}

		/// <summary>
		/// draw over lighting
		/// </summary>
		public void DrawOverLighting()
		{

			if (overlayColor.A >= 0)
				overlayColor *= 0.98f;//fade out overlay

			Vector2 size = ScreenManager.SmallFont.MeasureString(getHealthDescriptor(CurrentHealth));
			Vector2 textPos = new Vector2(1280 - size.X - 25, 20);

			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, getHealthDescriptor(CurrentHealth), textPos, Color.White);
			ScreenManager.SpriteBatch.Draw(overlay, Vector2.Zero, overlayColor);
			//ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, CurrentHealth.ToString() + " / " + MaxHealth.ToString(), )
			ScreenManager.SpriteBatch.End();

		
		}

		/// <summary>
		/// used for bosses only
		/// </summary>
		/// <param name="gameTime"></param>
		public void UpdateCooldownTimer(GameTime gameTime)
		{

			if (time <= cooldown)//while  the time is less than the cooldown time limit, add time
				time += (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		/// <summary>
		/// draw the health bar
		/// </summary>
		/// <param name="position"></param>
		public void DrawHealthBar(Vector2 position, float scale)
		{
			if (scale < .5f)
				scale = .5f;

			int outlineWidth = (int)(((healthBarOutline.Width-1) * MaxHealth) * scale);
			int outlineHeight = (int)(healthBarOutline.Height * scale);

			int healthWidth = (CurrentHealth * outlineWidth) / MaxHealth;
			Rectangle positionRect = new Rectangle((int)position.X + 1, (int)position.Y + 1, healthWidth, outlineHeight - 2);

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(healthBar, positionRect, Color.Red);

			for (int i = 0; i < MaxHealth; i++)
			{
				Rectangle newPos = new Rectangle((int)position.X + (int)(((healthBarOutline.Width-1) * scale) * i), (int)position.Y, (int)(healthBarOutline.Width * scale), (int)(healthBarOutline.Height * scale));
				ScreenManager.SpriteBatch.Draw(healthBarOutline, newPos, Color.White);
			}
			

			ScreenManager.SpriteBatch.End();
		}

		/// <summary>
		/// get the health discriptor based on the current health
		/// </summary>
		/// <param name="health"></param>
		/// <returns></returns>
		private string getHealthDescriptor(int health)
		{

			if (!GodMode)
			{
				switch (health)
				{
					case 5:
						return "Healthy";
					case 4:
						return "Hurt";
					case 3:
						return "Injured";
					case 2:
						return "Wounded";
					case 1:
						return "Mortally Wounded";
					case 0:
						return "Dead";
					default:
						return "Dead";
				}
			}
			else
				return "God Mode";
		}

		/// <summary>
		/// save health data
		/// </summary>
		public void Save()
		{
			PlayerData playerData = PlayerData.Load();
			playerData.Health = CurrentHealth;
			playerData.Save();
		}
	}
}
