using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook.Models
{
	public class OpponentMatchupNode : BaseMenuItem
	{
        public OpponentMatchupNode() { }
		public OpponentMatchupNode (string pTitle, PlayerCharacterNode parent)
		{
            this.Title = pTitle;
            this.Parent = parent;
            Notes = "";
			Wins = 0;
			Losses = 0;
		}

        [XmlElement("Notes")]
		public string Notes { get; set; }

        [XmlElement("Wins")]
		public int Wins { get; set; }

        [XmlElement("Losses")]
		public int Losses { get; set; }

        [XmlIgnore]
        public PlayerCharacterNode Parent { get; set; }

		public double GetWinLoss() {
			return Wins/Losses;
		}
	}
}

