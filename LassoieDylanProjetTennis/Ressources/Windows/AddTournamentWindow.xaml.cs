using LassoieDylanProjetTennis.Ressources.Backend;
using LassoieDylanProjetTennis.Ressources.DAO;
using LassoieDylanProjetTennis.Ressources.Factory;
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

            // Fetch all stadiums from the database
            var stadiumsFromDb = stadiumDAO.GetAll();

            // Clear the ObservableCollection first if it has old data
            _stadiums.Clear();

            // Add the stadiums to the ObservableCollection
            foreach (var stadium in stadiumsFromDb)
            {
                _stadiums.Add(stadium);
            }

            // Bind the ObservableCollection to the ComboBox
            StadiumComboBox.ItemsSource = _stadiums;
            StadiumComboBox.DisplayMemberPath = "NameOfStadium"; // Display the name of the stadium in the ComboBox
            StadiumComboBox.SelectedValuePath = "NameOfStadium"; // You can change this to another property if needed
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
                    CourtType = CourtType.Hard, // Set a default value or modify as needed
                    NbSpectators = 5000, // Default value, change as needed
                    Covered = false, // Default value, change as needed
                    StadiumName = selectedStadium.NameOfStadium
                };

                courtDAO.CreateCourt(newCourt, selectedStadium.NameOfStadium);
            }

            MessageBox.Show($"{courtsToCreate} courts have been successfully created for {selectedStadium.NameOfStadium}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }





        private void AddPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the PlayerDAO using your DAO factory
            var playerDAO = (PlayerDAO)daoFactory.GetPlayerDAO();

            // Clear the current items in the ListBox
            PlayersListBox.Items.Clear();

            // Get the list of male and female players using the PlayerDAO
            var malePlayers = playerDAO.GetMalePlayers();
            var femalePlayers = playerDAO.GetFemalePlayers();

            // Add male players to the ListBox
            foreach (var player in malePlayers)
            {
                PlayersListBox.Items.Add($"{player.FirstName} {player.LastName} ({player.GenderType})");
            }

            // Add female players to the ListBox
            foreach (var player in femalePlayers)
            {
                PlayersListBox.Items.Add($"{player.FirstName} {player.LastName} ({player.GenderType})");
            }
        }



        private void AddRefereesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the top 10 referees from the database
                var refereeDAO = (RefereeDAO)daoFactory.GetRefereeDAO();
                List<Referee> referees = refereeDAO.GetTop10Referees();

                // Clear the ListBox first
                RefereesListBox.Items.Clear();

                // Add each referee to the ListBox
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
            // Retrieve user input
            string tournamentName = TournamentNameTextBox.Text;
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;
            var selectedStadium = StadiumComboBox.SelectedItem as Stadium; // Assuming Stadium is the type

            // Basic validation (ensure all fields are filled)
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
                // Create a new Tournament object
                Tournament newTournament = new Tournament
                {
                    Name = tournamentName,
                    StartingDate = startDate.Value,
                    EndingDate = endDate.Value,
                    // Assuming there’s a Stadium property in the Tournament class
                    // to hold the selected stadium
                };

                // Store the new Tournament object in the Tag property for retrieval
                this.Tag = newTournament;

                // Save the tournament to the database
                var tournamentDAO = daoFactory.GetTournamentDAO();
                tournamentDAO.Create(newTournament);

                // Update the Participation column for all players and referees
                var playerDAO = (PlayerDAO)daoFactory.GetPlayerDAO();
                var refereeDAO = (RefereeDAO)daoFactory.GetRefereeDAO();

                foreach (var player in PlayersListBox.Items)
                {
                    var person = player as Person;
                    if (person != null)
                    {
                        person.Participation = tournamentName;
                        playerDAO.UpdateParticipation((Player)person);
                    }
                }

                foreach (var referee in RefereesListBox.Items)
                {
                    var person = referee as Person;
                    if (person != null)
                    {
                        person.Participation = tournamentName;
                        refereeDAO.UpdateParticipation((Referee)person);
                    }
                }

                // Set the DialogResult to true and close the dialog
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
