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
    /// Logique d'interaction pour EditRefereeWindow.xaml
    /// </summary>
    public partial class EditRefereeWindow : Window
    {
        public EditRefereeWindow(Referee referee)
        {
            InitializeComponent();

            // Populate the fields with the existing referee details
            FirstNameTextBox.Text = referee.FirstName;
            LastNameTextBox.Text = referee.LastName;
            NationalityTextBox.Text = referee.Nationality;
            GenderTypeComboBox.SelectedItem = GenderTypeComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == referee.GenderType.ToString());
            LeagueTextBox.Text = referee.League;

            // Store the original referee object in the Tag for later retrieval
            this.Tag = referee;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string nationality = NationalityTextBox.Text;
            string genderType = (GenderTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string league = LeagueTextBox.Text;

            // Basic validation (ensure all fields are filled)
            if (string.IsNullOrEmpty(nationality) || string.IsNullOrEmpty(genderType) || string.IsNullOrEmpty(league))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get the original referee object
            Referee originalReferee = this.Tag as Referee;

            // Update the referee details
            originalReferee.Nationality = nationality;
            originalReferee.GenderType = (GenderType)Enum.Parse(typeof(GenderType), genderType);
            originalReferee.League = league;

            // Set the DialogResult to true and close the dialog
            this.DialogResult = true;
            this.Close();
        }
    }
}
