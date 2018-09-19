//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Xml.Serialization;
using System.IO;

namespace Below
{
	/// <summary>
	/// stores all level and gamescreen information
	/// </summary>
	public class LevelData
	{
		public int LastScreen { get; set; }//the last screen that was loaded
		public int GroupIndex { get; set; }
		public int LevelIndex { get; set; }

		/// <summary>
		/// serialize leveldata to xml file
		/// </summary>
		public void Save()
		{
			using (FileStream stream = new FileStream(SaveLocations.LevelSavePath, FileMode.Create))
			{
				XmlSerializer XML = new XmlSerializer(typeof(LevelData));
				XML.Serialize(stream, this);
			}
		}

		/// <summary>
		/// return leveldata by deserializing the xml file
		/// </summary>
		/// <returns></returns>
		public static LevelData Load()
		{
			using (FileStream stream = new FileStream(SaveLocations.LevelSavePath, FileMode.Open))
			{
				XmlSerializer XML = new XmlSerializer(typeof(LevelData));
				return (LevelData)XML.Deserialize(stream);
			}
		}

	}
}
