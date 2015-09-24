using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootball.cs
{
    public class Defense : Player
    {
        //public Defense(Team newTeam, string f, string l, int bye, string position, int dollar)
        //    : base(newTeam, f, l, bye, position, dollar)
        //{
        //    base.Position = "DST";
        //}

        public Defense()
            : base()
        {
            base.Position = "DST";
        }
    }
}
