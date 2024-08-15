using LassoieDylanProjetTennis.Ressources.Backend;
using LassoieDylanProjetTennis.Ressources.DAO;
using LassoieDylanProjetTennis.Ressources.Factory;
using LassoieDylanProjetTennis.Ressources.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
    /// Logique d'interaction pour TournamentView.xaml
    /// </summary>
    public partial class TournamentView : Page
    {
        private AbstractDAOFactory daoFactory;
        private readonly TournamentDAO _tournamentDao;
        private ObservableCollection<Tournament> _tournaments;

        public TournamentView()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LassoieDylan"].ConnectionString;

            daoFactory = AbstractDAOFactory.GetFactory(DAOFactoryType.MS_SQL_FACTORY);
            _tournaments = new ObservableCollection<Tournament>();
            LoadTournament();
        }

        private void LoadTournament()
        {
            var tournamentDAO = daoFactory.GetTournamentDAO(); // Assuming you have a method to get the TournamentDAO

            // Fetch all tournaments from the database
            var tournamentsFromDb = tournamentDAO.GetAll();

            // Clear the ObservableCollection first if it has old data
            _tournaments.Clear();

            // Add the tournaments to the ObservableCollection
            foreach (var tournament in tournamentsFromDb)
            {
                _tournaments.Add(tournament);
            }

            // Bind the ObservableCollection to the DataGrid
            TournamentDataGrid.ItemsSource = _tournaments;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the AddTournamentDialog
            AddTournamentWindow dialog = new AddTournamentWindow();
            if (dialog.ShowDialog() == true)
            {
                // Retrieve the new Tournament object from the dialog's Tag property
                Tournament newTournament = dialog.Tag as Tournament;

                if (newTournament != null)
                {
                    // Add the new tournament to the database
                    try
                    {
                        var tournamentDAO = daoFactory.GetTournamentDAO();
                        bool success = tournamentDAO.Create(newTournament);

                        if (success)
                        {
                            // Add the new tournament to the ObservableCollection
                            _tournaments.Add(newTournament);
                            // No need to refresh the DataGrid as ObservableCollection handles it automatically
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the new tournament.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the DataGridRow that contains the clicked delete button
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                // Find the Tournament object associated with this row
                Tournament tournamentToDelete = deleteButton.DataContext as Tournament;
                if (tournamentToDelete != null)
                {
                    // Confirm the deletion with the user
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the tournament {tournamentToDelete.Name}?",
                                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Use the DAO to delete the tournament from the database
                            var tournamentDAO = daoFactory.GetTournamentDAO();
                            bool success = tournamentDAO.Delete(tournamentToDelete);

                            if (success)
                            {
                                // Remove the tournament from the ObservableCollection
                                _tournaments.Remove(tournamentToDelete);
                                // The DataGrid will update automatically since it's bound to the ObservableCollection
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the tournament.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}
