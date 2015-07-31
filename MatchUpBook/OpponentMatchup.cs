using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook
{
	public class OpponentMatchup
	{
		public OpponentMatchup ()
		{
			Wins = 0;
			Losses = 0;
		}

        [XmlElement("Title")]
		public string Title { get; set; }

        [XmlElement("Notes")]
		public string Notes { get; set; }

        [XmlElement("Wins")]
		public int Wins { get; set; }

        [XmlElement("Losses")]
		public int Losses { get; set; }

		public double GetWinLoss() {
			return Wins/Losses;
		}
	}
}

