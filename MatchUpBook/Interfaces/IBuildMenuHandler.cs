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

namespace MatchUpBook.Interfaces
{
    public interface IBuildMenuHandler
    {
        void AddGame(Game game);

        void AddCharacter(PlayerCharacter playerCharacter);

        void AddMatchup(OpponentMatchup opponentMatchup);

        void Remove(BaseMenuItem item);
    }
}