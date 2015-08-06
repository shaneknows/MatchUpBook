using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MatchUpBook.Models
{
    [XmlRoot("Menu")]
	public class MenuNode
	{
		public MenuNode ()
		{
            Games = new List<GameNode>();
		}

		[XmlElement("Game")]
		public List<GameNode> Games { get; set;}
	}
}

