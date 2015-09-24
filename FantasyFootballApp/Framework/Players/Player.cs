using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootball.cs
{
    [Serializable]
    public class Player
    {
        Game[] schedule = new Game[16];
        string club;

        string firstName;
        string lastName;
        string position = string.Empty;

        int byeWeek;
        int dollar = 0;
        int projectedPoints = 0;
        int strengthOfSchedule = 0;

        bool star = false;
        bool bench = false;

        public string First { get { return firstName; } set { firstName = value; } }
        public string Last { get { return lastName; } }
        public string Full { get { return firstName + " " + lastName; } }
        public string Position { get { return position; } set { position = value; } }

        public int Dollar { get { return dollar; } set { dollar = value; } }
        public int Points { get { return projectedPoints; } set { projectedPoints = value; } }
        public int PointsPerDollar { get { if (Dollar > 0) { return (int)(Points / Dollar); } else { return 0; } } }
        public int SOS { get { return strengthOfSchedule; } set { strengthOfSchedule = value; } }

        public string Club { get { return club; } }

        public bool Star { get { return star; } set { star = value; } }
        public bool Bench { get { return bench; } set { bench = value; } }

        public Player()
        {

        }

        public Player(string f, string l, string team, string position, int dollar, int points)
        {
            club = team;
            firstName = f;
            lastName = l;
            this.position = position;
            this.dollar = dollar;
            this.Points = points;
        }

        public string toBox()
        {
            return Full + ", " + position + " $" + Dollar + " - " + Points + " points";
        }

        public string ppdBox()
        {
            return Full + ", " + position + " $" + dollar + " - " + PointsPerDollar + " PPD";
        }

        public string moneyBox()
        {
            return Full + ", " + position + " $" + dollar;
        }

        public string sosBox()
        {
            return Full + ", " + position + " $" + dollar + " - " + SOS + " SoS";
        }

        public string toTeam()
        {
            return Full + " - $" + Dollar;
        }

        public string toOptimalTeam()
        {
            return Full + " - $" + Dollar + " (" + Points + ")";
        }

        public virtual int computeSuggestedValue()
        {
            return 0;
        }

        public Player copy()
        {
            return new Player(firstName, lastName, club, position, dollar, projectedPoints);
        }


    }
}
