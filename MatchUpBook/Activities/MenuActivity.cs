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
using MatchUpBook.Factories;
using MatchUpBook.Interfaces;
using System.Drawing;
using System.Xml.Serialization;
using System.Security.Permissions;

namespace MatchUpBook.Activities
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : Activity, IMenuHander
    {
        Menu menu;
        MenuFactory menuFactory;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            menuFactory = new MenuFactory();

            // Create your application here
			CreateMenu();

            SetContentView(GetHomeLayout());

        }

        public void CreateMenu()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "Data.xml");
            string content;

            //If local menu exists load it, otherwise create new menu based on template
            if(File.Exists(filename))
            {
                using (var streamReader = new StreamReader(filename))
                {
                    content = streamReader.ReadToEnd();
                }
                menu = menuFactory.GetMenu(content);
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Menu));
                using (var streamReader = new StreamReader(Assets.Open("Data.xml")))
                {
                    content = streamReader.ReadToEnd();
                }

                menu = menuFactory.GetMenu(content);

                using (var streamWriter = new StreamWriter(filename))
                {
                    serializer.Serialize(streamWriter, menu);
                }
            }
        }

        public void UpdateMenu()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Menu));
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "Data.xml");
            using (var streamWriter = new StreamWriter(filename))
            {
                serializer.Serialize(streamWriter, menu);
            }

            string content;

            using (var streamReader = new StreamReader(filename))
            {
                content = streamReader.ReadToEnd();
            }

            menu = menuFactory.GetMenu(content);
        }

        #region IMenuHandler Features
        public LinearLayout GetHomeLayout()
        {
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = "Matchup Book";

            layout.AddView(aLabel);
            foreach (var game in menu.Games)
            {
                var button = new Button(this);
                button.Text = game.Title;
                button.Click += (sender, e) =>
                { SetContentView(GetGameLayout(game)); };
                layout.AddView(button);
            }

            var createButton = new Button(this);
            createButton.Text = "Add new Game";
            createButton.Click += (sender, e) =>
            {
				SetContentView(GetAddGameLayout());
            };
			layout.AddView (createButton);

            var closeButton = new Button(this);
            closeButton.Text = "Close";
            closeButton.Click += (sender, e) =>
            {
                System.Environment.Exit(0);
            };
            layout.AddView(closeButton);

            return layout;
        }

        public LinearLayout GetGameLayout(Game game)
        {
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = game.Title;

            layout.AddView(aLabel);
            foreach (var character in game.Characters)
            {
                var button = new Button(this);
                button.Text = character.Title;
                button.Click += (sender, e) =>
                { SetContentView(GetCharacterLayout(character)); };
                layout.AddView(button);
            }
            var returnHomeButton = new Button(this);
            returnHomeButton.Text = "Return Home";
            returnHomeButton.Click += (sender, e) =>
            {
                SetContentView(GetHomeLayout());
            };
            layout.AddView(returnHomeButton);

            return layout;
        }

        public LinearLayout GetCharacterLayout(PlayerCharacter pCharacter)
        {
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = pCharacter.Title;

            layout.AddView(aLabel);
            foreach (var opponent in pCharacter.Opponents)
            {
                var button = new Button(this);
                button.Text = opponent.Title;
                button.Click += (sender, e) =>
                { SetContentView(GetOpponentLayout(pCharacter.Title, opponent)); };
                layout.AddView(button);
            }
            var returnHomeButton = new Button(this);
            returnHomeButton.Text = "Return Home";
            returnHomeButton.Click += (sender, e) =>
            {
                SetContentView(GetHomeLayout());
            };
            layout.AddView(returnHomeButton);

            return layout;
        }

        public LinearLayout GetOpponentLayout(string pPlayerCharacterTitle, OpponentMatchup pOpponent)
        {
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = pPlayerCharacterTitle + " VS " + pOpponent.Title;

            layout.AddView(aLabel);

            var winButton = new Button(this);
            winButton.Text = "Wins: " + pOpponent.Wins;
            winButton.Click += (sender, e) =>
            {
                pOpponent.Wins++;
                SetContentView(GetOpponentLayout(pPlayerCharacterTitle, pOpponent));
            };
            layout.AddView(winButton);

            var lossButton = new Button(this);
            lossButton.Text = "Losses: " + pOpponent.Losses;
            lossButton.Click += (sender, e) =>
            {
                pOpponent.Losses++;
                SetContentView(GetOpponentLayout(pPlayerCharacterTitle, pOpponent));
            };
            layout.AddView(lossButton);

            var notesLabel = new TextView(this);
            notesLabel.Text = pOpponent.Notes;
            layout.AddView(notesLabel);

            var returnHomeButton = new Button(this);
            returnHomeButton.Text = "Return Home";
            returnHomeButton.Click += (sender, e) =>
            {
                SetContentView(GetHomeLayout());
            };
            layout.AddView(returnHomeButton);

            return layout;
        }
        #endregion

        public LinearLayout GetAddGameLayout()
        {
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = "What is the name of the game?";
            layout.AddView(aLabel);

            var textField = new EditText(this);
            layout.AddView(textField);

            var saveButton = new Button(this);
            saveButton.Text = "Create";
            saveButton.Click += (sender, e) =>
            {
                menu.Games.Add(new Game(textField.Text));
                UpdateMenu();
                SetContentView(GetHomeLayout());
            };
            layout.AddView(saveButton);

            return layout;
        }
    }
}