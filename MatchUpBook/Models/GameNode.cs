using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook.Models
{
	public class GameNode : BaseMenuItem
	{
        public GameNode() { }

		public GameNode (string pTitle)
		{
            this.Title = pTitle;
            Characters = new List<PlayerCharacterNode>();
		}

        [XmlElement("PlayerCharacter")]
        public List<PlayerCharacterNode> Characters { get; set; }
	}
}

