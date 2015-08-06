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
using MatchUpBook.Interfaces;
using MatchUpBook.Models;
using System.Linq;

namespace MatchUpBook
{
    [Activity(Label = "MatchUpBook", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IMenuHandler
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

            //If local menu exists load it, otherwise create new menu
            if (File.Exists(filename))
            {
                using (var streamReader = new StreamReader(filename))
                {
                    content = streamReader.ReadToEnd();
                }
                menu = menuFactory.GetMenu(content);
            }
            else
            {
                menu = new MenuNode();
                UpdateMenu();
            }
        }

        public BaseMenuItem UpdateMenu(MenuItemType? type = null, BaseMenuItem item = null)
        {
            foreach(var game in menu.Games)
            {
                foreach(var character in game.Characters)
                {
                    character.Opponents = character.Opponents.OrderBy(x => x.Title).ToList();
                }
                game.Characters = game.Characters.OrderBy(x => x.Title).ToList();
            }
            menu.Games = menu.Games.OrderBy(x => x.Title).ToList();
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
            if(type.HasValue && item != null)
            {
                switch(type.Value)
                {
                    case MenuItemType.PlayerCharacter:
                        PlayerCharacterNode pCharacter = (PlayerCharacterNode)item;
                        return menu.Games.Where(x => x.Title == pCharacter.Parent.Title).First();
                    case MenuItemType.Opponent:
                        OpponentMatchupNode oMatchup = (OpponentMatchupNode)item;
                        return menu.Games.Where(x => x.Title == oMatchup.Parent.Parent.Title).First()
                            .Characters.Where(y => y.Title == oMatchup.Parent.Title).First();
                }
            }
            return null;
        }

        #region IMenuHandler Features
        public ScrollView GetHomeLayout()
        {
            var scrollView = new ScrollView(this);
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
            layout.AddView(createButton);

            var closeButton = new Button(this);
            closeButton.Text = "Close";
            closeButton.Click += (sender, e) =>
            {
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            };
            layout.AddView(closeButton);

            scrollView.AddView(layout);
            return scrollView; ;
        }

        public ScrollView GetGameLayout(GameNode game)
        {
            var scrollView = new ScrollView(this);
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

            var backButton = new Button(this);
            backButton.Text = "Back";
            backButton.Click += (sender, e) =>
            {
                SetContentView(GetHomeLayout());
            };
            layout.AddView(backButton);
            scrollView.AddView(layout);

            return scrollView;
        }

        public ScrollView GetCharacterLayout(PlayerCharacterNode pCharacter)
        {
            var scrollView = new ScrollView(this);
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
                { SetContentView(GetOpponentLayout(opponent)); };
                layout.AddView(button);
            }

            var createButton = new Button(this);
            createButton.Text = "Add new Opponent Matchup";
            createButton.Click += (sender, e) =>
            {
                SetContentView(GetAddNewLayout(MenuItemType.Opponent, pCharacter));
            };
            layout.AddView(createButton);

            var backButton = new Button(this);
            backButton.Text = "Back";
            backButton.Click += (sender, e) =>
            {
                SetContentView(GetGameLayout(pCharacter.Parent));
            };
            layout.AddView(backButton);
            scrollView.AddView(layout);

            return scrollView;
        }

        public ScrollView GetOpponentLayout(OpponentMatchupNode pOpponent)
        {
            var scrollView = new ScrollView(this);
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = pOpponent.Parent.Title + " VS " + pOpponent.Title;

            layout.AddView(aLabel);

            var winButton = new Button(this);
            winButton.Text = "Wins: " + pOpponent.Wins;
            winButton.Click += (sender, e) =>
            {
                pOpponent.Wins++;
                UpdateMenu();
                SetContentView(GetOpponentLayout(pOpponent));
            };
            layout.AddView(winButton);

            var lossButton = new Button(this);
            lossButton.Text = "Losses: " + pOpponent.Losses;
            lossButton.Click += (sender, e) =>
            {
                pOpponent.Losses++;
                UpdateMenu();
                SetContentView(GetOpponentLayout(pOpponent));
            };
            layout.AddView(lossButton);

            var notesLabel = new TextView(this);
            notesLabel.Text = pOpponent.Notes;
            layout.AddView(notesLabel);

            var editButton = new Button(this);
            editButton.Text = "Edit";
            editButton.Click += (sender, e) =>
            {
                SetContentView(GetEditLayout(pOpponent));
            };
            layout.AddView(editButton);

            var backButton = new Button(this);
            backButton.Text = "Back";
            backButton.Click += (sender, e) =>
            {
                SetContentView(GetCharacterLayout(pOpponent.Parent));
            };
            layout.AddView(backButton);
            scrollView.AddView(layout);

            return scrollView;
        }
        #endregion

        public ScrollView GetAddNewLayout(MenuItemType type, BaseMenuItem item = null)
        {
            var scrollView = new ScrollView(this);
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
                switch (type)
                {
                    case MenuItemType.Game:
                        menu.Games.Add(new GameNode(textField.Text));
                        UpdateMenu();
                        SetContentView(GetHomeLayout());
                        break;
                    case MenuItemType.PlayerCharacter:
                        var newCharacter = new PlayerCharacterNode(textField.Text, (GameNode)item);
                        ((GameNode)item).Characters.Add(newCharacter);
                        var gameItem = (GameNode)UpdateMenu(type, newCharacter);
                        SetContentView(GetGameLayout(gameItem));
                        break;
                    case MenuItemType.Opponent:
                        var newOpponent = new OpponentMatchupNode(textField.Text, (PlayerCharacterNode)item);
                        ((PlayerCharacterNode)item).Opponents.Add(newOpponent);
                        var newItem = (PlayerCharacterNode)UpdateMenu(type, newOpponent);
                        SetContentView(GetCharacterLayout(newItem));
                        break;
                    default:
                        break;
                }
            };
            layout.AddView(saveButton);
            scrollView.AddView(layout);

            return scrollView;
        }

        public ScrollView GetEditLayout(OpponentMatchupNode item)
        {
            var scrollView = new ScrollView(this);
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = string.Format("{0} VS {1} Notes:", item.Parent.Title, item.Title);
            layout.AddView(aLabel);

            var textField = new EditText(this);
            textField.Text = item.Notes;
            layout.AddView(textField);

            var saveButton = new Button(this);
            saveButton.Text = "Save";
            saveButton.Click += (sender, e) =>
            {
                item.Notes = textField.Text;
                UpdateMenu();
                SetContentView(GetOpponentLayout(item));
            };
            layout.AddView(saveButton);
            scrollView.AddView(layout);

            return scrollView;
        }
    }
}