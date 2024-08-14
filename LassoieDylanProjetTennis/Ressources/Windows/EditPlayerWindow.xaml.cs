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
    /// Logique d'interaction pour EditPlayerWindow.xaml
    /// </summary>
    public partial class EditPlayerWindow : Window
    {
        public EditPlayerWindow(Player player)
        {
            InitializeComponent();

            // Populate the fields with the existing player details
            FirstNameTextBox.Text = player.FirstName;
            LastNameTextBox.Text = player.LastName;
            NationalityTextBox.Text = player.Nationality;
            GenderTypeComboBox.SelectedItem = GenderTypeComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == player.GenderType.ToString());
            RankTextBox.Text = player.Rank.ToString();

            // Store the original player object in the Tag for later retrieval
            this.Tag = player;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string nationality = NationalityTextBox.Text;
            string genderType = (GenderTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string rankText = RankTextBox.Text;

            // Basic validation (ensure all fields are filled)
            if (string.IsNullOrEmpty(nationality) || string.IsNullOrEmpty(genderType) || string.IsNullOrEmpty(rankText))
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

            // Get the original player object
            Player originalPlayer = this.Tag as Player;

            // Update the player details
            originalPlayer.Nationality = nationality;
            originalPlayer.GenderType = (GenderType)Enum.Parse(typeof(GenderType), genderType);
            originalPlayer.Rank = rank;

            // Set the DialogResult to true and close the dialog
            this.DialogResult = true;
            this.Close();
        }
    }
}
