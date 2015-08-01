using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using System.Xml.Serialization;
using MatchUpBook.Factories;
using MatchUpBook.Activities;

namespace MatchUpBook
{
    [Activity(Label = "MatchUpBook", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            StartActivity(typeof(MenuActivity));
        }

    }
}


