using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Xml.Serialization;
using MatchUpBook.Models;

namespace MatchUpBook.Factories
{
    public interface IMenuFactory
    {
        MenuNode GetMenu(string fileContents);
    }

    public class MenuFactory : IMenuFactory
    {
        public MenuFactory() { }

        public MenuNode GetMenu(string fileContents)
        {
            MenuNode menu;

            XmlSerializer serializer = new XmlSerializer(typeof(MenuNode));
            using (TextReader reader = new StringReader(fileContents))
            {
                 menu = (MenuNode)serializer.Deserialize(reader);
            }
            return menu;
        }

        public MenuNode UpdateMenu(MenuNode menu, FileStream fileContents)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MenuNode));
            serializer.Serialize(fileContents, menu);
            return menu;
        }
    }
}