//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using Microsoft.Xna.Framework;

namespace Below
{
	/// <summary>
	/// base class all screens will inherit
	/// has all methods needed for the xna game loop
	/// </summary>
	public class GameScreen
	{
		public bool IsActive = true;
		public bool IsPopup = false;
		public bool Pausable = false;

		public Type Type;

		public Color BackgroundColor = Color.Black;
		
		public virtual void LoadContent() { }//loads screen content
		public virtual void Update(GameTime gameTime) { }//update screen logic
		public virtual void Draw(GameTime gameTime) { }//draw graphical assets - under lighting
		public virtual void DrawOverLighting(GameTime gameTime) { }//draw graphical assets - above lighting
		public virtual void UnloadContent() { /*ScreenManager.ContentMgr.Unload();*/ }//unload screen content

		/// <summary>
		/// constructor
		/// </summary>
		public GameScreen()
		{
			Type = GetType();
		}
	}
}
