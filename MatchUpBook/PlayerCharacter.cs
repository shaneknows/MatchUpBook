using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook
{
	public class PlayerCharacter
	{
		public PlayerCharacter ()
		{
		}
        [XmlElement("Title")]
		public string Title { get; set; }

        [XmlElement("OpponentMatchup")]
        public List<OpponentMatchup> Opponents { get; set; }

		public int GetPlayedGames()
		{
			//TODO: Implement count of played games off of children wins/losses
            return 0;
		}

		public int WinLoss()
		{
			//TODO: Implement win loss ration based on record of children
			return 0;
		}

	}
}

