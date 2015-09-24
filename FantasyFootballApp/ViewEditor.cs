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
    public class ViewEditor
    {
        MainWindow current = null;
        DateTime start;
        Menu menu;

        public static List<Action> actions;
        public static List<Action>.Enumerator actionEnumerator;

        public double Time
        {
            get { return (DateTime.Now - start).TotalSeconds; }
        }

        public ViewEditor(MainWindow w)
        {
            current = w;

            //menu = new Menu(current);
            GUI_Animations.Initialize(current);
            GUI_Changer.initialize(current);

            start = DateTime.Now;
            actions = new List<Action>();
            //actions.Add(() => menu.init_SelectLoadOrCreateState());

            actionEnumerator = actions.GetEnumerator();
        }

        public void ShowWelcomeMenu()
        {
            Brain.data = "Players.txt";

            GUI_Animations.Fade_All_Controls();

            Label welcome = new Label();

            welcome.Name = "wel";
            welcome.Width = 1000;
            welcome.HorizontalAlignment = HorizontalAlignment.Center;
            welcome.VerticalAlignment = VerticalAlignment.Center;
            welcome.HorizontalContentAlignment = HorizontalAlignment.Center;

            welcome.Foreground = Brushes.White;
            welcome.FontSize = 100;

            welcome.Content = "Welcome";

            current.root.Children.Add(welcome);

            GUI_Animations.Fade_Out_Control(welcome, 700);
        }

        public void initializeControls()
        {
            current.PlayerBox.SelectedItem = current.PlayerBox.Items.GetItemAt(0);
            current.upForBidButton.IsEnabled = false;
            current.lgButton.IsEnabled = false;
            current.opButton.IsEnabled = false;

            current.upForPanel.Visibility = Visibility.Collapsed;
        }

        public void PopulatePlayerBox(string position = "")
        {
            current.PlayerBox.Items.Clear();
            current.searchBox.Clear();

            List<Player> copy = Brain.MyDraft.Players;

            if (!position.Equals("Clear") && position.Length > 0)
            {
                copy = copy.Where(p => p.Position.Equals(position)).ToList();
            }

            foreach (Player p in copy.OrderByDescending(p => p.Dollar))
            {
                current.PlayerBox.Items.Add(p.toBox());
            }
        }

        public void ClearLabels()
        {
            current.nameLabel.Content = string.Empty;
            current.teamLabel.Content = string.Empty;
            current.dollarLabel.Content = string.Empty;
            current.ppdLabel.Content = string.Empty;
            current.pointsLabel.Content = string.Empty;

            current.scheduleBox.Items.Clear();
        }

        public void updateLabels(Player temp)
        {
            current.nameLabel.Content = temp.Full + " - " + temp.Position;
            current.teamLabel.Content = temp.Club;
            current.dollarLabel.Content = "$" + temp.Dollar;

            int projectedPoints = temp.Points;

            double ppd = 0;

            if (temp.Dollar > 0)
            {
                ppd = projectedPoints / temp.Dollar;
            }

            current.ppdLabel.Content = "PPD - " + ppd;

            current.pointsLabel.Content = "PTS - " + projectedPoints;
        }

        public void addItems(List<Player> input, string vert)
        {
            current.PlayerBox.Items.Clear();

            if (vert.Equals("ppd"))
            {
                foreach (Player p in input)
                {
                    current.PlayerBox.Items.Add(p.ppdBox());
                }
            }
            else if (vert.Equals("dollar"))
            {
                foreach (Player p in input)
                {
                    current.PlayerBox.Items.Add(p.moneyBox());
                }
            }
            else if (vert.Equals("sos"))
            {
                foreach (Player p in input)
                {
                    current.PlayerBox.Items.Add(p.sosBox());
                }
            }
            else
            {
                foreach (Player p in input)
                {
                    current.PlayerBox.Items.Add(p.toBox());
                }
            }

        }

        public void displaySearchResults(string search)
        {
            var sorted =
                from p in Brain.MyDraft.Players
                where p.Last.ToLower().Contains(search.ToLower())
                select p;

            if (sorted.ToList().Count == 0)
            {
                MessageBox.Show("No Players With Last Name: '" + search + "'");
                current.searchBox.Clear();
            }
            else
            {
                addItems(sorted.ToList(), "");
            }
        }

        private List<Player> gatherCurrentPlayers()
        {
            List<Player> curr = new List<Player>();

            foreach (string s in current.PlayerBox.Items)
            {
                curr.Add(Brain.MyDraft.Players.First(v => s.StartsWith(v.First + " " + v.Last, StringComparison.InvariantCultureIgnoreCase)));
            }

            return curr;
        }

        public void sortPlayerBox(string value)
        {
            var sorted =
                    from p in gatherCurrentPlayers()
                    select p;

            if(value.Equals("points"))
            {
                sorted =
                    from p in gatherCurrentPlayers()
                    orderby p.Points descending
                    select p;
            }
            else if (value.Equals("ppd"))
            {
                sorted =
                    from p in gatherCurrentPlayers()
                    orderby p.PointsPerDollar descending
                    select p;
            }
            else if (value.Equals("dollar"))
            {
                sorted =
                    from p in gatherCurrentPlayers()
                    orderby p.Dollar descending
                    select p;
            }
            else if (value.Equals("sos"))
            {
                sorted =
                    from p in gatherCurrentPlayers()
                    orderby p.SOS descending
                    select p;
            }
            else
            {
                sorted = Brain.MyDraft.Players;
            }
  
            current.PlayerBox.Items.Clear();
            ClearLabels();

            addItems(sorted.ToList(), "");
        }

        public void putPlayerUpForAuction(Player temp, double suggested)
        {
            Player currentPlayer = Brain.MyDraft.CurrentPlayerUpForAuction;

            current.upForLabel.Content = currentPlayer.Full + ", " + currentPlayer.Position;
            current.upForPanel.Opacity = 0;
            current.upForPanel.Visibility = Visibility.Visible;
            current.suggestedLabel.Content = "$" + suggested;

            GUI_Animations.Fade_In_Control(current.upForPanel, 600);
        }

        public void TakePlayerDownForAuction()
        {
            Brain.current.lineChart.Opacity = 0;
            current.upForLabel.Content = "";
            current.upForPanel.Visibility = Visibility.Hidden;
        }

        public void showDraftToOtherTeam()
        {
            AddToOtherTeam at = new AddToOtherTeam();

            foreach (FantasyTeam t in Brain.MyDraft.League)
            {
                at.leagueBox.Items.Add(t.Owner);
            }

            at.ShowDialog();

            if (Brain.other_value_entered)
            {
                PopulatePlayerBox(string.Empty);
                TakePlayerDownForAuction();

                Brain.other_value_entered = false;
                Brain.lastFivePlayersDrafted.Add(Brain.MyDraft.CurrentPlayerUpForAuction);
            }
        }

        public void showBudgets()
        {
            current.lblBudget.Content = 
                "QB(" + Brain.MyDraft.MyTeam.GetBudgetDollarForPosition("QB") + ")  " +
                "RB(" + Brain.MyDraft.MyTeam.GetBudgetDollarForPosition("RB") + ")  " +
                "WR(" + Brain.MyDraft.MyTeam.GetBudgetDollarForPosition("WR") + ")  " +
                "TE(" + Brain.MyDraft.MyTeam.GetBudgetDollarForPosition("TE") + ")"; 

            current.lblBudget.Visibility = Visibility.Visible;
        }

        public void hideBudgets()
        {
            current.lblBudget.Content = "";
            current.lblBudget.Visibility = Visibility.Hidden;
        }

        public void displayLeagueCreator()
        {
            //SetupWindow se = new SetupWindow();
            //se.ShowDialog();

            //if (Brain.readyToStartDraft)
            //{
            //    PopulatePlayerBox(string.Empty);
            //    current.setupButton.IsEnabled = false;
            //    PlayerGenerator pe = new PlayerGenerator();
            //    pe.processPlayers();

            //    Brain.leaguePool = 200 * Brain.fantasyLeague.Count;
            //    Brain.leagueStartPool = 200 * Brain.fantasyLeague.Count;

            //    current.PlayerBox.SelectedItem = current.PlayerBox.Items.GetItemAt(0);

            //    current.upForBidButton.IsEnabled = true;
            //    current.lgButton.IsEnabled = true;
            //    current.opButton.IsEnabled = true;
            //}
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
                    l.Height = (current.teamPanel.Height - 86) / totalRosterSpots;

                    l.FontSize = fontSize;
                        
                    l.Foreground = Brushes.White;
                    l.FontFamily = new FontFamily("Moire Light");
                    l.VerticalAlignment = VerticalAlignment.Center;
                    l.VerticalContentAlignment = VerticalAlignment.Center;
                    l.Content = position + " - ";
                    l.Foreground = Brushes.White;
                    l.BorderBrush = Brushes.Gray;
                    l.BorderThickness = new Thickness(1,1,1,0);

                    current.teamPanel.Children.Add(l);
                }

                count++;
            }

            Label proj = new Label();
            proj.Name = "projTotal";
            proj.Height = (current.teamPanel.Height - 86) / totalRosterSpots;
            proj.FontSize = fontSize;
            proj.Foreground = Brushes.White;
            proj.FontFamily = new FontFamily("Moire Light");
            proj.VerticalAlignment = VerticalAlignment.Center;
            proj.VerticalContentAlignment = VerticalAlignment.Center;
            proj.Content = "Proj. Total - ";
            proj.BorderThickness = new Thickness(1,1,1,1);
            proj.BorderBrush = Brushes.Gray;

            current.teamPanel.Children.Add(proj);
            current.teamPanel.Children.Add(GUI_Creator.atlas_Create(86, current.teamPanel.Width));

            foreach(Player play in Brain.MyDraft.MyTeam.Roster)
            {
                UpdateTeamBox(play);
            }
        }

        public void UpdateTeamBox(Player player)
        {
            StackPanel panelTeam = current.teamPanel;

            string playerPosition = player.Position;

            IEnumerable<Label> correspondingLabels = panelTeam.Children.OfType<Label>().Where(label => label.Content.ToString().StartsWith(playerPosition));
            string first = correspondingLabels.First().Content.ToString();
            
            Label nextOpenSlot = correspondingLabels.FirstOrDefault(lab => lab.Content.ToString().Split('-')[1].Length == 1);

            if (nextOpenSlot != null)
            {
                nextOpenSlot.Content += player.toTeam();

                Label projectedTotalLabel = panelTeam.Children.OfType<Label>().First(label => label.Name.Equals("projTotal"));
                projectedTotalLabel.Content = "Proj. Total - " + Brain.MyDraft.MyTeam.ProjectedTotal + " pts";
            }
            else
            {
                IEnumerable<Label> benchSlots = panelTeam.Children.OfType<Label>().Where(label => label.Content.ToString().StartsWith("BENCH"));
                nextOpenSlot = benchSlots.FirstOrDefault(lab => lab.Content.ToString().Split('-')[1].Length == 1);

                if (nextOpenSlot != null)
                {
                    nextOpenSlot.Content += player.toTeam();
                }
            }
        }

        public void UpdateBudgetBox()
        {
            current.budgetBox.Content = "$" + Brain.MyDraft.MyTeam.MoneyLeft;
        }
    }
}
