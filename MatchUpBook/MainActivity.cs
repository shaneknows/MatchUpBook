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

namespace MatchUpBook
{
    [Activity(Label = "MatchUpBook", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IMenuHander
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(GetHomeLayout());
        }

		#region IMenuHandler Features
        public LinearLayout GetHomeLayout()
        {
			string content;

			using (StreamReader sr = new StreamReader(Assets.Open("Data.xml")))
			{
				content = sr.ReadToEnd();
			}

			MenuFactory menuFactory = new MenuFactory();
			var menu = menuFactory.CreateMenu(content);

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

			var notesLabel = new TextView (this);
			notesLabel.Text = pOpponent.Notes;
			layout.AddView (notesLabel);

			var returnHomeButton = new Button (this);
			returnHomeButton.Text = "Return Home";
			returnHomeButton.Click += (sender, e) => 
			{
				SetContentView(GetHomeLayout());
			};
            layout.AddView(returnHomeButton);

            return layout;
        }
		#endregion
    }
}


