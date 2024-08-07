using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LassoieDylanProjetTennis.Ressources.Views
{
    /// <summary>
    /// Logique d'interaction pour TestTableauCRUD.xaml
    /// </summary>
    public partial class TestTableauCRUD : Page
    {
        public ObservableCollection<Tournament> Tournaments { get; set; }

        public TestTableauCRUD()
        {
            InitializeComponent();

            Tournaments = new ObservableCollection<Tournament>
            {
                new Tournament { Name = "Open d'Australie", Location = "Melbourne", StartDate = new DateTime(2024, 1, 15), EndDate = new DateTime(2024, 1, 28) },
                new Tournament { Name = "Roland Garros", Location = "Paris", StartDate = new DateTime(2024, 5, 27), EndDate = new DateTime(2024, 6, 10) },
                // Ajoutez plus de tournois ici
            };
            DataContext = this;

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (TournamentsDataGrid.SelectedItem is Tournament selectedTournament)
            {
                // Logique pour modifier le tournoi sélectionné
                MessageBox.Show($"Modifier le tournoi: {selectedTournament.Name}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TournamentsDataGrid.SelectedItem is Tournament selectedTournament)
            {
                // Logique pour supprimer le tournoi sélectionné
                if (MessageBox.Show($"Voulez-vous vraiment supprimer le tournoi: {selectedTournament.Name}?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Tournaments.Remove(selectedTournament);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour ajouter un nouveau tournoi
            MessageBox.Show("Ajouter un nouveau tournoi");
        }
    }
    public class Tournament
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
