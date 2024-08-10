using LassoieDylanProjetTennis.Ressources.Backend;
using LassoieDylanProjetTennis.Ressources.DAO;
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
    public partial class RefereesView : Page
    {
        private readonly RefereeDAO _refereeDao;
        private ObservableCollection<Referee> _referees;
        public RefereesView()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LassoieDylan"].ConnectionString;
            

            LoadReferees();
        }

        private void LoadReferees()
        {
           
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement logic to add a new referee
            // For example, show a dialog to get referee details, then:
            // Referee newReferee = ...; // get from dialog
            // _refereeDao.Add(newReferee);
            // _referees.Add(newReferee);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (TournamentsDataGrid.SelectedItem is Referee selectedReferee)
            {
                // Implement logic to edit the selected referee
                // For example, show a dialog with current details pre-filled, then:
                // selectedReferee.League = ...; // get updated value from dialog
                // _refereeDao.Update(selectedReferee);
                // Refresh the DataGrid to reflect changes
                LoadReferees();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
