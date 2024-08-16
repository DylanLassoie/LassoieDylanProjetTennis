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

namespace LassoieDylanProjetTennis.Ressources.Windows
{
    /// <summary>
    /// Logique d'interaction pour EditRefereeWindow.xaml
    /// </summary>
    public partial class EditRefereeWindow : Window
    {
        private Referee _originalReferee;

        public EditRefereeWindow(Referee referee)
        {
            InitializeComponent();

            _originalReferee = referee;
            DataContext = _originalReferee;

            FirstNameTextBox.Text = _originalReferee.FirstName;
            LastNameTextBox.Text = _originalReferee.LastName;
            NationalityTextBox.Text = _originalReferee.Nationality;
            GenderTypeComboBox.SelectedItem = GenderTypeComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _originalReferee.GenderType.ToString());
            LeagueTextBox.Text = _originalReferee.League;
        }

        private void RankTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string nationality = NationalityTextBox.Text;
            string genderType = (GenderTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string league = LeagueTextBox.Text;

            if (string.IsNullOrEmpty(nationality) || string.IsNullOrEmpty(genderType) || string.IsNullOrEmpty(league))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _originalReferee.Nationality = nationality;
            _originalReferee.GenderType = (GenderType)Enum.Parse(typeof(GenderType), genderType);
            _originalReferee.League = league;

            this.DialogResult = true;
            this.Close();
        }
    }
}
