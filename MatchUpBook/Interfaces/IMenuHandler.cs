using Android.Content;
using Android.Widget;
using MatchUpBook.Models;
using System;

namespace MatchUpBook.Interfaces
{
    public interface IMenuHandler
    {
        LinearLayout GetHomeLayout();

        LinearLayout GetGameLayout(GameNode pGame);

        LinearLayout GetCharacterLayout(PlayerCharacterNode pCharacter);

        LinearLayout GetOpponentLayout(string pPlayerCharacterTitle, OpponentMatchupNode pOpponent);
    }
}

