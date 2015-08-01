using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook
{
	public class Game : BaseMenuItem
	{
        public Game() { }

		public Game (string pTitle)
		{
            this.Title = pTitle;
            Characters = new List<PlayerCharacter>();
		}

        [XmlElement("PlayerCharacter")]
        public List<PlayerCharacter> Characters { get; set; }
	}
}

