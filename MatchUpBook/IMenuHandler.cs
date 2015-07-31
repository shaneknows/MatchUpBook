using Android.Content;
using Android.Widget;
using System;

namespace MatchUpBook
{
    public interface IMenuHander
    {
        LinearLayout GetHomeLayout();

        LinearLayout GetGameLayout(Game pGame);

        LinearLayout GetCharacterLayout(PlayerCharacter pCharacter);

        LinearLayout GetOpponentLayout(string pPlayerCharacterTitle, OpponentMatchup pOpponent);
    }
}

