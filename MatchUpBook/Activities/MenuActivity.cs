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
using MatchUpBook.Models;

namespace MatchUpBook.Activities
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : Activity, IMenuHander
    {
        MenuNode menu;
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
                XmlSerializer serializer = new XmlSerializer(typeof(MenuNode));
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
            XmlSerializer serializer = new XmlSerializer(typeof(MenuNode));
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
				SetContentView(GetAddNewLayout(MenuItemType.Game));
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

        public LinearLayout GetGameLayout(GameNode game)
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

            var createButton = new Button(this);
            createButton.Text = "Add new Player/Character";
            createButton.Click += (sender, e) =>
            {
                SetContentView(GetAddNewLayout(MenuItemType.PlayerCharacter, game));
            };
            layout.AddView(createButton);
            
            var returnHomeButton = new Button(this);
            returnHomeButton.Text = "Return Home";
            returnHomeButton.Click += (sender, e) =>
            {
                SetContentView(GetHomeLayout());
            };
            layout.AddView(returnHomeButton);

            return layout;
        }

        public LinearLayout GetCharacterLayout(PlayerCharacterNode pCharacter)
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
            
            var createButton = new Button(this);
            createButton.Text = "Add new Opponent Matchup";
            createButton.Click += (sender, e) =>
            {
                SetContentView(GetAddNewLayout(MenuItemType.Opponent, pCharacter));
            };
            layout.AddView(createButton);

            var returnHomeButton = new Button(this);
            returnHomeButton.Text = "Return Home";
            returnHomeButton.Click += (sender, e) =>
            {
                SetContentView(GetHomeLayout());
            };
            layout.AddView(returnHomeButton);

            return layout;
        }

        public LinearLayout GetOpponentLayout(string pPlayerCharacterTitle, OpponentMatchupNode pOpponent)
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

        public LinearLayout GetAddNewLayout(MenuItemType type, BaseMenuItem parent = null)
        {
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = string.Format("What is the name of the {0}?", type.ToString());
            layout.AddView(aLabel);

            var textField = new EditText(this);
            layout.AddView(textField);

            var saveButton = new Button(this);
            saveButton.Text = "Create";
            saveButton.Click += (sender, e) =>
            {
                switch(type)
                {
                    case MenuItemType.Game:
                        menu.Games.Add(new GameNode(textField.Text));
                        UpdateMenu();
                        SetContentView(GetHomeLayout());
                        break;
                    case MenuItemType.PlayerCharacter:
                        ((GameNode)parent).Characters.Add(new PlayerCharacterNode(textField.Text));
                        UpdateMenu();
                        SetContentView(GetGameLayout(((GameNode)parent)));
                        break;
                    case MenuItemType.Opponent:
                        ((PlayerCharacterNode)parent).Opponents.Add(new OpponentMatchupNode(textField.Text));
                        UpdateMenu();
                        SetContentView(GetCharacterLayout(((PlayerCharacterNode)parent)));
                        break;
                    default:
                        break;
                }
            };
            layout.AddView(saveButton);

            return layout;
        }
    }
}