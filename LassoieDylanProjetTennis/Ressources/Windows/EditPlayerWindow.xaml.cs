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
    /// Logique d'interaction pour EditPlayerWindow.xaml
    /// </summary>
    public partial class EditPlayerWindow : Window
    {

        private Player _player;

        public EditPlayerWindow(Player player)
        {
            InitializeComponent();
            _player = player;
            DataContext = _player;

            // Initialiser les champs avec les valeurs actuelles
            FirstNameTextBox.Text = _player.FirstName;
            LastNameTextBox.Text = _player.LastName;
            NationalityTextBox.Text = _player.Nationality;
            GenderTypeComboBox.SelectedItem = GenderTypeComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _player.GenderType.ToString());
            RankTextBox.Text = _player.Rank.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string nationality = NationalityTextBox.Text;
            string genderType = (GenderTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string rankText = RankTextBox.Text;

            if (string.IsNullOrEmpty(nationality) || string.IsNullOrEmpty(genderType) || string.IsNullOrEmpty(rankText))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(rankText, out int rank))
            {
                MessageBox.Show("Rank must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _player.Nationality = nationality;
            _player.GenderType = (GenderType)Enum.Parse(typeof(GenderType), genderType);
            _player.Rank = rank;

            this.DialogResult = true;
            this.Close();
        }
    }
}
