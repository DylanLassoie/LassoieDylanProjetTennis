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
    /// Logique d'interaction pour TournamentTreeWindow.xaml
    /// </summary>
    public partial class TournamentTreeWindow : Window
    {
        public TournamentTreeWindow()
        {
            InitializeComponent();
        }

        private void PopulateTournamentTree(List<Person> participants)
        {
            foreach (var match in participants)
            {
                Round1ListBox.Items.Add($"{match.FirstName} {match.LastName}");
            }

        }
    }
}
