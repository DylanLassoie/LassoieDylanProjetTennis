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
            var stadiumsFromDb = stadiumDAO.GetAll();

            _stadiums.Clear();

            foreach (var stadium in stadiumsFromDb)
            {
                _stadiums.Add(stadium);
            }

            StadiumDataGrid.ItemsSource = _stadiums;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddStadiumWindow dialog = new AddStadiumWindow();
            if (dialog.ShowDialog() == true)
            {
                Stadium newStadium = dialog.Tag as Stadium;

                if (newStadium != null)
                {
                    try
                    {
                        var stadiumDAO = daoFactory.GetStadiumDAO();
                        bool success = stadiumDAO.Create(newStadium);

                        if (success)
                        {

                            _stadiums.Add(newStadium);
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
            Button editButton = sender as Button;
            if (editButton != null)
            {

                Stadium stadiumToEdit = editButton.DataContext as Stadium;
                if (stadiumToEdit != null)
                {
                    EditStadiumWindow dialog = new EditStadiumWindow(stadiumToEdit);
                    if (dialog.ShowDialog() == true)
                    {
                        try
                        {
                            var stadiumDAO = daoFactory.GetStadiumDAO();
                            bool success = stadiumDAO.Update(stadiumToEdit);

                            if (success)
                            {
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
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                Stadium stadiumToDelete = deleteButton.DataContext as Stadium;
                if (stadiumToDelete != null)
                {
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete stadium {stadiumToDelete.NameOfStadium} located in {stadiumToDelete.Location}?",
                                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            var stadiumDAO = daoFactory.GetStadiumDAO();
                            bool success = stadiumDAO.Delete(stadiumToDelete);

                            if (success)
                            {
                                _stadiums.Remove(stadiumToDelete);
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
