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
using MatchUpBook.Models;

namespace MatchUpBook.Interfaces
{
    public interface IBuildMenuHandler
    {
        void AddGame(GameNode game);

        void AddCharacter(PlayerCharacterNode playerCharacter);

        void AddMatchup(OpponentMatchupNode opponentMatchup);

        void Remove(BaseMenuItem item);
    }
}