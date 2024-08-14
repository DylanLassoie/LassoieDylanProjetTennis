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
    /// Logique d'interaction pour StadiumView.xaml
    /// </summary>
    public partial class StadiumView : Page
    {
        private AbstractDAOFactory daoFactory;
        private readonly StadiumDAO _stadiumDao;
        private ObservableCollection<Stadium> _stadiums;

        public StadiumView()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LassoieDylan"].ConnectionString;

            daoFactory = AbstractDAOFactory.GetFactory(DAOFactoryType.MS_SQL_FACTORY);
            _stadiums = new ObservableCollection<Stadium>();
            LoadStadium();
        }

        private void LoadStadium()
        {
            var stadiumDAO = daoFactory.GetStadiumDAO();

            // Fetch all stadiums from the database
            var stadiumsFromDb = stadiumDAO.GetAll();

            // Clear the ObservableCollection first if it has old data
            _stadiums.Clear();

            // Add the stadiums to the ObservableCollection
            foreach (var stadium in stadiumsFromDb)
            {
                _stadiums.Add(stadium);
            }

            // Bind the ObservableCollection to the DataGrid
            StadiumDataGrid.ItemsSource = _stadiums;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de dialogue pour ajouter un stade
            AddStadiumWindow dialog = new AddStadiumWindow();
            if (dialog.ShowDialog() == true)
            {
                // Récupérer le nouvel objet Stadium depuis la propriété Tag de la fenêtre de dialogue
                Stadium newStadium = dialog.Tag as Stadium;

                if (newStadium != null)
                {
                    // Ajouter le nouveau stade dans la base de données
                    try
                    {
                        var stadiumDAO = daoFactory.GetStadiumDAO();
                        bool success = stadiumDAO.Create(newStadium);

                        if (success)
                        {
                            // Ajouter le nouveau stade à l'ObservableCollection
                            _stadiums.Add(newStadium);
                            // Pas besoin de rafraîchir le DataGrid car ObservableCollection gère cela automatiquement
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the new stadium.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            // Récupérer la ligne du DataGrid contenant le bouton de modification cliqué
            Button editButton = sender as Button;
            if (editButton != null)
            {
                // Trouver l'objet Stadium associé à cette ligne
                Stadium stadiumToEdit = editButton.DataContext as Stadium;
                if (stadiumToEdit != null)
                {
                    // Ouvrir la fenêtre EditStadiumWindow avec le stade sélectionné
                    EditStadiumWindow dialog = new EditStadiumWindow(stadiumToEdit);
                    if (dialog.ShowDialog() == true)
                    {
                        // Après la fermeture de la fenêtre de dialogue, mettre à jour le stade dans la base de données
                        try
                        {
                            var stadiumDAO = daoFactory.GetStadiumDAO();
                            bool success = stadiumDAO.Update(stadiumToEdit);

                            if (success)
                            {
                                // Rafraîchir le DataGrid pour afficher les données mises à jour
                                StadiumDataGrid.Items.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update the stadium.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            // Récupérer la ligne du DataGrid contenant le bouton de suppression cliqué
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                // Trouver l'objet Stadium associé à cette ligne
                Stadium stadiumToDelete = deleteButton.DataContext as Stadium;
                if (stadiumToDelete != null)
                {
                    // Confirmer la suppression avec l'utilisateur
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete stadium {stadiumToDelete.NameOfStadium} located in {stadiumToDelete.Location}?",
                                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Utiliser le DAO pour supprimer le stade de la base de données
                            var stadiumDAO = daoFactory.GetStadiumDAO();
                            bool success = stadiumDAO.Delete(stadiumToDelete);

                            if (success)
                            {
                                // Supprimer le stade de l'ObservableCollection
                                _stadiums.Remove(stadiumToDelete);
                                // Le DataGrid se mettra à jour automatiquement car il est lié à l'ObservableCollection
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the stadium.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
