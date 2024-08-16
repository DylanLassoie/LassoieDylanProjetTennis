using LassoieDylanProjetTennis.Ressources.Backend;
using LassoieDylanProjetTennis.Ressources.DAO;
using LassoieDylanProjetTennis.Ressources.Factory;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
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
    /// Logique d'interaction pour AddTournamentWindow.xaml
    /// </summary>
    public partial class AddTournamentWindow : Window
    {
        private AbstractDAOFactory daoFactory;
        private readonly StadiumDAO _stadiumDao;
        private ObservableCollection<Stadium> _stadiums;
        

        public AddTournamentWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LassoieDylan"].ConnectionString;

            daoFactory = AbstractDAOFactory.GetFactory(DAOFactoryType.MS_SQL_FACTORY);
            _stadiums = new ObservableCollection<Stadium>();

            LoadStadiums();
        }

        private void LoadStadiums()
        {
            var stadiumDAO = daoFactory.GetStadiumDAO();
            var stadiumsFromDb = stadiumDAO.GetAll();

            _stadiums.Clear();

            foreach (var stadium in stadiumsFromDb)
            {
                _stadiums.Add(stadium);
            }

            StadiumComboBox.ItemsSource = _stadiums;
            StadiumComboBox.DisplayMemberPath = "NameOfStadium"; 
            StadiumComboBox.SelectedValuePath = "NameOfStadium"; 
        }


        private void CreateCourtButton_Click(object sender, RoutedEventArgs e)
        {
            Stadium selectedStadium = StadiumComboBox.SelectedItem as Stadium;
            if (selectedStadium == null)
            {
                MessageBox.Show("Please select a stadium first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var courtDAO = (CourtDAO)daoFactory.GetCourtDAO();
            int nbCourtsAllowed = courtDAO.GetNbCourtsForStadium(selectedStadium.NameOfStadium);
            int existingCourts = courtDAO.GetCourtCountForStadium(selectedStadium.NameOfStadium);
            int courtsToCreate = nbCourtsAllowed - existingCourts;

            if (courtsToCreate <= 0)
            {
                MessageBox.Show("All courts for this stadium have already been created.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            for (int i = 0; i < courtsToCreate; i++)
            {
                Court newCourt = new Court
                {
                    CourtType = CourtType.Hard, 
                    NbSpectators = 5000, 
                    Covered = false, 
                    StadiumName = selectedStadium.NameOfStadium
                };

                courtDAO.CreateCourt(newCourt, selectedStadium.NameOfStadium);
            }

            MessageBox.Show($"{courtsToCreate} courts have been successfully created for {selectedStadium.NameOfStadium}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void AddPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            var playerDAO = (PlayerDAO)daoFactory.GetPlayerDAO();

            PlayersListBox.Items.Clear();

            var malePlayers = playerDAO.GetMalePlayers();
            var femalePlayers = playerDAO.GetFemalePlayers();

            foreach (var player in malePlayers)
            {
                PlayersListBox.Items.Add($"{player.FirstName} {player.LastName} ({player.GenderType})");
            }

            foreach (var player in femalePlayers)
            {
                PlayersListBox.Items.Add($"{player.FirstName} {player.LastName} ({player.GenderType})");
            }
        }


        private void AddRefereesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var refereeDAO = (RefereeDAO)daoFactory.GetRefereeDAO();
                List<Referee> referees = refereeDAO.GetTop10Referees();

                RefereesListBox.Items.Clear();

                foreach (var referee in referees)
                {
                    RefereesListBox.Items.Add($"{referee.FirstName} {referee.LastName} - {referee.Nationality}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding referees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string tournamentName = TournamentNameTextBox.Text;
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;
            var selectedStadium = StadiumComboBox.SelectedItem as Stadium;

            if (string.IsNullOrEmpty(tournamentName) || !startDate.HasValue || !endDate.HasValue || selectedStadium == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (endDate < startDate)
            {
                MessageBox.Show("End date cannot be before the start date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Tournament newTournament = new Tournament
                {
                    Name = tournamentName,
                    StartingDate = startDate.Value,
                    EndingDate = endDate.Value,
                    StadiumName = selectedStadium.NameOfStadium  
                };

                var tournamentDAO = daoFactory.GetTournamentDAO();
                tournamentDAO.Create(newTournament);

                var playerDAO = (PlayerDAO)daoFactory.GetPlayerDAO();
                foreach (var item in PlayersListBox.Items)
                {
                    var playerInfo = item.ToString();

                    int genderTextStartIndex = playerInfo.LastIndexOf(" (");

                    string fullName = playerInfo.Substring(0, genderTextStartIndex).Trim();

                    var nameParts = fullName.Split(' ');

                    string lastName = nameParts.Last();

                    string firstName = string.Join(" ", nameParts.Take(nameParts.Length - 1));

                    var player = playerDAO.FindByName(firstName, lastName);
                    if (player != null)
                    {
                        player.Participation = tournamentName;
                        playerDAO.UpdateParticipation(player);
                        Debug.WriteLine($"Found player: {firstName} {lastName} - Updating Participation to {tournamentName}");
                    }
                    else
                    {
                        Debug.WriteLine($"Player not found in the database: {firstName} {lastName}");
                    }
                }

                var refereeDAO = (RefereeDAO)daoFactory.GetRefereeDAO();
                foreach (var item in RefereesListBox.Items)
                {
                    var refereeInfo = item.ToString().Split('-');
                    var nameParts = refereeInfo[0].Trim().Split(' ');

                    string lastName = nameParts.Last();
                    string firstName = string.Join(" ", nameParts.Take(nameParts.Length - 1));

                    var referee = refereeDAO.FindByName(firstName, lastName);
                    if (referee != null)
                    {
                        referee.Participation = tournamentName;
                        refereeDAO.UpdateParticipation(referee);
                        Debug.WriteLine($"Found referee: {firstName} {lastName} - Updating Participation to {tournamentName}");
                    }
                    else
                    {
                        Debug.WriteLine($"Referee not found in the database: {firstName} {lastName}");
                    }
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the tournament: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
