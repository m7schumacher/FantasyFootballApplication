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
using System.Reflection;

namespace FantasyFootball.cs.View
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Maximized;

            Brain.widthOfScreen = SystemParameters.PrimaryScreenWidth;
            Brain.heightOfScreen = SystemParameters.PrimaryScreenWidth;

            if(GUI_Animations.menuGenerated == false)
            {
                createOptions();
                createInfo();
                createOperations();

                GUI_Animations.menuGenerated = true;
            }
        }

        public static int finished = 0;
        public static int[] SetupProgress = { 0, 0, 0, 0, 0 };

        public static StackPanel operations;
        public static StackPanel options;
        public static Label info;

        public void returnToBase(string buttonToDisable)
        {
            operations.Children.Clear();
            operations.Children.Add(info);

            foreach (Control c in options.Children)
            {
                GUI_Animations.Fade_In_Control(c, 700);
            }

            Button b = options.Children.OfType<Button>().First(bu => bu.Content.Equals(buttonToDisable));
            b.IsEnabled = true;
            b.Foreground = Brushes.Yellow;
            b.BorderBrush = Brushes.Yellow;

            if(!SetupProgress.Any(bo => bo == 0))
            {
                operations.Children.Clear();

                Button draft = GUI_Creator.Button_Create("DRAFT");
                draft.Foreground = Brushes.Yellow;
                draft.BorderBrush = Brushes.Yellow;
                draft.Margin = new Thickness(2);   

                draft.Click += draft_Click;

                operations.Children.Add(draft);
                GUI_Animations.Fade_In_Control(draft, 600);
            }
        }

        void exitMenu()
        {
            operations.Children.Clear();
            options.Children.Clear();
        }

        void draft_Click(object sender, RoutedEventArgs e)
        {
            //GUI_Changer.DraftState();
        }

        public void initializeMenuState()
        {
            GUI_Animations.Fade_All_Controls();

            Button teams = GUI_Creator.Button_Create("League");
            teams.MouseEnter += teams_MouseEnter;
            teams.MouseLeave += teams_MouseLeave;
            teams.Click += teams_Click;

            Button roster = GUI_Creator.Button_Create("Rosters");
            roster.MouseEnter += roster_MouseEnter;
            roster.MouseLeave += roster_MouseLeave;
            roster.Click += roster_Click;

            Button scoring = GUI_Creator.Button_Create("Scoring");
            scoring.MouseEnter += scoring_MouseEnter;
            scoring.MouseLeave += scoring_MouseLeave;
            scoring.Click += scoring_Click;

            Button players = GUI_Creator.Button_Create("Player Stats");
            players.MouseEnter += player_MouseEnter;
            players.MouseLeave += player_MouseLeave;
            players.Click += players_Click;

            Button budgets = GUI_Creator.Button_Create("Budgets");
            budgets.MouseEnter += budget_MouseEnter;
            budgets.MouseLeave += budget_MouseLeave;
            budgets.Click += budgets_Click;

            Button load = GUI_Creator.Button_Create("Load Existing");
            load.MouseEnter += load_MouseEnter;
            load.MouseLeave += load_MouseLeave;
            load.Click += load_Click;

            options.Children.Add(teams);
            options.Children.Add(roster);
            options.Children.Add(scoring);
            options.Children.Add(players);
            options.Children.Add(budgets);
            options.Children.Add(load);

            operations.Children.Add(info);

            //current.root.Children.Add(options);
            //current.root.Children.Add(operations);

            GUI_Animations.Fade_In_Control(teams, 1500);
            GUI_Animations.Fade_In_Control(roster, 1500);
            GUI_Animations.Fade_In_Control(scoring, 1500);
            GUI_Animations.Fade_In_Control(players, 1500);
            GUI_Animations.Fade_In_Control(budgets, 1500);
            GUI_Animations.Fade_In_Control(info, 1500);
            GUI_Animations.Fade_In_Control(load, 1500);
        }

        void load_Click(object sender, RoutedEventArgs e)
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            bool loaded = false;

            IEnumerable<string> names = assem.GetManifestResourceNames();

            if (names.Any(a => a.Contains("Load.txt")))
            {
                var result = MessageBox.Show("A custom setup exists, use it?", "", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    //Brain.loadCustomSetup();
                    loaded = true;
                }
            }

            if(loaded)
            {
                exitMenu();
                //GUI_Changer.DraftState();
            }
        }

        void budgets_Click(object sender, RoutedEventArgs e)
        {
            if(SetupProgress[0] == 1)
            {
                fadeOutButtons(sender);
                //GUI_Changer.BudgetState();
            }
            else
            {
                MessageBox.Show("Please create league members and set cap first");
            }      
        }

        void players_Click(object sender, RoutedEventArgs e)
        {
            fadeOutButtons(sender);
            //GUI_Changer.StatsState();
        }

        void scoring_Click(object sender, RoutedEventArgs e)
        {
            fadeOutButtons(sender);
           // GUI_Changer.ScoringState();
        }

        void teams_Click(object sender, RoutedEventArgs e)
        {
            fadeOutButtons(sender);
            //GUI_Changer.CreateLeague();
        }

        void roster_Click(object sender, RoutedEventArgs e)
        {
            fadeOutButtons(sender);
            //GUI_Changer.RosterState();
        }

        public void fadeOutButtons(object sender)
        {
            Button origin = (Button)sender;
            origin.IsEnabled = false;

            IEnumerable<Button> others = options.Children.OfType<Button>().Where(t => !t.Equals(origin));

            foreach (Button l in others)
            {
                GUI_Animations.Fade_Out_Control(l, 500);
            }
        }

        private void createOptions()
        {
            options = new StackPanel();

            options.Width = 400;
            options.Height = 600;
            options.HorizontalAlignment = HorizontalAlignment.Left;
            options.VerticalAlignment = VerticalAlignment.Center;
        }

        private void createOperations()
        {
            operations = new StackPanel();

            operations.Width = 800;
            operations.Height = 600;
            operations.HorizontalAlignment = HorizontalAlignment.Right;
            operations.VerticalAlignment = VerticalAlignment.Center;
            operations.Name = "operations";
        }

        private void createInfo()
        {
            FontFamily fam = new FontFamily("Yu Gothic Light");

            info = new Label();
            info.FontFamily = fam;
            info.FontSize = 35;
            info.Foreground = Brushes.Black;
            info.Background = Brushes.Transparent;
            info.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        private void clearInfo(object sender)
        {
            Button send = (Button)sender;

            if (send.IsEnabled)
            {
                info.Content = "";
            }
        }

        void roster_MouseLeave(object sender, MouseEventArgs e) { clearInfo(sender); }
        void roster_MouseEnter(object sender, MouseEventArgs e) { info.Content = "Select Roster Format"; }

        void scoring_MouseLeave(object sender, MouseEventArgs e) { clearInfo(sender); }
        void scoring_MouseEnter(object sender, MouseEventArgs e) { info.Content = "Select Scoring Format"; }

        void player_MouseLeave(object sender, MouseEventArgs e) { clearInfo(sender); }
        void player_MouseEnter(object sender, MouseEventArgs e) { info.Content = "Customize Player Statistics"; }

        void budget_MouseLeave(object sender, MouseEventArgs e) { clearInfo(sender); }
        void budget_MouseEnter(object sender, MouseEventArgs e) { info.Content = "Allocate Your Budgets"; }

        void load_MouseLeave(object sender, MouseEventArgs e) { clearInfo(sender); }
        void load_MouseEnter(object sender, MouseEventArgs e) { info.Content = "Load an Existing League Setup"; }

        void teams_MouseLeave(object sender, MouseEventArgs e) { clearInfo(sender); }
        void teams_MouseEnter(object sender, MouseEventArgs e) { info.Content = "Select the Number of Team in Your League"; }
    }
}
