using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class TournamentDAO : DAO<Tournament>
    {
        public TournamentDAO() : base()
        {
            //
        }

        public override bool Create(Tournament tournament)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "INSERT INTO Tournaments (Name, StartingDate, EndingDate) VALUES (@Name, @StartingDate, @EndingDate)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", tournament.Name);
                    cmd.Parameters.AddWithValue("@StartingDate", tournament.StartingDate);
                    cmd.Parameters.AddWithValue("@EndingDate", tournament.EndingDate);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }

        public override bool Delete(Tournament tournament)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Delete from the Tournament table
                        string deleteTournamentQuery = "DELETE FROM Tournament WHERE Name = @Name AND StartingDate = @StartingDate AND EndingDate = @EndingDate";
                        SqlCommand deleteTournamentCmd = new SqlCommand(deleteTournamentQuery, connection, transaction);
                        deleteTournamentCmd.Parameters.AddWithValue("@Name", tournament.Name);
                        deleteTournamentCmd.Parameters.AddWithValue("@StartingDate", tournament.StartingDate);
                        deleteTournamentCmd.Parameters.AddWithValue("@EndingDate", tournament.EndingDate);

                        int resultTournament = deleteTournamentCmd.ExecuteNonQuery();

                        if (resultTournament > 0)
                        {
                            // Commit the transaction if the deletion succeeds
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            // Rollback if deleting the Tournament failed
                            transaction.Rollback();
                            MessageBox.Show("Failed to delete the Tournament.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Rollback and display SQL error message
                        transaction.Rollback();
                        MessageBox.Show($"SQL Error: {ex.Message}", "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Capture and display any other exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        public override bool Update(Tournament tournament)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Tournaments SET StartingDate = @StartingDate, EndingDate = @EndingDate WHERE Name = @Name";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", tournament.Name);
                    cmd.Parameters.AddWithValue("@StartingDate", tournament.StartingDate);
                    cmd.Parameters.AddWithValue("@EndingDate", tournament.EndingDate);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }

        public override Tournament Find(int id)
        {
            Tournament tournament = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Tournaments WHERE IdTournament = @IdTournament";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdTournament", id);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tournament = new Tournament
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartingDate = reader.GetDateTime(reader.GetOrdinal("StartingDate")),
                                EndingDate = reader.GetDateTime(reader.GetOrdinal("EndingDate"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return tournament;
        }

        public override List<Tournament> GetAll()
        {
            List<Tournament> tournaments = new List<Tournament>();

            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    // SQL query to select all tournaments
                    string query = @"
            SELECT 
                Name,
                StartingDate,
                EndingDate
            FROM 
                Tournament";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tournament tournament = new Tournament
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartingDate = reader.GetDateTime(reader.GetOrdinal("StartingDate")),
                                EndingDate = reader.GetDateTime(reader.GetOrdinal("EndingDate"))
                            };

                            tournaments.Add(tournament);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }

            return tournaments;
        }


        // Optional: Add a method to find tournaments by name
        public Tournament FindByName(string name)
        {
            Tournament tournament = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Tournaments WHERE Name = @Name";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", name);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tournament = new Tournament
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartingDate = reader.GetDateTime(reader.GetOrdinal("StartingDate")),
                                EndingDate = reader.GetDateTime(reader.GetOrdinal("EndingDate"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return tournament;
        }
    }
}
