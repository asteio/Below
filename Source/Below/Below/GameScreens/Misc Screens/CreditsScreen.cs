//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// team roles and other credits
	/// </summary>
	public class CreditsScreen : GameScreen
	{
		private float speed = 3f;

		private Texture2D sharkboyLogo;
		private Vector2 sharkVector;
		private Texture2D belowLogo;
		private Vector2 belowVector;
		private Texture2D cogThoughtLogo;
		private Vector2 cogVector;

		private string h1 = "Hersh Vakharia";
		private Vector2 hv1;
		private string h2 = "Team Leader & Lead Programmer";
		private Vector2 hv2;

		private string a1 = "Andrew Burg";
		private Vector2 av1;
		private string a2 = "Visual Artist";
		private Vector2 av2;

		private string d1 = "Dakota Wilson";
		private Vector2 dv1;
		private string d2 = "Music Producer";
		private Vector2 dv2;

		private string s1 = "Sean Kosman";
		private Vector2 sv1;
		private string s2 = "Sound Producer";
		private Vector2 sv2;
		
		private string c1 = "All code and graphical/audio content is 100% originally created by the Shark Boys group";
		private Vector2 cv1;
		private string c2 = "aside from the methods and classes used directly from the Monogame/XNA Framework";
		private Vector2 cv2;

		/// <summary>
		/// load assets
		/// </summary>
		public override void LoadContent()
		{
			SoundManager.PlaySong("Credits", true);

			belowLogo = ImageTools.LoadTexture2D("Graphics/Logos/below_logo");
			belowVector = ImageTools.CenterImage_vect(belowLogo);

			sharkboyLogo = ImageTools.LoadTexture2D("Graphics/Logos/sharkboylogo");
			sharkVector = new Vector2(ImageTools.CenterImage_vect(sharkboyLogo).X, 1400);

			cogThoughtLogo = ImageTools.LoadTexture2D("Graphics/Logos/cognitive");
			cogVector = new Vector2(ImageTools.CenterImage(cogThoughtLogo).X, 3100);

			hv1 = new Vector2(TextTools.CenterText(ScreenManager.MediumFont, h1).X, 5000);
			hv2 = new Vector2(TextTools.CenterText(ScreenManager.SmallFont, h2).X, 5070);

			av1 = new Vector2(TextTools.CenterText(ScreenManager.MediumFont, a1).X, 6000);
			av2 = new Vector2(TextTools.CenterText(ScreenManager.SmallFont, a2).X, 6070);

			dv1 = new Vector2(TextTools.CenterText(ScreenManager.MediumFont, d1).X, 7000);
			dv2 = new Vector2(TextTools.CenterText(ScreenManager.SmallFont, d2).X, 7070);

			sv1 = new Vector2(TextTools.CenterText(ScreenManager.MediumFont, s1).X, 8000);
			sv2 = new Vector2(TextTools.CenterText(ScreenManager.SmallFont, s2).X, 8070);

			cv1 = new Vector2(TextTools.CenterText(ScreenManager.SmallFont, c1).X, 9000);
			cv2 = new Vector2(TextTools.CenterText(ScreenManager.SmallFont, c2).X, 9070);

			base.LoadContent();
		}

		/// <summary>
		/// update movement
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			if (ScreenManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
				ScreenManager.ChangeScreens(this, new MainMenuScreen());

			if (belowVector.Y > -1280)
				belowVector.Y -= speed;

			if (sharkVector.Y > -1280)
				sharkVector.Y -= speed;


			if (cogVector.Y > -1280)
				cogVector.Y -= speed;

			if (hv2.Y > -100)
			{
				hv1.Y -= speed;
				hv2.Y -= speed;
			}

			if (av2.Y > -100)
			{
				av1.Y -= speed;
				av2.Y -= speed;
			}

			if (dv2.Y > -100)
			{
				dv1.Y -= speed;
				dv2.Y -= speed;
			}

			if (sv2.Y > -100)
			{
				sv1.Y -= speed;
				sv2.Y -= speed;
			}

			if (cv2.Y > -100)
			{
				cv1.Y -= speed;
				cv2.Y -= speed;
			}



			if (cv2.Y < -80)
				ScreenManager.ChangeScreens(this, new MainMenuScreen());

			base.Update(gameTime);
		}

		/// <summary>
		/// draw over lighting
		/// </summary>
		/// <param name="gameTime"></param>
		public override void DrawOverLighting(GameTime gameTime)
		{

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(belowLogo, belowVector, Color.White);
			ScreenManager.SpriteBatch.Draw(sharkboyLogo, sharkVector, Color.White);
			ScreenManager.SpriteBatch.Draw(cogThoughtLogo, cogVector, Color.White);

			ScreenManager.SpriteBatch.DrawString(ScreenManager.MediumFont, h1, hv1, Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, h2, hv2, Color.White);

			ScreenManager.SpriteBatch.DrawString(ScreenManager.MediumFont, a1, av1, Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, a2, av2, Color.White);

			ScreenManager.SpriteBatch.DrawString(ScreenManager.MediumFont, d1, dv1, Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, d2, dv2, Color.White);

			ScreenManager.SpriteBatch.DrawString(ScreenManager.MediumFont, s1, sv1, Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, s2, sv2, Color.White);

			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, c1, cv1, Color.White);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.SmallFont, c2, cv2, Color.White);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}

	}
}
