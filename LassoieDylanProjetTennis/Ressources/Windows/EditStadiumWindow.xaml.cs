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
    /// Logique d'interaction pour EditStadiumWindow.xaml
    /// </summary>
    public partial class EditStadiumWindow : Window
    {
        public EditStadiumWindow(Stadium stadium)
        {
            InitializeComponent();

            // Populate the fields with the existing stadium details
            NameOfStadiumTextBox.Text = stadium.NameOfStadium;
            LocationTextBox.Text = stadium.Location;
            NbCourtsTextBox.Text = stadium.NbCourts.ToString();

            // Store the original stadium object in the Tag for later retrieval
            this.Tag = stadium;
        }

        private void NbCourtsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input
            string nameOfStadium = NameOfStadiumTextBox.Text;
            string location = LocationTextBox.Text;
            string nbCourtsText = NbCourtsTextBox.Text;

            // Basic validation (ensure all fields are filled)
            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(nbCourtsText))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ensure the number of courts is a valid integer
            if (!int.TryParse(nbCourtsText, out int nbCourts))
            {
                MessageBox.Show("Number of courts must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get the original stadium object
            Stadium originalStadium = this.Tag as Stadium;

            // Update the stadium details
            originalStadium.Location = location;
            originalStadium.NbCourts = nbCourts;

            // Set the DialogResult to true and close the dialog
            this.DialogResult = true;
            this.Close();
        }
    }
}
