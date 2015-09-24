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
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using FantasyFootballApp.Framework;

namespace FantasyFootball.cs
{
    public static class Brain
    {
        public static MainWindow current;

        public static List<Team> teams = new List<Team>();
        public static List<Player> lastFivePlayersDrafted = new List<Player>();

        public static double widthOfScreen;
        public static double heightOfScreen;

        public static string pathToSelf = @"C:\Users\mschuee\Documents\Visual Studio 2013\Projects\FantasyFootballApp\FantasyFootballApp\Files\";
        public static string pathToAtlas = @"C:\Users\mschuee\Documents\Visual Studio 2013\Projects\Atlas\Atlas\bin\Debug\Atlas.exe";
        public static string data = string.Empty;
        public static string ip = "172.20.0.237";
        public static int port = 5004;

        public static bool other_value_entered = true;
        public static bool dollarValueEntered = true;
        public static bool draft_cancelled = true;
        public static bool readyToStartDraft = false;
        public static bool custom = true;

        public static Brush foregroundTheme = Brushes.DarkGreen;
        public static Brush backgroundTheme = Brushes.Black;

        public static UIElementCollection Controls{ get { return current.root.Children; } }

        public static Draft MyDraft;

        public static void WakeUp()
        {
            IEnumerable<string> setup = File.ReadAllLines(pathToSelf + "Load.txt");
            InitializeFantasyLeague(setup);
            InitializeFootballPlayers();
        }

        private static void InitializeFantasyLeague(IEnumerable<string> lines)
        {
            MyDraft = new Draft();
            MyDraft.LeagueSalarayCap = Convert.ToInt32(lines.Last());

            CreateMyTeam(lines);

            foreach (string opponentName in lines.Where(line => line.StartsWith("-")))
            {
                FantasyTeam team = new FantasyTeam(opponentName.Substring(1));
                MyDraft.League.Add(team);
                MyDraft.TeamsInLeague++;
            }

            MyDraft.LeaguePool = MyDraft.LeagueStartPool = MyDraft.LeagueSalarayCap * MyDraft.TeamsInLeague;
        }

        private static void CreateMyTeam(IEnumerable<string> lines)
        {
            FantasyTeam mine = new FantasyTeam("Me");

            Dictionary<string, int> BudgetPercentages = new Dictionary<string, int>();
            Dictionary<string, int> PositionCounts = new Dictionary<string, int>();

            foreach(string str in lines.Where(line => line.Contains("_")))
            {
                string[] splitter = str.Split('_');
                string position = splitter[0];
                int percentage = Convert.ToInt16(splitter[1]);
                int count = Convert.ToInt16(splitter[2]);

                BudgetPercentages.Add(position, percentage);
                PositionCounts.Add(position, count);
            }

            mine.PercentageBudgetsByPosition = BudgetPercentages;

            MyDraft.RosterSpotsByPosition = PositionCounts;
            MyDraft.League.Add(mine);
            MyDraft.TeamsInLeague++;
            MyDraft.MyTeam = mine;
        }

        private static int findBudget(string s)
        {
            return Convert.ToInt32(s.Split(' ')[2]);
        }

        private static int getCount(string s)
        {
             return Convert.ToInt32(s.Split(' ')[3]);
        }

        private static void InitializeFootballPlayers()
        {
            string pathToPlayers = Brain.pathToSelf + "stats.csv";

            foreach (string pos in MyDraft.GetPositions())
            {
                MyDraft.PlayersByPosition.Add(pos, new List<Player>());
            }

            string[] lines = File.ReadAllLines(pathToPlayers);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                string[] splitter = line.Split(',');
                string first = splitter[1].Split(' ')[0];
                string second = splitter[1].Split(' ')[1];
                string position = splitter[14];
                string team = splitter[15];

                int points = (int)Convert.ToDouble(splitter[13]);
                int dollars = (int)Convert.ToInt16(splitter[16]);

                Player noob = new Player(first, second, team, position, dollars, points);

                MyDraft.PlayersByPosition[position].Add(noob);
                MyDraft.Players.Add(noob);
            }
        }

        public static void SendMessageTCP(string mess)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(ip), port);

                NetworkStream stream = client.GetStream();

                byte[] message = Encoding.ASCII.GetBytes(mess);
                stream.Write(message, 0, message.Length);

                stream.Close();
                client.Close();
            }
            catch(Exception trouble)
            {
                MessageBox.Show(trouble.Message);
            }
        }
    }
}
