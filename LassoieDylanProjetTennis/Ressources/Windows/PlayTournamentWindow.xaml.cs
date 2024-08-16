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
    /// Logique d'interaction pour PlayTournamentWindow.xaml
    /// </summary>
    public partial class PlayTournamentWindow : Window
    {
        private string tournamentName;

        public PlayTournamentWindow(string name)
        {
            InitializeComponent();

            tournamentName = name;
            TournamentNameTextBlock.Text = tournamentName;

            ScheduleTypeMenu.SelectedIndex = 0;
        }

        private void ScheduleTypeMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScheduleTypeMenu.SelectedItem is ListBoxItem selectedItem)
            {
                string scheduleType = selectedItem.Tag.ToString();
                UpdateDescription(scheduleType);
                NavigateToView(scheduleType);
            }
        }

        private void UpdateDescription(string scheduleType)
        {
            switch (scheduleType)
            {
                case "GentlemenSingle":
                    ScheduleDescriptionTextBlock.Text = "This is the Gentlemen's Singles tournament schedule.";
                    break;
                case "LadiesSingle":
                    ScheduleDescriptionTextBlock.Text = "This is the Ladies' Singles tournament schedule.";
                    break;
                case "GentlemenDouble":
                    ScheduleDescriptionTextBlock.Text = "This is the Gentlemen's Doubles tournament schedule.";
                    break;
                case "LadiesDouble":
                    ScheduleDescriptionTextBlock.Text = "This is the Ladies' Doubles tournament schedule.";
                    break;
                case "MixedDouble":
                    ScheduleDescriptionTextBlock.Text = "This is the Mixed Doubles tournament schedule.";
                    break;
                default:
                    ScheduleDescriptionTextBlock.Text = "Please select a tournament schedule.";
                    break;
            }
        }

        private void NavigateToView(string scheduleType)
        {
            Uri viewUri = null;
            switch (scheduleType)
            {
                case "GentlemenSingle":
                    viewUri = new Uri("Ressources/Views/GentlemenSingleView.xaml", UriKind.Relative);
                    break;
                case "LadiesSingle":
                    viewUri = new Uri("Ressources/Views/LadiesSingleView.xaml", UriKind.Relative);
                    break;
                case "GentlemenDouble":
                    viewUri = new Uri("Ressources/Views/GentlemenDoubleView.xaml", UriKind.Relative);
                    break;
                case "LadiesDouble":
                    viewUri = new Uri("Ressources/Views/LadiesDoubleView.xaml", UriKind.Relative);
                    break;
                case "MixedDouble":
                    viewUri = new Uri("Ressources/Views/MixedDoubleView.xaml", UriKind.Relative);
                    break;
            }

            if (viewUri != null)
            {
                ContentFrame.Navigate(viewUri);
            }
        }
    }
}
