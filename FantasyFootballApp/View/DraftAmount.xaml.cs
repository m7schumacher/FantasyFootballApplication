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
    /// Interaction logic for DraftAmount.xaml
    /// </summary>
    public partial class DraftAmount : Window
    {
        public DraftAmount()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int dollarAmount = Convert.ToInt32(amountTextBox.Text);
                string playerPosition = Brain.MyDraft.CurrentPlayerUpForAuction.Position;

                int dollarBudgetForPosition = Brain.MyDraft.MyTeam.GetBudgetDollarForPosition(playerPosition);
                
                if(dollarBudgetForPosition < dollarAmount)
                {
                    var outputBox = MessageBox.Show("Will Go Over Budget... Are you sure?", "", MessageBoxButton.OKCancel);

                    if (outputBox == MessageBoxResult.Cancel)                
                    {
                       return;
                    }
                }

                Brain.MyDraft.CurrentPlayerUpForAuction.Dollar = dollarAmount;
                Brain.dollarValueEntered = true;

                this.Close();
            }
            catch(Exception trouble)
            {
                MessageBox.Show("Invalid price");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
