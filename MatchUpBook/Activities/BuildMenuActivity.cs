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
using MatchUpBook.Interfaces;
using System.Xml.Serialization;
using System.IO;

namespace MatchUpBook.Activities
{
    [Activity(Label = "BuildMenuActivity")]
    public class BuildMenuActivity : Activity, IBuildMenuHandler
    {
		Menu menu;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here

        }

        public void AddGame(Game game)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Menu));
            using (TextWriter writer = new StreamWriter(Assets.Open("Data.xml")))
            {
                serializer.Serialize(writer, menu);
            }
        }

        public void AddCharacter(PlayerCharacter playerCharacter)
        {
            throw new NotImplementedException();
        }

        public void AddMatchup(OpponentMatchup opponentMatchup)
        {
            throw new NotImplementedException();
        }

        public void Remove(BaseMenuItem item)
        {
            throw new NotImplementedException();
        }
    }
}