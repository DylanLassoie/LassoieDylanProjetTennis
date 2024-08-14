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
            var playerDAO = daoFactory.GetPlayerDAO(); // Assuming you have a method to get the PlayerDAO

            // Fetch all players from the database
            var playersFromDb = playerDAO.GetAll();

            // Clear the ObservableCollection first if it has old data
            _players.Clear();

            // Add the players to the ObservableCollection
            foreach (var player in playersFromDb)
            {
                _players.Add(player);
            }

            // Bind the ObservableCollection to the DataGrid
            PlayerDataGrid.ItemsSource = _players;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the AddPlayerDialog
           AddPlayerWindow dialog = new AddPlayerWindow();
            if (dialog.ShowDialog() == true)
            {
                // Retrieve the new Player object from the dialog's Tag property
                Player newPlayer = dialog.Tag as Player;

                if (newPlayer != null)
                {
                    // Add the new player to the database
                    try
                    {
                        var playerDAO = daoFactory.GetPlayerDAO();
                        bool success = playerDAO.Create(newPlayer);

                        if (success)
                        {
                            // Add the new player to the ObservableCollection
                            _players.Add(newPlayer);
                            // No need to refresh the DataGrid as ObservableCollection handles it automatically
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
            // Get the DataGridRow that contains the clicked edit button
            Button editButton = sender as Button;
            if (editButton != null)
            {
                // Find the Player object associated with this row
                Player playerToEdit = editButton.DataContext as Player;
                if (playerToEdit != null)
                {
                    // Open the EditPlayerWindow with the selected player
                    EditPlayerWindow dialog = new EditPlayerWindow(playerToEdit);
                    if (dialog.ShowDialog() == true)
                    {
                        // After the dialog is closed, update the player in the database
                        try
                        {
                            var playerDAO = daoFactory.GetPlayerDAO();
                            bool success = playerDAO.Update(playerToEdit);

                            if (success)
                            {
                                // Refresh the DataGrid to show the updated data
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
            // Get the DataGridRow that contains the clicked delete button
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                // Find the Player object associated with this row
                Player playerToDelete = deleteButton.DataContext as Player;
                if (playerToDelete != null)
                {
                    // Confirm the deletion with the user
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete player {playerToDelete.FirstName} {playerToDelete.LastName}?",
                                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Use the DAO to delete the player from the database
                            var playerDAO = daoFactory.GetPlayerDAO();
                            bool success = playerDAO.Delete(playerToDelete);

                            if (success)
                            {
                                // Remove the player from the ObservableCollection
                                _players.Remove(playerToDelete);
                                // The DataGrid will update automatically since it's bound to the ObservableCollection
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
