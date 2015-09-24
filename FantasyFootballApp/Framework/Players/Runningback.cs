using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootball.cs
{
    public class Runningback : Player
    {
        public Runningback(string f, string l, string team, string position, int dollar, int points) : base(f, l, team, position, dollar, points)        
        {
            base.Position = position;
        }

        public Runningback() : base()
        {
            base.Position = "RB";
        }
    }
}
