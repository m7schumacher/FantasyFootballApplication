using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootball.cs
{
    public class Quarterback : Player
    {
        public Quarterback(string f, string l, string team, string position, int dollar, int points) : base(f, l, team, position, dollar, points)        
        {
            base.Position = "QB";
        }

        public Quarterback() : base()
        {
            base.Position = "QB";
        }

        public override int computeSuggestedValue()
        {
            return 0;
        }
    }
}
