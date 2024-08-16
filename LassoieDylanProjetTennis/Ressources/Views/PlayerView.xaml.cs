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
    /// Logique d'interaction pour PlayersView.xaml
    /// </summary>
    public partial class PlayerView : Page
    {
        private AbstractDAOFactory daoFactory;
        private readonly PlayerDAO _playerDao;
        private ObservableCollection<Player> _players;
        public PlayerView()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LassoieDylan"].ConnectionString;

            daoFactory = AbstractDAOFactory.GetFactory(DAOFactoryType.MS_SQL_FACTORY);
            _players = new ObservableCollection<Player>();
            LoadPlayer();
        }

        private void LoadPlayer()
        {
            var playerDAO = daoFactory.GetPlayerDAO();
            var playersFromDb = playerDAO.GetAll();

            _players.Clear();

            foreach (var player in playersFromDb)
            {
                _players.Add(player);
            }

            PlayerDataGrid.ItemsSource = _players;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
           AddPlayerWindow dialog = new AddPlayerWindow();
            if (dialog.ShowDialog() == true)
            {
                Player newPlayer = dialog.Tag as Player;

                if (newPlayer != null)
                {
                    try
                    {
                        var playerDAO = daoFactory.GetPlayerDAO();
                        bool success = playerDAO.Create(newPlayer);

                        if (success)
                        {
                            _players.Add(newPlayer);
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the new player.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            if (editButton != null)
            {
                Player playerToEdit = editButton.DataContext as Player;
                if (playerToEdit != null)
                {
                    EditPlayerWindow dialog = new EditPlayerWindow(playerToEdit);
                    if (dialog.ShowDialog() == true)
                    {
                        try
                        {
                            var playerDAO = daoFactory.GetPlayerDAO();
                            bool success = playerDAO.Update(playerToEdit);

                            if (success)
                            {
                                PlayerDataGrid.Items.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update the player.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                Player playerToDelete = deleteButton.DataContext as Player;
                if (playerToDelete != null)
                {
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete player {playerToDelete.FirstName} {playerToDelete.LastName}?",
                                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            var playerDAO = daoFactory.GetPlayerDAO();
                            bool success = playerDAO.Delete(playerToDelete);

                            if (success)
                            {
                                _players.Remove(playerToDelete);
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the player.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
