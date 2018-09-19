//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Xml.Serialization;
using System.IO;

namespace Below
{
	/// <summary>
	/// locally save player information
	/// </summary>
	public class PlayerData
	{
		public int Health { get; set; }//the players current health
		public int FlaskUses { get; set; }//number of remaining flask uses

		/// <summary>
		/// serialize player to xml file
		/// </summary>
		public void Save()
		{
			using (FileStream stream = new FileStream(SaveLocations.PlayerSavePath, FileMode.Create))
			{
				XmlSerializer XML = new XmlSerializer(typeof(PlayerData));
				XML.Serialize(stream, this);
			}
		}

		/// <summary>
		/// return playerdata by deserializing the xml file
		/// </summary>
		/// <returns></returns>
		public static PlayerData Load()
		{
			using (FileStream stream = new FileStream(SaveLocations.PlayerSavePath, FileMode.Open))
			{
				XmlSerializer XML = new XmlSerializer(typeof(PlayerData));
				return (PlayerData)XML.Deserialize(stream);
			}
		}
	}
}
