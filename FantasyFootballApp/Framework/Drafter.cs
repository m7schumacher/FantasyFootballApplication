using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using System.Timers;
using System.Reflection;
using System.IO;

namespace FantasyFootball.cs
{
    public class Drafter
    {
        public Drafter()
        {

        }

        public void DraftPlayer(Player draftedPlayer, FantasyTeam draftingTeam)
        {
            //string playerInfo = draftedPlayer.Full + ", " + draftedPlayer.Position + " - $" + draftedPlayer.Dollar;

            //int moneyAfterDrafting = draftingTeam.MoneyLeft - (16 - (Brain.myTeam.Roster.Count + 1) + draftedPlayer.Dollar);
            //bool canAffordToDraft = moneyAfterDrafting >= 0;

            //if (!canAffordToDraft)
            //{
            //    MessageBox.Show("Not Enough $$$");
            //}
            //else
            //{
            //    Brain.myTeam.addPlayer(draftedPlayer);

            //    if (Brain.lastFivePlayersDrafted.Count == 5)
            //    {
            //        Brain.lastFivePlayersDrafted.Remove(Brain.lastFivePlayersDrafted.ElementAt(0));
            //        Brain.lastFivePlayersDrafted.Add(draftedPlayer);
            //    }
            //    else
            //    {
            //        Brain.lastFivePlayersDrafted.Add(draftedPlayer);
            //    }
            //}
        }

        

    }
}
