using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FantasyFootball.cs
{
    [Serializable]
    public class FantasyTeam
    {
        public string Owner { get; set; }

        public int MoneyLeft { get; set; }
        public int ProjectedTotal { get; set; }
        public int AdjustedCap { get; set; }

        public List<Player> Roster { get; set; }
        public Dictionary<string, int> PercentageBudgetsByPosition { get; set; }

        public FantasyTeam(string owner)
        {
            Owner = owner;
            Roster = new List<Player>();
            MoneyLeft = AdjustedCap = Brain.MyDraft.LeagueSalarayCap;
            PercentageBudgetsByPosition = new Dictionary<string, int>();
        }

        private bool CanAfford(Player player)
        {
            return (player.Dollar + (16 - Roster.Count) < MoneyLeft);
        }

        public void addPlayer(Player playerToBeAdded)
        {
            if(playerToBeAdded.First != null)
            {
                if(!CanAfford(playerToBeAdded))
                {
                    MessageBox.Show("Not Enough Money!");
                    return;
                }

                Roster.Add(playerToBeAdded);
                MoneyLeft -= playerToBeAdded.Dollar;
                ProjectedTotal += playerToBeAdded.Points;
            }
        }

        public int GetBudgetDollarForPosition(string position)
        {
            int cap = Brain.MyDraft.LeagueSalarayCap;
            int percentage = PercentageBudgetsByPosition[position];

            return (int)(cap * (percentage / 100.0));
        }
        
        public int GetMockBudgetDollarForPosition(string position)
        {
            int dollars = (int)(AdjustedCap * (PercentageBudgetsByPosition[position] / 100.0));
            return dollars;
        }

        public void addMockPlayer(Player p)
        {
            if (p.First != null)
            {
                Roster.Add(p);
                MoneyLeft -= p.Dollar;
                ProjectedTotal += p.Points;

                int budgetPercentage = PercentageBudgetsByPosition[p.Position];

                int dollarsLeft = GetMockBudgetDollarForPosition(p.Position);
                double dollarsAfterDraft = dollarsLeft - p.Dollar;

                if(dollarsAfterDraft < 0)
                {
                    AdjustedCap += (int)dollarsAfterDraft;
                    PercentageBudgetsByPosition[p.Position] = 0;
                }
                else
                {
                    int newBudget = (int)(dollarsAfterDraft * 100 / Brain.MyDraft.LeagueSalarayCap);
                    PercentageBudgetsByPosition[p.Position] = newBudget;
                }
            }
        }
    }
}
