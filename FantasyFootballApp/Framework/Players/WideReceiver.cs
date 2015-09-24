using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootball.cs
{
    public class WideReceiver : Player
    {
        //public WideReceiver(Team newTeam, string f, string l, int bye, string position, int dollar) : base(newTeam,f,l,bye,position,dollar)
        //{
        //    base.Position = "WR";
        //}

        public WideReceiver() : base()
        {
            base.Position = "WR";
        }
    }
}
