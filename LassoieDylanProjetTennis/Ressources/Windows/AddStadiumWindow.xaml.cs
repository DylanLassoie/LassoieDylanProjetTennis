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
    /// Logique d'interaction pour AddStadiumWindow.xaml
    /// </summary>
    public partial class AddStadiumWindow : Window
    {
        public AddStadiumWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les données saisies par l'utilisateur
            string nameOfStadium = NameOfStadiumTextBox.Text;
            string location = LocationTextBox.Text;
            string nbCourtsText = NbCourtsTextBox.Text;

            // Validation de base pour s'assurer que tous les champs sont remplis
            if (string.IsNullOrEmpty(nameOfStadium) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(nbCourtsText))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // S'assurer que le nombre de courts est un entier valide
            if (!int.TryParse(nbCourtsText, out int nbCourts))
            {
                MessageBox.Show("Number of courts must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Créer un nouvel objet Stadium
            Stadium newStadium = new Stadium
            {
                NameOfStadium = nameOfStadium,
                Location = location,
                NbCourts = nbCourts
            };

            // Définir le DialogResult sur true et fermer la fenêtre
            this.DialogResult = true;

            // Stocker le nouvel objet Stadium dans la propriété Tag pour le récupérer ultérieurement
            this.Tag = newStadium;

            this.Close();
        }


        private void RankTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !int.TryParse(e.Text, out _);
        }
    }
}
