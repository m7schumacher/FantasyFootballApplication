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
using System.Windows.Media.Animation;

namespace FantasyFootball.cs
{
    public static class GUI_Changer
    {
        static MainWindow window;

        public static void initialize(MainWindow w)
        {
            window = w;
        }

        //public static void RosterState()
        //{
        //    RosterCreationState roster = new RosterCreationState(window);
        //    roster.rosterCreationState();
        //}

        //public static void BudgetState()
        //{
        //    States.BudgetState budget = new States.BudgetState(window);
        //    budget.budgetSettingState();
        //}

        //public static void StatsState()
        //{
        //    States.PlayerStats_State stat = new States.PlayerStats_State(window);
        //    stat.scoreEditState();
        //}

        //public static void CreateLeague()
        //{
        //    CreateLeagueState create = new CreateLeagueState(window);
        //    create.leagueCreationState();
        //}

        //public static void ScoringState()
        //{
        //    States.ScoringState score = new States.ScoringState(window);
        //    score.scoreEditState();
        //}

        //public static void DraftState()
        //{
        //    States.DraftState draft = new States.DraftState(window);
        //    draft.InitializeDraftState();
        //}
    }
}
