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
        private Stadium _originalStadium;

        public EditStadiumWindow(Stadium stadium)
        {
            InitializeComponent();

            _originalStadium = stadium;
            DataContext = _originalStadium; // Assurez-vous que le DataContext est bien assigné

            // Initialiser les champs avec les valeurs actuelles
            NameOfStadiumTextBox.Text = _originalStadium.NameOfStadium;
            LocationTextBox.Text = _originalStadium.Location;
            NbCourtsTextBox.Text = _originalStadium.NbCourts.ToString();
        }

        private void NbCourtsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input
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

            // Update the stadium details
            _originalStadium.Location = location;
            _originalStadium.NbCourts = nbCourts;

            // Set the DialogResult to true and close the dialog
            this.DialogResult = true;
            this.Close();
        }
    }
}
