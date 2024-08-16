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
            
        }

        public override bool Create(Tournament tournament)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    string query = @"
                INSERT INTO Tournament (Name, StartingDate, EndingDate, StadiumName)
                VALUES (@Name, @StartingDate, @EndingDate, @StadiumName)";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", tournament.Name);
                    cmd.Parameters.AddWithValue("@StartingDate", tournament.StartingDate);
                    cmd.Parameters.AddWithValue("@EndingDate", tournament.EndingDate);
                    cmd.Parameters.AddWithValue("@StadiumName", tournament.StadiumName);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"An SQL error occurred while creating the tournament: {ex.Message}", ex);
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
                        string deleteTournamentQuery = "DELETE FROM Tournament WHERE Name = @Name AND StartingDate = @StartingDate AND EndingDate = @EndingDate";
                        SqlCommand deleteTournamentCmd = new SqlCommand(deleteTournamentQuery, connection, transaction);
                        deleteTournamentCmd.Parameters.AddWithValue("@Name", tournament.Name);
                        deleteTournamentCmd.Parameters.AddWithValue("@StartingDate", tournament.StartingDate);
                        deleteTournamentCmd.Parameters.AddWithValue("@EndingDate", tournament.EndingDate);

                        int resultTournament = deleteTournamentCmd.ExecuteNonQuery();

                        if (resultTournament > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Failed to delete the Tournament.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"SQL Error: {ex.Message}", "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
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
                    string query = @"
            SELECT 
                Name,
                StartingDate,
                EndingDate,
                StadiumName
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
                                EndingDate = reader.GetDateTime(reader.GetOrdinal("EndingDate")),
                                StadiumName = reader.GetString(reader.GetOrdinal("StadiumName"))
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

        public List<Person> GetTournamentParticipants(string tournamentName)
        {
            List<Person> participants = new List<Person>();

            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT FirstName, LastName, Nationality, GenderType
                FROM Person
                WHERE Participation = @TournamentName
                ORDER BY LastName, FirstName";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@TournamentName", tournamentName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Person player = new Person
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Nationality = reader.GetString(reader.GetOrdinal("Nationality")),
                                GenderType = (GenderType)Enum.Parse(typeof(GenderType), reader.GetString(reader.GetOrdinal("GenderType")))
                            };

                            participants.Add(player);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred while fetching tournament participants!", ex);
            }

            return participants;
        }
    }
}
