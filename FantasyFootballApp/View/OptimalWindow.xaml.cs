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
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using System.IO;

namespace FantasyFootball.cs
{
    /// <summary>
    /// Interaction logic for OptimalWindow.xaml
    /// </summary>
    public partial class OptimalWindow : Window
    {
        Dictionary<string, int> pos_counts = new Dictionary<string, int>();
        FantasyTeam mockTeam;

        public int ProjectedPoints { get { return mockTeam.ProjectedTotal; } }

        public OptimalWindow(Player player = null)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 30;

            InitializeTeamBox();
            
            mockTeam = RunOptimizer(player);

            foreach(Player play in mockTeam.Roster)
            {
                AddPlayerToTeamBox(play);
            }

           // DisplayStatistics();
        }

        public static FantasyTeam RunOptimizer(Player playerUpForAuction)
        {
            FantasyTeam mock = new FantasyTeam("mock");

            foreach(string key in Brain.MyDraft.MyTeam.PercentageBudgetsByPosition.Keys)
            {
                mock.PercentageBudgetsByPosition.Add(key, Brain.MyDraft.MyTeam.PercentageBudgetsByPosition[key]);
            }
            
            foreach (Player p in Brain.MyDraft.MyTeam.Roster)
            {
                mock.addMockPlayer(p);
            }

            if(playerUpForAuction != null)
            {
                mock.addMockPlayer(playerUpForAuction);
            }

            foreach (string position in new string[]{"K","DST","TE","QB","RB","WR"})
            {
                int numberPositionRosterSpots = mock.Roster.Count(p => p.Position.Equals(position));
                OptimizePosition(mock, position, numberPositionRosterSpots);
            }

            return mock;
        }

        private static void OptimizePosition(FantasyTeam mockTeam, string position, int spotsAlreadyFilled)
        {
            int numberOfTotalSpots = Brain.MyDraft.RosterSpotsByPosition[position];
            int percentageBudgetForPosition = mockTeam.PercentageBudgetsByPosition[position];
            int percentageBudgetForBench = mockTeam.PercentageBudgetsByPosition["BENCH"];

            List<Player> players = new List<Player>();
            players.AddRange(Brain.MyDraft.PlayersByPosition[position]);

            while (spotsAlreadyFilled < numberOfTotalSpots)
            {
                int dollarBudgetForPosition = mockTeam.GetBudgetDollarForPosition(position);

                int numberOfSpotsAvailable = numberOfTotalSpots - spotsAlreadyFilled;
                int dollarBudgetForThisPlayer = numberOfSpotsAvailable > 1 ? (int)Math.Ceiling(dollarBudgetForPosition / 2.0) : dollarBudgetForPosition;

                Player bestPlayer = players.Where(p => p.Dollar <= dollarBudgetForThisPlayer && !mockTeam.Roster.Contains(p))
                    .OrderByDescending(s => s.Points).First();

                mockTeam.addMockPlayer(bestPlayer);

                players.Remove(bestPlayer);
                spotsAlreadyFilled++;
            }

            if (mockTeam.PercentageBudgetsByPosition[position] > 0)
            {
                var increase = mockTeam.PercentageBudgetsByPosition[position] * Brain.MyDraft.LeagueSalarayCap / 100;
                mockTeam.AdjustedCap += increase;
            }
        }

        public void InitializeTeamBox()
        {
            panelTeam.Children.Clear();

            int totalRosterSpots = Brain.MyDraft.GetSizeOfStarters() + 1;

            int count = 0;
            int size = totalRosterSpots > 18 ? 10 : 20;

            foreach (string position in Brain.MyDraft.GetPositions().Where(p => !p.Equals("BENCH")))
            {
                double positionCount = Brain.MyDraft.RosterSpotsByPosition[position];

                for (int i = 0; i < positionCount; i++)
                {
                    Label l = new Label();
                    l.Name = position + count;
                    l.Height = panelTeam.Height / totalRosterSpots;

                    l.FontSize = size;

                    l.Foreground = Brushes.White;
                    l.FontFamily = new FontFamily("Moire Light");
                    l.VerticalAlignment = VerticalAlignment.Center;
                    l.VerticalContentAlignment = VerticalAlignment.Center;
                    l.Content = position + " -\t";
                    l.BorderBrush = Brushes.White;
                    l.BorderThickness = new Thickness(1);
                    l.Opacity = 0;

                    panelTeam.Children.Add(l);
                    GUI_Animations.Fade_In_Control(l, 1000);
                }

                count++;
            }

            Label proj = new Label();
            proj.Name = "projTotal";
            proj.Height = panelTeam.Height / totalRosterSpots;
            proj.FontSize = size;
            proj.Foreground = Brushes.White;
            proj.FontFamily = new FontFamily("Moire Light");
            proj.VerticalAlignment = VerticalAlignment.Center;
            proj.VerticalContentAlignment = VerticalAlignment.Center;
            proj.Content = "Proj. Total - ";
            proj.BorderThickness = new Thickness(1);
            proj.BorderBrush = Brushes.White;
            proj.Opacity = 0;

            GUI_Animations.Fade_In_Control(proj, 1000);

            panelTeam.Children.Add(proj);
        }

        public void DisplayStatistics()
        {
            //foreach (Player p in mockTeam.Roster)
            //{
            //    AddPlayerToTeamBox(p);
            //}

            //foreach(string key in Brain.MyDraft.GetPositions())
            //{
            //    int budgetPercentage = Brain.MyDraft.percentageBudgetedByPosition[key];

            //    Label lab = new Label();
            //    lab.Foreground = Brushes.White;
            //    lab.Background = Brushes.Transparent;
            //    lab.Opacity = 0;

            //    if(key.Equals("BENCH"))
            //    {
            //        lab.Content = "BNC - \t" + budget;
            //    }
            //    else
            //    {
            //        lab.Content = key + " - \t" + budget;
            //    }

            //    lab.Height = budgetStack.Height / total;
            //    lab.FontSize = 20;

            //    budgetStack.Children.Add(lab);
            //    GUI_Animations.Fade_In_Control(lab, 1000);
            //}

            //double perWeek = (double)mockTeam.ProjectedTotal / 16;
            //double perDollar = (double)mockTeam.ProjectedTotal / Brain.leagueSalaryCap;

            //perWeek = Math.Round(perWeek, 3);
            //perDollar = Math.Round(perDollar, 3);

            //week.Opacity = 0;
            //dollar.Opacity = 0;

            //week.Content = "Points Per Week:\n" + perWeek;
            //dollar.Content = "Points Per $$$:\n" + perDollar;

            //GUI_Animations.Fade_In_Control(week, 1000);
            //GUI_Animations.Fade_In_Control(dollar, 1000);

            //List<KeyValuePair<string,int>> pairs = new List<KeyValuePair<string,int>>();
            //int all = mockTeam.ProjectedTotal;

            //foreach(string pos in Brain.MyDraft.GetPositions())
            //{
            //    int tot = 0;

            //    if(!pos.Equals("BENCH"))
            //    {
            //        foreach(Player p in mockTeam.Roster.Where(player => player.Position.Equals(pos)))
            //        {
            //            tot += p.Points;
            //            all -= p.Points;
            //        }

            //        KeyValuePair<string, int> next = new KeyValuePair<string, int>(pos + "  ", tot);
            //        pairs.Add(next);
            //    }
            //}

            //((BarSeries)mcChart.Series[0]).ItemsSource = pairs;
        }

        public void AddPlayerToTeamBox(Player playerToBeAdded)
        {
            string position = playerToBeAdded.Position;

            IEnumerable<Label> corresponding = panelTeam.Children.OfType<Label>().Where(label => label.Content.ToString().StartsWith(position));

            Label nextSlot = corresponding.FirstOrDefault(lab => lab.Content.ToString().Length < 7);

            if (nextSlot != null)
            {
                nextSlot.Content += playerToBeAdded.toTeam();

                Label projected = panelTeam.Children.OfType<Label>().Last();
                projected.Content = "Proj. Total - " + mockTeam.ProjectedTotal + " pts";
            }
            else
            {
                IEnumerable<Label> benchSlots = panelTeam.Children.OfType<Label>().Where(label => label.Content.ToString().StartsWith("BENCH"));
                nextSlot = benchSlots.FirstOrDefault(lab => lab.Content.ToString().Length == 8);

                if (nextSlot != null)
                {
                    nextSlot.Content += playerToBeAdded.toTeam();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
