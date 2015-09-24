using FantasyFootball.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootballApp.Framework
{
    [Serializable]
    public class Draft
    {
        public int TeamsInLeague { get; set; }
        public int LeagueSalarayCap { get; set; }
        public int LeaguePool { get; set; }
        public int LeagueStartPool { get; set; }

        public Player CurrentPlayerUpForAuction { get; set; }
        public Player SelectedPlayer { get; set; }

        public List<FantasyTeam> League { get; set; }
        public List<Player> Players { get; set; }

        public Dictionary<string, List<Player>> PlayersByPosition { get; set; }
        public Dictionary<string, int> RosterSpotsByPosition { get; set; }
        public Dictionary<string, int> percentageBudgetedByPosition { get; set; }

        public FantasyTeam MyTeam { get; set; }

        public Draft()
        {
            TeamsInLeague = 0;
            LeagueSalarayCap = 0;
            LeaguePool = 0;
            LeagueStartPool = 0;

            CurrentPlayerUpForAuction = null;
            SelectedPlayer = null;

            League = new List<FantasyTeam>();
            Players = new List<Player>();

            PlayersByPosition = new Dictionary<string, List<Player>>();
            RosterSpotsByPosition = new Dictionary<string, int>();

            MyTeam = null;
        }

        public IEnumerable<string> GetPositions()
        {
            return RosterSpotsByPosition.Keys.Where(k => RosterSpotsByPosition[k] > 0);
        }

        public int GetSizeOfStarters()
        {
            int total = 0;

            foreach (string key in RosterSpotsByPosition.Keys.Where(k => !k.Equals("BENCH")))
            {
                total += RosterSpotsByPosition[key];
            }

            return total;
        }

        public int GetSizeOfRoster()
        {
            return RosterSpotsByPosition.Values.Sum();
        }

        public FantasyTeam GetTeamByOwner(string owner)
        {
            return League.First(f => f.Owner.Equals(owner));
        }
    }
}
