using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook
{
	public class Game
	{
		public Game ()
		{
		}

        [XmlElement("Title")]
		public string Title { get; set; }

        [XmlElement("PlayerCharacter")]
        public List<PlayerCharacter> Characters { get; set; }
	}
}

