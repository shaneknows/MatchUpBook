using Android.Content;
using Android.Widget;
using MatchUpBook.Models;
using System;

namespace MatchUpBook.Interfaces
{
    public interface IMenuHandler
    {
        ScrollView GetHomeLayout();

        ScrollView GetGameLayout(GameNode pGame);

        ScrollView GetCharacterLayout(PlayerCharacterNode pCharacter);

        ScrollView GetOpponentLayout(OpponentMatchupNode pOpponent);
    }
}

