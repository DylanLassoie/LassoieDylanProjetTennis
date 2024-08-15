using LassoieDylanProjetTennis.Ressources.Backend;
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

namespace LassoieDylanProjetTennis.Ressources.Windows
{
    /// <summary>
    /// Logique d'interaction pour AddTournamentWindow.xaml
    /// </summary>
    public partial class AddTournamentWindow : Window
    {
        public AddTournamentWindow()
        {
            InitializeComponent();
        }

        private void CreateCourtButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPlayersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddRefereesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input
            string tournamentName = TournamentNameTextBox.Text;
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;
            var selectedStadium = StadiumComboBox.SelectedItem as Stadium; // Assuming Stadium is the type

            // Basic validation (ensure all fields are filled)
            if (string.IsNullOrEmpty(tournamentName) || !startDate.HasValue || !endDate.HasValue || selectedStadium == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (endDate < startDate)
            {
                MessageBox.Show("End date cannot be before the start date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new Tournament object
            Tournament newTournament = new Tournament
            {
                Name = tournamentName,
                StartingDate = startDate.Value,
                EndingDate = endDate.Value,
                // Assuming there’s a Stadium property in the Tournament class
                // to hold the selected stadium
            };

            // Set the DialogResult to true and close the dialog
            this.DialogResult = true;

            // Store the new Tournament object in the Tag property for retrieval
            this.Tag = newTournament;

            this.Close();
        }
    }
}
