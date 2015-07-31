using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook
{
    [XmlRoot("Menu")]
	public class Menu
	{
		public Menu ()
		{
			
		}

		[XmlElement("Game")]
		public List<Game> Games { get; set;}
	}
}

