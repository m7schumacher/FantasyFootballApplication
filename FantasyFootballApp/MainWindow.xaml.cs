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
using System.Reflection;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using FantasyFootballApp.Framework;
using System.Windows.Controls.DataVisualization.Charting;
using System.Runtime.Serialization.Formatters.Binary;

namespace FantasyFootball.cs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer clock = new DispatcherTimer();
        public ViewEditor view;
        public Drafter drafter;
        public Ticker tick;
        public DraftInitializer starter;

        int hour = 0;
        int minute = 0;
        int second = 0;

        public MainWindow()
        {
            InitializeComponent();

            Brain.WakeUp();

            var result1 = MessageBox.Show("Load existing draft?",
                "Important Question",
                MessageBoxButton.YesNo);

            if(result1 == MessageBoxResult.Yes)
            {
                Draft copy;

                string path = Brain.pathToSelf + "Past.bin";
                try
                {
                    FileStream readerFileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    BinaryFormatter formatter = new BinaryFormatter();

                    copy = (Draft)formatter.Deserialize(readerFileStream);

                    readerFileStream.Close();

                    Brain.MyDraft = copy;
                }
                catch(Exception e)
                {
                    MessageBox.Show("No past draft found");
                }
            }


            view = new ViewEditor(this);

            InitializeScreen();
            InitializeControls();
        }

        void InitializeScreen()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 30;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;

            Brain.widthOfScreen = this.Width;
            Brain.heightOfScreen = this.Height;

            Brain.current = this;
        }

        void InitializeControls()
        {
            clock.Interval = new TimeSpan(0, 0, 1);
            clock.Tick += clock_Tick;

            view.PopulatePlayerBox();
            view.InitializeTeamBox();

            if (PlayerBox.Items.Count > 0)
            {
                PlayerBox.SelectedItem = PlayerBox.Items.GetItemAt(0);
            }

            budgetBox.Content = "$" + Brain.MyDraft.MyTeam.MoneyLeft;
            upForPanel.Visibility = Visibility.Hidden;
            upForBidButton.IsEnabled = true;
            lgButton.IsEnabled = true;
            opButton.IsEnabled = true;
            setupButton.IsEnabled = false;

            clock.Start();
        }

        void clock_Tick(object sender, EventArgs e)
        {
            second++;

            if (second == 60)
            {
                minute++;
                second = 0;

                if (minute == 60)
                {
                    hour++;
                    minute = 0;
                }
            }

            setupButton.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(Object state)
            {
                string clock = hour + ":" + minute + ":" + second;
                setupButton.Content = clock;

                return null;

            }), null);
        }

      
        //Selected player changed
        private void PlayerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(PlayerBox.SelectedItem != null)
            {
                string[] lit = PlayerBox.SelectedItem.ToString().Split(' ');
                string name = lit[0] + " " + lit[1].Trim(',');

                Player temp = Brain.MyDraft.Players.First(t => t.Full.Equals(name));
                Brain.MyDraft.SelectedPlayer = temp;

                view.updateLabels(temp);
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string content = b.Content.ToString();

            string filter = content.Equals("C") ? "" : content;

            view.PopulatePlayerBox(filter);
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            string pos = b.Name;

            view.sortPlayerBox(pos);
        }

        //search button
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string search = searchBox.Text;

            if(search.Length > 0)
            {
                view.displaySearchResults(search);
            }
        }

        double[] scores;
        double[] dollarValues;

        private void getOptimalTotal(Player player, int index)
        {
            dollarValues[index] = player.Dollar;

            FantasyTeam mocker = OptimalWindow.RunOptimizer(player);
            scores[index] = mocker.ProjectedTotal;
        }

        private double ComputeSuggestedDollar(double[] sc, double[] d, int original)
        {
            int index = 0;

            for (int i = 0; i < sc.Length; i++)
            {
                if(sc[i] < original)
                {
                    index = (i - 1) > 0 ? i - 1 : 0;
                    break;
                }
            }

            return d[index];
        }

        //put a player up for bid
        private void PutUpForBid_Click(object sender, RoutedEventArgs e)
        {
            Player player = Brain.MyDraft.SelectedPlayer;
            Brain.MyDraft.CurrentPlayerUpForAuction = player;

            int projectedDollarValue = player.Dollar;
            double projectedTotal = 0;

            int optimalAtProjected = OptimalWindow.RunOptimizer(null).ProjectedTotal;

            List<double> projections = new List<double>();
            List<int> dollars = new List<int>();
            List<FantasyTeam> teams = new List<FantasyTeam>();

            int start = (int)Math.Floor(projectedDollarValue * .5);
            int end = (int)Math.Floor(projectedDollarValue * 1.5);

            scores = new double[end - start];
            dollarValues = new double[end - start];
            Task[] tasks = new Task[end - start];

            for (int i = 0; i < tasks.Length; i++)
            {
                Player play = player.copy();
                play.Dollar = start + i;

                int index = i;

                tasks[i] = Task.Factory.StartNew(() => getOptimalTotal(play, index));
            }

            Task.WaitAll(tasks);

            double suggestedDollar = ComputeSuggestedDollar(scores, dollarValues, optimalAtProjected);

            view.putPlayerUpForAuction(player, suggestedDollar);

            lineChart.Opacity = 1;
            List<KeyValuePair<double, double>> pairs = new List<KeyValuePair<double, double>>();

            for (int i = 0; i < scores.Length; i++)
			{
                var doll = dollarValues[i];
                var pts = scores[i];

                KeyValuePair<double, double> next = new KeyValuePair<double, double>(doll, pts);
                pairs.Add(next);
			}

            ((LineSeries)lineChart.Series[0]).ItemsSource = pairs;
        }

        //trigger (Draft) a player
        private void Trigger_Click(object sender, RoutedEventArgs e)
        {
            Player playerUpForAuction = Brain.MyDraft.CurrentPlayerUpForAuction;

            DraftAmount draftWindow = new DraftAmount();
            draftWindow.ShowDialog();

            if(Brain.dollarValueEntered)
            {
                Brain.MyDraft.MyTeam.addPlayer(playerUpForAuction);

                view.UpdateTeamBox(playerUpForAuction);
                view.PopulatePlayerBox(string.Empty);
                view.ClearLabels();
                view.TakePlayerDownForAuction();
                view.UpdateBudgetBox();

                Brain.dollarValueEntered = false;
            }
        }

        private void addToBench(Player p)
        {
            IEnumerable<Label> bench = teamPanel.Children.OfType<Label>().Where(t => t.Name.StartsWith("B"));

            int counter = 0;

            while(bench.ElementAt(counter).Content.ToString().Replace("BN -",string.Empty).Length == 0)
            {
                counter++;
            }

            bench.ElementAt(counter).Content += p.toTeam();
        }

        //pass on player
        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            view.showDraftToOtherTeam();
            //tick.UpdateTicker();
        }

        //View League Button
        private void LeagueWindow_Click(object sender, RoutedEventArgs e)
        {
            LeagueWindow le = new LeagueWindow();
            le.ShowDialog();
        }

        //Optimizer button
        private void Optimal_Click(object sender, RoutedEventArgs e)
        {
            OptimalWindow op = new OptimalWindow();
            op.ShowDialog();
        }
    
        //take down button
        private void TakeDown_Click(object sender, RoutedEventArgs e)
        {
            Brain.MyDraft.CurrentPlayerUpForAuction = null;
            view.TakePlayerDownForAuction();
        }

        //shows budgets when mouse enters the budget box
        private void budgetBox_MouseEnter(object sender, MouseEventArgs e)
        {
            view.showBudgets();
        }

        //hide budgets on mouse leave
        private void budgetBox_MouseLeave(object sender, MouseEventArgs e)
        {
            view.hideBudgets();
        }

        //opens player editor
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            PlayerEditor pe = new PlayerEditor();

            pe.ShowDialog();

            view.PopulatePlayerBox("");
        }

        public void tickerStart()
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Inner_MouseEnter(object sender, MouseEventArgs e)
        {
            //Outer.Fill = Brushes.Gray;
        }

        private void Inner_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void mockOptimalButton_Click(object sender, RoutedEventArgs e)
        {
            OptimalWindow optimal = new OptimalWindow(Brain.MyDraft.CurrentPlayerUpForAuction);
            optimal.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Draft dr = Brain.MyDraft;
            string path = Brain.pathToSelf + "Past.bin";

            using (Stream stream = File.Open(path, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, dr);
            }
        }
    }
}
