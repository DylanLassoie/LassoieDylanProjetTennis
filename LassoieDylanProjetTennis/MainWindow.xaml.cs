using System.Windows;
using System.Windows.Controls;
using LassoieDylanProjetTennis.Ressources.Views;
namespace LassoieDylanProjetTennis
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new HomeView());
        }

        private void NavigationMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavigationMenu.SelectedItem is ListBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Home":
                        MainFrame.Navigate(new HomeView());
                        break;
                    case "Tournaments":
                        MainFrame.Navigate(new TournamentView());
                        break;
                    case "Stadiums":
                        MainFrame.Navigate(new StadiumView());
                        break;
                    case "Players":
                        MainFrame.Navigate(new PlayerView());
                        break;
                    case "Referees":
                        MainFrame.Navigate(new RefereeView());
                        break;
                }
            }
        }
    }
}
