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
    /// Interaction logic for AddToOtherTeam.xaml
    /// </summary>
    public partial class AddToOtherTeam : Window
    {
        public AddToOtherTeam()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void teamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FantasyTeam currentTeam = Brain.MyDraft.League.FirstOrDefault(tm => tm.Owner.Equals(leagueBox.SelectedItem));
                Player currentPlayer = Brain.MyDraft.CurrentPlayerUpForAuction;
                currentPlayer.Dollar = Convert.ToInt16(priceBox.Text);

                currentTeam.addPlayer(currentPlayer);
                this.Close();
            }
            catch(Exception trouble)
            {
                MessageBox.Show("Unable to Add Player");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Brain.draft_cancelled = true;
            this.Close();
        }

        private void leagueBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
