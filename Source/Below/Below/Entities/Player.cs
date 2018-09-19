//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Below
{
	/// <summary>
	/// main player class
	/// </summary>
	public class Player : Entity
	{
		private bool devMode = false;

		//animations
		private Texture2D playerStanding;
		private Animation playerForward;
		private Animation playerBackward;
		private Animation playerJump;
		private SpriteEffects flip = SpriteEffects.None;

		//moving arm stuff
		private Texture2D arm;
		private Rectangle armPosition;
		private Vector2 armOrigin = Vector2.Zero;
		private float rotation = 0.0f;
		
		//flashlight stuff
		private SpotLight flashLight;
		private Vector2 lightPos;

		//health stuff
		public Health Health { get; private set; }
		public Canteen Canteen { get; private set; }

		//user movement input
		private float movement;
		public int Direction { get;  set; }

		//for collisions
		private float previousBottom;

		//jumping states
		private bool isJumping;
		private bool wasJumping;
		private float jumpTime;

		//current level player is on
		private Level level;

		//raycast of where the player is looking
		public Ray2D LineOfSight { get; private set; }
		
		
		public bool IsDead { get; private set; }

		public bool MovementEnabled {  get; set; } = true;
		
		private Vector2 position;
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }
		}

		private Vector2 velocity;
		public Vector2 Velocity
		{
			get { return velocity; }
			set { velocity = value; }
		}

		public bool OnGround { get; private set; }

		public Rectangle LocalBounds { get; private set; }//bounds within the screen
		private Rectangle boundingRectangle;//global bounding rect
		/// <summary>
		/// players bounding rectangle
		/// </summary>
		public Rectangle BoundingRectangle//global bounding rect
		{
			get
			{
				int left = (int)Math.Round(Position.X - 0) + boundingRectangle.X;
				int top = (int)Math.Round(Position.Y - 0) + boundingRectangle.Y;

				return new Rectangle(left, top, boundingRectangle.Width, boundingRectangle.Height);
			}
		}

		/// <summary>
		/// create instance of player
		/// </summary>
		/// <param name="Level"></param>
		/// <param name="Position"></param>
		public Player(Level Level, Vector2 Position)
		{
			level = Level;
			LoadContent();
			Reset(position, false);
		}
		
		/// <summary>
		/// load player content
		/// </summary>
		public void LoadContent()
		{
			Health = new Health(5, true);
			Canteen = new Canteen(this);

			playerStanding = ImageTools.LoadTexture2D("Characters/Player/standing");
			playerForward = new Animation(ImageTools.LoadTexture2D("Characters/Player/character_walk_right"), 6, true);
			playerBackward = new Animation(ImageTools.LoadTexture2D("Characters/Player/character_walk_backward"), 4, true);
			playerJump = new Animation(ImageTools.LoadTexture2D("Characters/Player/jump"), 1, true);


			arm = ImageTools.LoadTexture2D("Characters/Player/arm");
			armPosition = new Rectangle((int)position.X, (int)position.Y, arm.Width, arm.Height);

			lightPos = new Vector2(-300, -300);


			ScreenManager.LightingEngine.ClearLights();//so light doesen't stay when new level is loaded
			flashLight = new SpotLight()
			{
				IsEnabled = true,
				Color = Color.White,
				Power = 1f,
				LightDecay = 200,
				Position = new Vector3(lightPos.X, lightPos.Y, 5), //position.z controls width of the focus
				SpotBeamWidthExponent = 36,
				DirectionZ = 0f//0f//-.1f
			};
			flashLight.SpotRotation = 1f;
			ScreenManager.LightingEngine.AddLight(flashLight);
			
			boundingRectangle = new Rectangle(0, 0, 32, 64);
			

			setLineOfSight();

			
		}

		/// <summary>
		/// reset the player
		/// </summary>
		/// <param name="position"></param>
		public void Reset(Vector2 position, bool isDead)
		{
			Position = position;
			Velocity = Vector2.Zero;

			if (isDead)
			{
				isDead = false;//player is alive again
				Health.Heal(5);//reset health
			}
		}

		/// <summary>
		/// calculate the rotation of the arm so the that it will follow the mouse
		/// TODO - light position is at center of player right now, might want it to follow end of arm
		/// </summary>
		private void calculateRotation()
		{
			
			//use trig between player pos and mouse position to calculate the rotation on the arm
			MouseState ms = Mouse.GetState();
			float dx = ms.X - (position.X - ScreenManager.Camera.Position.X);
			float dy = ms.Y - position.Y;//vertical distance between player and mouse
			
			rotation = (float)Math.Atan(dy / dx);//tangent of vert distance/horizontal distance is the angle of the rotation


			//make angle go into quadrant 2 and 3 instead of popping back in quadrant 1 and 4 when the change in x is negative
			if (dx < 0)
				rotation += MathHelper.Pi;


			//set flashlight rotation before the rotation gets modified for the flipping of the player
			flashLight.SpotRotation = rotation;


			//need to reset the rotation to set the flip of the player
			if (rotation > 1.57f)
			{
				dx *= -1;
				dy *= -1;

				rotation = (float)Math.Atan(dy / dx);//reset the rotation

				flip = SpriteEffects.FlipHorizontally;
				armOrigin = new Vector2(arm.Width, 0);//reset the origin of the sprite
			}
			else
			{
				flip = SpriteEffects.None;
				armOrigin = new Vector2(0, 0);
			}
			
			

		}

		/// <summary>
		/// set the line of sight raycast
		/// TODO - make ray extend past mouse
		/// </summary>
		private void setLineOfSight()
		{
			MouseState mouseState = Mouse.GetState();
			LineOfSight = new Ray2D(new Vector2(LocalBounds.X + 16, LocalBounds.Y + 32), new Vector2(mouseState.X, mouseState.Y));
		}

		/// <summary>
		/// update the game logic
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{

			Vector2 localPos = ScreenManager.GlobalToLocal(position.X, position.Y);//convert global pos to local pos
			LocalBounds = new Rectangle((int)localPos.X, (int)localPos.Y, 32, 64);//set the local bounds
			flashLight.Position = new Vector3(localPos.X + 16, localPos.Y + 32, 5);//set the postion of the flashlight using the local position

			calculateRotation();//calculate the rotation of the arm

			getInput();//check for keyboard input

			applyPhysics(gameTime);//apply all player physics

			setLineOfSight();//set the raycast line of site

			Health.UpdateCooldownTimer(gameTime);//update the damage cooldown

			//adjust arm positions based on the flip of the character
			if (flip == SpriteEffects.None)
				armPosition = new Rectangle((int)position.X + 12, (int)position.Y + 22, arm.Width, arm.Height);
			else
				armPosition = new Rectangle((int)position.X + 20, (int)position.Y + 22, arm.Width, arm.Height);
			

			//healing
			if (ScreenManager.IsKeyPressed(Keybindings.Heal))
				Canteen.Heal();

			//developer info
			if (ScreenManager.IsKeyPressed(Keybindings.DevKey2))
			{
				devMode = !devMode;
				Health.GodMode = !Health.GodMode;
			}
			
			//flashlight toggle - keep?
			if (ScreenManager.IsKeyPressed(Keybindings.FlashlightToggle))
				flashLight.IsEnabled = !flashLight.IsEnabled;
			
			
			//set if the player is alive or not based on the health
			if (Health.IsDead)
				IsDead = true;
			else
				IsDead = false;
			
			//set the direction of the player based on the horizontal velocity
			if (velocity.X > 0)
				Direction = -1;
			else if (velocity.X < 0)
				Direction = 1;
			else
				Direction = 0;
			

			movement = 0.0f;//reset movment
			isJumping = false;//reset jump state
		}

		/// <summary>
		/// set horizontal and vertical movment from user input
		/// </summary>
		/// <param name="keyboardState"></param>
		private void getInput()
		{
			if (ScreenManager.IsKeyDown(Keybindings.Left) && MovementEnabled)
				movement = -1.0f;
			else if (ScreenManager.IsKeyDown(Keybindings.Right) && MovementEnabled)
				movement = 1.0f;

			//set jump state
			isJumping = (ScreenManager.IsKeyDown(Keybindings.Jump) || ScreenManager.IsKeyDown(Keybindings.AltJump)) && MovementEnabled;
		}

		/// <summary>
		/// update player velocity and position based on constants and current user input
		/// </summary>
		/// <param name="gameTime"></param>
		private void applyPhysics(GameTime gameTime)
		{
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 previousPosition = Position;

			//set velocity as a combination of horizontal movement and downward gravity acceleration
			velocity.X += movement * MoveAcceleration * elapsed;
			//set a cap to the fall speed
			velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
		
			//check for jumps
			velocity.Y = doJump(velocity.Y, gameTime);

			//horizontal movement drag
			if (OnGround)
				velocity.X *= GroundDragFactor;
			else
				velocity.X *= AirDragFactor;

			//cap the velocity so the player can't run faster than the top speed
			velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

			//apply the velocity to the position
			Position += velocity * elapsed;
			Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(position.Y));

			//check if player is colliding with level
			handleCollisions();

			//if collision stopped movement, reset velocity
			if (Position.X == previousPosition.X)
				velocity.X = 0;

			if (Position.Y == previousPosition.Y)
				velocity.Y = 0;
		}


		/// <summary>
		/// calculate y velocity for jumping
		/// </summary>
		/// <remarks>
		/// during jump ascent, y velocity is a power curve
		/// during descent, gravity is the only force
		/// </remarks>
		/// <param name="velocityY">
		/// The player's current velocity along the Y axis.
		/// </param>
		/// <returns>
		/// A new Y velocity if beginning or continuing a jump.
		/// Otherwise, the existing Y velocity.
		/// </returns>
		private float doJump(float velY, GameTime gameTime)
		{
			if (isJumping)
			{
				if ((!wasJumping && OnGround) || jumpTime > 0.0f)
				{
					if (jumpTime == 0.0f)
					{
						SoundManager.PlaySound("jump", 1f);
					}
					jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
				}

				//ascent of jump
				if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
				{
					//power curve
					velY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
				}
				else
				{
					//top of jump
					jumpTime = 0.0f;
				}
			}
			else
			{
				//not jumping or cancels current jump
				jumpTime = 0.0f;
			}
			wasJumping = isJumping;

			return velY;
		}

		/// <summary>
		/// detect and deal with all collisions between the player and the tiles
		/// TODO: optimize
		/// BUG - player "sticks" to bottom of tiles while jumping
		///		fix by setting jumpTime to 0 if player hits bottom of an impassable tile
		/// </summary>
		private void handleCollisions()
		{
			Rectangle bounds = BoundingRectangle;

			OnGround = false;

			//inefficient way to check for collisions as it cycles through every existing tile
			for (int h = 0; h < level.Height; h++)
			{
				for (int w = 0; w < level.Width; w++)
				{
					// If this tile is collidable,
					TileCollision collision = level.GetCollision(h, w);
					if (collision != TileCollision.Passable)
					{

						// Determine collision depth (with direction) and magnitude.
						Rectangle tileBounds = level.GetBounds(h, w);
						Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
						if (depth != Vector2.Zero)
						{
							float absDepthX = Math.Abs(depth.X);
							float absDepthY = Math.Abs(depth.Y);

							// Resolve the collision along the shallow axis.
							if (absDepthY < absDepthX || collision == TileCollision.Platform)
							{
								// If we crossed the top of a tile, we are on the ground.
								if (previousBottom <= tileBounds.Top)
									OnGround = true;

								// Ignore platforms, unless we are on the ground.
								if (collision == TileCollision.Impassable || OnGround)
								{
									// Resolve the collision along the Y axis.
									Position = new Vector2(Position.X, Position.Y + depth.Y);

									// Perform further collisions with the new bounds.
									bounds = BoundingRectangle;
								}
							}
							else if (collision == TileCollision.Impassable) // Ignore platforms.
							{
								// Resolve the collision along the X axis.
								Position = new Vector2(Position.X + depth.X, Position.Y);
								
								// Perform further collisions with the new bounds.
								bounds = BoundingRectangle;

								
							}
						}
					}
				}
				
			}
			
			previousBottom = bounds.Bottom;
		}
		

		/// <summary>
		/// draw the player animations
		/// </summary>
		/// <param name="gameTime"></param>
		public void Draw(GameTime gameTime)
		{

			if (MovementEnabled)
			{
				if (!OnGround)
					playerJump.DrawFrame(0, position, flip);
				else
				{
					if (flip == SpriteEffects.FlipHorizontally)
					{
						if (Direction == -1)
							playerBackward.PlayAnimation(gameTime, position, flip);
						else if (Direction == 1)
							playerForward.PlayAnimation(gameTime, position, flip);
						else
							ScreenManager.SpriteBatch.Draw(playerStanding, Position, null, Color.White, 0, Vector2.Zero, 1, flip, 1);
					}
					else
					{
						if (Direction == 1)
							playerBackward.PlayAnimation(gameTime, position, flip);
						else if (Direction == -1)
							playerForward.PlayAnimation(gameTime, position, flip);
						else
							ScreenManager.SpriteBatch.Draw(playerStanding, Position, null, Color.White, 0, Vector2.Zero, 1, flip, 1);
					}
				}
			}
			else
				ScreenManager.SpriteBatch.Draw(playerStanding, Position, null, Color.White, 0, Vector2.Zero, 1, flip, 1);


			ScreenManager.SpriteBatch.Draw(arm, armPosition, null, Color.White, rotation, armOrigin, flip, 1);
			//ScreenManager.SpriteBatch.End();
		}

		/// <summary>
		/// draw players useful player data on screen over lighting
		/// </summary>
		public void DrawHUD()
		{
			Health.DrawOverLighting();
			Health.DrawHealthBar(new Vector2(1100, 65), 1f);
			Canteen.DrawOverLighting();
			DrawDevInfo();

		}

		/// <summary>
		/// draw player info use for development
		/// </summary>
		public void DrawDevInfo()
		{
			if (devMode)
			{
				MouseState ms = Mouse.GetState();

				ScreenManager.SpriteBatch.Begin();

				ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, "Global Player Pos = " + BoundingRectangle.ToString(), Vector2.Zero, Color.White);
				ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, "Local Player Pos = " + LocalBounds.ToString(), new Vector2(0, 20), Color.White);
				ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, "Player Velocity = " + velocity.ToString(), new Vector2(0, 40), Color.White);
				ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, "Mouse Pos = " + ms.Position.ToString(), new Vector2(0, 60), Color.White);
				ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, "Camera Pos = " + level.CamType + " " + ScreenManager.Camera.Position.ToString(), new Vector2(0, 80), Color.White);
				
				ScreenManager.SpriteBatch.End();

				LineOfSight.Draw();

			}
		}

		/// <summary>
		/// helper method to use in gamescreens to check for player death
		/// </summary>
		/// <param name="currentScreen"></param>
		/// <param name="targetScreen"></param>
		public void CheckDeath(GameScreen currentScreen, GameScreen targetScreen)
		{
			if (IsDead)
				ScreenManager.ChangeScreens(currentScreen, targetScreen);
		}

		/// <summary>
		/// save player data
		/// </summary>
		public void Save()
		{
			Health.Save();
			Canteen.Save();	
		}

	}
}
