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
    /// Interaction logic for PlayerEditor.xaml
    /// </summary>
    public partial class PlayerEditor : Window
    {
        private bool clear = false;
        private Player current = new Player();
        private List<Player> copy = new List<Player>();

        public PlayerEditor()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            foreach(Player p in Brain.MyDraft.Players)
            {
                copy.Add(p);
            }

            pBoxRefresh();
        }

        public PlayerEditor(string data)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if(!clear)
            {
                string search = srcBox.Text;

                playerBox.Items.Clear();

                foreach (Player p in Brain.MyDraft.Players.Where(p => p.Last.ToLower().StartsWith(search.ToLower())))
                {
                    playerBox.Items.Add(p.toBox());
                }

                clear = true;
                searchButton.Content = "Clear";
            }
            else
            {
                playerBox.Items.Clear();
                srcBox.Text = string.Empty;

                foreach (Player p in Brain.MyDraft.Players)
                {
                    playerBox.Items.Add(p.toBox());
                }

                clear = false;
                searchButton.Content = "Search";
            }
           
        }

        private void playerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(playerBox.SelectedItem != null)
            {
                string selected = playerBox.SelectedItem.ToString();

                string name = selected.Substring(0, selected.IndexOf(','));

                current = copy.First(s => s.Full.Equals(name));

                moneyBox.Text = current.Dollar.ToString();
                pointsBox.Text = current.Points.ToString();

                starCheck.IsChecked = current.Star;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            int money = Convert.ToInt32(moneyBox.Text);
            int points = Convert.ToInt32(pointsBox.Text);
            bool st = starCheck.IsChecked.Value;

            current.Dollar = money;
            current.Points = points;
            current.Star = st;

            refresh();
        }

        private void pBoxRefresh()
        {
            playerBox.Items.Clear();

            foreach (Player p in copy)
            {
                playerBox.Items.Add(p.toBox());
            }
        }

        private void refresh()
        {
            current = new Player();
            moneyBox.Text = string.Empty;
            pointsBox.Text = string.Empty;

            starCheck.IsChecked = false;
            srcBox.Text = string.Empty;
            searchButton.Content = "Search";
            clear = false;

            pBoxRefresh();
        }

        private void writeToFile()
        {

        }

        private void leaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
    }
}
