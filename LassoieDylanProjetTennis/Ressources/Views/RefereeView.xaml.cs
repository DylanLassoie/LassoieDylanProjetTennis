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
            var refereesFromDb = refereeDAO.GetAll();

            _referees.Clear();

            foreach (var referee in refereesFromDb)
            {
                _referees.Add(referee);
            }

            RefereeDataGrid.ItemsSource = _referees;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddRefereeWindow dialog = new AddRefereeWindow();
            if (dialog.ShowDialog() == true)
            {
                Referee newReferee = dialog.Tag as Referee;

                if (newReferee != null)
                {
                    try
                    {
                        var refereeDAO = daoFactory.GetRefereeDAO();
                        bool success = refereeDAO.Create(newReferee);

                        if (success)
                        {
                            _referees.Add(newReferee);
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
            Button editButton = sender as Button;
            if (editButton != null)
            {
                Referee refereeToEdit = editButton.DataContext as Referee;
                if (refereeToEdit != null)
                {
                    EditRefereeWindow dialog = new EditRefereeWindow(refereeToEdit);
                    if (dialog.ShowDialog() == true)
                    {
                        try
                        {
                            var refereeDAO = daoFactory.GetRefereeDAO();
                            bool success = refereeDAO.Update(refereeToEdit);

                            if (success)
                            {
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
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                Referee refereeToDelete = deleteButton.DataContext as Referee;
                if (refereeToDelete != null)
                {

                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete referee {refereeToDelete.FirstName} {refereeToDelete.LastName}?",
                                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            var refereeDAO = daoFactory.GetRefereeDAO();
                            bool success = refereeDAO.Delete(refereeToDelete);

                            if (success)
                            {
                                _referees.Remove(refereeToDelete);
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
