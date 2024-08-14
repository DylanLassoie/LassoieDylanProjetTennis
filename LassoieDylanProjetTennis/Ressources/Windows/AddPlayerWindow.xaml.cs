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
    /// Logique d'interaction pour AddPlayerWindow.xaml
    /// </summary>
    public partial class AddPlayerWindow : Window
    {
        public AddPlayerWindow()
        {
            InitializeComponent();
        }
        private void RankTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string nationality = NationalityTextBox.Text;
            string genderType = (GenderTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string rankText = RankTextBox.Text;

            // Basic validation (ensure all fields are filled)
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(nationality) || string.IsNullOrEmpty(genderType) ||
                string.IsNullOrEmpty(rankText))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ensure rank is a valid integer
            if (!int.TryParse(rankText, out int rank))
            {
                MessageBox.Show("Rank must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new Player object
            Player newPlayer = new Player
            {
                FirstName = firstName,
                LastName = lastName,
                Nationality = nationality,
                GenderType = (GenderType)Enum.Parse(typeof(GenderType), genderType),
                Rank = rank
            };

            // Set the DialogResult to true and close the dialog
            this.DialogResult = true;

            // Store the new Player object in the Tag property for retrieval
            this.Tag = newPlayer;

            this.Close();
        }

    }
}
