using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootball.cs
{
    [Serializable]
    public class Team
    {
        private List<Game> schedule = new List<Game>();

        private string teamName;
        private string abbreviation;
        int byeweek = 0;

        public Team()
        {

        }

        public Team(string n)
        {
            teamName = n;
        }

        public int ByeWeek
        {
            get { return byeweek; }
            set { byeweek = value; }
        }

        public string TeamName
        {
            get { return teamName; }
        }

        public string Abbreviation
        {
            get { return abbreviation; }
            set { abbreviation = value; }
        }

        public List<Game> Schedule
        {
            get { return schedule; }
        }
    }
}
