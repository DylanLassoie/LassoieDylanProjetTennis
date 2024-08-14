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
    /// Logique d'interaction pour RefereesView.xaml
    /// </summary>
    public partial class RefereeView : Page
    {
        private AbstractDAOFactory daoFactory;
        private readonly RefereeDAO _refereeDao;
        private ObservableCollection<Referee> _referees;
        public RefereeView()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LassoieDylan"].ConnectionString;

            daoFactory = AbstractDAOFactory.GetFactory(DAOFactoryType.MS_SQL_FACTORY);
            _referees = new ObservableCollection<Referee>();
            LoadReferee();
        }

        private void LoadReferee()
        {
            var refereeDAO = daoFactory.GetRefereeDAO();

            // Fetch all referees from the database
            var refereesFromDb = refereeDAO.GetAll();

            // Clear the ObservableCollection first if it has old data
            _referees.Clear();

            // Add the referees to the ObservableCollection
            foreach (var referee in refereesFromDb)
            {
                _referees.Add(referee);
            }

            // Bind the ObservableCollection to the DataGrid
            RefereeDataGrid.ItemsSource = _referees;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the AddRefereeDialog
            AddRefereeWindow dialog = new AddRefereeWindow();
            if (dialog.ShowDialog() == true)
            {
                // Retrieve the new Referee object from the dialog's Tag property
                Referee newReferee = dialog.Tag as Referee;

                if (newReferee != null)
                {
                    // Add the new referee to the database
                    try
                    {
                        var refereeDAO = daoFactory.GetRefereeDAO();
                        bool success = refereeDAO.Create(newReferee);

                        if (success)
                        {
                            // Add the new referee to the ObservableCollection
                            _referees.Add(newReferee);
                            // No need to refresh the DataGrid as ObservableCollection handles it automatically
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the new referee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                // Find the Referee object associated with this row
                Referee refereeToEdit = editButton.DataContext as Referee;
                if (refereeToEdit != null)
                {
                    // Open the EditRefereeWindow with the selected referee
                    EditRefereeWindow dialog = new EditRefereeWindow(refereeToEdit);
                    if (dialog.ShowDialog() == true)
                    {
                        // After the dialog is closed, update the referee in the database
                        try
                        {
                            var refereeDAO = daoFactory.GetRefereeDAO();
                            bool success = refereeDAO.Update(refereeToEdit);

                            if (success)
                            {
                                // Refresh the DataGrid to show the updated data
                                RefereeDataGrid.Items.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update the referee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                // Find the Referee object associated with this row
                Referee refereeToDelete = deleteButton.DataContext as Referee;
                if (refereeToDelete != null)
                {
                    // Confirm the deletion with the user
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete referee {refereeToDelete.FirstName} {refereeToDelete.LastName}?",
                                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Use the DAO to delete the referee from the database
                            var refereeDAO = daoFactory.GetRefereeDAO();
                            bool success = refereeDAO.Delete(refereeToDelete);

                            if (success)
                            {
                                // Remove the referee from the ObservableCollection
                                _referees.Remove(refereeToDelete);
                                // The DataGrid will update automatically since it's bound to the ObservableCollection
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the referee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
