//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// the base level class
	/// </summary>
	public class Level 
	{
		private Texture2D atlas;

		private Color[,] colorMap;
		private Tile[,] tiles;

		public Player Player { get; private set; }

		public bool ExitReached = false;

		public string CamType = "static";
		
		//the row of tiles in the atlas that will be used
		public int TileSetRow = 0;

		private List<Rectangle> exits = new List<Rectangle>();
		private List<GrabberEnemy> grabberEnemies = new List<GrabberEnemy>();
		private List<Rectangle> waterTiles = new List<Rectangle>();

		public Portal Portal { get; private set; }

		/// <summary>
		/// constructer
		/// </summary>
		/// <param name="BmpPath"></param>
		public Level(string BmpPath)
		{
			//set color map from bitmap
			colorMap = ImageLoad.LoadArray(BmpPath);

			//create tile map with same dimensions as color map
			tiles = new Tile[colorMap.GetLength(0), colorMap.GetLength(1)];
		}

		
		#region Load tiles

		/// <summary>
		/// load tiles[,] from the colorMap
		/// </summary>
		/// <param name="colorMap"></param>
		public virtual void LoadLevel()
		{
			atlas = ImageTools.LoadTexture2D("Tiles/Atlas");

			Player = new Player(this, new Vector2(ScreenManager.SCREEN_WIDTH/2, ScreenManager.SCREEN_HEIGHT/2));
			
			//cycle through each color in the color map and assign a tile
			for (int h = 0; h < colorMap.GetLength(0); h++)
			{
				for (int w = 0; w < colorMap.GetLength(1); w++)
				{
					if (colorMap[h, w] == Colors.Start)
					{
						//load player positioning and center camera
						LoadPlayer(h, w);
						ScreenManager.Camera.CenterX(Player.BoundingRectangle);
					}
					else if (colorMap[h, w] == Colors.End)
					{
						LoadExit(h, w);
					}
					else if (colorMap[h, w] == Colors.Grabber)
					{
						//load enemy tile
						LoadGrabberEnemy(h, w);
					}
					else if (colorMap[h, w] == Colors.Water)
					{
						//load water tile
						LoadWater(h, w);
					}
					else if (colorMap[h, w] == Colors.Portal)
					{
						//load a portal
						Portal = new Portal(this, new Vector2(w * Tile.Size, (h - 3) * Tile.Size));
					}

					//place the tile
					tiles[h, w] = PlaceTile(colorMap[h, w]);
				}
			}

			
		}

		/// <summary>
		/// set the Player position from given tile coordinates
		/// </summary>
		/// <param name="h"></param>
		/// <param name="w"></param>
		private void LoadPlayer(int h, int w)
		{
			Player.Position = new Vector2(w * Tile.Size, (h - 1) * Tile.Size);
		}

		/// <summary>
		/// get tilese that will act as an exit
		/// </summary>
		/// <param name="h"></param>
		/// <param name="w"></param>
		private void LoadExit(int h, int w)
		{
			exits.Add(new Rectangle(w * Tile.Size, h * Tile.Size, Tile.Size, Tile.Size));
		}

		/// <summary>
		/// load grabber enemy tile
		/// </summary>
		/// <param name="h"></param>
		/// <param name="w"></param>
		private void LoadGrabberEnemy(int h, int w)
		{
			grabberEnemies.Add(new GrabberEnemy(Player, new Vector2(w * Tile.Size, h * Tile.Size)));
		}

		/// <summary>
		/// load water blocks
		/// </summary>
		/// <param name="h"></param>
		/// <param name="w"></param>
		private void LoadWater(int h, int w)
		{
			waterTiles.Add(new Rectangle(w * Tile.Size, h * Tile.Size, Tile.Size, Tile.Size));
		}

		/// <summary>
		/// load tile with the collumn index
		/// </summary>
		/// <param name="textureName"></param>
		/// <param name="collision"></param>
		/// <returns></returns>
		private Tile LoadTile(int collumnIndex, TileCollision collision)
		{
			return new Tile(ImageTools.GetSourceRect(TileSetRow, collumnIndex), collision);
		}

		/// <summary>
		/// load a tile based on a specific color
		/// </summary>
		/// <param name="pixCol"></param>
		/// <returns></returns>
		private Tile PlaceTile(Color pixCol)
		{
			if (pixCol == Colors.Water)
				return LoadTile(0, TileCollision.Passable);
			else if (pixCol == Colors.Platform)
				return LoadTile(10, TileCollision.Platform);
			else if (pixCol == Colors.T1)
				return LoadTile(0, TileCollision.Impassable);
			else if (pixCol == Colors.T2)
				return LoadTile(1, TileCollision.Impassable);
			else if (pixCol == Colors.T3)
				return LoadTile(2, TileCollision.Impassable);
			else if (pixCol == Colors.T4)
				return LoadTile(3, TileCollision.Impassable);
			else if (pixCol == Colors.T5)
				return LoadTile(4, TileCollision.Impassable);
			else if (pixCol == Colors.T6)
				return LoadTile(5, TileCollision.Impassable);
			else if (pixCol == Colors.T7)
				return LoadTile(6, TileCollision.Impassable);
			else if (pixCol == Colors.T8)
				return LoadTile(7, TileCollision.Impassable);
			else if (pixCol == Colors.T9)
				return LoadTile(8, TileCollision.Impassable);
			else if (pixCol == Colors.T10)
				return LoadTile(9, TileCollision.Impassable);
			else if (pixCol == Colors.T11)
				return LoadTile(10, TileCollision.Impassable);
			else if (pixCol == Colors.Transparent)
				return new Tile(Rectangle.Empty, TileCollision.Impassable);
			else
				return new Tile(Rectangle.Empty, TileCollision.Passable);//empty tile
			
		}
		#endregion

		#region collisions and bounds stuff

		/// <summary>
		/// Gets the collision mode of the tile at a particular location.
		/// </summary>
		public TileCollision GetCollision(int h, int w)
		{
			return tiles[h, w].Collision;
		}

		/// <summary>
		/// Gets the bounding rectangle of a tile in world space.
		/// </summary>        
		public Rectangle GetBounds(int h, int w)
		{
			return new Rectangle(w * Tile.Size, h * Tile.Size, Tile.Size, Tile.Size);
			//return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
		}

		/// <summary>
		/// returns the tile a specific point is in
		/// BUG - Not currently working correctly, causes game to lag a lot
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public Vector2 GetSurroundingTile(Point point)
		{
			int X = point.X / Tile.Size;
			int Y = point.Y / Tile.Size;
			return new Vector2(X, Y);
		}

		/// <summary>
		/// Width of level measured in tiles.
		/// </summary>
		public int Width
		{
			get { return tiles.GetLength(1); }
		}

		/// <summary>
		/// Height of the level measured in tiles.
		/// </summary>
		public int Height
		{
			get { return tiles.GetLength(0); }
		}

		#endregion

		/// <summary>
		/// update game logic
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="keyboardState"></param>
		public virtual void Update(GameTime gameTime)
		{


			//set the camera based on teh cam type
			if (CamType.ToLower() == "center")
				ScreenManager.Camera.CenterX(Player.BoundingRectangle);
			else if (CamType.ToLower() == "static")
				ScreenManager.Camera.Position = Vector2.Zero;
			else {}

			checkExit();
			checkWater();

			Player.Update(gameTime);

			//update existing grabber enemies
			if (grabberEnemies.Count != 0)
			{
				foreach (GrabberEnemy enemy in grabberEnemies)
				{
					enemy.Update(gameTime);
				}
			}

		}

		/// <summary>
		/// check if the Player intersects with an exit
		/// </summary>
		/// <param name="currentScreen"></param>
		/// <param name="nextScreen"></param>
		private void checkExit()//GameScreen currentScreen, GameScreen nextScreen)
		{
			foreach (Rectangle exit in exits)
			{
				if (Player.BoundingRectangle.Intersects(exit))
				{
					Player.Save();
					ExitReached = true;
				}
			}
		}

		/// <summary>
		/// check if player has refilled water from water source
		/// </summary>
		private void checkWater()
		{
			foreach (Rectangle water in waterTiles)
			{
				if (Player.BoundingRectangle.IsWithin(water, 32) && ScreenManager.IsKeyPressed(Keybindings.Interact))
				{
					Player.Canteen.Refill();
				}
			}
		}


		/// <summary>
		/// draw tile map and entities
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Draw(GameTime gameTime)
		{
			ScreenManager.StartCameraSpriteBatch();
			DrawTiles(gameTime);
			Player.Draw(gameTime);
			
			if (grabberEnemies.Count != 0)
			{
				foreach (GrabberEnemy enemy in grabberEnemies)
				{
					enemy.Draw(gameTime);
				}
			}
			
			//draw a transparent water tile over top 
			foreach (Rectangle waterTile in waterTiles)
			{
				ScreenManager.SpriteBatch.Draw(atlas, waterTile, ImageTools.GetSourceRect(3, 0), Color.White);
			}

			ScreenManager.EndCameraSpriteBatch();
			
			
		}

		/// <summary>
		/// draw things over lighting
		/// </summary>
		public virtual void DrawOverLighting(GameTime gameTime)
		{
			Player.DrawHUD();

			//only draw the portal if it isn't null
			if (Portal != null)
				Portal.UpdateAndDraw();
		}

		/// <summary>
		/// draw the tiles on the screen
		/// </summary>
		/// <param name="gameTime"></param>
		public void DrawTiles(GameTime gameTime)
		{
			for (int h = 0; h < Height; h++)
			{
				for (int w = 0; w < Width; w++)
				{
					//only draw tile if it has a source rect and is in the viewing window of the camera
					if (tiles[h, w].Source != Rectangle.Empty && 
						GetBounds(h, w).Intersects(new Rectangle((int)ScreenManager.Camera.Position.X, (int)ScreenManager.Camera.Position.Y, ScreenManager.SCREEN_WIDTH, ScreenManager.SCREEN_HEIGHT)))
					{
						ScreenManager.SpriteBatch.Draw(atlas, new Vector2(w, h) * Tile.Size, tiles[h, w].Source, Color.White);
					}
				}
			}
		}

		/// <summary>
		/// set the state of a portal
		/// </summary>
		/// <param name="state"></param>
		public void SetPortalState(bool state)
		{
			if (Portal != null)
				Portal.IsActive = state;
		}

		/// <summary>
		/// reset to a new level
		/// </summary>
		/// <param name="bmpPath"></param>
		public void Reset(string bmpPath)
		{
			exits.Clear();
			grabberEnemies.Clear();
			waterTiles.Clear();

			colorMap = ImageLoad.LoadArray(bmpPath);

			tiles = new Tile[colorMap.GetLength(0), colorMap.GetLength(1)];

			Player.Save();

			LoadLevel();

		}
	}
}