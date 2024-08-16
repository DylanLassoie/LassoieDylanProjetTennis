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
            var tournamentDAO = daoFactory.GetTournamentDAO();
            var tournamentsFromDb = tournamentDAO.GetAll();

            _tournaments.Clear();

            foreach (var tournament in tournamentsFromDb)
            {
                _tournaments.Add(tournament);
            }

            TournamentDataGrid.ItemsSource = _tournaments;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTournamentWindow dialog = new AddTournamentWindow();
            if (dialog.ShowDialog() == true)
            {
                Tournament newTournament = dialog.Tag as Tournament;

                if (newTournament != null)
                {
                    try
                    {
                        var tournamentDAO = daoFactory.GetTournamentDAO();
                        bool success = tournamentDAO.Create(newTournament);

                        if (success)
                        {
                            _tournaments.Add(newTournament);
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
            Button playButton = sender as Button;
            if (playButton != null)
            {
                Tournament selectedTournament = playButton.DataContext as Tournament;
                if (selectedTournament != null)
                {
                    PlayTournamentWindow playWindow = new PlayTournamentWindow(selectedTournament.Name);
                    playWindow.ShowDialog();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                Tournament tournamentToDelete = deleteButton.DataContext as Tournament;
                if (tournamentToDelete != null)
                {
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the tournament {tournamentToDelete.Name}?",
                                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            var tournamentDAO = daoFactory.GetTournamentDAO();
                            bool success = tournamentDAO.Delete(tournamentToDelete);

                            if (success)
                            {
                                _tournaments.Remove(tournamentToDelete);
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
