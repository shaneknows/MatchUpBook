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

namespace MatchUpBook.Factories
{
    public interface IMenuFactory
    {
        Menu GetMenu(string fileContents);
    }

    public class MenuFactory : IMenuFactory
    {
        public MenuFactory() { }

        public Menu GetMenu(string fileContents)
        {
            Menu menu;

            XmlSerializer serializer = new XmlSerializer(typeof(Menu));
            using (TextReader reader = new StringReader(fileContents))
            {
                 menu = (Menu)serializer.Deserialize(reader);
            }
            return menu;
        }

        public Menu UpdateMenu(Menu menu, FileStream fileContents)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Menu));
            serializer.Serialize(fileContents, menu);
            return menu;
        }
    }
}