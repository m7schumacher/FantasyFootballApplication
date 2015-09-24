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

namespace FantasyFootball.cs
{
    /// <summary>
    /// Interaction logic for LeagueWindow.xaml
    /// </summary>
    public partial class LeagueWindow : Window
    {
        private static int total = 0;

        string bestOwner = string.Empty;
        string worstOwner = string.Empty;
        string mostMoneyLeft = string.Empty;
        string leastMoneyLeft = string.Empty;

        public LeagueWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 30;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;

            List<FantasyTeam> league = Brain.MyDraft.League;

            foreach(FantasyTeam ft in league)
            {
                teamsBox.Items.Add(ft.Owner);
            }

            FantasyTeam richestTeam = league.OrderByDescending(p => p.MoneyLeft).First();
            FantasyTeam poorestTeam = league.OrderBy(p => p.MoneyLeft).First();
            FantasyTeam bestTeam = league.OrderByDescending(f => f.ProjectedTotal).First();
            FantasyTeam worstTeam = league.OrderBy(p => p.ProjectedTotal).First();

            bestOwner = bestTeam.Owner + " (" + bestTeam.ProjectedTotal + ")";
            worstOwner = worstTeam.Owner + " (" + worstTeam.ProjectedTotal + ")";
            mostMoneyLeft = richestTeam.Owner + " ($" + richestTeam.MoneyLeft + ")";
            leastMoneyLeft = poorestTeam.Owner + " ($" + poorestTeam.MoneyLeft + ")";

            poolLeft.Content = "$" + Brain.MyDraft.LeaguePool + " left";

            highestProjected.Content = "Best Team - " + bestOwner;
            lowestProject.Content = "Worst Team - " + worstOwner;
            mostLeft.Content = "Most $$$ - " + mostMoneyLeft;
            leastLeft.Content = "Least $$$ - " + leastMoneyLeft;

            InitializeTeamBox();
        }

        private void teamsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            total = 0;

            resetTeamBox();

            FantasyTeam temp = Brain.MyDraft.GetTeamByOwner(teamsBox.SelectedItem.ToString());
            teamLabel.Content = temp.Owner;

            foreach(Player p in temp.Roster)
            {
                updateTeamBox(p, temp);
            }
        }

        public void InitializeTeamBox()
        {
            int totalRosterSpots = Brain.MyDraft.GetSizeOfRoster() + 1;
            int count = 0;
            double sumOfHeights = 0;
            int fontSize = totalRosterSpots > 18 ? 10 : 20;

            foreach (string position in Brain.MyDraft.GetPositions())
            {
                double positionCount = Brain.MyDraft.RosterSpotsByPosition[position];

                for (int i = 0; i < positionCount; i++)
                {
                    Label l = new Label();
                    l.Name = position + count;
                    l.Height = Math.Ceiling(panelTeam.Height / (totalRosterSpots + 1));
                    sumOfHeights += l.Height;

                    l.FontSize = fontSize;

                    l.Foreground = Brushes.White;
                    l.FontFamily = new FontFamily("Moire Light");
                    l.VerticalAlignment = VerticalAlignment.Center;
                    l.VerticalContentAlignment = VerticalAlignment.Center;
                    l.Content = position + " - ";
                    l.Foreground = Brushes.White;
                    l.BorderBrush = Brushes.Gray;
                    l.BorderThickness = new Thickness(1, 1, 1, 0);

                    panelTeam.Children.Add(l);
                }

                count++;
            }

            Label proj = new Label();
            proj.Name = "projTotal";
            proj.Height = panelTeam.Height - sumOfHeights;
            proj.FontSize = fontSize;
            proj.Foreground = Brushes.White;
            proj.FontFamily = new FontFamily("Moire Light");
            proj.VerticalAlignment = VerticalAlignment.Center;
            proj.VerticalContentAlignment = VerticalAlignment.Center;
            proj.Content = "Proj. Total - ";
            proj.BorderThickness = new Thickness(1, 1, 1, 1);
            proj.BorderBrush = Brushes.Gray;

            panelTeam.Children.Add(proj);
        }

        public void resetTeamBox()
        {
            string projected = "Proj. Total - ";

            foreach(Label l in panelTeam.Children)
            {
                if (l.Content.ToString().StartsWith("K"))
                {
                    l.Content = l.Content.ToString().Substring(0, 3);
                }
                else if (l.Content.ToString().StartsWith("D"))
                {
                    l.Content = l.Content.ToString().Substring(0, 6);
                }
                else if(l.Content.ToString().StartsWith("P"))
                {
                    l.Content = l.Content.ToString().Substring(0, projected.Length);
                }
                else
                {
                    l.Content = l.Content.ToString().Substring(0, l.Content.ToString().IndexOf("-") + 1);
                }
            }
        }

        public void updateTeamBox(Player p, FantasyTeam team)
        {
            string playerPosition = p.Position;

            IEnumerable<Label> correspondingLabels = panelTeam.Children.OfType<Label>().Where(label => label.Content.ToString().StartsWith(playerPosition));
            Label nextOpenSlot = correspondingLabels.FirstOrDefault(lab => lab.Content.ToString().Split('-')[1].Length == 0);

            if (nextOpenSlot != null)
            {
                nextOpenSlot.Content += p.toTeam();

                Label projectedTotalLabel = panelTeam.Children.OfType<Label>().First(label => label.Name.Equals("projTotal"));
                projectedTotalLabel.Content = "Proj. Total - " + team.ProjectedTotal + " pts";
            }
            else
            {
                IEnumerable<Label> benchSlots = panelTeam.Children.OfType<Label>().Where(label => label.Content.ToString().StartsWith("BENCH"));
                nextOpenSlot = benchSlots.FirstOrDefault(lab => lab.Content.ToString().Length == 8);

                if (nextOpenSlot != null)
                {
                    nextOpenSlot.Content += p.toTeam();
                }
            }
        }

        //public void updateTeamBoxQB(Player play)
        //{
        //    string tester = QB1.Content.ToString();

        //    if (QB1.Content.ToString().Replace("QB -", string.Empty).Length == 0)
        //    {
        //        QB1.Content += play.toTeam();
        //    }
        //    else
        //    {
        //        Label[] bench = { BN1, BN2, BN3, BN4, BN5, BN6 };

        //        Label l = new Label();
        //        int counter = 0;

        //        while (bench[counter].Content.ToString().Replace("BN -", string.Empty).Length > 0)
        //        {
        //            counter++;
        //        }

        //        if (counter != 5)
        //        {
        //            bench[counter].Content += play.toTeam();
        //        }
        //    }
        //}

        //public void updateTeamBoxRB(Player play)
        //{
        //    string tester = RB1.Content.ToString();

        //    if (RB1.Content.ToString().Replace("RB -", string.Empty).Length == 0)
        //    {
        //        RB1.Content += play.toTeam();
        //    }
        //    else if (RB2.Content.ToString().Replace("RB -", string.Empty).Length == 0)
        //    {
        //        RB2.Content += play.toTeam();
        //    }
        //    else
        //    {
        //        Label[] bench = { BN1, BN2, BN3, BN4, BN5, BN6 };

        //        Label l = new Label();
        //        int counter = 0;

        //        while (bench[counter].Content.ToString().Replace("BN -", string.Empty).Length > 0)
        //        {
        //            counter++;
        //        }

        //        if (counter != 5)
        //        {
        //            bench[counter].Content += play.toTeam();
        //        }
        //    }
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
