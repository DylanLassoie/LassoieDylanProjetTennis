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
            DataContext = _originalStadium;

            NameOfStadiumTextBox.Text = _originalStadium.NameOfStadium;
            LocationTextBox.Text = _originalStadium.Location;
            NbCourtsTextBox.Text = _originalStadium.NbCourts.ToString();
        }

        private void NbCourtsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string location = LocationTextBox.Text;
            string nbCourtsText = NbCourtsTextBox.Text;

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(nbCourtsText))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(nbCourtsText, out int nbCourts))
            {
                MessageBox.Show("Number of courts must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _originalStadium.Location = location;
            _originalStadium.NbCourts = nbCourts;

            this.DialogResult = true;
            this.Close();
        }
    }
}
