using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootball.cs
{
    public class Kicker : Player
    {
        public Kicker(string f, string l, string team, string position, int dollar, int points) : base(f, l, team, position, dollar, points)
        {
            base.Position = "K";
        }

        public Kicker() : base()
        {
            base.Position = "K";
        }
    }
}
