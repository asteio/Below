//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Xml.Serialization;
using System.IO;

namespace Below
{
	/// <summary>
	/// locally save user settings
	/// </summary>
	public class Settings
	{
		public bool MusicOn { get; set; }
		public bool SoundOn { get; set; }
		public bool NewGame { get; set; }

		/// <summary>
		/// serialize settings to xml file
		/// </summary>
		/// <param name="xmlPath"></param>
		public void Save()
		{
			using (FileStream stream = new FileStream(SaveLocations.SettingsSavePath, FileMode.Create))
			{
				XmlSerializer XML = new XmlSerializer(typeof(Settings));
				XML.Serialize(stream, this);
			}
		}

		/// <summary>
		/// return settings by deserializing xml file
		/// </summary>
		/// <param name="xmlPath"></param>
		/// <returns></returns>
		public static Settings Load()
		{
			using (FileStream stream = new FileStream(SaveLocations.SettingsSavePath, FileMode.Open))
			{
				XmlSerializer XML = new XmlSerializer(typeof(Settings));
				return (Settings)XML.Deserialize(stream);
			}
		}
	}
}
