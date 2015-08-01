using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook.Models
{
	public class PlayerCharacterNode : BaseMenuItem
	{
        public PlayerCharacterNode() { }

		public PlayerCharacterNode (string pTitle)
		{
            this.Title = pTitle;
            Opponents = new List<OpponentMatchupNode>();
		}

        [XmlElement("OpponentMatchup")]
        public List<OpponentMatchupNode> Opponents { get; set; }

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

