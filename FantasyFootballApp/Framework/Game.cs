using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootball.cs
{
    [Serializable]
    public class Game
    {
        Team opponent;
        bool home;

        public Team Opponent { get { return opponent; } }
        public bool Home { get { return home; } }

        public Game(Team t, bool h)
        {
            opponent = t;
            home = h;
        }
    }
}
