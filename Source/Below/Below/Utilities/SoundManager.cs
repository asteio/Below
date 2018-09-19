//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Below
{

	/// <summary>
	/// manage all sounds being played
	/// </summary>
	public static class SoundManager
	{

		public static Song CurrentSong { get; private set; }
		public static bool IsMusicPlaying { get; private set; }

		//holds any looping sounds that are currently playing
		private static Dictionary<string, SoundEffectInstance> LoopingSounds = new Dictionary<string, SoundEffectInstance>();

		#region music
		/// <summary>
		/// play music
		/// </summary>
		/// <param name="song"></param>
		/// <param name="repeat">will be looped if true</param>
		public static void PlaySong(string name, bool repeat)
		{
			Song song = ScreenManager.ContentMgr.Load<Song>("Audio/Music/" + name);

			MediaPlayer.Volume = .4f;

			MediaPlayer.Stop();

			if (repeat)
				MediaPlayer.IsRepeating = true;
			else
				MediaPlayer.IsRepeating = false;

			MediaPlayer.Play(song);

			CurrentSong = song;
			IsMusicPlaying = true;
		}
		/// <summary>
		/// stop the playing music
		/// </summary>
		public static void StopMusic()
		{
			MediaPlayer.Stop();
			IsMusicPlaying = false;
		}
		#endregion

		/// <summary>
		/// play sound effect once
		/// </summary>
		/// <param name="sound"></param>
		public static void PlaySound(string name, float volume)
		{
			if (ScreenManager.Settings.SoundOn)
			{
				SoundEffectInstance soundInstance = ScreenManager.ContentMgr.Load<SoundEffect>("Audio/Sound/" + name).CreateInstance();
				soundInstance.Volume = volume;

				soundInstance.Play();
			}
		}

		/// <summary>
		/// check sound states during update
		/// </summary>
		public static void Update()
		{
			//music toggle
			if (!ScreenManager.Settings.MusicOn)
			{
				MediaPlayer.IsMuted = true;
				IsMusicPlaying = false;
			}
			else
			{
				MediaPlayer.IsMuted = false;
				IsMusicPlaying = true;
			}

			
			
			//sound toggle
			if (!ScreenManager.Settings.SoundOn)
			{
				SoundEffect.MasterVolume = 0f;
			}
			else
			{
				SoundEffect.MasterVolume = 1f;
			}
			
		}

		/// <summary>
		/// add sound to loopingsounds
		/// </summary>
		/// <param name="sound"></param>
		public static void AddLoopingSound(string name, float volume)
		{
			//using a SoundEffectInstance will allow the volume to change
			SoundEffectInstance soundInstance = ScreenManager.ContentMgr.Load<SoundEffect>("Audio/Sound/" + name).CreateInstance();
			soundInstance.Volume = volume;
			soundInstance.IsLooped = true;

			LoopingSounds.Add(name, soundInstance);

			LoopingSounds[name].Play();
		}


		/// <summary>
		/// stop any looping sounds that are playing
		/// </summary>
		public static void StopLoopingSounds()
		{
			foreach (SoundEffectInstance sound in LoopingSounds.Values)
			{
				sound.Stop();
			}

			LoopingSounds.Clear();
		}

		/// <summary>
		/// stop specific looping sounds
		/// </summary>
		/// <param name="name"></param>
		public static void StopLoopingSound(string name)
		{
			if (LoopingSounds.ContainsKey(name))
			{
				LoopingSounds[name].Stop();
				LoopingSounds.Remove("name");
			}
		}

		

		/// <summary>
		/// stop all sounds
		/// </summary>
		public static void StopAll()
		{
			MediaPlayer.Stop();
			IsMusicPlaying = false;

			StopLoopingSounds();
		}

	}
}
