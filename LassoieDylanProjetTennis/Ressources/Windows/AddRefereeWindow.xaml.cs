using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LassoieDylanProjetTennis.Ressources.Views
{
    /// <summary>
    /// Logique d'interaction pour AddRefereeWindow.xaml
    /// </summary>
    public partial class AddRefereeWindow : Window
    {

        public AddRefereeWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string nationality = NationalityTextBox.Text;
            string genderType = (GenderTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string league = LeagueTextBox.Text;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(nationality) || string.IsNullOrEmpty(genderType) ||
                string.IsNullOrEmpty(league))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Referee newReferee = new Referee
            {
                FirstName = firstName,
                LastName = lastName,
                Nationality = nationality,
                GenderType = (GenderType)Enum.Parse(typeof(GenderType), genderType),
                League = league
            };

            this.DialogResult = true;
            this.Tag = newReferee;

            this.Close();
        }
    }
}
