//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// dialogue box system that runs off of a text document script
	/// </summary>
	public class DialogueBox
	{
		private Texture2D box;
		private Vector2 boxLocation;
		private Vector2 portraitLocation;

		private Texture2D currentPortrait;//texture of the person who is currently talking

		private List<string> script;//entire script, including who is talking and what they say
		private List<string> dialogues;//only the dialogues
		private List<string> whoTalking;//who says the dialogue
		private List<Color> textColors;

		public int Index { get; private set; } = 0;//which line of the script 
		
		private bool isActive = false;
		public bool IsActive
		{
			get { return isActive; }
			set { isActive = value; }
		}

		private string currentDialogue = "";

		private float time = 0f;
		private float charDelay = .05f;
		private bool timerRunning = true;
		private int charIndex = 0;

		public bool Finished { get; private set; } = false;
		
		
		/// <summary>
		/// load a new script
		/// </summary>
		/// <param name="path"></param>
		private void SetScript(string path)
		{
			script = new List<string>(File.ReadAllLines(path));
		}

		/// <summary>
		/// create instance of dialogue box
		/// </summary>
		/// <param name="scriptPath"></param>
		public DialogueBox(string scriptPath)
		{
			SetScript(scriptPath);

			dialogues = new List<string>();
			whoTalking = new List<string>();
			textColors = new List<Color>();
		}

		/// <summary>
		/// load graphical content and load the dialogues and characters talking in the script
		/// </summary>
		public void LoadDBox()
		{
			box = ImageTools.LoadTexture2D("Graphics/GUI/dbox");

			boxLocation = new Vector2(ImageTools.CenterImage_vect(box).X, 650);
			portraitLocation = new Vector2(boxLocation.X + 1, boxLocation.Y + 1);


			//split the script up
			foreach (string line in script)
			{
				string[] splitLine = line.Split('=');
				dialogues.Add(splitLine[1]);
				whoTalking.Add(splitLine[0]);
			}

			//custom coloring
			for (int i = 0; i < dialogues.Count; i++)
			{
				if (dialogues[i][0] == '*')
				{
					if (dialogues[i][1] == 'b')
					{
						textColors.Add(Color.Aqua);
					}
					else if (dialogues[i][1] == 'r')
					{
						textColors.Add(Color.Red);
					}

					dialogues[i] = dialogues[i].Remove(0, 2);
				}
				else
					textColors.Add(Color.White);
				
			}

			currentPortrait = ImageTools.LoadTexture2D("Characters/Portraits/" + whoTalking[0] + "Portrait");
			

		}

		/// <summary>
		/// update game logic
		/// </summary>
		public void Update(GameTime gameTime)
		{

			if (isActive)
			{
				if (timerRunning)
					time += (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (currentDialogue != dialogues[Index])
				{

					//to make the dialogue show up character by character
					if (time >= charDelay)
					{
						SoundManager.PlaySound("charSound", .1f);
						currentDialogue += dialogues[Index][charIndex];
						charIndex++;
						time = 0;
					}

				}


				bool isDialogueLeft = true;

				if (isDialogueLeft)
				{

					if (ScreenManager.AnyKeyPressed()) 
					{
						if (Index < dialogues.Count() - 1 && currentDialogue == dialogues[Index])
						{
							Index++;
							currentDialogue = "";
							timerRunning = true;
							charIndex = 0;
						}
						else if (Index < dialogues.Count() - 1 && currentDialogue != dialogues[Index])
						{
							currentDialogue = dialogues[Index];
						}
						else
							isDialogueLeft = false;

						//set the current portrait to the person who is currently talking
						currentPortrait = ImageTools.LoadTexture2D("Characters/Portraits/" + whoTalking[Index] + "Portrait");

						
					}
				}

				if (!isDialogueLeft)
				{
					if (ScreenManager.AnyKeyPressed())
					{
						isActive = false;
						Finished = true;
					}
				}
				
			}
		}

		/// <summary>
		/// draw the dialogue box
		/// </summary>
		public void Draw()
		{
			if (isActive)
			{
				ScreenManager.SpriteBatch.Begin();

				ScreenManager.SpriteBatch.Draw(box, boxLocation, Color.Gray);
				ScreenManager.SpriteBatch.Draw(currentPortrait, portraitLocation, Color.White);

				ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, 
					TextTools.WrapText(ScreenManager.ExtraSmallFont, currentDialogue, 280), 
					new Vector2(boxLocation.X + 140, boxLocation.Y + 8), 
					textColors[Index]);

				ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, 
					whoTalking[Index], 
					TextTools.CenterTextWithinRect(whoTalking[Index], 
					ScreenManager.ExtraSmallFont, 
					new Rectangle((int)boxLocation.X + 1, (int)boxLocation.Y + 130, 128, 34)), 
					Color.White);

				ScreenManager.SpriteBatch.DrawString(ScreenManager.ExtraSmallFont, "Press any key to continue...",
					new Vector2(boxLocation.X + 140, boxLocation.Y + 135),
					Color.White);

				ScreenManager.SpriteBatch.End();
			}
		}

		/// <summary>
		/// reset the dialogue box
		/// </summary>
		public void Reset()
		{
			if (!isActive)
			{
				Index = 0;
				isActive = true;
				Finished = false;
			}
		}

		
	}
}
